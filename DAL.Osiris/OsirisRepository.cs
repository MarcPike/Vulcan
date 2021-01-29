using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DAL.Osiris
{
    public class OsirisRepository
    {

        private const string HES_CONNECTION_STRING =
            @"data source=S-US-SQL2\Q12;initial catalog=HES; integrated security=True; Connection Timeout=30;";

        private string _osirisConnectionString = string.Empty;

        public static bool IsSupportedCoid(string coid)
        {
            if (string.IsNullOrWhiteSpace(coid)) return false;

            //To add support for a new coid, edit the following items:
            //HES.dbo.DoGetOsirisFolderForDirectory
            //HES.dbo.DoGetOsirisConnectionString
            //The switch statement in this.LoadOsirisDocumentList().

            var supportedCoids = new List<string>()
            {
                "CAN",
                "CHN",
                "INC",
                "SIN",
                "SPL",
                "DUB",
                "MSA",
                "EUR"
            };

            return supportedCoids.Contains(coid);
        }

        public static string GetRootTagNumber(string tagNumber)
        {
            if (string.IsNullOrEmpty(tagNumber)) return "";
            var nonNumericCharacters = new Regex(@"\D");
            var numericOnlyString = nonNumericCharacters.Replace(tagNumber, string.Empty);
            return numericOnlyString ?? "";
        }

        private List<OsirisDocType> _docTypes = null;
        private string _tEmpFolderPath = "";

        /// <summary>
        /// The folder where the PDFs will be written to.  Default value is the LocalApplicationData folder.
        /// </summary>
        public string LocalApplicationDataPath
        {
            get
            {
                if (string.IsNullOrEmpty(_tEmpFolderPath))
                {
                    _tEmpFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                }

                return _tEmpFolderPath;
            }
            set => _tEmpFolderPath = value;
        }

        public async Task<List<OsirisDocInfo>> GetOsirisDocumentListAsync(string coid, string tagNumber, string heatNumber)
        {
            var getOsirisDocsTask = Task.Factory.StartNew<List<OsirisDocInfo>>((stateObj) =>
            {
                var osirisDocs = GetOsirisDocumentList(coid, tagNumber, heatNumber);
                return osirisDocs;
            }, 2000);

            return await getOsirisDocsTask;
        }

        /// <summary>
        /// Gets a list of available Osiris docs for the supplied parameters.
        /// </summary>
        /// <param name="coid"></param>
        /// <param name="tagNumber"></param>
        /// <param name="heatNumber"></param>
        /// <returns></returns>
        public List<OsirisDocInfo> GetOsirisDocumentList(string coid, string tagNumber, string heatNumber)
        {
            var docList = new List<OsirisDocInfo>();
            var docSql = GetDocumentSearchSql(coid);
            if (docSql.IsEmpty) return docList;

            tagNumber = GetRootTagNumber(tagNumber);

            //Set the connection string for this coid
            SetCurrentCoid(coid);


            var docSearchFieldValue = docSql.FieldName == "TagNumber" ? tagNumber : heatNumber;

            using (var conn = new SqlConnection(_osirisConnectionString))
            {
                var sql = string.Format(docSql.SearchSQL + "", docSearchFieldValue);
                var cmd = new SqlCommand(sql, conn) { CommandType = CommandType.Text, CommandTimeout = 360 };

                conn.Open();

                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    var odi = new OsirisDocInfo
                    {
                        ID = (long)dr["DMT_DOCID"],
                        COID = coid,
                        TypeID = (int)dr["DMT_DOCTYPEID"],
                    };

                    odi.TypeName = GetDocTypeName(odi.TypeID);
                    odi.FormatID = (int)dr["DMT_DOCFORMATID"];
                    odi.FormatName = GetDocFormatName(odi.FormatID);
                    odi.Name = (string)dr["DMT_DOCNAME"];
                    odi.Description = (string)dr["DMT_DOCDESCRIPTION"];
                    //odi.DKL_DocID = (long)dr["DKL_DOCID"];

                    docList.Add(odi);
                }

                dr.Close();
                conn.Close();
            }

            return docList;

        }

        /// <summary>
        /// Edits the specified document.
        /// </summary>
        /// <param name="coid"></param>
        /// <param name="dmtDocId"></param>
        public void EditOsirisDocument(string coid, long dmtDocId)
        {
            var ssName = "";
            var uLink = "";

            switch (coid)
            {
                case "INC":
                case "CAN":
                    ssName = "http://osiris.us.howcogroup.com/editpdf.aspx?MODE=D&DOC=";
                    break;
                case "CHN":
                    ssName = "http://osiris.CN.howcogroup.com/editpdf.aspx?MODE=D&DOC=";
                    break;
                case "SIN":
                case "MSA":
                    ssName = "http://osiris.SG.howcogroup.com/editpdf.aspx?MODE=D&DOC=";
                    break;
                case "SPL":
                case "DUB":
                    ssName = "http://osiris.UK.howcogroup.com/editpdf.aspx?MODE=D&DOC=";
                    break;
            }

            uLink = ssName;
            uLink += dmtDocId;
            uLink += "&SDOC=-1&MKLOCAL=Y";

            using (var proc = new Process() { EnableRaisingEvents = false })
            {
                proc.StartInfo.FileName = uLink;
                proc.Start();
            }
        }

        public MemoryStream GetOsirisDocumentAsStream(string coid, long docId)
        {

            SetCurrentCoid(coid);

            using (var conn = new SqlConnection(_osirisConnectionString))
            {
                using (var p = new Process())
                {
                    using (var daViewDocument = new SqlDataAdapter($"EXEC dbo.ViewDocument {docId}", conn))
                    {
                        var dsFileData = new DataSet();
                        daViewDocument.SelectCommand.CommandType = CommandType.Text;
                        daViewDocument.SelectCommand.CommandTimeout = 360;
                        daViewDocument.Fill(dsFileData);


                        var mimeType = dsFileData.Tables[0].Rows[0]["DT_MIMETYPE"];
                        if (mimeType != DBNull.Value)
                        {
                            if ((string)mimeType == "application/pdf")
                            {
                                var buffer = (byte[])dsFileData.Tables[1].Rows[0]["ND_BINARYDATA"];
                                return new MemoryStream(buffer);
                            }
                            else if ((string)mimeType == "image/tiff")
                            {
                                var buffer = (byte[])dsFileData.Tables[1].Rows[0]["ND_BINARYDATA"];
                                return new MemoryStream(buffer);
                            }
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Opens the specified document.  You must first have called ShowOsirusDocumentList() or SetOsirisConnectionString() so that the proper connection string is set.
        /// </summary>
        /// <param name="docId"></param>
        public void OpenOsirisDocument(long docId)
        {

            using (var conn = new SqlConnection(HES_CONNECTION_STRING))
            {
                using (var p = new Process())
                {
                    using (var daViewDocument = new SqlDataAdapter($"EXEC dbo.ViewDocument {docId}", conn))
                    {
                        var dsFileData = new DataSet();
                        daViewDocument.SelectCommand.CommandType = CommandType.Text;
                        daViewDocument.SelectCommand.CommandTimeout = 360;
                        daViewDocument.Fill(dsFileData);

                        var filePath = $@"{LocalApplicationDataPath}\QNG";

                        //Make sure path exists first.  If so, this line will be ignored.
                        Directory.CreateDirectory(filePath);

                        var mimeType = dsFileData.Tables[0].Rows[0]["DT_MIMETYPE"];
                        if (mimeType != DBNull.Value)
                        {
                            var fileName = "";
                            if ((string)mimeType == "application/pdf")
                            {
                                fileName = $@"{filePath}\{docId}.pdf";
                                var fs = new FileStream(fileName, FileMode.Create);
                                var buffer = (byte[])dsFileData.Tables[1].Rows[0]["ND_BINARYDATA"];
                                fs.Write(buffer, 0, buffer.Length);
                                fs.Close();
                                p.StartInfo.FileName = fileName;
                                p.Start();
                            }
                            else if ((string)mimeType == "image/tiff")
                            {
                                fileName = $@"{filePath}\{docId}.tiff";
                                var fs = new FileStream(fileName, FileMode.Create);
                                var buffer = (byte[])dsFileData.Tables[1].Rows[0]["ND_BINARYDATA"];
                                fs.Write(buffer, 0, buffer.Length);
                                fs.Close();
                                p.StartInfo.FileName = fileName;
                                p.Start();
                            }
                        }
                    }
                }
            }
        }

        public void SetCurrentCoid(string coid)
        {
            _osirisConnectionString = GetOsirisConnectionString(coid);
            _docTypes = GetDocTypes();
        }

        private DocumentSearchSQL GetDocumentSearchSql(string COID)
        {
            var docSql = new DocumentSearchSQL();
            if (string.IsNullOrWhiteSpace(COID)) return docSql;

            var dtSearchSql = new DataTable();

            using (var conn = new SqlConnection(HES_CONNECTION_STRING))
            {
                conn.Open();

                var dataAdapter =
                    new SqlDataAdapter($"SELECT * FROM dbo.DoGetOsirisDocSearchSQLV1('{COID}')", conn)
                    {
                        SelectCommand =
                        {
                            CommandTimeout = 360,
                            CommandType = CommandType.Text
                        }
                    };


                dataAdapter.Fill(dtSearchSql);

                if (dtSearchSql.Rows.Count > 0)
                {
                    docSql.SearchSQL = (string)dtSearchSql.Rows[0]["SearchSQL"];
                    docSql.FieldName = (string)dtSearchSql.Rows[0]["FieldName"];
                }

                conn.Close();
            }

            return docSql;
        }

        /// <summary>
        /// Gets a list of doc types for the current coid.
        /// </summary>
        /// <returns></returns>
        private List<OsirisDocType> GetDocTypes()
        {
            var typeList = new List<OsirisDocType>();



            //No error handling here.  Method call should be wrapped in a try/catch block.
            using (var conn = new SqlConnection(_osirisConnectionString))
            {
                const string sql = "SELECT * FROM DocumentTypes";
                var cmd = new SqlCommand(sql, conn)
                {
                    CommandType = CommandType.Text,
                    CommandTimeout = 360
                };

                conn.Open();

                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    var odt = new OsirisDocType
                    {
                        ID = (int)dr["DTP_DOCTYPEID"],
                        Name = (string)dr["DTP_DOCTYPENAME"]
                    };


                    typeList.Add(odt);
                }

                dr.Close();
                conn.Close();
            }

            return typeList;
        }

        private string GetDocFormatName(int formatID)
        {
            if (formatID == 1)
                return "PDF";
            else if (formatID == 2)
                return "Tiff";
            else
                return "??";
        }

        private string GetDocTypeName(int typeID)
        {
            return (from t in _docTypes
                    where t.ID == typeID
                    select t.Name).FirstOrDefault();
        }

        /// <summary>
        /// Gets the Osiris connection string to be used, which differs based on coid.
        /// </summary>
        /// <param name="coid"></param>
        /// <returns>The connection string that was set.</returns>
        private string GetOsirisConnectionString(string coid)
        {
            var result = new DataTable();

            using (var conn = new SqlConnection(HES_CONNECTION_STRING))
            {
                conn.Open();

                var dataAdapter =
                    new SqlDataAdapter($"SELECT OsirisConnection = dbo.DoGetOsirisConnectionString('{coid}')",
                        conn) {SelectCommand = {CommandType = CommandType.Text}};


                dataAdapter.Fill(result);

                conn.Close();

            }

            _osirisConnectionString = (string)result.Rows[0]["OsirisConnection"];
            _osirisConnectionString.Replace(@"\", @"");

            return _osirisConnectionString;
        }


    }
}

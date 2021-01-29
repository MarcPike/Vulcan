using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.FileOperations;
using DAL.HRS.Mongo.DocClass.Hrs_User;
using DAL.HRS.Mongo.DocClass.SecurityRole;
using DAL.HRS.Mongo.Helpers;
using DAL.HRS.Mongo.Models;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Repository;
using ExcelDataReader;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using MongoDB.Bson;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using UTL.Hrs.EmployeeImport;

namespace HRS.WebApi2.Controllers
{
    [EnableCors("CorsPolicy")]
    //[Produces("application/json")]
    //[Consumes("application/json")]
    public class FileController: BaseController
    {
        private readonly IHelperEmployee _helperEmployee;
        private readonly IHelperSecurity _helperSecurity;
        private readonly IHelperFile _helperFile;

        public FileController(IHelperEmployee helperEmployee, IHelperSecurity helperSecurity, IHelperFile helperFile)
        {
            _helperEmployee = helperEmployee;
            _helperSecurity = helperSecurity;
            _helperFile = helperFile;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("files/ExportEmployeeAvatars/{path}")]
        public async Task<JsonResult> ExportEmployeeAvatars(string path)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);
                var hrsUser = _helperUser.GetHrsUser(tokenData.UserId);

                if (!hrsUser.SystemAdmin) throw new Exception("Only SystemAdmins can perform this task");

                await _helperFile.ExportEmployeeAvatars(path);

                result.Success = true;
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.Message;
                result.Success = false;
            }
            return JsonResultWithStatusCode(result, tokenData.StatusCodeResult);


        }

        [AllowAnonymous]
        [HttpPost]
        [Route("files/UploadEmployeeImage")]
        public ActionResult UploadEmployeeImage()
        {
            dynamic result = new ExpandoObject();
            result.Success = false; 
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                var model = GetFileUploadModelFromHeader();
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                var file = HttpContext.Request.Form.Files[0];
                if (file == null)
                {
                    throw new Exception("No file sent to Upload");
                }

                var employee = SaveEmployeeImage(model, file);
                result.EmployeeModel = new EmployeeModel(employee);
                result.Success = true;

            }
            catch (Exception e)
            {
                result.ErrorMessage = e.Message;
                result.Success = false;
            }

            return JsonResultWithStatusCode(result, tokenData.StatusCodeResult);
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("files/UploadDocumentForId")]
        public ActionResult UploadDocumentForId()
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                var model = GetFileUploadModelFromHeader();
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                var file = HttpContext.Request.Form.Files[0];
                if (file == null)
                {
                    throw new Exception("No file sent to Upload");
                }

                if (model.ModuleName == "EmployeeImage")
                {
                    var employee = SaveEmployeeImage(model, file);
                    result.EmployeeModel = new EmployeeModel(employee);
                    result.Success = true;

                    return JsonResultWithStatusCode(result, tokenData.StatusCodeResult);
                }

                var mongoContext = new HrsContext();

                var hrsUser = _helperUser.GetHrsUser(tokenData.UserId);
                var securityRole = GetSecurityRole(model.IsHrs, hrsUser);
                var systemModule = GetSystemModule(model.ModuleName, securityRole);

                var fileOperations = _helperSecurity.GetFileOperationsForModule(tokenData.UserId, systemModule.Id);

                if (!fileOperations.CanUpload)
                {
                    throw new Exception("User does not have FileUpload privileges");
                }


                //var file = model.File;

                var ms = new MemoryStream();
                file.CopyTo(ms);
                var fileBytes = ms.ToArray();

                result.NewFileInfo = _helperFile.UploadDocument(mongoContext, fileBytes, model.FileName, model.DocumentDate, ObjectId.Parse(model.BaseDocumentId), model.ModuleName, model.DocumentType, tokenData.UserId, model.Comments);

                result.Success = true;
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.Message;
                result.Success = false;
            }

            return JsonResultWithStatusCode(result, tokenData.StatusCodeResult);
        }

        private Employee SaveEmployeeImage(FileUploadModel model, IFormFile file)
        {
            var employee = Employee.Helper.FindById(model.BaseDocumentId);
            if (employee == null) throw new Exception("Employee not found");

            var fileName = file.FileName;
            var filePath = Path.Combine(@"\\s-us-web02\EmployeeImages", fileName);
            using (var fileStream =
                new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
            {
                file.CopyTo(fileStream);
            }

            employee.EmployeeImageFileName = fileName;
            Employee.Helper.Upsert(employee);
            return employee;
        }

        private FileUploadModel GetFileUploadModelFromHeader()
        {
            StringValues contents = new StringValues();
            HttpContext.Request.Form.TryGetValue("fileMetadata", out contents);

            var fileUploadInfo = JObject.Parse(contents);

            var model = new FileUploadModel()
            {
                BaseDocumentId = fileUploadInfo.GetValue("baseDocumentId").ToString(),
                Comments = fileUploadInfo.GetValue("comments").ToString(),
                DocumentDate = DateTime.Parse(fileUploadInfo.GetValue("documentDate").ToString()),
                DocumentType = fileUploadInfo.GetValue("documentType").ToString(),
                FileName = fileUploadInfo.GetValue("fileName").ToString(),
                IsHrs = fileUploadInfo.GetValue("isHrs").ToString() == "True",
                ModuleName = fileUploadInfo.GetValue("moduleName").ToString()
            };
            return model;
        }


        [AllowAnonymous]
        [HttpGet]
        [Route("files/GetAllDocumentsForId/{moduleName}/{isHrs}/{baseDocumentId}")]
        public JsonResult GetAllDocumentsForId(string moduleName, bool isHrs, string baseDocumentId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);
                var hrsUser = _helperUser.GetHrsUser(tokenData.UserId);

                var securityRole = GetSecurityRole(isHrs, hrsUser);
                var systemModule = GetSystemModule(moduleName, securityRole);
                var fileOperations = _helperSecurity.GetFileOperationsForModule(tokenData.UserId, systemModule.Id);
                if (!fileOperations.CanDownload)
                {
                    throw new Exception("User does not have FileDownload privileges");
                }

                result.Files = _helperFile.GetAllAttachmentsForDocument(baseDocumentId, moduleName).OrderByDescending(x=>x.DocumentDate);

                result.Success = true;
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.Message;
                result.Success = false;
            }
            return JsonResultWithStatusCode(result, tokenData.StatusCodeResult);

        }


        [AllowAnonymous]
        [HttpGet]
        [Route("fileAttachment/DownloadAttachmentForId/{hrsUserId}/{moduleName}/{isHrs}/{baseDocumentId}/{documentId}")]
        public IActionResult DownloadAttachmentForId(string hrsUserId, string moduleName, bool isHrs, string baseDocumentId, string documentId)
        {
            //var tokenData = GetTokenDataFromHeaders();
            try
            {
                //ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                var hrsUser = _helperUser.GetHrsUser(hrsUserId);
                var securityRole = GetSecurityRole(isHrs, hrsUser);
                var systemModule = GetSystemModule(moduleName, securityRole);
                var fileOperations = _helperSecurity.GetFileOperationsForModule(hrsUserId, systemModule.Id);
                if (!fileOperations.CanDownload)
                {
                    throw new Exception("User does not have FileDownload privileges");
                }
                var attachment = FileAttachmentsHrs.GetAllAttachmentsForDocument(ObjectId.Parse(baseDocumentId), moduleName)
                    .SingleOrDefault(x => x.Id == ObjectId.Parse(documentId));

                if (attachment == null) throw new Exception("Attachment not found");

                var fileName = Path.GetFileName(attachment.Filename);
                var memory = FileAttachmentsHrs.DownloadAsStream(attachment);
                memory.Position = 0;
                byte[] fileData = ReadFully(memory);

                return File(fileData, GetContentType(fileName), fileName);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        [AllowAnonymous]
        [HttpGet]
        [Route("fileAttachment/DownloadAttachmentBytesForId/{moduleName}/{isHrs}/{baseDocumentId}/{documentId}")]
        public JsonResult DownloadAttachmentBytesForId(string moduleName, bool isHrs, string baseDocumentId, string documentId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                var hrsUser = _helperUser.GetHrsUser(tokenData.UserId);
                var securityRole = GetSecurityRole(isHrs, hrsUser);
                var systemModule = GetSystemModule(moduleName, securityRole);
                var fileOperations = _helperSecurity.GetFileOperationsForModule(tokenData.UserId, systemModule.Id);
                if (!fileOperations.CanDownload)
                {
                    throw new Exception("User does not have FileDownload privileges");
                }
                var attachment = FileAttachmentsHrs.GetAllAttachmentsForDocument(ObjectId.Parse(baseDocumentId), moduleName)
                    .SingleOrDefault(x => x.Id == ObjectId.Parse(documentId));

                if (attachment == null) throw new Exception("Attachment not found");

                var fileName = Path.GetFileName(attachment.Filename);
                var memory = FileAttachmentsHrs.DownloadAsStream(attachment);
                memory.Position = 0;

                byte[] fileData = ReadFully(memory);

                //var last10Bytes = fileData.ToString().Substring(fileData.Count() - 10, 10).ToCharArray();

                result.FileName = fileName;
                result.MimeType = GetContentType(fileName);
                result.FileInfo = attachment;
                result.FileData = fileData;
                result.Success = true;

            }
            catch (Exception e)
            {
                result.Success = false;
                result.ErrorMessage = e.Message;
            }
            return JsonResultWithStatusCode(result, tokenData.StatusCodeResult);

        }

        [AllowAnonymous]
        [HttpGet]
        [Route("fileAttachment/RemoveAttachmentForId/{moduleName}/{isHrs}/{documentId}")]
        public JsonResult RemoveAttachmentForId(string moduleName, bool isHrs, bool isHse, string documentId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);
                var hrsUser = _helperUser.GetHrsUser(tokenData.UserId);
                var securityRole = GetSecurityRole(isHrs, hrsUser);
                var systemModule = GetSystemModule(moduleName, securityRole);
                var fileOperations = _helperSecurity.GetFileOperationsForModule(tokenData.UserId, systemModule.Id);
                if (!fileOperations.CanDelete)
                {
                    throw new Exception("User does not have FileDelete privileges");
                }
                //var attachment = FileAttachmentsHrs.GetAllAttachmentsForDocument(employee, moduleName)
                //    .SingleOrDefault(x => x.Id == ObjectId.Parse(documentId));

                //if (attachment == null) throw new Exception("Attachment not found");

                FileAttachmentsHrs.Remove(ObjectId.Parse(documentId));
                result.Success = true;

            }
            catch (Exception e)
            {
                result.ErrorMessage = e.Message;
                result.Success = false;
            }
            return JsonResultWithStatusCode(result, tokenData.StatusCodeResult);

        }

        [AllowAnonymous]
        [HttpPost]
        [Route("files/UploadCompensationImport")]
        public ActionResult UploadCompensationImport()
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                var model = GetCompensationUploadModelFromHeader();
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                var file = HttpContext.Request.Form.Files[0];
                if (file == null)
                {
                    throw new Exception("No file sent to Upload");
                }

                var mongoContext = new HrsContext();

                var hrsUser = _helperUser.GetHrsUser(tokenData.UserId);
                var securityRole = GetSecurityRole(model.IsHrs, hrsUser);
                var systemModule = GetSystemModule(model.ModuleName, securityRole);
                var hrsUserRef = _helperUser.GetHrsUser(tokenData.UserId).AsHrsUserRef();




                var fileOperations = _helperSecurity.GetFileOperationsForModule(tokenData.UserId, systemModule.Id);

                if (!fileOperations.CanUpload)
                {
                    throw new Exception("User does not have FileUpload privileges");
                }
                                
                var excelReader = ExcelDataReader.ExcelReaderFactory.CreateReader(file.OpenReadStream());                
                DataSet ds = excelReader.AsDataSet(new ExcelDataSetConfiguration()
                {
                    ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                    {
                        UseHeaderRow = true
                    }
                });



                //var importer = new CompensationImport(@"C:\src\tfs\Vulcan Technology\UTL.Hrs.EmployeeImport\ImportFolder\Compensation Import.xlsx");

                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataTable dt in ds.Tables)
                    {
                        var importer = new CompensationImport(dt);
                        importer.Execute(hrsUserRef);
                    }

                } else 
                {
                    result.ErrorMessage = "No data found";
                    result.Success = false;
                }

                //result.NewFileInfo = _helperFile.UploadDocument(mongoContext, fileBytes, model.FileName, ObjectId.Parse(model.BaseDocumentId), model.ModuleName, tokenData.UserId);

                result.Success = true;
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.Message;
                result.Success = false;
            }

            return JsonResultWithStatusCode(result, tokenData.StatusCodeResult);
        }


        public FileUploadModel GetCompensationUploadModelFromHeader()
        {
            StringValues contents = new StringValues();
            HttpContext.Request.Form.TryGetValue("fileMetadata", out contents);

            var fileUploadInfo = JObject.Parse(contents);

            var model = new FileUploadModel()
            {
                //BaseDocumentId = fileUploadInfo.GetValue("baseDocumentId").ToString(),
                FileName = fileUploadInfo.GetValue("fileName").ToString(),
                IsHrs = fileUploadInfo.GetValue("isHrs").ToString() == "True",
                ModuleName = fileUploadInfo.GetValue("moduleName").ToString(),
            };

            return model;
        }




        private static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {

                    ms.Write(buffer, 0, read);
                }

                var data = ms.ToArray();
                return TrimNullCharactersFromEndOfByteArray(data);
            }
        }

        private static byte[] TrimNullCharactersFromEndOfByteArray(byte[] data)
        {
            int i = data.Length - 1;
            while (data[i] == 0)
                --i;
            // now foo[i] is the last non-zero byte
            byte[] result = new byte[i + 1];
            Array.Copy(data, result, i + 1);
            return result;

        }

        private static SecurityRole GetSecurityRole(bool isHrs, HrsUser hrsUser)
        {
            SecurityRole securityRole = hrsUser.HrsSecurity.GetRole();
            if (!isHrs)
            {
                securityRole = hrsUser.HseSecurity.GetRole();
            }

            if (securityRole == null) throw new Exception("User does not have Security Role defined");
            return securityRole;
        }

        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            var result = types[ext];
            //if (result == "application/pdf")
            //{
            //    result = "application/octet-stream";
            //}

            return result;
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"},
                {".avi", "video/x-msvideo"},
                {".mp4", "video/mp4"},
                {".mp4v", "video/mp4"},
                {".mpg", "video/mpeg"},
                {".mov", "video/quicktime"},
                {".mpeg", "video/mpeg"},
                {".flv", "video/x-flv"},
            };

        }

        private static SystemModule GetSystemModule(string moduleName, SecurityRole securityRole)
        {
            var systemModule = securityRole.Modules.FirstOrDefault(x => x.ModuleType.Name == moduleName);
            if (systemModule == null) throw new Exception("Module not found");
            return systemModule;
        }

        private static SecurityRole GetSecurityRole(string roleName, bool isHrs, bool isHse)
        {
            var securityRole = new RepositoryBase<SecurityRole>().AsQueryable().LastOrDefault(x =>
                x.RoleType.Name == roleName &&
                x.RoleType.IsHrsRole == isHrs &&
                x.RoleType.IsHseRole == isHse);
            if (securityRole == null) throw new Exception("Role not found");
            return securityRole;
        }

    }
}

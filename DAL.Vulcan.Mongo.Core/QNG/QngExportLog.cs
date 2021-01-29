using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Core.DocClass;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.Core.QNG
{
    public class QngExportLog : BaseDocument
    {
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime ExportTime { get; set; }

        public int RowsExported { get; set; }
        public int RowsSkipped { get; set; }
        public QngExportStatus Status { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime Started { get; set; } = DateTime.Now;

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime Finished { get; set; } = DateTime.MinValue;

        public TimeSpan ElapsedTime
        {
            get
            {
                return (Finished != DateTime.MinValue) ? Finished - Started : TimeSpan.Zero;
            }
        }

        

        public List<string> Errors { get; set; } = new List<string>();

        public QngExportLog()
        {
            ExportTime = DateTime.Now;
            Status = QngExportStatus.Initializing;
        }


    }
}

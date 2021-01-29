using System;
using System.Collections.Generic;
using System.Linq;
using DAL.HRS.Mongo.DocClass.Hse;
using DAL.HRS.Mongo.DocClass.Properties;

namespace DAL.HRS.Mongo.Models
{
    public class MedicalWorkStatusModel : BaseModel
    {
        public string Id { get; set; } 
        public PropertyValueRef Type { get; set; }
        public DateTime? Date { get; set; }
        public string Comments { get; set; } = string.Empty;

        public MedicalWorkStatusModel()
        {
            
        }

        public MedicalWorkStatusModel(MedicalWorkStatus s)
        {
            Id = s.Id.ToString();
            Type = s.Type;
            Date = _encryption.Decrypt<DateTime?>(s.Date);
            Comments = _encryption.Decrypt<string>(s.Comments);
        }

        public MedicalWorkStatus AsMedicalWorkStatus()
        {
            return new MedicalWorkStatus()
            {
                Id = Guid.Parse(Id),
                Type = Type,
                Date = _encryption.Encrypt<DateTime?>(Date),
                Comments = _encryption.Encrypt<string>(Comments)

            };
        }

        public static List<MedicalWorkStatusModel> ConvertList(List<MedicalWorkStatus> list)
        {
            if ((list == null) || (list.Count == 0)) return new List<MedicalWorkStatusModel>();

            return list.Select(x => new MedicalWorkStatusModel(x)).ToList();
        }

        public static List<MedicalWorkStatus> ConvertBackToBase(List<MedicalWorkStatusModel> list)
        {
            if ((list == null) || (list.Count == 0)) return new List<MedicalWorkStatus>();

            return list.Select(x => x.AsMedicalWorkStatus()).ToList();
        }
    }
}
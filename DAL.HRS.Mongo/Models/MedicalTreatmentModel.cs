using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.Hse;

namespace DAL.HRS.Mongo.Models
{
    public class MedicalTreatmentModel : BaseModel
    {
        public string Id { get; set; } 
        public DateTime? DateOfTreatment { get; set; }
        public string TreatmentFacility { get; set; } = string.Empty;
        public string Doctor { get; set; } = string.Empty;
        public bool FollowupRequired { get; set; } = false;
        public DateTime? DateOfFollowup { get; set; }

        public MedicalTreatmentModel()
        {
                
        }

        public MedicalTreatment AsMedicalTreatment()
        {
            return new MedicalTreatment()
            {
                Id = Guid.Parse(Id),
                TreatmentFacility = _encryption.Encrypt<string>(TreatmentFacility),
                DateOfTreatment = _encryption.Encrypt<DateTime?>(DateOfTreatment),
                Doctor = _encryption.Encrypt<string>(Doctor),
                FollowupRequired = _encryption.Encrypt<bool>(FollowupRequired),
                DateOfFollowup = _encryption.Encrypt<DateTime?>(DateOfFollowup)

            };
        }

        public MedicalTreatmentModel(MedicalTreatment t)
        {
            if (t.Id == null) t.Id = Guid.NewGuid();

            Id = t.Id.ToString();
            DateOfTreatment = _encryption.Decrypt<DateTime?>(t.DateOfTreatment);
            TreatmentFacility = _encryption.Decrypt<string>(t.TreatmentFacility);
            Doctor = _encryption.Decrypt<string>(t.Doctor);
            FollowupRequired = _encryption.Decrypt<bool>(t.FollowupRequired);
            DateOfFollowup = _encryption.Decrypt<DateTime?>(t.DateOfFollowup);
        }

        public static List<MedicalTreatmentModel> ConvertList(List<MedicalTreatment> list)
        {
            if ((list == null) || (list.Count == 0)) return new List<MedicalTreatmentModel>();

            return list.Select(x => new MedicalTreatmentModel(x)).ToList();
        }

        public static List<MedicalTreatment> ConvertBackToBase(List<MedicalTreatmentModel> list)
        {
            if ((list == null) || (list.Count == 0)) return new List<MedicalTreatment>();

            return list.Select(x => x.AsMedicalTreatment()).ToList();
        }

    }
}

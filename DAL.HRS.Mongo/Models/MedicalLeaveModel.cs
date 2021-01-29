using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.Hse;
using DAL.Vulcan.Mongo.Base.Encryption;

namespace DAL.HRS.Mongo.Models
{
    public class MedicalLeaveModel
    {
        public string Id { get; set; }
        public DateTime? DateOfLeave { get; set; }
        public DateTime? DateOfExpectedReturn { get; set; }
        public DateTime? DateOfActualReturn { get; set; }
        public decimal? HoursAway { get; set; }
        public decimal? DaysAway { get; set; }
        public decimal? DaysRestrictedWork { get; set; }

        public MedicalLeaveModel() { }

        public MedicalLeaveModel(MedicalLeave l)
        {
            var enc = Encryption.NewEncryption;
            Id = l.Id.ToString();
            DateOfActualReturn = enc.Decrypt<DateTime?>(l.DateOfActualReturn);
            DateOfLeave = enc.Decrypt<DateTime?>(l.DateOfLeave);
            DateOfExpectedReturn = enc.Decrypt<DateTime?>(l.DateOfExpectedReturn);
            HoursAway = enc.Decrypt<decimal?>(l.HoursAway);
            DaysAway = enc.Decrypt<decimal?>(l.DaysAway);
            DaysRestrictedWork = enc.Decrypt<decimal?>(l.DaysRestrictedWork);
        }

        public MedicalLeave AsMedicalLeave()
        {
            var enc = Encryption.NewEncryption;
            if (Id == null) Id = Guid.NewGuid().ToString();
            return new MedicalLeave() 
            {
                Id = Guid.Parse(Id),
                DateOfActualReturn = enc.Encrypt<DateTime?>(DateOfActualReturn),
                DateOfLeave = enc.Encrypt<DateTime?>(DateOfLeave),
                DateOfExpectedReturn = enc.Encrypt<DateTime?>(DateOfExpectedReturn),
                HoursAway = enc.Encrypt<decimal?>(HoursAway),
                DaysAway = enc.Encrypt<decimal?>(DaysAway),
                DaysRestrictedWork = enc.Encrypt<decimal?>(DaysRestrictedWork)
            };
        }

        public static List<MedicalLeave> ConvertBackToBase(List<MedicalLeaveModel> list)
        {
            if ((list == null) || (list.Count == 0)) return new List<MedicalLeave>();

            return list.Select(x => x.AsMedicalLeave()).ToList();
        }

        public static List<MedicalLeaveModel> ConvertList(List<MedicalLeave> list)
        {
            if ((list == null) || (list.Count == 0)) return new List<MedicalLeaveModel>();

            return list.Select(x => new MedicalLeaveModel(x)).ToList();
        }
    }
}

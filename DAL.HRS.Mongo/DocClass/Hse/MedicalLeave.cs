using System;
using DAL.HRS.Mongo.Models;
using DAL.Vulcan.Mongo.Base.Encryption;

namespace DAL.HRS.Mongo.DocClass.Hse
{
    public class MedicalLeave
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public byte[] DateOfLeave { get; set; }
        public byte[] DateOfExpectedReturn { get; set; }
        public byte[] DateOfActualReturn { get; set; }
        public byte[] HoursAway { get; set; }
        public byte[] DaysAway { get; set; }
        public byte[] DaysRestrictedWork { get; set; }

        public override int GetHashCode()
        {
            var data = new MedicalLeaveModel(this);

            var hashValue = string.Empty;
            hashValue += data.DateOfLeave?.ToLongDateString() ?? "null";
            hashValue += data.DateOfExpectedReturn?.ToLongDateString() ?? "null";
            hashValue += data.DateOfActualReturn?.ToLongDateString() ?? "null";
            hashValue += data.HoursAway.ToString();
            hashValue += data.DaysAway.ToString();

            return hashValue.GetHashCode();
        }

        public override string ToString()
        {
            var enc = Encryption.NewEncryption;
            return
                $"DateOfLeave: [{enc.Decrypt<DateTime>(DateOfLeave)}]";
        }
    }
}
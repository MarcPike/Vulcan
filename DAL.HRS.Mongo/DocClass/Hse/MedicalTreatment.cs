using System;
using DAL.HRS.Mongo.Models;
using DAL.Vulcan.Mongo.Base.Encryption;

namespace DAL.HRS.Mongo.DocClass.Hse
{
    public class MedicalTreatment
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public byte[] DateOfTreatment { get; set; }
        public byte[] TreatmentFacility { get; set; }
        public byte[] Doctor { get; set; }
        public byte[] FollowupRequired { get; set; }
        public byte[] DateOfFollowup { get; set; }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override string ToString()
        {
            var enc = Encryption.NewEncryption;
            var dateOfTreatment = enc.Decrypt<DateTime?>(DateOfTreatment);
            var treatmentFacility = enc.Decrypt<string>(TreatmentFacility);
            var doctor = enc.Decrypt<string>(Doctor);
            var followupRequired = enc.Decrypt<bool>(FollowupRequired);
            var dateOfFollowup = enc.Decrypt<DateTime?>(DateOfFollowup);


            return $"DateOfTreatment: {dateOfTreatment} @ {treatmentFacility} Doctor: {doctor} Followup Required: {followupRequired} Followup Date: {followupRequired}";
        }
    }
}
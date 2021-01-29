using System;

namespace DAL.HRS.Mongo.DocClass.Hse
{
    public class CoshhProductGhsClassification
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public GhsClassificationTypeRef ClassificationType { get; set; }
        public string Comment { get; set; }
    }
}
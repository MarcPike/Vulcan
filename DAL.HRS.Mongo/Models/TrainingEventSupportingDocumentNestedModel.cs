using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.FileOperations;
using DAL.HRS.Mongo.DocClass.Training;

namespace DAL.HRS.Mongo.Models
{
    public class TrainingEventSupportingDocumentNestedModel
    {
        public TrainingEventSmallModel TrainingEvent { get; }
        public List<SupportingDocumentModel> SupportingDocuments { get; set; }

        public TrainingEventSupportingDocumentNestedModel()
        {
        }

        public TrainingEventSupportingDocumentNestedModel(TrainingEventSmallModel trainingEvent, List<SupportingDocumentModel> docs)
        {
            TrainingEvent = trainingEvent;
            SupportingDocuments = docs;
        }

    }

    public class TrainingEventSupportingDocumentFlatModel
    {
        public TrainingEventSmallModel TrainingEvent { get; set; }
        public SupportingDocumentModel SupportingDocument { get; set; }

        public static List<TrainingEventSupportingDocumentFlatModel> ConvertNestedToFlat(List<TrainingEventSupportingDocumentNestedModel> values)
        {
            var results = new List<TrainingEventSupportingDocumentFlatModel>();

            foreach (var source in values)
            {
                foreach (var doc in source.SupportingDocuments)
                {
                    results.Add(new TrainingEventSupportingDocumentFlatModel()
                    {
                        SupportingDocument = doc,
                        TrainingEvent = source.TrainingEvent
                    });
                }
            }

            return results;
        }

    }

}

using System;
using System.Collections.Generic;
using System.Linq;

namespace Mrp.Prototype.MrpClasses
{
    public class Job : MrpObject
    {
        public Resource Resource { get; set; }
        public WorkOrderItem WorkOrderItem { get; set; }
        public int ReceivedPieces { get; set; } = 0;
        public int RequiredPieces { get; set; } = 0;
        public int CompletedPieces { get; set; } = 0;
        public int TransportPieces { get; set; } = 0;
        public ShopScheduler ShopScheduler { get; set; }
        public List<JobHistory> JobHistory { get; set; } = new List<JobHistory>();
        public JobStatus JobStatus { get; set; } = JobStatus.Scheduled;
        public TransportStatus TransportStatus { get; set; } = TransportStatus.None;

        public void StartWork(ShopWorker worker)
        {
            if (Resource.CurrentJob != null)
            {
                throw new Exception($"Another Job is being performed by {Resource.CurrentJob.JobHistory.Last().ShopWorker.Name}. This Job must be Suspended for Completed before starting another Job.");
            }

            if (TransportStatus != TransportStatus.Here) throw new Exception($"Transport Status is {TransportStatus} and needs to be (Here)");

            JobHistory.Add(new JobHistory()
            {
                Job = this,
                StartTime = DateTime.Now,
            });

            JobStatus = JobStatus.Working;
            Resource.CurrentJob = this;
        }

        public void Suspend(int completed)
        {
            Complete(completed);

            JobStatus = JobStatus.Completed;
            // Need to create Transport Request

        }

        public void Complete(int completed)
        {
            if (completed > RequiredPieces) throw new ArgumentException("You cannot Complete more that what is required");

            CompletedPieces += completed;
            RequiredPieces -= completed;
            TransportPieces += completed;
            JobStatus = JobStatus.Suspended;
            TransportStatus = TransportStatus.Requested;

            var lastJobHistory = JobHistory.Last();
            lastJobHistory.CompletedPieces = completed;

            // Need to create Transport Request

        }

        public Job(Resource resource, WorkOrderItem workOrderItem, ShopScheduler shopScheduler, int requiredPieces)
        {
            Resource = resource;
            WorkOrderItem = workOrderItem;
            ShopScheduler = shopScheduler;
            RequiredPieces = requiredPieces;
        }

    }
}
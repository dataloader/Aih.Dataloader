using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aih.DataLoader.Tools.Models
{
    public class BatchStatus
    {
        public string BatchName { get; set; }
        public string BatchId { get; set; }
        public string BatchRefrence { get; set; }
        public string Status { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? StartLoadTime { get; set; }
        public DateTime? StartTransformTime { get; set; }
        public DateTime? StartSaveTime { get; set; }
        public DateTime? StartCleanupTime { get; set; }
        public DateTime? FinishTime { get; set; }
        public string Comment { get; set; }

        public override string ToString()
        {
            return BatchName + " " + BatchId + " " + Status;
        }

    }
}

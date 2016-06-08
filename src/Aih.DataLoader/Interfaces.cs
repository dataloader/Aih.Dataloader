using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aih.DataLoader.Models;

namespace Aih.DataLoader.Interfaces
{


    public interface ILoaderConfigHandler
    {
        Dictionary<string, string> GetLoaderConfig(string loaderContainerName, string loaderName);
    }


    public interface IStatusHandler
    {
        bool CreateBatchStatusRecord(BatchStatus status);
        bool UpdateBatchStatusRecord(BatchStatus status);

        bool BatchExists(string batchid);
        BatchStatus GetStatusRecord(string batchname, string batchid);
        IList<BatchStatus> GetUnhandledFailedBatches();
        bool SetBatchHandled(BatchStatus status);
    }


}

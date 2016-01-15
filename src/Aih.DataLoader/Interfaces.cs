using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aih.DataLoader.Tools.Models;

namespace Aih.DataLoader.Tools
{
    public interface IPropertyHandler
    {
        Dictionary<string, string> GetProperties(string batchname);
    }


    public interface IStatusHandler
    {
        bool CreateBatchStatusRecord(BatchStatus status);
        bool UpdateBatchStatusRecord(BatchStatus status);
        bool BatchExists(string batchid);
    }


}

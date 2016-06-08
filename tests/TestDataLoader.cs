using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Aih.DataLoader;
using Aih.DataLoader.Models;

namespace tests
{
    public class TestDataLoader : BaseDataLoader
    {
        public int CallCount { get; set; }
        public int ConfigCount { get; set; }

        public override string Initialize()
        {
            CallCount = 1;
            ConfigCount = _config.Count;

            string loadId = Guid.NewGuid().ToString();
            _cntx = new LoaderContext() { LoadDateTime = DateTime.Now, LoadId = loadId };
            return loadId;
        }

        public override void LoadData()
        {
            CallCount++;
        }

        public override void TransformData()
        {
            CallCount++;
        }

        public override void SaveData()
        {
            CallCount++;
        }

        public override void CleanUp()
        {
            CallCount++;
        }

    }
}

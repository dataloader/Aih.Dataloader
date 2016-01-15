using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aih.DataLoader.Tools.Exceptions
{
    public class DataLoaderException : Exception
    {
        private string _status;

        public DataLoaderException(string message, string status) : base(message)
        {
            _status = status;
        }

        public string Status
        {
            get
            {
                return _status;
            }
        }

    }
}

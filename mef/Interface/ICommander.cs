using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnzoCommanderSDK.mef.Interface
{
    public interface ICommander
    {
        void Init(string apiKey);
        Task<bool> Execute(string mapping, bool sendFile = false);
    }
}

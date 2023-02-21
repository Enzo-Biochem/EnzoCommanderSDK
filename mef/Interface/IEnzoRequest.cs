using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnzoCommanderSDK.mef.Interface
{
    public interface IEnzoRequest
    {
        MultipartFormDataContent MultipartFormFactory(string apiKey, string mapping, string fileName, bool sendFile);
    }
}

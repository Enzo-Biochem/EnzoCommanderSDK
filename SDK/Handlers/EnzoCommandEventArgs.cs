using EnzoCommanderSDK.SDK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnzoCommanderSDK.SDK.Handlers
{
    public class EnzoCommandEventArgs:EventArgs
    {
        public string Sender { get; set; } = "";
        public ErrorResponse? Error { get; set; } = null;

    }
}

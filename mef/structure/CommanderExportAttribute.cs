using EnzoCommanderSDK.mef.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnzoCommanderSDK.mef.structure
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class CommanderExportAttribute : ExportAttribute, IEnzoCommandData
    {
        public string Mapping { get; set; }
        public bool CanGet { get; set; }
        public bool CanSend { get; set; }
        public CommanderExportAttribute(string mapping, bool canGet = false, bool canSend = false)
        {
            Mapping = mapping;
            CanGet = canGet;
            CanSend = canSend;
        }
    }
}

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
    [AttributeUsage(AttributeTargets.Class,AllowMultiple =true)]
    public class CommanderExportAttribute:ExportAttribute,IEnzoCommandData
    {
           public string Mapping { get; set; }

        public CommanderExportAttribute(string mapping)
        {
            Mapping = mapping;
        }
    }
}

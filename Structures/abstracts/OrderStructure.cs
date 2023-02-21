using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnzoCommanderSDK.Structures.abstracts
{
    public abstract class OrderStructure
    {
        [Ignore]
        public string FileName { get; set; }

        [Ignore]
        public DateTime? FileStamp { get; set; }
        public OrderStructure(string fileName)
        {
            FileName = fileName;
        }
    }
}

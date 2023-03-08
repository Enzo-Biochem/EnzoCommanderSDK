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
        public string LoadFileName { get; set; }

        [Ignore]
        public DateTime? LoadFileDate { get; set; }
        [Name("LoadStatus")]
        [Optional]
        public int? LoadStatus { get; set; }
        public OrderStructure() { }

        public OrderStructure(string fileName)
        {
            LoadFileName = fileName;
        }

    }
}

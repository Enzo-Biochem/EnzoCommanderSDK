using EnzoCommanderSDK.Structures.abstracts;
using EnzoCommanderSDK;
using Name = CsvHelper.Configuration.Attributes.NameAttribute;

namespace EnzoCommanderSDK.Models
{
    public class OrderItem : OrderStructure
    {
        public OrderItem() { }
        public OrderItem(string fileName) : base(fileName)
        {

        }

        [Name("No_")]
        public string? No { get; set; }
        [Name("Order Number")]
        public string? OrderNumber { get; set; }
        [Name("Document No_")]
        public string? DocumentNo { get; set; }
        [Name("Quantity")]
        public int? Quantity { get; set; }
        [Name("Unit of Measurement")]
        public string? UnitOfMeasurement { get; set; }
        [Name("Unit Price")]
        public double? UnitPrice { get; set; }
    }
}

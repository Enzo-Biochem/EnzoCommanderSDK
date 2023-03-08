using EnzoCommanderSDK.Structures.abstracts;
using EnzoCommanderSDK;
using @Name = CsvHelper.Configuration.Attributes.NameAttribute;
using CsvHelper.Configuration.Attributes;

namespace EnzoCommanderSDK.Models
{
    public class OrderItem : OrderStructure
    {
        public OrderItem() { }
        public OrderItem(string fileName) : base(fileName)
        {

        }

        [@Name("No")]
        public string? No { get; set; }
        [@Name("WooCommerce Order ID")]
        public string? WooCommerceOrderId { get; set; }
        [@Name("Document No_")]
        public string? DocumentNo { get; set; }
        [@Name("Type")]
        public string? Type { get; set; }
        [@Name("Quantity")]
        public int? Quantity { get; set; }
        [@Name("Unit of Measurement")]
        public string? UnitOfMeasurement { get; set; }
        [@Name("Unit Price")]
        public double? UnitPrice { get; set; }
        [@Name("Line_Amount")]
        public string? LineAmount { get; set; }
        [@Name("Tax_Amount")]
        public string? TaxAmount { get; set; }

        [@Name(nameof(ItemStatus))]
        [Optional]
        public int? ItemStatus { get; set; }


        //[@Name("No_")]
        //public string? No { get; set; }
        //[@Name("Order Number")]
        //public string? OrderNumber { get; set; }
        //[@Name("Document No_")]
        //public string? DocumentNo { get; set; }
        //[@Name("Quantity")]
        //public int? Quantity { get; set; }
        //[@Name("Unit of Measurement")]
        //public string? UnitOfMeasurement { get; set; }
        //[@Name("Unit Price")]
        //public double? UnitPrice { get; set; }
    }
}

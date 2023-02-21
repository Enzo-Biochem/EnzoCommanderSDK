using EnzoCommanderSDK.mef.Attributes;

namespace EnzoCommanderSDK.Models
{
    //[Obsolete("Will not be implemented", true)]
    /// <summary>
    /// Used to create csv file being sent to our website.
    /// </summary>
    public class Order : OrderHeader
    {
        //Since this Model is going to be used to extrectly pass the 
        public Order() : base("") { }
        public Order(string fileName) : base(fileName) { }
        [IgnoreProperty]
        public List<OrderItem> Detail { get; set; } = new List<OrderItem>();

        public OrderHeader GetOrderHeader()
        {
            return (OrderHeader)this;
        }

    }
}

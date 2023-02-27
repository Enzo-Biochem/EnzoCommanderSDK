using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnzoCommanderSDK.Structures.interfaces
{
    public interface IOrderHeader
    {

        [Name("No_")]
        public string? No { get; set; }
        [Name("Order Number")]
        public string? OrderNumber { get; set; }
        [Name("Sell-to Customer No_")]
        public string? SellToCustomerNo { get; set; }
        [Name("Bill-to Customer No_")]
        public string? BillToCustomerNo { get; set; }
        [Name("Bill-to Name")]
        public string? BillToName { get; set; }
        [Name("Bill-to Address")]
        public string? BillToAddress { get; set; }
        [Name("Bill-to Address 2")]
        public string? BillToAddress2 { get; set; }
        [Name("Bill-to City")]
        public string? BillToCity { get; set; }
        [Name("Bill-to County")]
        public string? BillToCounty { get; set; }
        [Name("Bill-to Post Code")]
        public string? BillToPostCode { get; set; }
        [Name("Bill-to Country_Region Code")]
        public string? BillToCountryRegionCode { get; set; }
        [Name("Bill-to Phone No_")]
        public string? BillToPhoneNo { get; set; }
        [Name("Ship-to Name")]
        public string? ShipToName { get; set; }
        [Name("Ship-to Address")]
        public string? ShipToAddress { get; set; }
        [Name("Ship-to Address 2")]
        public string? ShipToAddress2 { get; set; }
        [Name("Ship-to City")]
        public string? ShipToCity { get; set; }
        [Name("Ship-to County")]
        public string? ShipToCounty { get; set; }
        [Name("Ship-to Post Code")]
        public string? ShipToPostCode { get; set; }
        [Name("Ship-to Country_Region Code")]
        public string? ShipToCountryRegionCode { get; set; }
        [Name("Order Date")]
        public DateTime? OrderDate { get; set; }
        [Name("Order Placed By Email")]
        public string? OrderPlacedByEmail { get; set; }
        [Name("enzo_user_id")]
        public string? EnzoUserId { get; set; }
    }
}

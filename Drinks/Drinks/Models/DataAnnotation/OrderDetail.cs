using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Drinks.Models
{
    public partial class OrderDetails
    {
        public string Orderer { get; set; }
        public int TotalByOrderer { get{
            return Orders.OrderDetails.Where(ood => ood.CreateId == CreateId).Sum(s => s.Price * s.Quantity);
        } }
    }
}
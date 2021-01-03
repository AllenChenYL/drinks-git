using Drinks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Drinks.ViewModels
{
    public class OrderVM : Order
    {
        public string Creater { get; set; }
        public bool IsOverTime { get{
            return DateTime.Now > EndDate;
        } }
        public bool HasDetail { get; set; }
    }
}
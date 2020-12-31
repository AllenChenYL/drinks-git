using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Drinks.ViewModels
{
    public class OrderDetailExportVM
    {
        [Display(Name = "訂購人")]
        public string Orderer { get; set; }
        
        [Display(Name = "飲料名稱")]
        public string Name { get; set; }
        
        [Display(Name = "大小")]
        public string Size { get; set; }
        
        [Display(Name = "甜度")]
        public string SugarLevel { get; set; }
        
        [Display(Name = "冰量")]
        public string IceLevel { get; set; }
        
        [Display(Name = "數量")]
        public int Quantity { get; set; }
        
        [Display(Name = "備註")]
        public string Memo { get; set; }
    }
}
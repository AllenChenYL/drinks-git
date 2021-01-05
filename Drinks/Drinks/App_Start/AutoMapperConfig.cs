using AutoMapper;
using Drinks.Models;
using Drinks.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Drinks.App_Start
{
    public class AutoMapperConfig
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg =>{
                     cfg.AddProfile<OrdersProfile>();
                     cfg.AddProfile<StoresProfile>();
                });
        }
    }

    public class OrdersProfile : Profile
    {
        protected override void Configure() 
        {
            Mapper.CreateMap<Orders, OrdersVM>();
            Mapper.CreateMap<OrdersVM, Orders>();
            Mapper.CreateMap<OrderDetails, OrderDetailsExportVM>();
        }
    }

    public class StoresProfile : Profile 
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Stores, StoresVM>();
            Mapper.CreateMap<StoresVM, Stores>();
        }
    }
}
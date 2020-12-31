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
                     cfg.AddProfile<OrderProfile>();
                     cfg.AddProfile<StoreProfile>();
                });
        }
    }

    public class OrderProfile : Profile
    {
        protected override void Configure() 
        {
            Mapper.CreateMap<Order, OrderVM>();
            Mapper.CreateMap<OrderVM, Order>();
            Mapper.CreateMap<OrderDetail, OrderDetailExportVM>();
        }
    }

    public class StoreProfile : Profile 
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Store, StoreVM>();
            Mapper.CreateMap<StoreVM, Store>();
        }
    }
}
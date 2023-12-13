using AutoMapper;
using System;
using DAL.Models;


namespace Shoe.Controllers
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<DAL.Models.Shoe, Models.Shoe>();
            CreateMap<Models.Shoe, DAL.Models.Shoe>();
            CreateMap<DAL.Models.Brand, Models.Brand>();
            CreateMap<Models.Brand, DAL.Models.Brand>();
            CreateMap<DAL.Models.Feature, Models.Feature>();
            CreateMap<Models.Feature, DAL.Models.Feature>();
            CreateMap<DAL.Models.Style, Models.Style>();
            CreateMap<Models.Style, DAL.Models.Style>();
            CreateMap<DAL.Models.ShoesVariant, Models.ShoesVariant>();
            CreateMap<Models.ShoesVariant, DAL.Models.ShoesVariant>();
            CreateMap<DAL.Models.Color, Models.Color>();
            CreateMap<Models.Color, DAL.Models.Color>();
            CreateMap<DAL.Models.Size, Models.Size>();
            CreateMap<Models.Size, DAL.Models.Size>();
        }
        
    }
}

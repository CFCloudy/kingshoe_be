using AutoMapper;
using System;
using DAL.Models;
using DTO.models;

namespace DTO.Utils
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<DAL.Models.Shoe, DTO.models.ShoeDTO>();
            CreateMap<DTO.models.ShoeDTO, DAL.Models.Shoe>();
            CreateMap<DAL.Models.Brand, DTO.models.BrandDTO>();
            CreateMap<Brand, CreateBrandDTO>();
            CreateMap<CreateBrandDTO, Brand>();
            CreateMap<Brand, CreateFeatureDTO>();
            CreateMap<CreateFeatureDTO, Brand>();
            CreateMap<Brand, CreateSizeDTO>();
            CreateMap<CreateSizeDTO, Brand>();
            CreateMap<Brand, CreateStyleDTO>();
            CreateMap<CreateStyleDTO, Brand>();
            CreateMap<Brand, CreateColorDTO>();
            CreateMap<CreateColorDTO, Brand>();
            CreateMap<DTO.models.BrandDTO, DAL.Models.Brand>();
            CreateMap<DAL.Models.Feature, DTO.models.FeatureDTO>();
            CreateMap<DTO.models.FeatureDTO, DAL.Models.Feature>();
            CreateMap<DAL.Models.Style, DTO.models.StyleDTO>();
            CreateMap<DTO.models.StyleDTO, DAL.Models.Style>();
            CreateMap<DAL.Models.ShoesVariant, DTO.models.ShoesVariantDTO>();
            CreateMap<DTO.models.ShoesVariantDTO, DAL.Models.ShoesVariant>();
            CreateMap<DAL.Models.Color, DTO.models.ColorDTO>();
            CreateMap<DTO.models.ColorDTO, DAL.Models.Color>();
            CreateMap<DAL.Models.Size, DTO.models.SizeDTO>();
            CreateMap<DTO.models.SizeDTO, DAL.Models.Size>();
        }

    }
}

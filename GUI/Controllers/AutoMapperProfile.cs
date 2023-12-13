using AutoMapper;
using System;
using DAL.Models;


namespace Shoe.Controllers
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<DAL.Models.Shoe, GUI.Models.Shoe>();
            CreateMap<GUI.Models.Shoe, DAL.Models.Shoe>();
            CreateMap<DAL.Models.Brand, GUI.Models.Brand>();
            CreateMap<GUI.Models.Brand, DAL.Models.Brand>();
            CreateMap<DAL.Models.Feature, GUI.Models.Feature>();
            CreateMap<GUI.Models.Feature, DAL.Models.Feature>();
            CreateMap<DAL.Models.Style, GUI.Models.Style>();
            CreateMap<GUI.Models.Style, DAL.Models.Style>();
            CreateMap<DAL.Models.ShoesVariant, GUI.Models.ShoesVariant>();
            CreateMap<GUI.Models.ShoesVariant, DAL.Models.ShoesVariant>();
            CreateMap<DAL.Models.Color, GUI.Models.Color>();
            CreateMap<GUI.Models.Color, DAL.Models.Color>();
            CreateMap<DAL.Models.Size, GUI.Models.Size>();
            CreateMap<GUI.Models.Size, DAL.Models.Size>(); 
            CreateMap<Voucher, GUI.Models.Voucher>();
            CreateMap<GUI.Models.Voucher, Voucher>();
            CreateMap<VoucherBank, GUI.Models.VoucherBank>();
            CreateMap<GUI.Models.VoucherBank, VoucherBank>();
            CreateMap<VouchersUseLog, GUI.Models.VouchersUseLog>();
            CreateMap<GUI.Models.VouchersUseLog, VouchersUseLog>();
        }
        
    }
}

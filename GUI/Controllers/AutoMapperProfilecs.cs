using AutoMapper;
using System;
using DAL.Models;

namespace Shoe.Controllers
{
    public class AutoMapperProfilecs: Profile
    {
        public AutoMapperProfilecs()
        {
            CreateMap<Voucher, GUI.Models.Voucher>();
            CreateMap<GUI.Models.Voucher, Voucher>();
            CreateMap<VoucherBank, GUI.Models.VoucherBank>();
            CreateMap<GUI.Models.VoucherBank, VoucherBank>();
            CreateMap<VouchersUseLog, GUI.Models.VouchersUseLog>();
            CreateMap<GUI.Models.VouchersUseLog, VouchersUseLog>();
        }
    }
}

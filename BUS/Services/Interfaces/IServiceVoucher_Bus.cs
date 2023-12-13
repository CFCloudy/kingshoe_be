using BUS.IServices;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS.IService
{
    public interface IServiceVoucher_Bus: IServicesBus<Voucher>
    {
        public Task<List<Voucher>> GetAllVoucher();
    }
}

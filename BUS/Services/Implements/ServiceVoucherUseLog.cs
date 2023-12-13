
using BUS.IService;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS.Service
{
    internal class ServiceVoucherUseLog : IServiceVoucher_Bus
    {
        public ServiceVoucherUseLog(ShoeStoreContext shoeStoreContext)
        {
        }

        public Task<bool> Delete(Voucher entity)
        {
            throw new NotImplementedException();
        }

        public Task<List<Voucher>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<List<Voucher>> GetAllVoucher()
        {
            throw new NotImplementedException();
        }

        public Task<Voucher> Getone(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Insert(Voucher entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(Voucher entity)
        {
            throw new NotImplementedException();
        }
    }
}

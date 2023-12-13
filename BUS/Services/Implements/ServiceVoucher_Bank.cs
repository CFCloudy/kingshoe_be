using BUS.IService;
using DAL.Models;
using DAL.Repositories.Implements;
using DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS.Service
{
    public class ServiceVoucher_Bank : IServiceVoucherBank
    {
        private IRepository_VouchersBank _repositoryVoucherBank;
        private ShoeStoreContext _shoeStore;
        public ServiceVoucher_Bank()
        {
            _repositoryVoucherBank = new RepositoryVoucherBank(new ShoeStoreContext()); ;
            _shoeStore = new ShoeStoreContext(); ;
        }

        public async Task<bool> Delete(VoucherBank entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<VoucherBank>> GetAll()
        {
            return _repositoryVoucherBank.GetAll().Where(c => c.Status.Value != false).ToList();
        }

        public async Task<VoucherBank> Getone(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Insert(VoucherBank entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(VoucherBank entity)
        {
            throw new NotImplementedException();
        }
    }
}

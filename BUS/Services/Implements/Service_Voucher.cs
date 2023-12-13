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
    public class Service_Voucher : IServiceVoucher_Bus
    {
        private IRepositoryVouchers repositoryVouchers;
        private ShoeStoreContext shoeStore;
        public Service_Voucher()
        {
            repositoryVouchers = new RepositoryVoucher(new ShoeStoreContext());
            shoeStore = new ShoeStoreContext();
        }

        public async Task<bool> Delete(Voucher entity)
        {
            var voucher = await Getone(entity.Id);
            if (voucher == null)
                return false;
            voucher.Status = 1;
            shoeStore.Vouchers.Update(voucher);
            shoeStore.SaveChanges();
            return true;
        }

        public async Task<List<Voucher>> GetAllVoucher()
        {
            return  repositoryVouchers.GetAll().Where(c => c.Status.Value != 1).OrderByDescending(c => c.Id).ToList();
        }

        public Task<List<Voucher>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<Voucher> Getone(int id)
        {
            var voucher = repositoryVouchers.GetVoucher(id);
            if (voucher == null)
                return null;
            return voucher.Status == 1 ? null:voucher;
        }

        public async Task<bool> Insert(Voucher entity)
        {
            var voucher = await GetAllVoucher();
            entity.Id = voucher.Any() ? voucher.Max(c => c.Id) + 1 : 0;
            entity.Status = 0;
            entity.CreatedAt = DateTime.Now;
            Voucher voucher1 = new Voucher()
            {
                CreatedAt = DateTime.Now,
                DiscountMaxValue = entity.DiscountMaxValue,
                DiscountType = entity.DiscountType,
                DiscountValue = entity.DiscountValue,
                EndDate = entity.EndDate,
                //LimitQuantity = entity.LimitQuantity,
                ModifiedAt = DateTime.Now,
                RequriedValue = entity.RequriedValue,
                StartDate = entity.StartDate,
                Status = entity.Status,
                VoucherCode = entity.VoucherCode,
                VoucherContent = entity.VoucherContent,

            };
            if (string.IsNullOrEmpty(entity.VoucherCode))
                return false;
            if (shoeStore.Vouchers.FirstOrDefault(x => x.VoucherCode == entity.VoucherCode) != null)
                return false;
            shoeStore.Vouchers.Add(voucher1);
            shoeStore.SaveChanges();
            return true;
        }

        public async Task<bool> Update(Voucher entity)
        {
            var vouchers = Getone(entity.Id);
            var voucher1 = repositoryVouchers.GetVoucher(entity.Id);
            //voucher1.CreatedAt = DateTime.Now;
            voucher1.DiscountMaxValue = entity.DiscountMaxValue;
            voucher1.DiscountType = entity.DiscountType;
            voucher1.DiscountValue = entity.DiscountValue;
            voucher1.EndDate = DateTime.Now;
            //voucher1.LimitQuantity = entity.LimitQuantity;
            voucher1.ModifiedAt = DateTime.Now;
            voucher1.RequriedValue = entity.RequriedValue;
            voucher1.StartDate = DateTime.Now;
            voucher1.Status = entity.Status;
            voucher1.VoucherCode = entity.VoucherCode;
            voucher1.VoucherContent = entity.VoucherContent;
            if (string.IsNullOrEmpty(entity.VoucherContent))
                return false;
            shoeStore.Vouchers.Update(voucher1);
            shoeStore.SaveChanges();
            return true;
        }
        //public Tuple<List<Voucher>, int> GetByRequest(string searchKey = "", int currentPage = 1, int rowPerPage = 20)
        //{
        //    currentPage--;
        //    var skip = currentPage * rowPerPage;
        //    return repositoryVouchers.GetListOrderByRequest(searchKey, skip, rowPerPage);
        //}
    }
}

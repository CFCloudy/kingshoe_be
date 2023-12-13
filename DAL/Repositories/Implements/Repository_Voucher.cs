using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Implements
{
    public class RepositoryVoucher : Repositories_dal<Voucher>, IRepositoryVouchers
    {
        public RepositoryVoucher(ShoeStoreContext _context) : base(_context)
        {

        }
        public override bool Delete(Voucher voucher)
        {
            var result = 0;
            try
            {
                var model = _DbSet.FirstOrDefault(c => c.Id == voucher.Id);
                if (model == null) return false;
                voucher.Status = 1;
                _DbSet.Update(voucher);
                result = _Context.SaveChanges();
            }
            catch (Exception dbEx)
            {
            }
            return result > 0;
        }

        public Voucher GetVoucher(int id)
        {
            return _DbSet.FirstOrDefault(c => c.Id == id);
        }
        //public Tuple<List<Voucher>, int> GetListOrderByRequest(string key, int skip, int take)
        //{
        //    try
        //    {
        //        var data = _DbSet.AsNoTracking().Where(c => String.IsNullOrEmpty(key) || c.VoucherCode == key || (c.VoucherContent ?? "").ToLower().Contains(key.ToLower()));
        //        var total = data.Count();
        //        var result = data.Skip(skip).Take(take).ToList();//skip bỏ qua bn --- take lấy bao nhiêu
        //        return Tuple.Create(result, total);
        //    }
        //    catch (Exception)
        //    {
        //        return new Tuple<List<Voucher>, int>(new List<Voucher>(), 0);
        //    }
        //}
    }
}

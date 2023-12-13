using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces
{
    public interface IRepositoryVouchers: Repositories_dal<Voucher>
    {
        Voucher GetVoucher(int id);
        //public Tuple<List<Voucher>, int> GetListOrderByRequest(string key, int skip, int take);
    }
}

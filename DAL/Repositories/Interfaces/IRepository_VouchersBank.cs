using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces
{
    public interface IRepository_VouchersBank: Repositories_dal<VoucherBank>
    {
        VoucherBank GetVoucherBank(int voucherId); 
    }
}

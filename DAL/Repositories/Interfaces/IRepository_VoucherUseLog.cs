using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces
{
    public interface IRepository_VoucherUseLog: Repositories_dal<VouchersUseLog>
    {
        VouchersUseLog GetVouchersUseLog(int id);
    }
}

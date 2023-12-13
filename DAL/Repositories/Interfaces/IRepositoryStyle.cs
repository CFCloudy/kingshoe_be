using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces
{
    public interface IRepositoryStyle : IRepository<Style>
    {
        Style GetStyle(int id);
    }
}

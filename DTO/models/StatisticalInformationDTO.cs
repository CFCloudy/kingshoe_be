using AutoMapper.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.models
{
    public class StatisticalInformationDTO
    {
        public decimal TotalAmountSold { get; set; } = 0;
        public int TotalNumberOfProductsSold { get; set; } = 0;
        public int TotalNumberOfCustomers { get; set; } = 0;
    }
}

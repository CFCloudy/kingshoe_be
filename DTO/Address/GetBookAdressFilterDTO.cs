using DTO.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Address
{
    public class GetBookAdressFilterDTO : PagedAndSortedResultRequestDto, IShouldNormalize
    {
        public int UserId { get; set; }

        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "item.CreationTime";
            }
        }
    }
}

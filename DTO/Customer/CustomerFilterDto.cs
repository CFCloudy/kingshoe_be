using DTO.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Customer
{
    public class CustomerFilterDto : PagedAndSortedResultRequestDto, IShouldNormalize
    {
        public string? TenKhachHang { get; set; }


        
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "item.CreatedAt";
            }
        }
    }

    public class UpdateProfiles { 
    
        public int? Image {get;set; }
        public bool Gender { get; set; }

        public string? PhoneNumber { get; set; }

        public string? FullName { get; set; }

        public int Id {get; set; }  
    }

}

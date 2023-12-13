using DTO.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.models
{
    public class FilterShoeDTO : PagedAndSortedResultRequestDto
    {
        public List<BrandDTO>? BrandDTOs {get; set;}
        public List<FeatureDTO>? FeatureDTOs {get; set;}
        public List<StyleDTO>? StyleDTOs {get; set;}
        public List<SizeDTO>? SizeDTOs {get; set;}
        public List<ColorDTO>? ColorDTOs {get; set;}
        public string? NameShoe {get; set;}
        public bool? IsDecrease { get; set; }
        public bool? IsAscending { get; set; }

        public bool? Status { get; set; } 

    }
}

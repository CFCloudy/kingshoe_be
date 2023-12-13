using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.models
{
    public class CreateFeatureDTO
    {
        public string FeatureName { get; set; } = string.Empty;
        public int? DisplayOrder { get; set; }
    }
}

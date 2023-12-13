using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.models
{
    public class FeatureDTO
    {
        public FeatureDTO()
        {

        }
        public int Id { get; set; }
        public string FeatureName { get; set; }
        public string Type { get; set; }
    }
}

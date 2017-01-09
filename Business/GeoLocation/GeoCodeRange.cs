using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace loconotes.Business.GeoLocation
{
    public class GeoCodeRange
    {
        public double MinimumLatitude { get; set; }
        public double MaximumLatitude { get; set; }
        public double MinimumLongitude { get; set; }
        public double MaximumLongitude { get; set; }
    }
}

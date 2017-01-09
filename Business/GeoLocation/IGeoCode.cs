using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace loconotes.Business.GeoLocation
{
    public interface IGeoCode
    {
        double? LatitudeGeoCode { get; }
        double? LongitudeGeoCode { get; }
    }
}

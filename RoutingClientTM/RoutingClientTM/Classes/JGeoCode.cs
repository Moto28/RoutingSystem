using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutingClientTM
{
    public class Address
    {
        public string adminDistrict2 { get; set; }
        public string formattedAddress { get; set; }
        public string countryRegion { get; set; }
        public string postalCode { get; set; }
        public string locality { get; set; }
        public string adminDistrict { get; set; }
    }

    public class GeocodePoint
    {
        public string calculationMethod { get; set; }
        public List<double> coordinates { get; set; }
        public string type { get; set; }
        public List<string> usageTypes { get; set; }
    }

    public class PointR
    {
        public List<double> coordinates { get; set; }
        public string type { get; set; }
    }

    public class Resource
    {
        public List<string> matchCodes { get; set; }
        public Address address { get; set; }
        public string entityType { get; set; }
        public string __type { get; set; }
        public List<double> bbox { get; set; }
        public string confidence { get; set; }
        public List<GeocodePoint> geocodePoints { get; set; }
        public string name { get; set; }
        public Point point { get; set; }
    }

    public class ResourceSet
    {
        public List<Resource> resources { get; set; }
        public int estimatedTotal { get; set; }
    }

    public class JGeoCode
    {
        public string traceId { get; set; }
        public string copyright { get; set; }
        public string statusDescription { get; set; }
        public string brandLogoUri { get; set; }
        public List<ResourceSet> resourceSets { get; set; }
        public string authenticationResultCode { get; set; }
        public int statusCode { get; set; }
    }
}

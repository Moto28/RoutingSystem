using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace RoutingClientTM
{
    public class MetaInfo
    {

        [JsonProperty("Timestamp")]
        public DateTime Timestamp { get; set; }
    }

    public class MatchQuality
    {

        [JsonProperty("State")]
        public double State { get; set; }

        [JsonProperty("City")]
        public double City { get; set; }

        [JsonProperty("Street")]
        public IList<double> Street { get; set; }

        [JsonProperty("HouseNumber")]
        public int HouseNumber { get; set; }
    }

    public class DisplayPosition
    {

        [JsonProperty("Latitude")]
        public double Latitude { get; set; }

        [JsonProperty("Longitude")]
        public double Longitude { get; set; }
    }

    public class NavigationPosition
    {

        [JsonProperty("Latitude")]
        public double Latitude { get; set; }

        [JsonProperty("Longitude")]
        public double Longitude { get; set; }
    }

    public class TopLeft
    {

        [JsonProperty("Latitude")]
        public double Latitude { get; set; }

        [JsonProperty("Longitude")]
        public double Longitude { get; set; }
    }

    public class BottomRight
    {

        [JsonProperty("Latitude")]
        public double Latitude { get; set; }

        [JsonProperty("Longitude")]
        public double Longitude { get; set; }
    }

    public class MapView
    {

        [JsonProperty("TopLeft")]
        public TopLeft TopLeft { get; set; }

        [JsonProperty("BottomRight")]
        public BottomRight BottomRight { get; set; }
    }

    public class AdditionalData
    {

        [JsonProperty("value")]
        public string value { get; set; }

        [JsonProperty("key")]
        public string key { get; set; }
    }

    public class Address
    {

        [JsonProperty("Label")]
        public string Label { get; set; }

        [JsonProperty("Country")]
        public string Country { get; set; }

        [JsonProperty("State")]
        public string State { get; set; }

        [JsonProperty("County")]
        public string County { get; set; }

        [JsonProperty("City")]
        public string City { get; set; }

        [JsonProperty("District")]
        public string District { get; set; }

        [JsonProperty("Street")]
        public string Street { get; set; }

        [JsonProperty("HouseNumber")]
        public string HouseNumber { get; set; }

        [JsonProperty("PostalCode")]
        public string PostalCode { get; set; }

        [JsonProperty("AdditionalData")]
        public IList<AdditionalData> AdditionalData { get; set; }
    }

    public class Location
    {

        [JsonProperty("LocationId")]
        public string LocationId { get; set; }

        [JsonProperty("LocationType")]
        public string LocationType { get; set; }

        [JsonProperty("DisplayPosition")]
        public DisplayPosition DisplayPosition { get; set; }

        [JsonProperty("NavigationPosition")]
        public IList<NavigationPosition> NavigationPosition { get; set; }

        [JsonProperty("MapView")]
        public MapView MapView { get; set; }

        [JsonProperty("Address")]
        public Address Address { get; set; }
    }

    public class Result
    {

        [JsonProperty("Relevance")]
        public double Relevance { get; set; }

        [JsonProperty("MatchLevel")]
        public string MatchLevel { get; set; }

        [JsonProperty("MatchQuality")]
        public MatchQuality MatchQuality { get; set; }

        [JsonProperty("MatchType")]
        public string MatchType { get; set; }

        [JsonProperty("Location")]
        public Location Location { get; set; }
    }

    public class View
    {

        [JsonProperty("_type")]
        public string _type { get; set; }

        [JsonProperty("ViewId")]
        public double ViewId { get; set; }

        [JsonProperty("Result")]
        public IList<Result> Result { get; set; }
    }

    public class Response
    {

        [JsonProperty("MetaInfo")]
        public MetaInfo MetaInfo { get; set; }

        [JsonProperty("View")]
        public IList<View> View { get; set; }
    }

    public class GeoCode
    {

        [JsonProperty("Response")]
        public Response Response { get; set; }
    }

}



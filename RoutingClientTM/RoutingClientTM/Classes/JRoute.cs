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

    public class JRoute
    {
        public string authenticationResultCode { get; set; }
        public string brandLogoUri { get; set; }
        public string copyright { get; set; }
        public ResourcesetR[] resourceSets { get; set; }
        public int statusCode { get; set; }
        public string statusDescription { get; set; }
        public string traceId { get; set; }
    }

    public class ResourcesetR
    {
        public int estimatedTotal { get; set; }
        public ResourceJ[] resources { get; set; }
    }

    public class ResourceJ
    {
        public string __type { get; set; }
        public float[] bbox { get; set; }
        public string distanceUnit { get; set; }
        public string durationUnit { get; set; }
        public Routelegs[] routeLegs { get; set; }
        public string trafficCongestion { get; set; }
        public string trafficDataUsed { get; set; }
        public float travelDistance { get; set; }
        public int travelDuration { get; set; }
        public int travelDurationTraffic { get; set; }
    }

    public class Routelegs
    {
        public Actualend actualEnd { get; set; }
        public Actualstart actualStart { get; set; }
        public object[] alternateVias { get; set; }
        public int cost { get; set; }
        public string description { get; set; }
        public Itineraryitem[] itineraryItems { get; set; }
        public Routesubleg[] routeSubLegs { get; set; }
        public float travelDistance { get; set; }
        public int travelDuration { get; set; }
    }

    public class Actualends
    {
        public string type { get; set; }
        public float[] coordinates { get; set; }
    }

    public class Actualstarts
    {
        public string type { get; set; }
        public float[] coordinates { get; set; }
    }

    public class Itineraryitems
    {
        public string compassDirection { get; set; }
        public Detail[] details { get; set; }
        public string iconType { get; set; }
        public Instruction instruction { get; set; }
        public bool isRealTimeTransit { get; set; }
        public Maneuverpoint maneuverPoint { get; set; }
        public int realTimeTransitDelay { get; set; }
        public string sideOfStreet { get; set; }
        public string towardsRoadName { get; set; }
        public float travelDistance { get; set; }
        public int travelDuration { get; set; }
        public string travelMode { get; set; }
        public string exit { get; set; }
        public string[] signs { get; set; }
        public Hint[] hints { get; set; }
    }

    public class Instructions
    {
        public string formattedText { get; set; }
        public string maneuverType { get; set; }
        public string text { get; set; }
    }

    public class Maneuverpoints
    {
        public string type { get; set; }
        public float[] coordinates { get; set; }
    }

    public class Details
    {
        public int compassDegrees { get; set; }
        public int[] endPathIndices { get; set; }
        public string maneuverType { get; set; }
        public string mode { get; set; }
        public string[] names { get; set; }
        public string roadType { get; set; }
        public int[] startPathIndices { get; set; }
        public string[] locationCodes { get; set; }
        public Roadshieldrequestparameters roadShieldRequestParameters { get; set; }
    }

    public class m
    {
        public int bucket { get; set; }
        public Shields[] shields { get; set; }
    }

    public class Shields
    {
        public string[] labels { get; set; }
        public int roadShieldType { get; set; }
    }

    public class Hints
    {
        public string hintType { get; set; }
        public string text { get; set; }
    }

    public class Routesublegs
    {
        public Endwaypoint endWaypoint { get; set; }
        public Startwaypoint startWaypoint { get; set; }
        public float travelDistance { get; set; }
        public int travelDuration { get; set; }
    }

    public class Endwaypoints
    {
        public string type { get; set; }
        public float[] coordinates { get; set; }
        public string description { get; set; }
        public bool isVia { get; set; }
        public string locationIdentifier { get; set; }
        public int routePathIndex { get; set; }
    }

    public class Startwaypoints
    {
        public string type { get; set; }
        public float[] coordinates { get; set; }
        public string description { get; set; }
        public bool isVia { get; set; }
        public string locationIdentifier { get; set; }
        public int routePathIndex { get; set; }
    }

}





package com.route.client;



    public class RawRoute
    {
        public String authenticationResultCode;
        public String brandLogoUri;
        public String copyright;
        public Resourceset[] resourceSets;
        public int statusCode;
        public String statusDescription;
        public String traceId;
    }

    class Resourceset
    {
        public int estimatedTotal;
        public ResourceMJ[] resources;
    }

    class ResourceMJ
    {
        public String __type;
        public float[] bbox;
        public String id;
        public String distanceUnit;
        public String durationUnit;
        public Routeleg[] routeLegs;
        public Routepath routePath;
        public float travelDistance;
        public int travelDuration;
        public int travelDurationTraffic;
    }

     class Routepath
    {
        public Object[] generalizations;
        public Line line;
    }

     class Line {
         public String type;
         public float[][] coordinates;
     }

     class Routeleg
    {
        public Actualend actualEnd;
        public Actualstart actualStart;
        public Object[] alternateVias;
        public int cost;
        public String description;
        public Endlocation endLocation;
        public Itineraryitem[] itineraryItems;
        public String routeRegion;
        public Routesubleg[] routeSubLegs;
        public Startlocation startLocation;
        public float travelDistance;
        public int travelDuration;
    }

     class Actualend
    {
        public String type;
        public float[] coordinates;
    }

     class Actualstart
    {
        public String type;
        public float[] coordinates;
    }

     class Endlocation
    {
        public float[] bbox;
        public String name;
        public Point point;
        public AddressMJ address;
        public String confidence;
        public String entityType;
        public Geocodepoint[] geocodePoints;
        public String[] matchCodes;
    }

     class Point
    {
        public String type;
        public float[] coordinates;
    }

     class AddressMJ
    {
        public String adminDistrict;
        public String adminDistrict2;
        public String countryRegion;
        public String formattedAddress;
        public String locality;
    }

     class Geocodepoint
    {
        public String type;
        public float[] coordinates;
        public String calculationMethod;
        public String[] usageTypes;
    }

     class Startlocation
    {
        public float[] bbox;
        public String name;
        public Point1 point;
        public Address1 address;
        public String confidence;
        public String entityType;
        public Geocodepoint1[] geocodePoints;
        public String[] matchCodes;
    }

     class Point1
    {
        public String type;
        public float[] coordinates;
    }

     class Address1
    {
        public String adminDistrict;
        public String adminDistrict2;
        public String countryRegion;
        public String formattedAddress;
        public String locality;
    }

     class Geocodepoint1
    {
        public String type;
        public float[] coordinates;
        public String calculationMethod;
        public String[] usageTypes;
    }

     class Itineraryitem
    {
        public String compassDirection;
        public Detail[] details;
        public String exit;
        public String iconType;
        public Instruction instruction;
        public Maneuverpoint maneuverPoint;
        public String sideOfStreet;
        public String tollZone;
        public String towardsRoadName;
        public String transitTerminus;
        public float travelDistance;
        public int travelDuration;
        public String travelMode;
        public String[] signs;
        public Warning[] warnings;
        public Hint[] hints;
    }

     class Instruction
    {
        public Object formattedText;
        public String maneuverType;
        public String text;
    }

     class Maneuverpoint
    {
        public String type;
        public float[] coordinates;
    }

     class Detail
    {
        public int compassDegrees;
        public int[] endPathIndices;
        public String[] locationCodes;
        public String maneuverType;
        public String mode;
        public String[] names;
        public String roadType;
        public int[] startPathIndices;
        public Roadshieldrequestparameters roadShieldRequestParameters;
    }

     class Roadshieldrequestparameters
    {
        public int bucket;
        public Shield[] shields;
    }

     class Shield
    {
        public String[] labels;
        public int roadShieldType;
    }

     class Warning
    {
        public String origin;
        public String severity;
        public String text;
        public String to;
        public String warningType;
    }

     class Hint
    {
        public Object hintType;
        public String text;
    }

     class Routesubleg
    {
        public Endwaypoint endWaypoint;
        public Startwaypoint startWaypoint;
        public float travelDistance;
        public int travelDuration;
    }

     class Endwaypoint
    {
        public String type;
        public float[] coordinates;
        public String description;
        public boolean isVia;
        public String locationIdentifier;
        public int routePathIndex;
    }

     class Startwaypoint
    {
        public String type;
        public float[] coordinates;
        public String description;
        public boolean isVia;
        public String locationIdentifier;
        public int routePathIndex;
    }



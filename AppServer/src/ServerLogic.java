
import org.json.simple.JSONArray;
import org.json.simple.JSONObject;
import org.json.simple.parser.JSONParser;

import javax.ws.rs.*;
import javax.ws.rs.core.MediaType;
import java.io.Serializable;
import java.rmi.Remote;
import java.rmi.RemoteException;
import java.rmi.registry.LocateRegistry;
import java.rmi.registry.Registry;
import java.util.ArrayList;


// The Java class will be hosted at the URI path "/logic"
@Path("/logic")
public class ServerLogic implements IDataLayer, Serializable, Remote {

    IDataLayer stub;

    boolean isLoggedIn = false;
    JSONArray conJsonArray = new JSONArray();
    boolean result = false;
    String result1;
    ArrayList<String> routes = new ArrayList<String>();


    public ServerLogic() {
        try{
            Registry registry = LocateRegistry.getRegistry(1098);
            // Looking up the registry for the remote object
             stub = (IDataLayer) registry.lookup("record2");

        }
        catch (Exception ex)
        {
            System.out.println(ex.toString());
        }
    }

    // The Java method will process HTTP GET requests
    @POST
    // The Java method will produce content identified by the MIME Media type "text/plain"
    @Path("login")
    @Produces(MediaType.TEXT_PLAIN)
    public boolean UserSignIn(@FormParam("userid") String UserId,
                             @FormParam("password") String Password) throws RemoteException {
        try
        {
            isLoggedIn = stub.UserSignIn(UserId,Password);
            return isLoggedIn;
        }
        catch (Exception ex)
        {
            System.out.println(ex.toString());
            return isLoggedIn;
        }
    }

    @GET
    @Produces(MediaType.TEXT_PLAIN)
    @Path("add/user/{usertype}/{fname}/{sname}/{drivertype}/{licensetype}/{cpctype}/{email}")
    public boolean AddUser(@PathParam("usertype")Integer userType,
                          @PathParam("fname")String fName,
                          @PathParam("sname")String sName,
                          @PathParam("drivertype")Integer driverType,
                          @PathParam("licensetype")Integer licenseType,
                          @PathParam("cpctype")Integer cpcType,
                          @PathParam("email") String email) throws RemoteException {

        //calls the remote method on dataserver
        try{
            result = stub.AddUser(userType,fName,sName,driverType,licenseType,cpcType,email);
            return result;
        }
        catch (Exception ex)
        {
            System.out.println(ex.toString());
            return result;
        }
    }

    @GET
    @Produces(MediaType.APPLICATION_JSON)
    @Path("allusers")
    public String GetAllUsers() throws RemoteException {

        return stub.GetAllUsers();
    }

    @GET
    @Produces(MediaType.APPLICATION_JSON)
    @Path("user/{id}")
    public String GetUser(@PathParam("id")String userId) throws RemoteException{
         return stub.GetUser(userId);
    }

    @GET
    @Produces(MediaType.APPLICATION_JSON)
    @Path("allbuses")
    public String GetAllBuses() throws RemoteException {

        return stub.AllBuses();
    }

    @GET
    @Produces(MediaType.TEXT_PLAIN)
    @Path("edit/user/{userid}/{usertype}/{fname}/{sname}/{drivertype}/{licensetype}/{cpctype}/{email}")
    public boolean EditUsers(@PathParam("usertype")Integer userType,
                             @PathParam("userid") String userid,
                             @PathParam("fname")String fName,
                             @PathParam("sname")String sName,
                             @PathParam("drivertype")Integer driverType,
                             @PathParam("licensetype")Integer licenseType,
                             @PathParam("cpctype")Integer cpcType,
                             @PathParam("email") String email) throws RemoteException {
        //calls the remote method on dataserver
        try {
            result = stub.EditUsers(userType, userid, fName, sName, driverType, licenseType, cpcType, email);
            return result;
        } catch (Exception ex) {
            System.out.println(ex.toString());
            return result;
        }

    }

    @GET
    @Produces(MediaType.TEXT_PLAIN)
    @Path("delete/user{userid}")
    public boolean DeleteUsers(@PathParam("userid") String userid) throws RemoteException {

        //calls the remote method on dataserver
        try{
            result = stub.DeleteUsers(userid);
            return result;
        }
        catch (Exception ex)
        {
            System.out.println(ex.toString());
            return result;
        }
    }

    @GET
    @Produces(MediaType.APPLICATION_JSON)
    @Path("allbuses/convert")
    public String AllBuses() throws RemoteException {

        String response = stub.AllBuses();

        try {
            JSONParser parser = new JSONParser();
            Object jsonObj = parser.parse(response);
            JSONArray jsonArray = (JSONArray) jsonObj;

            for (Object json : jsonArray) {

                JSONObject obj = (JSONObject) json;


                String bType = String.valueOf(obj.get("bus_type"));
                String tType = String.valueOf(obj.get("tacho"));
                String reg = String.valueOf(obj.get("bus_reg"));

                obj.put("bus_reg",reg.toUpperCase());

                if(bType.contains("0"))
                {
                    obj.put("bus_type", "Single Deck");
                }
                else if(bType.contains("1"))
                {
                    obj.put("bus_type", "Double Deck");
                }
                else if(bType.contains("2"))
                {
                    obj.put("bus_type", "Mini bus");
                }
                else if(bType.contains("3"))
                {
                    obj.put("bus_type", "Coach");
                }
                if(tType.contains("0"))
                {
                    obj.put("tacho", "Digital");
                }
                else if(tType.contains("1"))
                {
                    obj.put("tacho", "Analogue");
                }
                conJsonArray.add(obj);
            }
        }
        catch(Exception ex)
        {
            System.out.println(ex.toString());
        }
        return  conJsonArray.toString();
    }

    @GET
    @Produces(MediaType.APPLICATION_JSON)
    @Path("bus/{reg}")
    public String GetBus(@PathParam("reg") String reg) throws RemoteException {

        String response = stub.GetBus(reg);

        JSONObject obj;
        try{
            JSONParser parser = new JSONParser();
            Object jsonObj = parser.parse(response);

             obj = (JSONObject) jsonObj;

            String bType = String.valueOf(obj.get("bus_type"));
            String tType = String.valueOf(obj.get("tacho"));
            String regT = String.valueOf(obj.get("bus_reg"));

            obj.put("bus_reg",regT.toUpperCase());

            if(bType.contains("0"))
            {
                obj.put("bus_type", "Single Deck");
            }
            else if(bType.contains("1"))
            {
                obj.put("bus_type", "Double Deck");
            }
            else if(bType.contains("2"))
            {
                obj.put("bus_type", "Mini bus");
            }
            else if(bType.contains("3"))
            {
                obj.put("bus_type", "Coach");
            }
            if(tType.contains("0"))
            {
                obj.put("tacho", "Digital");
            }
            else if(tType.contains("1"))
            {
                obj.put("tacho", "Analogue");
            }
        }
        catch (Exception ex)
        {
            return ex.toString();
        }
        return  obj.toJSONString();
    }

    @GET
    @Produces(MediaType.APPLICATION_JSON)
    @Path("all/drivers")
    public String AllDrivers() throws RemoteException {

        String response = stub.AllDrivers();

        try{
            JSONParser parser = new JSONParser();
            Object jsonObj = parser.parse(response);
            JSONArray jsonArray = (JSONArray) jsonObj;

            for (Object json: jsonArray) {

                 JSONObject obj = (JSONObject) json;

                 String lType = String.valueOf(obj.get("license_type"));
                 String cType = String.valueOf(obj.get("cpc_type"));
                 String dType = String.valueOf(obj.get("driver_type"));

                 if(lType.contains("0"))
                 {
                     obj.put("license_type", "Full");
                 }
                 else if(lType.contains("1"))
                 {
                     obj.put("license_type", "Mini-Bus");
                 }

                if(cType.contains("0"))
                {
                    obj.put("cpc_type", "Analogue");
                }
                else if(cType.contains("1"))
                {
                    obj.put("cpc_type", "Digital");
                }

                if(dType.contains("0"))
                {
                    obj.put("driver_type", "Service");
                }
                else if(dType.contains("1"))
                {
                    obj.put("driver_type", "School");
                }
                else if(dType.contains("2"))
                {
                    obj.put("driver_type", "Mini");
                }

                 conJsonArray.add(obj);
            }
        }
        catch (Exception ex)
        {
            return ex.toString();
        }
        return  conJsonArray.toJSONString();
    }

    @GET
    @Produces(MediaType.APPLICATION_JSON)
    @Path("get/driver/{id}")
    public String GetDriver(@PathParam("id") String id) throws RemoteException {
        String response = stub.GetDriver(id);

        JSONObject obj;
        try{
            JSONParser parser = new JSONParser();
            Object jsonObj = parser.parse(response);
            obj= (JSONObject) jsonObj;

            String lType = String.valueOf(obj.get("license_type"));
            String cType = String.valueOf(obj.get("cpc_type"));
            String dType = String.valueOf(obj.get("driver_type"));
            if(lType.contains("0"))
            {
                obj.put("license_type", "Full");
            }
            else if(lType.contains("1"))
            {
                obj.put("license_type", "Mini-Bus");
            }
            if(cType.contains("0"))
            {
                obj.put("cpc_type", "Analogue");
            }
            else if(cType.contains("1"))
            {
                obj.put("cpc_type", "Digital");
            }
            if(dType.contains("0"))
            {
                obj.put("driver_type", "Service");
            }
            else if(dType.contains("1"))
            {
                obj.put("driver_type", "School");
            }
            else if(dType.contains("2"))
            {
                obj.put("driver_type", "Mini");
            }
        }
        catch (Exception ex)
        {
            return ex.toString();
        }
        return  obj.toJSONString();
    }

    @GET
    @Produces(MediaType.TEXT_PLAIN)
    @Path("add/bus/{reg}/{type}/{weight}/{height}/{width}/{capacity}/{year}/{model}/{tacho}")
    public boolean AddBus(@PathParam("reg") String reg,
                          @PathParam("type") Integer type,
                          @PathParam("weight") Double weight,
                          @PathParam("height") Double height,
                          @PathParam("width") Double width,
                          @PathParam("capacity") Integer capacity,
                          @PathParam("year") Integer year,
                          @PathParam("model") String model,
                          @PathParam("tacho") Integer tacho) throws RemoteException {
        try {
            result = stub.AddBus(reg,type,weight,height,width,capacity,year,model,tacho);
            return result;
        } catch (Exception ex) {
            System.out.println(ex.toString());
            return result;
        }
    }

    @GET
    @Produces(MediaType.TEXT_PLAIN)
    @Path("edit/bus/{reg}/{type}/{weight}/{height}/{width}/{capacity}/{year}/{model}/{tacho}")
    public boolean EditBus(@PathParam("reg") String reg,
                           @PathParam("type") Integer type,
                           @PathParam("weight") Double weight,
                           @PathParam("height") Double height,
                           @PathParam("width") Double width,
                           @PathParam("capacity") Integer capacity,
                           @PathParam("year") Integer year,
                           @PathParam("model") String model,
                           @PathParam("tacho") Integer tacho) throws RemoteException {
        try {
            result = stub.EditBus(reg,type,weight,height,width,capacity,year,model,tacho);
            return result;
        } catch (Exception ex) {
            System.out.println(ex.toString());
            return result;
        }
    }

    @GET
    @Produces(MediaType.TEXT_PLAIN)
    @Path("delete/bus{reg}")
    public boolean DeleteBus(@PathParam("reg") String reg) throws RemoteException {
        //calls the remote method on dataserver
        try{
            result = stub.DeleteBus(reg);
            return result;
        }
        catch (Exception ex)
        {
            System.out.println(ex.toString());
            return result;
        }
    }

    @GET
    @Produces(MediaType.APPLICATION_JSON)
    @Path("geo/{search}")
    public String GetGeoCords(@PathParam("search") String search)throws RemoteException{

        RestClient client = new RestClient();
        try{

            JSONParser parser = new JSONParser();
            //splits the search term and formats the url to be sent to bing maps geo coding API
            String[] convert = search.split(" ");
            String converted = "";
            for (int i = 0; i < convert.length; i++){
                converted += convert[i] + "%20" ;
            }
            //returns json string response to call client
            String response =  client.sendGeo(converted);
            Object jsonObj = parser.parse(response);
            return jsonObj.toString();
        }
        catch (Exception ex)
        {
            return ex.toString();
        }
    }

    @GET
    @Produces(MediaType.APPLICATION_JSON)
    @Path("route/{wps}")
    public String GetTruckRoute(@PathParam("wps") String waypoints,
                              @QueryParam("weight") String weight,
                              @QueryParam("width") String width,
                              @QueryParam("height") String height) throws RemoteException{


        RestClient client = new RestClient();
        try{
            String response  = client.WaypointRequest(waypoints,height,width,weight);
            return response;
        }
        catch (Exception ex)
        {
            return ex.toString();
        }
    }

    @POST
    @Produces(MediaType.TEXT_PLAIN)
    @Path("job/add/{driver_id}/{bus_reg}/{start}/{end}/{pass_num}")
    public boolean AddJob (@PathParam("driver_id") String driver_id,
                           @PathParam("bus_reg") String bus_reg,
                           @PathParam("start")String start,
                           @PathParam("end")String end,
                           @FormParam("date")String date,
                           @FormParam("route") String route,
                           @PathParam("pass_num") int passNum) throws RemoteException  {

        //create a for each loop for this
        try{
            result = stub.AddJob(driver_id,bus_reg,route,start,end,date,passNum);
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    @GET
    @Produces(MediaType.APPLICATION_JSON)
    @Path("job/all")
    public String AllJobs() throws RemoteException {

        String response = stub.AllJobs();
        return response;
    }

    @GET
    @Produces(MediaType.APPLICATION_JSON)
    @Path("job/{id}")
    public String GetJob(@PathParam("id") String id) throws RemoteException {

        return stub.GetJob(id);
    }

    @GET
    @Produces(MediaType.TEXT_PLAIN)
    @Path("job/delete/{id}")
    public boolean DeleteJob(@PathParam("id") String id) throws RemoteException {
        result = stub.DeleteJob(id);
        return result;
    }

    @POST
    @Produces(MediaType.TEXT_PLAIN)
    @Path("job/edit/{jobid}/{driverid}/{busreg}/{start}/{end}/{pass_num}")
    public boolean EditJob(@PathParam("jobid") String job_id,
                           @PathParam("driverid") String driver_id,
                           @PathParam("busreg") String bus_reg,
                           @FormParam("route") String route,
                           @PathParam("start") String start_location,
                           @PathParam("end") String end_location,
                           @FormParam("date") String date,
                           @PathParam("pass_num") int pass) throws RemoteException {
        result = stub.EditJob(job_id,driver_id,bus_reg,route,start_location,end_location,date,pass);
        return result;
    }

    @POST
    @Produces(MediaType.TEXT_PLAIN)
    @Path("jobs")
    public String GetUsersJobs(@FormParam("userid") String userId) throws RemoteException{
        return stub.GetUsersJobs(userId);
    }

    @GET
    @Produces(MediaType.TEXT_PLAIN)
    @Path("jobs/bus/{reg}")
    public String GetBusJobs(@PathParam("reg") String reg) throws RemoteException{
        return stub.GetBusJobs(reg);
    }

}

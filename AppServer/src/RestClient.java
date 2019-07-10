import java.io.BufferedReader;
import java.io.DataOutputStream;
import java.io.InputStreamReader;
import java.net.HttpURLConnection;
import java.net.URL;
import javax.net.ssl.HttpsURLConnection;

public class RestClient {

    private String key = "CrCjR7pSdAIFkFfdsQh8~CrVmub8yWrGaYyduTmGf4A~Auovbg_NbAdEufRVtEY7QyfQchD-V9OXDgkYC6xii_2IvSiSgg4bv4nNdBwzTjmd";
    private String base = "http://dev.virtualearth.net/REST/v1/";


        // HTTP GET request for geo codes
        public String sendGeo(String message) throws Exception {

            //runs the geo request
            try{
                URL obj = new URL(base +"Locations?q="+message+"&key="+key);
                String response = Request(obj);
                return response;
            }
            catch (Exception ex){
                return ex.toString();
            }
        }

        //POST request that sends waypoints to BING maos truck routing API
        public String WaypointRequest(String message, String height, String width, String weight) throws Exception{

            //runs the truck route request
            try{
                URL obj = new URL(base+"Routes/Truck?"+ message +"&vehicleHeight="+height+"&vehicleWidth="+width+"&vehicleWeight="+weight+"&routeAttributes=routePath"+"&key="+ key);
                String response = Request(obj);
                return response;
            }
            catch (Exception ex){
                return ex.toString();
            }
        }


        //Default POST request
        private String Request(URL obj) throws Exception{
            try{

                //builds a http get request and writes out the response
                HttpURLConnection con = (HttpURLConnection) obj.openConnection();

                // optional default is GET
                con.setRequestMethod("GET");

                int responseCode = con.getResponseCode();

                BufferedReader in = new BufferedReader(
                        new InputStreamReader(con.getInputStream()));
                String inputLine;
                StringBuffer response = new StringBuffer();

                while ((inputLine = in.readLine()) != null) {
                    response.append(inputLine);
                }
                in.close();

                //print result
                System.out.println(response.toString());
                return response.toString();
            }
            catch(Exception ex)
            {
                return ex.toString();
            }

        }


}

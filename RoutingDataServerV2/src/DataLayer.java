//created by craig cheney Edinburgh Napier University (Honors Project) 2019
import org.json.simple.JSONArray;
import org.json.simple.JSONObject;
import java.rmi.RemoteException;
import java.rmi.server.UnicastRemoteObject;
import java.sql.*;

@SuppressWarnings("Duplicates")
public  class DataLayer extends UnicastRemoteObject implements IDataLayer {

   //creates variables private to the data server
   private Statement statement;
   private Connection conn;
   private JSONArray  userArray = new JSONArray();
   private JSONObject userParam;
   private JSONArray  busArray = new JSONArray();
   private JSONObject busParam;
   private JSONArray  jobArray = new JSONArray();
   private JSONObject jobParam;
   private String dUsername = null;
   private String dPassword = null;
   private int counter = 0;
   private int rows = 0;


   //instantiates the IDataLayer interface, the functions used remotely by the application server.
    protected DataLayer() throws RemoteException {
        super();
        try {
            Class.forName("com.mysql.cj.jdbc.Driver");
            conn = DriverManager.getConnection("jdbc:mysql://localhost/routing?verifyServerCertificate=false&useSSL=true", "java", "java");
            statement = conn.createStatement();
        } catch (Exception ex) {
            System.out.println(ex.toString());
        }
    }

    public boolean UserSignIn(String userId, String password) {

        try {

            String query = "SELECT * FROM users WHERE (user_id ='" + userId + "' AND  user_password ='"+password+"')";
            ResultSet results = statement.executeQuery(query);

            results.next();
            {
                dUsername = results.getString("user_id");
                dPassword = results.getString("user_password");
                return true;
            }

        } catch (Exception ex) {
            System.out.println(ex.toString());
            return false;
        }
    }
    public String GetUser(String id){
        try{

            Class.forName("com.mysql.cj.jdbc.Driver");
            conn = DriverManager.getConnection("jdbc:mysql://localhost/routing?verifyServerCertificate=false&useSSL=true?&useJDBCCompliantTimezoneShift=true&useLegacyDatetimeCode=false&serverTimezone=UTC","java","java");
            statement = conn.createStatement();

            String query = "SELECT * FROM users WHERE user_id ="+id;
            ResultSet results = statement.executeQuery(query);


            while(results.next())
            {
                userParam = new JSONObject();

                String userId = results.getString("user_id");
                String userType = results.getString("user_type");
                String fName = results.getString("first_name");
                String sName = results.getString("second_name");
                String driverType = results.getString("driver_type");
                String licenseType = results.getString("license_type");
                String cpcType = results.getString("cpc_type");
                String email = results.getString("e_mail");

                userParam.put("user_id",userId);
                userParam.put("user_type",userType);
                userParam.put("first_name",fName);
                userParam.put("second_name",sName);
                userParam.put("driver_type",driverType);
                userParam.put("license_type",licenseType);
                userParam.put("cpc_type",cpcType);
                userParam.put("e_mail",email);
            }

        }
        catch (Exception ex)
        {
            return ex.toString();
        }

        return userParam.toJSONString();
    }
    public boolean AddUser(Integer userType, String fName, String sName, Integer driverType, Integer licenseType, Integer cpcType, String email) {

        // the mysql insert statement
        String query = "INSERT INTO users (user_type,first_name,second_name,driver_type,license_type,cpc_type,e_mail) values (?,?,?,?,?,?,?)";

        try {

            // create the mysql insert prepared statement
            PreparedStatement preparedStmt = conn.prepareStatement(query);

            preparedStmt.setInt(1, userType);
            preparedStmt.setString(2, fName);
            preparedStmt.setString(3, sName);
            preparedStmt.setInt(4, driverType);
            preparedStmt.setInt(5, licenseType);
            preparedStmt.setInt(6, cpcType);
            preparedStmt.setString(7, email);
            // execute the prepared statement
            rows = preparedStmt.executeUpdate();
            return true;


        } catch (SQLException ex) {
            System.out.println(ex.toString());
            return false;
        }
    }
    public String GetAllUsers()  {

        userArray.clear();
        counter = 0;

        try{

            Class.forName("com.mysql.cj.jdbc.Driver");
            conn = DriverManager.getConnection("jdbc:mysql://localhost/routing?verifyServerCertificate=false&useSSL=true&useJDBCCompliantTimezoneShift=false&useLegacyDatetimeCode=false&serverTimezone=Europe/London","java","java");
            statement = conn.createStatement();

            String query = "SELECT * FROM users ";
            ResultSet results = statement.executeQuery(query);


            while(results.next())
            {
                userParam = new JSONObject();

                String userId = results.getString("user_id");
                String userType = results.getString("user_type");
                String fName = results.getString("first_name");
                String sName = results.getString("second_name");
                String driverType = results.getString("driver_type");
                String licenseType = results.getString("license_type");
                String cpcType = results.getString("cpc_type");
                String email = results.getString("e_mail");

                userParam.put("user_id",userId);
                userParam.put("user_type",userType);
                userParam.put("first_name",fName);
                userParam.put("second_name",sName);
                userParam.put("driver_type",driverType);
                userParam.put("license_type",licenseType);
                userParam.put("cpc_type",cpcType);
                userParam.put("e_mail",email);



                userArray.add(counter,userParam);
                counter++;
            }

        }
        catch (Exception ex)
        {
           return ex.toString();
        }

        return userArray.toJSONString();
    }
    public boolean EditUsers(Integer userType,String userId, String fName, String sName, Integer driverType, Integer licenseType, Integer cpcType, String email) {

        String query = "UPDATE users SET user_type = ?, first_name = ?, second_name = ?, driver_type = ?, license_type = ?, cpc_type = ?, e_mail = ? WHERE user_id ="+userId;

        try{
            // the mysql insert statement
            PreparedStatement preparedStmt = conn.prepareStatement(query);
            preparedStmt.setInt(1, userType);
            preparedStmt.setString(2, fName);
            preparedStmt.setString(3, sName);
            preparedStmt.setInt(4, driverType);
            preparedStmt.setInt(5, licenseType);
            preparedStmt.setInt(6, cpcType);
            preparedStmt.setString(7, email);

            preparedStmt.executeUpdate();
            return true;
        }
        catch (SQLException ex)
        {
            System.out.println(ex.toString());
            return false;
        }
    }
    public boolean DeleteUsers(String userId) {

        String query = "DELETE FROM users WHERE user_id ='"+userId + "'";

        try{
            PreparedStatement preparedStmt = conn.prepareStatement(query);
            preparedStmt.execute();
            return  true;

        }
        catch (SQLException ex)
        {
            System.out.println(ex.toString());
            return false;
        }
    }
    public String AllBuses()  {

        busArray.clear();
        counter = 0;

        try{

            Class.forName("com.mysql.cj.jdbc.Driver");
            conn = DriverManager.getConnection("jdbc:mysql://localhost/routing?verifyServerCertificate=false&useSSL=true&useJDBCCompliantTimezoneShift=false&useLegacyDatetimeCode=false&serverTimezone=Europe/London","java","java");
            statement = conn.createStatement();

            String query = "SELECT * FROM buses ";
            ResultSet results = statement.executeQuery(query);


            while(results.next())
            {
                busParam = new JSONObject();

                String reg = results.getString("bus_reg");
                String type = results.getString("bus_type");
                String weight = results.getString("bus_weight");
                String height = results.getString("bus_height");
                String width = results.getString("bus_width");
                String capacity = results.getString("bus_capacity");
                String year = results.getString("year");
                String model = results.getString("model");
                String tacho = results.getString("tacho");

                busParam.put("bus_reg",reg);
                busParam.put("bus_type",type);
                busParam.put("bus_weight",weight);
                busParam.put("bus_height",height);
                busParam.put("bus_width",width);
                busParam.put("bus_capacity",capacity);
                busParam.put("year",year);
                busParam.put("model",model);
                busParam.put("tacho",tacho);


                busArray.add(counter,busParam);
                counter++;
            }

        }
        catch (Exception ex)
        {
            return ex.toString();
        }

        return busArray.toJSONString();
    }
    public String GetBus(String id)  {
        try{

            Class.forName("com.mysql.cj.jdbc.Driver");
            conn = DriverManager.getConnection("jdbc:mysql://localhost/routing?verifyServerCertificate=false&useSSL=true&useJDBCCompliantTimezoneShift=false&useLegacyDatetimeCode=false&serverTimezone=Europe/London","java","java");
            statement = conn.createStatement();

            String query =  "SELECT * FROM buses WHERE bus_reg ='"+id + "'";
            ResultSet results = statement.executeQuery(query);


            while(results.next())
            {
                busParam = new JSONObject();

                String reg = results.getString("bus_reg");
                String type = results.getString("bus_type");
                String weight = results.getString("bus_weight");
                String height = results.getString("bus_height");
                String width = results.getString("bus_width");
                String capacity = results.getString("bus_capacity");
                String year = results.getString("year");
                String model = results.getString("model");
                String tacho = results.getString("tacho");

                busParam.put("bus_reg",reg);
                busParam.put("bus_type",type);
                busParam.put("bus_weight",weight);
                busParam.put("bus_height",height);
                busParam.put("bus_width",width);
                busParam.put("bus_capacity",capacity);
                busParam.put("year",year);
                busParam.put("model",model);
                busParam.put("tacho",tacho);
            }

        }
        catch (Exception ex)
        {
            return ex.toString();
        }
        return busParam.toJSONString();
    }
    public boolean AddBus(String reg, Integer type, Double weight, Double height, Double width, Integer capacity, Integer year, String model, Integer tacho) {

        String query = "INSERT INTO buses (bus_reg,bus_type,bus_weight,bus_height,bus_width,bus_capacity,year,model,tacho) values (?,?,?,?,?,?,?,?,?)";

        try{
            // the mysql insert statement
            PreparedStatement preparedStmt = conn.prepareStatement(query);
            preparedStmt.setString(1, reg);
            preparedStmt.setInt(2, type);
            preparedStmt.setDouble(3, weight);
            preparedStmt.setDouble(4, height);
            preparedStmt.setDouble(5, width);
            preparedStmt.setInt(6, capacity);
            preparedStmt.setInt(7, year);
            preparedStmt.setString(8, model);
            preparedStmt.setInt(9, tacho);

            preparedStmt.executeUpdate();
            return true;
        }
        catch (SQLException ex)
        {
            System.out.println(ex.toString());
            return false;
        }
    }
    public boolean EditBus(String reg, Integer type, Double weight, Double height, Double width, Integer capacity, Integer year, String model, Integer tacho){

        String query = "UPDATE buses SET bus_reg = ?, bus_type = ?, bus_weight = ?, bus_height = ?, bus_width = ?, bus_capacity = ?, year = ?, model = ?, tacho = ? WHERE bus_reg='"+reg + "'";

        try{
            PreparedStatement preparedStmt = conn.prepareStatement(query);
            preparedStmt.setString(1, reg);
            preparedStmt.setInt(2, type);
            preparedStmt.setDouble(3, weight);
            preparedStmt.setDouble(4, height);
            preparedStmt.setDouble(5, width);
            preparedStmt.setInt(6, capacity);
            preparedStmt.setInt(7, year);
            preparedStmt.setString(8, model);
            preparedStmt.setInt(9, tacho);

            preparedStmt.executeUpdate();
            return true;
        }
        catch (SQLException ex)
        {
            System.out.println(ex.toString());
            return false;
        }
    }
    public boolean DeleteBus(String reg)  {

        String query = "DELETE FROM buses WHERE bus_reg='"+reg + "'";

        try{
            PreparedStatement preparedStmt = conn.prepareStatement(query);
            preparedStmt.execute();
            return  true;

        }
        catch (SQLException ex)
        {
            System.out.println(ex.toString());
            return false;
        }
    }
    public String AllDrivers()  {
        userArray.clear();
        counter = 0;

        try{

            Class.forName("com.mysql.cj.jdbc.Driver");
            conn = DriverManager.getConnection("jdbc:mysql://localhost/routing?verifyServerCertificate=false&useSSL=true&useJDBCCompliantTimezoneShift=false&useLegacyDatetimeCode=false&serverTimezone=Europe/London","java","java");
            statement = conn.createStatement();

            String query = "SELECT * FROM users ";
            ResultSet results = statement.executeQuery(query);


            while(results.next())
            {
                userParam = new JSONObject();

                String userId = results.getString("user_id");
                String fName = results.getString("first_name");
                String sName = results.getString("second_name");
                String driverType = results.getString("driver_type");
                String licenseType = results.getString("license_type");
                String cpcType = results.getString("cpc_type");


                userParam.put("user_id",userId);
                userParam.put("first_name",fName);
                userParam.put("second_name",sName);
                userParam.put("driver_type",driverType);
                userParam.put("license_type",licenseType);
                userParam.put("cpc_type",cpcType);




                userArray.add(counter,userParam);
                counter++;
            }

        }
        catch (Exception ex)
        {
            return ex.toString();
        }

        return userArray.toJSONString();
    }
    public String GetDriver(String id)  {
        try{

            Class.forName("com.mysql.cj.jdbc.Driver");
            conn = DriverManager.getConnection("jdbc:mysql://localhost/routing?verifyServerCertificate=false&useSSL=true&useJDBCCompliantTimezoneShift=false&useLegacyDatetimeCode=false&serverTimezone=Europe/London","java","java");
            statement = conn.createStatement();

            String query = "SELECT * FROM users WHERE user_id ='"+id + "'";
            ResultSet results = statement.executeQuery(query);


            while(results.next())
            {
                userParam = new JSONObject();

                String userId = results.getString("user_id");
                String fName = results.getString("first_name");
                String sName = results.getString("second_name");
                String driverType = results.getString("driver_type");
                String licenseType = results.getString("license_type");
                String cpcType = results.getString("cpc_type");


                userParam.put("user_id",userId);
                userParam.put("first_name",fName);
                userParam.put("second_name",sName);
                userParam.put("driver_type",driverType);
                userParam.put("license_type",licenseType);
                userParam.put("cpc_type",cpcType);
            }

        }
        catch (Exception ex)
        {
            return ex.toString();
        }

        return userParam.toJSONString();
    }
    public boolean AddJob(String driver_id, String bus_reg,String route,String start_location,String end_location,String date,int pass_num) {

        // the mysql insert statement
        String query = "INSERT INTO jobs (driver_id,bus_reg,route,start_location,end_location,date_time,pass_num) values (?,?,?,?,?,?,? )";

        try {

            // create the mysql insert prepared statement
            PreparedStatement preparedStmt = conn.prepareStatement(query);
            preparedStmt.setString(1,driver_id);
            preparedStmt.setString(2,bus_reg);
            preparedStmt.setString(3,route);
            preparedStmt.setString(4,start_location);
            preparedStmt.setString(5,end_location);
            preparedStmt.setString(6,date);
            preparedStmt.setInt(7,pass_num);



            // execute the prepared statement
            rows = preparedStmt.executeUpdate();
            return true;


        } catch (SQLException ex) {
            System.out.println(ex.toString());
            return false;
        }
    }
    public String AllJobs() {
        jobArray.clear();
        counter = 0;

        try{

            Class.forName("com.mysql.cj.jdbc.Driver");
            conn = DriverManager.getConnection("jdbc:mysql://localhost/routing?verifyServerCertificate=false&useSSL=true&useJDBCCompliantTimezoneShift=false&useLegacyDatetimeCode=false&serverTimezone=Europe/London","java","java");
            statement = conn.createStatement();

            String query = "SELECT * FROM jobs ";
            ResultSet results = statement.executeQuery(query);

            while(results.next())
            {
                jobParam = new JSONObject();

                String jobId = results.getString("job_id");
                String driverId = results.getString("driver_id");
                String busReg = results.getString("bus_reg");
                String route = results.getString("route");
                String startLocation = results.getString("start_location");
                String endLocation = results.getString("end_location");
                String date = results.getString("date_time");
                int pass = results.getInt("pass_num");
                int isComplete = results.getInt("pass_num");


                jobParam.put("job_id",jobId);
                jobParam.put("driver_id",driverId);
                jobParam.put("bus_reg",busReg);
                jobParam.put("route",route);
                jobParam.put("start_location",startLocation);
                jobParam.put("end_location",endLocation);
                jobParam.put("date",date);
                jobParam.put("pass_num",pass);
                jobParam.put("is_complete",isComplete);

                jobArray.add(counter,jobParam);
                counter++;
            }

        }
        catch (Exception ex)
        {
            return ex.toString();
        }

        return jobArray.toJSONString();
    }
    public String GetJob(String id)  {

        try{

            Class.forName("com.mysql.cj.jdbc.Driver");
            conn = DriverManager.getConnection("jdbc:mysql://localhost/routing?verifyServerCertificate=false&useSSL=true&useJDBCCompliantTimezoneShift=false&useLegacyDatetimeCode=false&serverTimezone=Europe/London","java","java");
            statement = conn.createStatement();

            String query =  "SELECT * FROM jobs WHERE job_id ='"+id + "'";
            ResultSet results = statement.executeQuery(query);


            while(results.next())
            {
                jobParam = new JSONObject();

                String jobId = results.getString("job_id");
                String driverId = results.getString("driver_id");
                String busReg = results.getString("bus_reg");
                String route = results.getString("route");
                String startLocation = results.getString("start_location");
                String endLocation = results.getString("end_location");
                String date = results.getString("date_time");
                int pass = results.getInt("pass_num");
                int isComplete = results.getInt("pass_num");


                jobParam.put("job_id",jobId);
                jobParam.put("driver_id",driverId);
                jobParam.put("bus_reg",busReg);
                jobParam.put("route",route);
                jobParam.put("start_location",startLocation);
                jobParam.put("end_location",endLocation);
                jobParam.put("date_time",date);
                jobParam.put("pass_num",pass);
                jobParam.put("is_complete",isComplete);
            }

        }
        catch (Exception ex)
        {
            return ex.toString();
        }
        return jobParam.toJSONString();
    }
    public boolean DeleteJob(String id) {

        String query = "DELETE FROM jobs WHERE job_id='"+id + "'";

        try{
            PreparedStatement preparedStmt = conn.prepareStatement(query);
            preparedStmt.execute();
            return  true;

        }
        catch (SQLException ex)
        {
            System.out.println(ex.toString());
            return false;
        }
    }
    public boolean EditJob(String job_id, String driver_id, String bus_reg, String route, String start_location, String end_location, String date,int pass_num)  {
        String query = "UPDATE jobs SET driver_id = ?, bus_reg = ?, route = ?, start_location = ?, end_location = ?, date_time = ?, pass_num = ? WHERE job_id='"+job_id + "'";

        try{
            PreparedStatement preparedStmt = conn.prepareStatement(query);
            preparedStmt.setString(1, driver_id);
            preparedStmt.setString(2, bus_reg);
            preparedStmt.setString(3, route);
            preparedStmt.setString(4, start_location);
            preparedStmt.setString(5, end_location);
            preparedStmt.setString(6, date);
            preparedStmt.setInt(7,pass_num);


            preparedStmt.executeUpdate();
            return true;
        }
        catch (SQLException ex)
        {
            System.out.println(ex.toString());
            return false;
        }
    }
    public String GetUsersJobs(String userid) {

        jobArray.clear();
        counter = 0;

        try{

            Class.forName("com.mysql.cj.jdbc.Driver");
            conn = DriverManager.getConnection("jdbc:mysql://localhost/routing?verifyServerCertificate=false&useSSL=true&useJDBCCompliantTimezoneShift=false&useLegacyDatetimeCode=false&serverTimezone=Europe/London","java","java");
            statement = conn.createStatement();

            String query = "SELECT * FROM jobs WHERE driver_id ='"+userid+"'";
            ResultSet results = statement.executeQuery(query);


            while(results.next())
            {
                jobParam = new JSONObject();

                String jobId = results.getString("job_id");
                String driver_id = results.getString("driver_id");
                String reg = results.getString("bus_reg");
                String route = results.getString("route");
                String start = results.getString("start_location");
                String end = results.getString("end_location");
                String date = results.getString("Date_time");
                int pass = results.getInt("pass_num");
                int isComplete = results.getInt("is_complete");


                jobParam.put("job_id",jobId);
                jobParam.put("driver_id",driver_id);
                jobParam.put("bus_reg",reg);
                jobParam.put("route",route);
                jobParam.put("start_location",start);
                jobParam.put("end_location",end);
                jobParam.put("Date_time",date);
                jobParam.put("pass_num",pass);
                jobParam.put("is_complete",isComplete);


                jobArray.add(counter,jobParam);
                counter++;
            }

        }
        catch (Exception ex)
        {
            return ex.toString();
        }

        return jobArray.toJSONString();
    }
    public String GetBusJobs(String bus_reg) {
        jobArray.clear();
        counter = 0;

        try{

            Class.forName("com.mysql.cj.jdbc.Driver");
            conn = DriverManager.getConnection("jdbc:mysql://localhost/routing?verifyServerCertificate=false&useSSL=true&useJDBCCompliantTimezoneShift=false&useLegacyDatetimeCode=false&serverTimezone=Europe/London","java","java");
            statement = conn.createStatement();

            String query = "SELECT * FROM jobs WHERE bus_reg ='"+bus_reg+"'";
            ResultSet results = statement.executeQuery(query);


            while(results.next())
            {
                jobParam = new JSONObject();

                String jobId = results.getString("job_id");
                String driver_id = results.getString("driver_id");
                String reg = results.getString("bus_reg");
                String route = results.getString("route");
                String start = results.getString("start_location");
                String end = results.getString("end_location");
                String date = results.getString("Date_time");
                int pass = results.getInt("pass_num");
                int isComplete = results.getInt("is_complete");


                jobParam.put("job_id",jobId);
                jobParam.put("driver_id",driver_id);
                jobParam.put("bus_reg",reg);
                jobParam.put("route",route);
                jobParam.put("start_location",start);
                jobParam.put("end_location",end);
                jobParam.put("Date_time",date);
                jobParam.put("pass_num",pass);
                jobParam.put("is_complete",isComplete);


                jobArray.add(counter,jobParam);
                counter++;
            }

        }
        catch (Exception ex)
        {
            return ex.toString();
        }

        return jobArray.toJSONString();
    }

}

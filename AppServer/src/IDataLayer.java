

import java.io.Serializable;
import java.rmi.Remote;
import java.rmi.RemoteException;

public interface IDataLayer extends Remote, Serializable {

    //-------------------------------------------------------------------user database functions--------------------------------------------------------------------------

    boolean UserSignIn(String userId, String password) throws RemoteException;

    boolean AddUser(Integer userType,String fName,String sName,Integer driverType,Integer licenseType,Integer cpcType,String email) throws  RemoteException;

    boolean EditUsers(Integer userType,String userId,String fName,String sName,Integer driverType,Integer licenseType,Integer cpcType,String email) throws RemoteException;

    boolean DeleteUsers(String userId) throws RemoteException;

    String GetAllUsers() throws RemoteException;

    String GetUser(String id) throws RemoteException;

    //--------------------------------------------------------------------bus database functions---------------------------------------------------------------------------

    boolean AddBus(String reg, Integer type, Double weight, Double height, Double width, Integer capacity, Integer year, String model, Integer tacho) throws  RemoteException;

    boolean EditBus(String reg, Integer type, Double weight, Double height, Double width, Integer capacity, Integer year, String model, Integer tacho) throws RemoteException;

    boolean DeleteBus(String reg) throws RemoteException;

    String AllBuses() throws RemoteException;

    String GetBus(String reg) throws RemoteException;

    //-------------------------------------------------------------------driver database functions--------------------------------------------------------------------------

    String AllDrivers() throws RemoteException;

    String GetDriver(String id) throws  RemoteException;

    //--------------------------------------------------------------------job database functions----------------------------------------------------------------------------

    boolean AddJob(String driver_id, String bus_reg,String route,String start_location,String end_location,String date,int passNum) throws RemoteException;

    String AllJobs() throws RemoteException;

    String GetJob(String id) throws RemoteException;

    boolean DeleteJob (String id) throws RemoteException;

    boolean EditJob(String job_id, String driver_id, String bus_reg,String route,String start_location,String end_location,String date, int passNum) throws RemoteException;

    String GetUsersJobs(String userid) throws RemoteException;

    String GetBusJobs(String reg) throws RemoteException;

}

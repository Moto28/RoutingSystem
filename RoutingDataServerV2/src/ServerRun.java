//created by craig cheney Edinburgh Napier University (Honors Project) 2019
import java.rmi.RemoteException;
import java.rmi.registry.LocateRegistry;
import java.rmi.registry.Registry;
import java.rmi.server.UnicastRemoteObject;


public class ServerRun {

    public static boolean isRegistered = false;
    public static IDataLayer service;

    public static void main(String[] args) throws RemoteException {

        try {

            //Start the registry
            Registry registry = LocateRegistry.createRegistry(1098);

            // Instantiate the concrete class
            DataLayer data = new DataLayer();

            //Bind the stub to the name "Account" in the registry
            registry.bind("record2",data);

            System.out.println("Server ready");


        } catch (Exception e) {
            System.err.println("Server exception: " + e.toString());
            e.printStackTrace();
        }
    }
}

package com.route.client;

import android.content.Intent;
import android.graphics.Color;
import android.location.Location;
import android.location.LocationProvider;
import android.os.PersistableBundle;
import android.support.annotation.NonNull;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.support.v7.widget.Toolbar;
import android.util.Log;
import android.view.View;
import android.widget.Button;
import android.widget.Toast;

import com.google.gson.Gson;
import com.google.gson.JsonArray;
import com.google.gson.JsonObject;
import com.google.gson.JsonParser;
import com.mapbox.android.core.location.LocationEngineListener;
import com.mapbox.android.core.location.LocationEnginePriority;
import com.mapbox.android.core.location.LocationEngineProvider;
import com.mapbox.android.core.permissions.PermissionsListener;
import com.mapbox.android.core.permissions.PermissionsManager;
import com.mapbox.api.directions.v5.DirectionsCriteria;
import com.mapbox.api.directions.v5.models.DirectionsResponse;
import com.mapbox.api.directions.v5.models.DirectionsRoute;
import com.mapbox.api.matching.v5.MapboxMapMatching;
import com.mapbox.api.matching.v5.models.MapMatchingResponse;
import com.mapbox.mapboxsdk.annotations.PolylineOptions;
import com.mapbox.mapboxsdk.style.sources.GeoJsonSource;
import com.mapbox.geojson.Feature;
import com.mapbox.geojson.FeatureCollection;
import com.mapbox.geojson.LineString;
import com.mapbox.geojson.Point;
import com.mapbox.mapboxsdk.Mapbox;
import com.mapbox.mapboxsdk.annotations.Marker;
import com.mapbox.mapboxsdk.annotations.MarkerOptions;
import com.mapbox.mapboxsdk.camera.CameraUpdateFactory;
import com.mapbox.mapboxsdk.geometry.LatLng;
import com.mapbox.mapboxsdk.maps.MapView;
import com.mapbox.mapboxsdk.maps.MapboxMap;
import com.mapbox.mapboxsdk.maps.OnMapReadyCallback;
import com.mapbox.mapboxsdk.plugins.locationlayer.LocationLayerPlugin;

import com.mapbox.android.core.location.LocationEngine;
import com.mapbox.mapboxsdk.plugins.locationlayer.modes.CameraMode;
import com.mapbox.mapboxsdk.plugins.locationlayer.modes.RenderMode;
import com.mapbox.services.android.navigation.ui.v5.NavigationLauncher;
import com.mapbox.services.android.navigation.ui.v5.NavigationLauncherOptions;
import com.mapbox.services.android.navigation.ui.v5.NavigationViewOptions;
import com.mapbox.services.android.navigation.ui.v5.listeners.NavigationListener;
import com.mapbox.services.android.navigation.ui.v5.route.NavigationMapRoute;
import com.mapbox.services.android.navigation.v5.navigation.NavigationRoute;

import java.util.ArrayList;
import java.util.List;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class MainActivity extends AppCompatActivity implements OnMapReadyCallback, LocationEngineListener,  PermissionsListener , MapboxMap.OnMapClickListener {

    private MapView mapView;
    private MapboxMap map;
    private PermissionsManager permissionsManager;
    private LocationEngine locationEngine;
    private LocationLayerPlugin locationLayerPlugin;
    private Location originLocation;
    private Point originPosition;
    private Point destinationPosition;
    private Marker destinationMarker;
    private Button startButton;
    private ArrayList<Point> coords = new ArrayList<>();
    private List<LatLng> line = new ArrayList<>();
    private NavigationMapRoute navigationMapRoute;
    private NavigationRoute nav;
    private DirectionsRoute routeodfgoh;
    private static final String TAG = "MainActivity";
    private static RawRoute rawRoute = null;
    private int coordSize;


    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        //gets mapbox view
        Mapbox.getInstance(this, getString(R.string.access_token));
        setContentView(R.layout.activity_main);
        Toolbar toolbar = findViewById(R.id.toolbar);
        setSupportActionBar(toolbar);

        //gets passed parameters and converts to json
        Intent intent = getIntent();
        String job = intent.getStringExtra("job");
        JsonObject jobObj;
        JsonParser parser = new JsonParser();
        Object jsonObj = parser.parse(job);
        jobObj = (JsonObject) jsonObj;
        final String route = jobObj.toString();

        //convert to json to java object
        Gson gson = new Gson();
        rawRoute = gson.fromJson(route, RawRoute.class);

        //adds points for each itinerary item to coords list
        for (int i = 0; i < rawRoute.resourceSets[0].resources[0].routeLegs[0].itineraryItems.length; i++) {

           /* float[] latLng = rawRoute.resourceSets[0].resources[0].routePath.line.coordinates[i];
            Point temp = Point.fromLngLat(latLng[0],latLng[1]);*/
            //reversed lat and long
            Point temp = Point.fromLngLat(rawRoute.resourceSets[0].resources[0].routeLegs[0].itineraryItems[i].maneuverPoint.coordinates[1],
                    rawRoute.resourceSets[0].resources[0].routeLegs[0].itineraryItems[i].maneuverPoint.coordinates[0]);
            coords.add(temp);
        }

        coordSize = coords.size() - 1;

        //loads map on screen and sets map style
        mapView = findViewById(R.id.mapView);
        mapView.onCreate(savedInstanceState);
        mapView.getMapAsync(this);

        //start button and mapbox controls
        startButton = findViewById(R.id.startButton);


        //starts navigation when button clicked
        startButton.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {


                //add navigation
                NavigationLauncherOptions options = NavigationLauncherOptions.builder()
                        .directionsRoute(routeodfgoh)
                        .shouldSimulateRoute(true)
                        .snapToRoute(true)
                        .build();
                NavigationLauncher.startNavigation(MainActivity.this, options);

            }
        });


    }
    @Override
    public void onMapReady(final MapboxMap mapboxMap) {

        map = mapboxMap;
        startButton.setEnabled(true);
        GetRoute();

        //user location and map matching
       /* enableLocation();
        map.addOnMapClickListener(this);*/


       /* Integer[] OD = new Integer[2];
        OD[0] = 0;
        OD[1]= coords.size() -1;
        MapboxMapMatching.builder()
                .accessToken(Mapbox.getAccessToken())
                .coordinates(coords)
                .steps(true)
                .waypoints(OD)
                .voiceInstructions(true)
                .bannerInstructions(true)
                .profile(DirectionsCriteria.PROFILE_DRIVING)
                .build()
                .enqueueCall(new Callback<MapMatchingResponse>() {

                    @Override
                    public void onResponse(Call<MapMatchingResponse> call, Response<MapMatchingResponse> response) {
                        if (response.isSuccessful()) {

                            try{
                                routeodfgoh = response.body().matchings().get(0).toDirectionRoute();

                                if(navigationMapRoute != null){
                                    navigationMapRoute.removeRoute();
                                }
                                else {
                                    navigationMapRoute = new NavigationMapRoute(null,mapView,map);
                                }

                                //how to draw the map think map matching is work
                                navigationMapRoute.addRoute(routeodfgoh);

                            }
                            catch (Exception ex){
                                Toast.makeText(MainActivity.this,ex.toString(),Toast.LENGTH_LONG);
                            }


                        }
                    }

                    @Override
                    public void onFailure(Call<MapMatchingResponse> call, Throwable throwable) {

                        Log.e("","",throwable);
                    }
                });
*/

    }

    private void enableLocation(){
        if(PermissionsManager.areLocationPermissionsGranted(this)){
            //do stuff
            createLocationEngine();
            createLocationLayer();
        }
        else{
            permissionsManager = new PermissionsManager(this);
            permissionsManager.requestLocationPermissions(this);
        }
    }

    @SuppressWarnings("MissingPermission")
    private void createLocationEngine(){
        locationEngine = new LocationEngineProvider(this).obtainBestLocationEngineAvailable();
        locationEngine.setPriority(LocationEnginePriority.HIGH_ACCURACY);
        locationEngine.activate();

        Location lastLocation = locationEngine.getLastLocation();
        if(lastLocation != null){
            originLocation = lastLocation;
        }
        else{
            locationEngine.addLocationEngineListener(this);
        }
    }

    @SuppressWarnings("MissingPermission")
    private void createLocationLayer(){
        locationLayerPlugin = new LocationLayerPlugin(mapView,map,locationEngine);
        locationLayerPlugin.setLocationLayerEnabled(true);
        locationLayerPlugin.setCameraMode(CameraMode.TRACKING);
        locationLayerPlugin.setRenderMode(RenderMode.NORMAL);
    }

    private void setCameraPosition(Location location){
        map.animateCamera(CameraUpdateFactory.newLatLngZoom(new LatLng(location.getLatitude(),location.getLongitude()),13));
    }

    @Override
    public void onMapClick(@NonNull LatLng point) {

        //adds marker
       /* if(destinationMarker != null){
            map.removeMarker(destinationMarker);
        }
        destinationMarker = map.addMarker(new MarkerOptions().position(point));

        destinationPosition = Point.fromLngLat(point.getLongitude(),point.getLatitude());
        originPosition = Point.fromLngLat(originLocation.getLongitude(),point.getLatitude());
        getRoute(originPosition,destinationPosition);
        startButton.setEnabled(true);
        startButton.setBackgroundResource(R.color.mapboxBlue);*/
    }

    @Override
    public void onConnected() {
        locationEngine.removeLocationUpdates();
    }

    @Override
    public void onLocationChanged(Location location) {
        if(location != null){
            originLocation = location;
            setCameraPosition(location);
        }
    }

    @Override
    public void onExplanationNeeded(List<String> permissionsToExplain) {
        //present toast or dialog
    }

    @Override
    public void onPermissionResult(boolean granted) {
        if(granted){
            enableLocation();
        }
    }

    @Override
    public void onRequestPermissionsResult(int requestCode, @NonNull String[] permissions, @NonNull int[] grantResults) {
        super.onRequestPermissionsResult(requestCode, permissions, grantResults);
    }

    @SuppressWarnings("MissingPermission")
    @Override
    protected void onStart() {
        super.onStart();
        if(locationEngine != null){
            locationEngine.removeLocationUpdates();
        }
        if(locationLayerPlugin != null){
            locationLayerPlugin.onStart();
        }
        mapView.onStart();
    }

    @Override
    protected void onResume() {
        super.onResume();
        mapView.onResume();
    }

    @Override
    protected void onPause() {
        super.onPause();
        mapView.onPause();
    }

    @Override
    protected void onStop() {
        super.onStop();
        if(locationEngine != null){
            locationEngine.removeLocationUpdates();
        }
        if(locationLayerPlugin != null){
            locationLayerPlugin.onStop();
        }
        mapView.onStop();
    }

    @Override
    public void onSaveInstanceState(Bundle outState, PersistableBundle outPersistentState) {
        super.onSaveInstanceState(outState);
        mapView.onSaveInstanceState(outState);
    }

    @Override
    public void onLowMemory() {
        super.onLowMemory();
        mapView.onLowMemory();
    }

    @Override
    protected void onDestroy() {
        super.onDestroy();
        mapView.onDestroy();
        if(locationEngine != null){
            locationEngine.deactivate();
        }
    }

    public void GetRoute(){



        // if you cant get something out of mapbox request will need to be limited to 100 cords. unless can be paid for
        originPosition = (coords.remove(0));
      destinationPosition = (coords.remove(coordSize -3));

        /*NavigationRoute.builder()
                .accessToken(Mapbox.getAccessToken())
                .origin(start)
                .destination(end)
                .alternatives(false)
                .build()
                .getRoute(new Callback<DirectionsResponse>() {
                    @Override
                    public void onResponse(@NonNull Call<DirectionsResponse> call, @NonNull Response<DirectionsResponse> response) {

                        if (response.isSuccessful()) {

                            try {
                                assert response.body() != null;
                                routeodfgoh = response.body().routes().get(0);

                                if (navigationMapRoute != null) {
                                    navigationMapRoute.removeRoute();
                                } else {
                                    navigationMapRoute = new NavigationMapRoute(null, mapView, map);
                                }

                                //how to draw the map think map matching is work
                                navigationMapRoute.addRoute(routeodfgoh);

                            } catch (Exception ex) {
                                Toast.makeText(MainActivity.this, ex.toString(), Toast.LENGTH_LONG);
                            }

                        }
                    }

                    @Override
                    public void onFailure(Call<DirectionsResponse> call, Throwable t) {

                    }
                });*/

        //builds navigation route using waypoints
        int counter = 0;
        NavigationRoute.Builder builder = NavigationRoute.builder()
                .accessToken(Mapbox.getAccessToken())
                .origin(originPosition)
                .profile(DirectionsCriteria.PROFILE_DRIVING)
                .destination(destinationPosition);

        for (Point waypoint : coords) {
            counter++;
            if((counter % 2) != 0)
            {
                builder.addWaypoint(waypoint);
            }
        }

        builder.build().getRoute(new Callback<DirectionsResponse>() {
            @Override
            public void onResponse(@NonNull Call<DirectionsResponse> call, @NonNull Response<DirectionsResponse> response) {

                if(!response.isSuccessful()){
                    int test =  response.code();
                    String test2 = response.message();
                }
                else if (response.isSuccessful()) {

                    try {
                        routeodfgoh = response.body().routes().get(0);

                        if (navigationMapRoute != null) {
                            navigationMapRoute.removeRoute();
                        } else {
                            navigationMapRoute = new NavigationMapRoute(null, mapView, map);
                        }

                        //how to draw the map think map matching is work
                        navigationMapRoute.addRoute(routeodfgoh);

                    } catch (Exception ex) {
                        Toast.makeText(MainActivity.this, ex.toString(), Toast.LENGTH_LONG);
                    }
                }
            }

            @Override
            public void onFailure(@NonNull Call<DirectionsResponse> call, @NonNull Throwable t) {

               Log.e("help me","whhhhhyyyyyyyyyy",t);

            }
        });
    }
}

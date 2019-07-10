package com.route.client;

import android.content.Intent;
import android.support.v4.view.ViewPager;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.support.v7.widget.Toolbar;
import android.view.Menu;
import android.view.MenuInflater;
import android.view.MenuItem;
import android.widget.AdapterView;
import android.widget.TextView;
import android.widget.Toast;

import com.android.volley.Request;
import com.android.volley.RequestQueue;
import com.android.volley.Response;
import com.android.volley.VolleyError;
import com.android.volley.toolbox.JsonArrayRequest;
import com.android.volley.toolbox.JsonObjectRequest;
import com.android.volley.toolbox.StringRequest;
import com.android.volley.toolbox.Volley;
import com.google.gson.JsonArray;
import com.google.gson.JsonObject;
import com.google.gson.JsonParser;

import org.json.JSONArray;
import org.json.JSONObject;

import java.util.HashMap;
import java.util.Map;

public class HomeActivity extends AppCompatActivity {

    private ViewPager viewPager;
    private SlideAdaptor slideAdaptor;
    private JsonArray jobs;
    private RequestQueue queue;
    private String response;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_home);


        //adds cutsom action bar
        Toolbar toolbar = findViewById(R.id.toolbar);
        setSupportActionBar(toolbar);
        queue = Volley.newRequestQueue(this);

        //gets jobs for slides
        TextView textView = findViewById(R.id.userView);
        textView.setText(" Welcome: Craig Cheney             User Id: 10022");
        populate();
        queue.start();
    }


    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        MenuInflater inflater = getMenuInflater();
        inflater.inflate(R.menu.menu,menu);
        return super.onCreateOptionsMenu(menu);
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        return super.onOptionsItemSelected(item);
    }

    public void populate(){


        String url = "http://192.168.43.187:8080/AppServer_war_exploded/logic/jobs";

        // Request a string response
        StringRequest postRequest = new StringRequest(Request.Method.POST, url,
                new Response.Listener<String>() {
                    @Override
                    public void onResponse(String Response) {

                        try{
                            response = Response;
                            JsonParser parser = new JsonParser();
                            Object jsonObj = parser.parse(response);
                            jobs= (JsonArray) jsonObj;
                        }
                        catch (Exception ex){

                        }

                        if(jobs != null)
                        {
                            //creates and adds slide adaptor
                            viewPager = findViewById(R.id.viewpager);
                            slideAdaptor = new SlideAdaptor(HomeActivity.this,jobs);
                            viewPager.setAdapter(slideAdaptor);
                        }
                    }
                },
                new Response.ErrorListener() {
                    @Override
                    public void onErrorResponse(VolleyError error) {
                        error.printStackTrace();
                    }
                }
        ) {
            @Override
            protected Map<String, String> getParams()
            {
                Map<String, String> params = new HashMap<>();
                // the POST parameters:
                params.put("userid", "10028");
                return params;
            }
        };

        queue.add(postRequest);
    }

}

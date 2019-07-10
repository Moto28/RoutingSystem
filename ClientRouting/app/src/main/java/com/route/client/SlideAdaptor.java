package com.route.client;

import android.app.AlertDialog;
import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.graphics.Color;
import android.support.annotation.NonNull;
import android.support.v4.view.PagerAdapter;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.TextView;
import android.widget.Toast;

import com.android.volley.Request;
import com.android.volley.RequestQueue;
import com.android.volley.Response;
import com.android.volley.VolleyError;
import com.android.volley.toolbox.StringRequest;
import com.android.volley.toolbox.Volley;
import com.google.gson.JsonArray;
import com.google.gson.JsonObject;

import org.json.JSONObject;

import java.util.HashMap;
import java.util.Map;

import static com.mapbox.mapboxsdk.Mapbox.getApplicationContext;

public class SlideAdaptor extends PagerAdapter {

    private Context context;
    private LayoutInflater layoutInflater;
    private RequestQueue queue;
    private JsonArray jobs;


    //list of images
    public int[] lst_images ={
        R.drawable.route,
        R.drawable.route4,
        R.drawable.route2,
        R.drawable.route,
        R.drawable.route4,
        R.drawable.route2,
        R.drawable.route,
        R.drawable.route4,
        R.drawable.route2,
        R.drawable.route
    };


    //list of background colours
    public int[] lst_background ={
            Color.rgb(58,54,223),
            Color.rgb(65,100,251),
            Color.rgb(111,193,255),
            Color.rgb(58,54,223),
            Color.rgb(65,100,251),
            Color.rgb(111,193,255),
            Color.rgb(58,54,223),
            Color.rgb(65,100,251),
            Color.rgb(111,193,255),
            Color.rgb(58,54,223)
    };

    public SlideAdaptor(Context context,JsonArray Jobs){
        this.context = context;
        jobs = Jobs;
    }

    @Override
    public int getCount() {
        return jobs.size();
    }

    @Override
    public boolean isViewFromObject(@NonNull View view, @NonNull Object object) {
        return (view== (LinearLayout)object);
    }

    @NonNull
    @Override
    public Object instantiateItem(@NonNull ViewGroup container, final int position) {
        layoutInflater = (LayoutInflater) context.getSystemService(context.LAYOUT_INFLATER_SERVICE);
        View view = layoutInflater.inflate(R.layout.slide,container,false);
        LinearLayout layout = view.findViewById(R.id.linearlayoutS);
        ImageView imgslide =  view.findViewById((R.id.slideimg));
        TextView text = view.findViewById(R.id.texttitle);
        TextView text1 = view.findViewById(R.id.bus);
        TextView text2 = view.findViewById(R.id.start);
        TextView text3 = view.findViewById(R.id.end);
        TextView text4 = view.findViewById(R.id.date);
        layout.setBackgroundColor(lst_background[position]);
        imgslide.setImageResource(lst_images[position]);

        final JsonObject obj = jobs.get(position).getAsJsonObject();
        String job = obj.get("job_id").getAsString();
        String reg = obj.get("bus_reg").getAsString();
        String start = obj.get("start_location").getAsString();
        String end = obj.get("end_location").getAsString();
        String date = obj.get("Date_time").getAsString();
        String pass = obj.get("pass_num").getAsString();
        String complete = obj.get("is_complete").getAsString();

        text.setText("Job ID: "+job);
        text1.setText("Bus : "+ reg);
        text2.setText("Start : "+ start);
        text3.setText("End : "+ end);
        text4.setText("Date : "+ date);
        container.addView(view);

        view.setOnClickListener(new View.OnClickListener(){
            public void onClick(View v){

                // Build an AlertDialog
                AlertDialog.Builder builder = new AlertDialog.Builder(context);

                // Set a title for alert dialog
                builder.setTitle("Start Job "+"placeholder");

                // Ask the final question
                builder.setMessage("Are you sure you want to start this job?");

                // Set the alert dialog yes button click listener
                builder.setPositiveButton("Yes", new DialogInterface.OnClickListener() {
                    @Override
                    public void onClick(DialogInterface dialog, int which) {
                        // Do something when user clicked the Yes button
                        Intent myIntent = new Intent(context, MainActivity.class);
                        myIntent.putExtra("job", obj.get("route").getAsString());
                        context.startActivity(myIntent);

                    }
                });

                // Set the alert dialog no button click listener
                builder.setNegativeButton("No", new DialogInterface.OnClickListener() {
                    @Override
                    public void onClick(DialogInterface dialog, int which) {
                        // Do something when No button clicked
                        Toast.makeText(context,
                                "Selection cancelled",Toast.LENGTH_SHORT).show();
                    }
                });

                AlertDialog dialog = builder.create();
                // Display the alert dialog on interface
                dialog.show();
            }
        });


        return view;

    }

    @Override
    public void destroyItem(@NonNull ViewGroup container, int position, @NonNull Object object) {
       container.removeView((LinearLayout)object);
    }
}

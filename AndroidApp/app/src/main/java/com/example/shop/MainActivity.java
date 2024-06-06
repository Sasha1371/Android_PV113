package com.example.shop;

import android.os.Bundle;
import android.widget.ImageView;

import androidx.activity.EdgeToEdge;
import androidx.appcompat.app.AppCompatActivity;

import com.bumptech.glide.Glide;
import com.bumptech.glide.request.RequestOptions;

public class MainActivity extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        EdgeToEdge.enable(this);
        setContentView(R.layout.activity_main);

        ImageView imageView = (ImageView) findViewById(R.id.ivRegImage);

        Glide.with(this)
                .load("https://uxwing.com/wp-content/themes/uxwing/download/peoples-avatars/corporate-user-icon.png")
                .apply(new RequestOptions().override(400))
                .into(imageView);
    }
}
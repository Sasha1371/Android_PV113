package com.example.shop;

import android.os.Bundle;
import android.widget.ImageView;

import androidx.activity.EdgeToEdge;
import androidx.appcompat.app.AppCompatActivity;
import androidx.core.graphics.Insets;
import androidx.core.view.ViewCompat;
import androidx.core.view.WindowInsetsCompat;

import com.bumptech.glide.Glide;
import com.bumptech.glide.request.RequestOptions;

public class MainActivity extends BaseActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        EdgeToEdge.enable(this);
        setContentView(R.layout.activity_main);

        ViewCompat.setOnApplyWindowInsetsListener(findViewById(R.id.main), (v, insets) -> {
            Insets systemBars = insets.getInsets(WindowInsetsCompat.Type.systemBars());
            v.setPadding(systemBars.left, systemBars.top, systemBars.right, systemBars.bottom);
            return insets;
        });

        ImageView imageView = (ImageView) findViewById(R.id.ivRegImage);

//        String url = "http://10.0.2.2:5174/images/1.jpg";
        String url = "http://192.168.50.18:5174/images/1.jpg";
        Glide.with(this)
//                .load("https://uxwing.com/wp-content/themes/uxwing/download/peoples-avatars/corporate-user-icon.png")
                .load(url)
                .apply(new RequestOptions().override(400))
                .into(imageView);

        setupBottomNavigationView(R.id.bottom_navigation);
    }
}
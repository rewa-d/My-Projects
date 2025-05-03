package com.example.tictactoe;

import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;

import androidx.appcompat.app.AppCompatActivity;


public class HomePage extends AppCompatActivity {
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_home_page);

        Button multiplayerButton = findViewById(R.id.multiplayerButton);
        Button playWithAIButton = findViewById(R.id.playWithAIButton);

        multiplayerButton.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                // Navigate to AddPlayers activity for multiplayer mode
                Intent intent = new Intent(HomePage.this, AddPlayers.class);
                startActivity(intent);

            }
        });

        playWithAIButton.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                // Navigate to PlayWithAIActivity for AI mode
                Intent intent = new Intent(HomePage.this, AddPlayersAI.class);
                startActivity(intent);
            }
        });
    }
}

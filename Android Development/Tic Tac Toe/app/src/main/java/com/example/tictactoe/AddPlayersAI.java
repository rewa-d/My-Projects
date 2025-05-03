package com.example.tictactoe;

import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.Toast;

import androidx.appcompat.app.AppCompatActivity;

public class AddPlayersAI extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_add_players_ai); // New layout for single-player mode

        EditText playerOne = findViewById(R.id.playerOne); // Input for Player One name
        Button startGameButton = findViewById(R.id.startGameButton);

        startGameButton.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                String getPlayerOneName = playerOne.getText().toString();

                if (getPlayerOneName.isEmpty()) {
                    Toast.makeText(AddPlayersAI.this, "Please enter your name", Toast.LENGTH_SHORT).show();
                } else {
                    // Send Player One name to PlayWithAIActivity
                    Intent intent = new Intent(AddPlayersAI.this, PlayWithAIActivity.class);
                    intent.putExtra("playerOne", getPlayerOneName);
                    startActivity(intent);
                }
            }
        });
    }
}

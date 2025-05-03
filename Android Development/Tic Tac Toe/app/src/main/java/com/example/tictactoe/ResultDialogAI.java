package com.example.tictactoe;

import android.app.Dialog;
import android.content.Context;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.TextView;

import androidx.annotation.NonNull;

public class ResultDialogAI extends Dialog {

    private final String message;               // Result message (e.g., "Player 1 Wins!")
    private final PlayWithAIActivity activity;  // Reference to PlayWithAIActivity for restarting or exiting

    public ResultDialogAI(@NonNull Context context, String message, PlayWithAIActivity activity) {
        super(context);
        this.message = message;
        this.activity = activity;
    }

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_result_dialog_ai); // Ensure this XML file exists

        // Find views
        TextView messageText = findViewById(R.id.messageText);
        Button startAgainButton = findViewById(R.id.startAgainButton);
        Button exitButton = findViewById(R.id.exitButton);

        // Set the result message
        messageText.setText(message);

        // Restart Button Logic
        startAgainButton.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                dismiss();             // Close the dialog
                activity.restartMatch(); // Restart the match by calling PlayWithAIActivity's method
            }
        });

        // Exit Button Logic
        exitButton.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                dismiss();             // Close the dialog
                activity.finish();     // Finish PlayWithAIActivity and exit the game

            }
        });
    }
}

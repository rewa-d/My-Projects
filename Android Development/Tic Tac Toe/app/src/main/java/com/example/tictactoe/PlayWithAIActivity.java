package com.example.tictactoe;

import android.content.Intent;
import android.os.Bundle;
import android.widget.ImageView;

import androidx.appcompat.app.AppCompatActivity;

import com.example.tictactoe.databinding.ActivityMainBinding;

import java.util.ArrayList;
import java.util.List;

public class PlayWithAIActivity extends AppCompatActivity {

    ActivityMainBinding binding;
    private final List<int[]> combinationList = new ArrayList<>();
    private int[] boxPositions = {0, 0, 0, 0, 0, 0, 0, 0, 0}; // 9 zeros
    private int playerTurn = 1; // 1 = Player, 2 = AI
    private int totalSelectedBoxes = 1;
    private String playerOneName = "Player One";

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        binding = ActivityMainBinding.inflate(getLayoutInflater());
        setContentView(binding.getRoot());

        // Get Player One's name from intent
        Intent intent = getIntent();
        playerOneName = intent.getStringExtra("playerOne");
        if (playerOneName == null || playerOneName.isEmpty()) {
            playerOneName = "Player One";
        }

        binding.playerOneName.setText(playerOneName);
        binding.playerTwoName.setText("AI"); // AI fixed name

        // Initialize winning combinations inline
        combinationList.add(new int[]{0, 1, 2});
        combinationList.add(new int[]{3, 4, 5});
        combinationList.add(new int[]{6, 7, 8});
        combinationList.add(new int[]{0, 3, 6});
        combinationList.add(new int[]{1, 4, 7});
        combinationList.add(new int[]{2, 5, 8});
        combinationList.add(new int[]{0, 4, 8});
        combinationList.add(new int[]{2, 4, 6});

        setOnClickListeners();
    }

    private void setOnClickListeners() {
        binding.image1.setOnClickListener(view -> handlePlayerMove((ImageView) view, 0));
        binding.image2.setOnClickListener(view -> handlePlayerMove((ImageView) view, 1));
        binding.image3.setOnClickListener(view -> handlePlayerMove((ImageView) view, 2));
        binding.image4.setOnClickListener(view -> handlePlayerMove((ImageView) view, 3));
        binding.image5.setOnClickListener(view -> handlePlayerMove((ImageView) view, 4));
        binding.image6.setOnClickListener(view -> handlePlayerMove((ImageView) view, 5));
        binding.image7.setOnClickListener(view -> handlePlayerMove((ImageView) view, 6));
        binding.image8.setOnClickListener(view -> handlePlayerMove((ImageView) view, 7));
        binding.image9.setOnClickListener(view -> handlePlayerMove((ImageView) view, 8));
    }

    private void handlePlayerMove(ImageView imageView, int position) {
        if (isBoxSelectable(position)) {
            performAction(imageView, position, 1); // Player's move

            if (checkResults()) {
                showResultDialog(playerOneName + " Wins!");
                return;
            } else if (totalSelectedBoxes == 9) {
                showResultDialog("Match Draw!");
                return;
            }

            // AI's turn
            performAIMove();
        }
    }

    private void performAIMove() {
        int bestMove = findBestMove();

        if (bestMove != -1) {
            performAction(getImageViewByPosition(bestMove), bestMove, 2); // AI move
        }
    }

    private void performAction(ImageView imageView, int position, int currentPlayer) {
        boxPositions[position] = currentPlayer; // Mark the position
        imageView.setImageResource(currentPlayer == 1 ? R.drawable.ximage : R.drawable.oimage); // Set image

        totalSelectedBoxes++;

        if (checkResults()) {
            String winner = (currentPlayer == 1) ? "Player Wins!" : "AI Wins!";
            showResultDialog(winner);
        } else if (totalSelectedBoxes == 9) {
            showResultDialog("Match Draw!");
        }
    }


    private boolean checkResults() {
        for (int[] combination : combinationList) {
            if (boxPositions[combination[0]] != 0 &&
                    boxPositions[combination[0]] == boxPositions[combination[1]] &&
                    boxPositions[combination[1]] == boxPositions[combination[2]]) {
                return true;
            }
        }
        return false;
    }

    public void restartMatch() {
        boxPositions = new int[]{0, 0, 0, 0, 0, 0, 0, 0, 0}; // Reset the board
        playerTurn = 1; // Set turn back to Player
        totalSelectedBoxes = 1;

        // Reset all grid images
        binding.image1.setImageResource(R.drawable.white_box);
        binding.image2.setImageResource(R.drawable.white_box);
        binding.image3.setImageResource(R.drawable.white_box);
        binding.image4.setImageResource(R.drawable.white_box);
        binding.image5.setImageResource(R.drawable.white_box);
        binding.image6.setImageResource(R.drawable.white_box);
        binding.image7.setImageResource(R.drawable.white_box);
        binding.image8.setImageResource(R.drawable.white_box);
        binding.image9.setImageResource(R.drawable.white_box);

        // Reset the player and AI layout highlights
        binding.playerOneLayout.setBackgroundResource(R.drawable.black_border);
        binding.playerTwoLayout.setBackgroundResource(R.drawable.white_box);
    }


    private int findBestMove() {
        int bestValue = Integer.MIN_VALUE;
        int bestMove = -1;

        for (int i = 0; i < 9; i++) {
            if (boxPositions[i] == 0) {
                boxPositions[i] = 2; // AI move
                int moveValue = minimax(0, false);
                boxPositions[i] = 0; // Undo move

                if (moveValue > bestValue) {
                    bestMove = i;
                    bestValue = moveValue;
                }
            }
        }
        return bestMove;
    }

    private int minimax(int depth, boolean isMaximizing) {
        int score = evaluate();
        if (score == 10) return score;
        if (score == -10) return score;
        if (totalSelectedBoxes == 9) return 0;

        if (isMaximizing) {
            int best = Integer.MIN_VALUE;
            for (int i = 0; i < 9; i++) {
                if (boxPositions[i] == 0) {
                    boxPositions[i] = 2;
                    best = Math.max(best, minimax(depth + 1, false));
                    boxPositions[i] = 0;
                }
            }
            return best;
        } else {
            int best = Integer.MAX_VALUE;
            for (int i = 0; i < 9; i++) {
                if (boxPositions[i] == 0) {
                    boxPositions[i] = 1;
                    best = Math.min(best, minimax(depth + 1, true));
                    boxPositions[i] = 0;
                }
            }
            return best;
        }
    }

    private int evaluate() {
        for (int[] combination : combinationList) {
            if (boxPositions[combination[0]] == boxPositions[combination[1]] &&
                    boxPositions[combination[1]] == boxPositions[combination[2]]) {
                if (boxPositions[combination[0]] == 2) return 10; // AI win
                if (boxPositions[combination[0]] == 1) return -10; // Player win
            }
        }
        return 0;
    }

    private ImageView getImageViewByPosition(int position) {
        switch (position) {
            case 0: return binding.image1;
            case 1: return binding.image2;
            case 2: return binding.image3;
            case 3: return binding.image4;
            case 4: return binding.image5;
            case 5: return binding.image6;
            case 6: return binding.image7;
            case 7: return binding.image8;
            case 8: return binding.image9;
            default: return null;
        }
    }

    private boolean isBoxSelectable(int position) {
        return boxPositions[position] == 0;
    }

    private void showResultDialog(String message) {
        ResultDialogAI resultDialog = new ResultDialogAI(PlayWithAIActivity.this, message, PlayWithAIActivity.this);
        resultDialog.setCancelable(false);
        resultDialog.show();
    }
}

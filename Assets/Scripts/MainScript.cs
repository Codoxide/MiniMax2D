using System;
using UnityEngine;
using UnityEngine.UI;
public class MainScript : MonoBehaviour
{
   public Button[] buttons;

    char[] board = new char[9];
    bool playerTurn = true;   // Player = X, AI = O

    void Start()
    {
        for (int i = 0; i < 9; i++)
        {
            board[i] = ' ';
            buttons[i].transform.GetChild(0).GetComponent<Text>().text = "";
        }
    }

    // PLAYER CLICK
    public void PlayerMove(int index)
    {
        if (playerTurn && board[index] == ' ')
        {
            SetMove(index, 'X');
            playerTurn = false;

            if (!GameOver())
                Invoke("AIMove", 0.3f);
        }


    }

    // AI MOVE
    void AIMove()
    {
        int move = GetBestMove();
        SetMove(move, 'O');
        playerTurn = true;
        Console.WriteLine(GameOver());
    }

    void SetMove(int index, char symbol)
    {
        board[index] = symbol;
        buttons[index].GetComponentInChildren<Text>().text = symbol.ToString();
        buttons[index].interactable = false;
    }

    // ================= GAME STATE =================

    bool GameOver()
    {
        if (CheckWin('X'))
        {
            Debug.Log("Player Wins");
            DisableButtons();
            return true;
        }

        if (CheckWin('O'))
        {
            Debug.Log("AI Wins");
            DisableButtons();
            return true;
        }

        if (IsDraw())
        {
            Debug.Log("Draw");
            return true;
        }

        return false;
    }

    void DisableButtons()
    {
        foreach (Button b in buttons)
            b.interactable = false;
    }

    // ================= MINIMAX =================

    int GetBestMove()
    {
        int bestScore = -10000;
        int move = 0;

        for (int i = 0; i < 9; i++)
        {
            if (board[i] == ' ')
            {
                board[i] = 'O';
                int score = Minimax(false);
                board[i] = ' ';
                if (score > bestScore)
                {
                    bestScore = score;
                    move = i;
                }
            }
        }
        return move;
    }

    int Minimax(bool aiTurn)
    {
        if(CheckWin('X'))
        {
            return -1;
        }
        if(CheckWin('O'))
        {
            return 1;
        }
        if(IsDraw())
        {
            return 0;
        }

        if(aiTurn == true)
        {
            int bestScore = -100000;
            for(int i = 0; i < 9; i++)
            {
                if(board[i] == ' ')
                {
                    board[i] = 'O';
                    bestScore = Math.Max(bestScore, Minimax(false));
                    board[i] = ' ';
                }

            }
            return bestScore;
        }
        else
        {
            
            int bestScore = 100000;
            for(int i = 0; i < 9; i++)
            {
                if(board[i] == ' ')
                {
                    board[i] = 'X';
                    bestScore = Math.Min(bestScore, Minimax(true));
                    board[i] = ' ';
                }

            }
            return bestScore;
        }
    }


    bool CheckWin(char p)
    {
        int[,] winLines = {
            {0,1,2},{3,4,5},{6,7,8},
            {0,3,6},{1,4,7},{2,5,8},
            {0,4,8},{2,4,6}
        };

        for (int i = 0; i < 8; i++)
            if (board[winLines[i,0]] == p && board[winLines[i,1]] == p && board[winLines[i,2]] == p) 
                return true;

        return false;
    }

    bool IsDraw()
    {
        foreach (char c in board)
            if (c == ' ') return false;
        return true;
    }
    
}

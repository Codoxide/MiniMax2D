<div align="center">

# 🤖 Minimax Games

Tic Tac Toe and Connect 4 against an AI that uses the minimax algorithm. Turn the search depth up or down and see how much smarter it gets.

![Unity](https://img.shields.io/badge/Unity-6-black?logo=unity)
![C#](https://img.shields.io/badge/C%23-512BD4?logo=csharp&logoColor=white)
![Algorithm](https://img.shields.io/badge/algorithm-minimax-orange)
![visitors](https://visitor-badge.laobi.icu/badge?page_id=Codoxide.Minimax-Games)

</div>

---

## Preview

| Tic Tac Toe | Connect 4 |
|:---:|:---:|
| ![Tic Tac Toe](screenshots/tictactoe.png) | ![Connect 4](screenshots/connect4.png) |

## Features

- **Two games**: Tic Tac Toe and Connect 4, each in its own scene
- **Minimax AI** that looks ahead at possible moves before choosing one
- **Adjustable depth** to control how many moves ahead the AI looks
- **Play against it** and watch how the AI's choices change as the depth changes

## How It Works

Minimax treats the game as a tree of possible futures. On its turn, the AI:

1. Tries every legal move
2. For each one, imagines every reply you could make, then every reply it could make, and so on
3. Stops after a set number of moves, called the **depth**, and scores each position
4. Assumes you'll always pick your best move, and picks the move that leaves it in the best position anyway

The AI maximizes its own score while assuming the player minimizes it, which is where the name comes from.

## Depth

Depth is how many moves ahead the AI looks. Higher depth means a stronger AI, but every extra level multiplies the number

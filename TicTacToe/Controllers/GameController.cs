using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using TicTacToe.Models;
using System.Collections.Generic;
using System.Linq;

namespace TicTacToe.Controllers
{
        [ApiController]
        [Route("api/[controller]")]
        [ApiVersion("1.0")] 
        public class GameController : ControllerBase
        {
            private readonly List<Game> _games = new List<Game>();
            private int _currentId = 1;

            [HttpGet]
            public IActionResult Get()
            {
                return Ok(_games);
            }

            [HttpGet("{id}")]
            public IActionResult Get(int id)
            {
                var game = _games.FirstOrDefault(g => g.Id == id);
                if (game == null)
                {
                    return NotFound();
                }
                return Ok(game);
            }

            [HttpPost]
            public IActionResult CreateGame(Game game)
            {
                game.Id = _currentId++;
                game.Board = new string[3, 3] { { "-", "-", "-" }, { "-", "-", "-" }, { "-", "-", "-" } };
                game.CurrentPlayer = game.Player1;
                game.Player1 = "Игрок 1";
                game.Player2 = "Игрок 2";
                _games.Add(game);
                return CreatedAtAction(nameof(Get), new { id = game.Id }, game);
            }

            [HttpPost("{id}/move")]
            public IActionResult MakeMove(int id, [FromBody] Move move)
            {
                var game = _games.FirstOrDefault(g => g.Id == id);
                if (game == null)
                {
                    return NotFound();
                }
                if (game.CurrentPlayer != move.Player)
                {
                    return BadRequest("Сейчас ход соперника!");
                }
                if ((game.Board[move.Row, move.Column]) != "-")
                {
                    return BadRequest("Неправильный ход");
                }

                game.Board[move.Row, move.Column] = move.Player == game.Player1 ? "X" : "O";
                game.Moves.Add(move);
                game.CurrentPlayer = game.CurrentPlayer == game.Player1 ? game.Player2 : game.Player1;

                var winner = CheckWinner(game.Board);
                if (winner != "-")
                {
                    game.Winner = winner;
                }

                return Ok(game);
            }

            private string CheckWinner(string[,] board)
            {
                for (int row = 0; row < 3; row++)
                {
                    if (board[row, 0] != "-" && board[row, 0] == board[row, 1] && board[row, 1] == board[row, 2])
                    {
                        return board[row, 0];
                    }
                }

                for (int col = 0; col < 3; col++)
                {
                    if (board[0, col] != "-" && board[0, col] == board[1, col] && board[1, col] == board[2, col])
                    {
                        return board[0, col];
                    }
                }

                if (board[0, 0] != "-" && board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2])
                {
                    return board[0, 0];
                }

                if (board[0, 2] != "-" && board[0, 2] == board[1, 1] && board[1, 1] == board[2, 0])
                {
                    return board[0, 2];
                }

                if (!board.Cast<string>().Any(p => p == "-"))
                {
                    return "Ничья";
                }

                return "-";
            }
        }
}

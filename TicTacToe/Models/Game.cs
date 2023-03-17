namespace TicTacToe.Models
{
    public class Game
    { 
        // Определяем переменные для модели Game
        public int Id { get; set; }
        public string Player1 { get; set; }
        public string Player2 { get; set; }
        public string CurrentPlayer { get; set; }
        public string Winner { get; set; }
        public string[,] Board { get; set; }

        public List<Move> Moves { get; set; } = new List<Move>();
    }
}

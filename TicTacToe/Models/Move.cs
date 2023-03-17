namespace TicTacToe.Models
{
    public class Move
    {
        // Определяем переменные для модели Move
        public int GameId { get; set; }
        public string Player { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
    }
}

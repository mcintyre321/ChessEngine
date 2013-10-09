namespace ChessEngine
{
    public class ChessPiece
    {
        public Square Square { get; internal set; }
        public PieceType PieceType {get; internal set;}
        public PieceColour PieceColour {get; internal set;}
        public bool HasMoved { get; set; }
    }
}
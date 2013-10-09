using System.Collections.Generic;
using System.Linq;

namespace ChessEngine
{
    public class Square
    {
        public int X{get; internal set;}
        public int Y{get; internal set;}
        public ChessPiece Piece { get; internal set; }
        public ChessBoard Board { get; internal set; }
        public IEnumerable<Square> GetMoves()
        {
            if (this.Piece == null) return Enumerable.Empty<Square>();
            return this.Piece.PieceType.GetMoves(Piece).Where (sq => sq != null);
        }
    }
}
using System.Collections.Generic;
using System.Linq;

namespace ChessEngine
{
    static class RookMovement
    {
        public static IEnumerable<Square> GetMoves(Piece piece) 
        {
            return 		piece.Square.Walk(x:  1, allow: piece.CanEnter()).TakeWhile (s => s != null)
                   		     .Concat(piece.Square.Walk(x: -1, allow: piece.CanEnter()).TakeWhile (s => s != null))
                   		     .Concat(piece.Square.Walk(y: 1, allow: piece.CanEnter()).TakeWhile (s => s != null))
                   		     .Concat(piece.Square.Walk(y: -1, allow: piece.CanEnter()).TakeWhile (s => s != null));
        }
    }
}
﻿using System.Collections.Generic;
using System.Linq;

namespace ChessEngine
{
    static class BishopMovement
    {
        public static IEnumerable<Square> GetMoves(ChessPiece piece) 
        {
            return 		piece.Square.Walk(x:  1, y : 1, allow: piece.CanEnter()).TakeWhile (s => s != null)
                   		     .Concat(piece.Square.Walk(x: -1, y : 1, allow: piece.CanEnter()).TakeWhile (s => s != null))
                   		     .Concat(piece.Square.Walk(x:  1, y: -1, allow: piece.CanEnter()).TakeWhile (s => s != null))
                   		     .Concat(piece.Square.Walk(x: -1, y: -1, allow: piece.CanEnter()).TakeWhile (s => s != null));
        }
    }
}
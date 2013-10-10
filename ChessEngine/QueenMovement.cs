using System;
using System.Collections.Generic;
using System.Linq;

namespace ChessEngine
{
    static class QueenMovement
    {
        public static IEnumerable<Square> GetMoves(ChessPiece piece)
        {
            return ChessEngine.Directions.Compass.SelectMany(dir => 
                piece.Square.Walk(dir.Item1, dir.Item2, allow: piece.CanEnter()).TakeWhile(s => s != null)
            );
        }
    }
}
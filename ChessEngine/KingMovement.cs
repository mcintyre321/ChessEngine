using System;
using System.Collections.Generic;

namespace ChessEngine
{
    static class KingMovement
    {
        public static IEnumerable<Square> GetMoves(Piece king)
        {
            foreach (var adjacentSquare in king.Square.AdjacentSquares(allow:  king.CanEnter()))
            {
                yield return adjacentSquare;
            }

          
        }
    }
}
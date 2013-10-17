using System;
using System.Collections.Generic;

namespace ChessEngine
{
    static class KnightMovement
    {
        public static IEnumerable<Square> GetMoves(Piece piece)
        {
            var square = piece.Square;
            var canEnter = piece.CanEnter();
            return GetMoves(square, canEnter);
        }

        public static IEnumerable<Square> GetMoves(Square square, Func<Square, bool> canEnter)
        {
            yield return square.Nav(x: 1, y: 2, allow: canEnter);
            yield return square.Nav(x: 2, y: 1, allow: canEnter);
            yield return square.Nav(x: 2, y: -1, allow: canEnter);
            yield return square.Nav(x: 1, y: -2, allow: canEnter);
            yield return square.Nav(x: -1, y: -2, allow: canEnter);
            yield return square.Nav(x: -2, y: -1, allow: canEnter);
            yield return square.Nav(x: -2, y: 1, allow: canEnter);
            yield return square.Nav(x: -1, y: 2, allow: canEnter);
        }
    }
}
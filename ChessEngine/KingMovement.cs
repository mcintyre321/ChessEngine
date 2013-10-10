using System;
using System.Collections.Generic;

namespace ChessEngine
{
    static class KingMovement
    {
        public static IEnumerable<Square> GetMoves(Piece king)
        {
            var opponentColour = king.Colour.Opponent;
            Func<Square, bool> kingIsAllowedToEnterASquare = sq => king.CanEnter()(sq) && !sq.IsThreatenedBy(opponentColour);
            foreach (var adjacentSquare in king.Square.AdjacentSquares(allow: kingIsAllowedToEnterASquare))
            {
                yield return adjacentSquare;
            }

            if (!king.HasMoved)
            {	
                var leftRook = king.Square.Nav(x: -4).Piece;
                if (leftRook != null && !leftRook.HasMoved)
                {
                    if (king.Square.Nav(x: -3).Piece == null
                        &&  king.Square.Nav(x: -2).Piece == null
                        && !king.Square.Nav(x: -2).IsThreatenedBy(opponentColour)
                        &&  king.Square.Nav(x: -1).Piece == null
                        && !king.Square.Nav(x: -1).IsThreatenedBy(opponentColour)
                        ) 	yield return king.Square.Nav(x: -2);
                }
                var rightRook = king.Square.Nav(x: 3).Piece;
                if (rightRook != null && !rightRook.HasMoved)
                {
                    if (king.Square.Nav(x: 2).Piece == null
                        && !king.Square.Nav(x: 2).IsThreatenedBy(opponentColour)
                        && king.Square.Nav(x: 1).Piece == null
                        && !king.Square.Nav(x: 1).IsThreatenedBy(opponentColour)
                        ) yield return king.Square.Nav(x: -2);
                }
			
            }
        }
    }
}
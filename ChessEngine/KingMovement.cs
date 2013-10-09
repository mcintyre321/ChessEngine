using System.Collections.Generic;

namespace ChessEngine
{
    static class KingMovement
    {
        public static IEnumerable<Square> GetMoves(ChessPiece king)
        {
            var opponentColour = king.PieceColour == PieceColour.White ? PieceColour.Black : PieceColour.White;
            for (var x = -1; x < 2; x++)
            {
                for (var y = -1; y < 2; y++)
                {
                    if (x == 0 && y == 0) continue;
                    yield return king.Square.Nav(
                        x: x,
                        y: y,
                        allow: sq => king.CanEnter()(sq) && !king.Square.IsThreatenedBy(opponentColour)
                        );
                }
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
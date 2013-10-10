using System;
using System.Collections.Generic;
using System.Linq;

namespace ChessEngine
{
    static class Extensions
    {
        public static bool IsOpponentOf(this ChessPiece potentialOppenent, ChessPiece piece)
        {
            if (potentialOppenent == null) return false;
            return potentialOppenent.PieceColour != piece.PieceColour;
        }

        public static Func<Square, bool> CanEnter(this ChessPiece piece)
        {
            return sq => sq.Piece == null || sq.Piece.IsOpponentOf(piece);
        }
        public static bool IsThreatenedBy(this Square square, PieceColour enemyColour)
        {
            var enemyKing = square.Game.Pieces.Single(p => p.PieceType == PieceType.King && p.PieceColour == enemyColour);
            var threatenedByNonKingPieces = square.Game.Pieces.Where(p => p.PieceColour == enemyColour).Where(p => p.PieceType != PieceType.King) //would get unpleasantly recursive if we wen't down that route...
                                   .SelectMany(p => p.GetMoves(getThreats: true));
            var threatenedByKing = enemyKing.Square.AdjacentSquares();
            return threatenedByNonKingPieces.Concat(threatenedByKing).Contains(square);
        }

	
        public static Square Nav(this Square square, int x = 0, int y = 0, Func<Square, bool> allow = null)
        {
            allow = allow ?? (sq => true);
            if (square == null) return null;
            if ((square.X + x) > 7 || (square.Y + y) > 7) return null;
            if ((square.X + x) < 0 || (square.Y + y) < 0) return null;
            var newSquare = square.Game.Board[square.X + x][square.Y + y];
            return allow(newSquare) ? newSquare : null;
        }  
	
        public static IEnumerable<Square> Walk(this Square square, int x = 0, int y = 0, Func<Square, bool> allow = null)
        {
            Square next = square;
            do{
                next = next.Nav(x, y, allow);
                yield return next;
            }while(next != null);
        }
        public static IEnumerable<Square> AdjacentSquares(this Square square, Func<Square, bool> allow = null)
        {
            return ChessEngine.Directions.Compass.Select(d => square.Nav(x: d.Item1, y: d.Item2, allow: allow));
        }
    }

    internal class Directions
    {
        public static IEnumerable<Tuple<int, int>> Compass
        {
            get
            {
                for (var x = -1; x < 2; x++)
                {
                    for (var y = -1; y < 2; y++)
                    {
                        if ((x == 0) && (y == 0)) continue;
                        yield return Tuple.Create(x, y);
                    }
                }
            }
        }
    }
}
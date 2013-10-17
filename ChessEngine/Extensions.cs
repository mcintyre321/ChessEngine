using System;
using System.Collections.Generic;
using System.Linq;

namespace ChessEngine
{
    static class Extensions
    {
        public static bool IsOpponentOf(this Piece potentialOppenent, Piece piece)
        {
            if (potentialOppenent == null) return false;
            return potentialOppenent.Colour != piece.Colour;
        }

        public static Func<Square, bool> CanEnter(this Piece piece)
        {
            return sq => sq.Piece == null || sq.Piece.IsOpponentOf(piece);
        }

        public static bool IsThreatenedBy(this Square square, PieceColour enemyColour)
        {
            return square.Game.Pieces.Where(p => p.Colour == enemyColour && p.PieceType != PieceType.King)
                         .SelectMany(p => p.ThreatenedSquares)
                         .Any(ts => ts == square);
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
        public static IEnumerable<Square> Walk(this Square square, Tuple<int, int> direction, Func<Square, bool> allow = null)
        {
            return square.Walk(direction.Item1, direction.Item2, allow);
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
        public static IEnumerable<T> TakeUpTo<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            foreach (T item in source)
            {
                yield return item;
                if (predicate(item))
                {
                    yield break;
                }
            }
        } 

        public static IEnumerable<Check> GetChecks(this Piece king)
        {
            Func<IEnumerable<Square>, bool> isCheck = path =>
            {
                var firstPieceInDirection = path.Where(sq => sq != null)
                    .Select(sq => sq.Piece).
                    FirstOrDefault(p => p != null);
                return firstPieceInDirection != null &&
                       firstPieceInDirection.ThreatenedSquares.Contains(king.Square);
            };
            var checkDirections = Directions.Compass.Select(d => king.Square.Walk(d));
            var checkPaths = checkDirections.Where(isCheck).Select(cd => cd.TakeUpTo(sq => sq.Piece != null));
            foreach (var checkPath in checkPaths)
            {
                yield return new Check() {SquaresThatCanBeMovedToInOrderToBreakCheck = checkPath};
            }
            
            var squareContainingCheckingKnight = KnightMovement.GetMoves(king.Square,
                                                         sq =>
                                                         sq.Piece != null &&
                                                         sq.Piece.Colour == king.Colour.Opponent &&
                                                         sq.Piece.PieceType == PieceType.Knight).SingleOrDefault(x => x != null);
            if (squareContainingCheckingKnight != null)
            {
                yield return new Check() {SquaresThatCanBeMovedToInOrderToBreakCheck = new[] {squareContainingCheckingKnight}};
            }
        }
    }

    internal class Check
    {
        public IEnumerable<Square> SquaresThatCanBeMovedToInOrderToBreakCheck { get; internal set; }
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
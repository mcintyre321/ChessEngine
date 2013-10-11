using System;
using System.Collections.Generic;
using System.Linq;

namespace ChessEngine
{
    public class Piece
    {
        public Square Square { get; internal set; }
        public PieceType PieceType { get; internal set; }
        public PieceColour Colour { get; internal set; }
        public bool HasMoved { get; set; }


        public IEnumerable<Square> PotentialMoves()
        {
            if (Square.Game.ColourWhoseTurnItIs != this.Colour) return Enumerable.Empty<Square>();
            var potentialMoves = ThreatenedSquares;
            if (this.PieceType == PieceType.King)
            {
                potentialMoves = potentialMoves.Where(sq => !sq.IsThreatenedBy(Colour.Opponent)).ToArray();
                potentialMoves = potentialMoves.Concat(GetCastlingMoves());
            }
            else
            {
                var king = Square.Game.Pieces.Single(p => p.Colour == this.Colour && p.PieceType == PieceType.King);
                var pin = Directions.Compass.Select(d => king.Square.Walk(d.Item1, d.Item2)).SingleOrDefault(IsPin);
                if (pin != null) potentialMoves = potentialMoves.Intersect(pin);
                //var squaresContainingCheckingPieces = Square.Game.EnemiesOf(this)
                //                                            .Where(p => p.GetMoves(true).Contains(king.Square))
                //                                            .Select(p => p.Square).ToArray();
                //if (squaresContainingCheckingPieces.Any()) potentialMoves = potentialMoves.Intersect(squaresContainingCheckingPieces);
            }
            return potentialMoves;
        }

        private IEnumerable<Square> GetCastlingMoves()
        {
            if (!this.HasMoved)
            {
                var leftRook = this.Square.Nav(x: -4).Piece;
                if (leftRook != null && !leftRook.HasMoved)
                {
                    if (this.Square.Nav(x: -3).Piece == null
                        && this.Square.Nav(x: -2).Piece == null
                        && this.Square.Nav(x: -1).Piece == null
                        && !this.Square.Nav(x: -2).IsThreatenedBy(this.Colour.Opponent)
                        && !this.Square.Nav(x: -1).IsThreatenedBy(this.Colour.Opponent)
                        ) yield return this.Square.Nav(x: -2);
                }
                var rightRook = this.Square.Nav(x: 3).Piece;
                if (rightRook != null && !rightRook.HasMoved)
                {
                    if (this.Square.Nav(x: 2).Piece == null
                        && this.Square.Nav(x: 1).Piece == null
                        && !this.Square.Nav(x: 2).IsThreatenedBy(this.Colour.Opponent)
                        && !this.Square.Nav(x: 1).IsThreatenedBy(this.Colour.Opponent)
                        ) yield return this.Square.Nav(x: 2);
                }
            }
        }

        internal IEnumerable<Square> ThreatenedSquares
        {
            get { return this.PieceType.GetThreatenedSquares(this).Where(sq => sq != null); }
        }


        private bool IsPin(IEnumerable<Square> squaresFromKing)
        {
            var piecesOnPath = squaresFromKing.Where(sq => sq != null && sq.Piece != null).Take(2).ToArray();
            if (piecesOnPath.Length == 2)
            {
                var thisPieceIsBetweenKingAndEnemy = piecesOnPath[0].Piece == this;
                var nextPieceIsEnemy = piecesOnPath[1].Piece.Colour != this.Colour;
                var nextPieceIsPieceThatCanPin = PieceType.PinningTypes.Contains(piecesOnPath[1].Piece.PieceType);
                if (thisPieceIsBetweenKingAndEnemy && nextPieceIsEnemy && nextPieceIsPieceThatCanPin)
                    return true;
            }
            return false;
        }
    }
}
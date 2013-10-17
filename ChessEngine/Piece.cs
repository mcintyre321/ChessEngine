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


        public IEnumerable<Move> PotentialMoves()
        {
            if (Square.Game.ColourWhoseTurnItIs != this.Colour) return Enumerable.Empty<Move>();
            var moveableSquares = ThreatenedSquares;
            if (this.PieceType == PieceType.King)
            {
                moveableSquares = moveableSquares.Where(sq => !sq.IsThreatenedBy(Colour.Opponent)).ToArray();
                moveableSquares = moveableSquares.Concat(GetCastlingMoves());
            }
            else
            {
                var king = Square.Game.Pieces.Single(p => p.Colour == this.Colour && p.PieceType == PieceType.King);
                var pin = Directions.Compass.Select(d => king.Square.Walk(d)).SingleOrDefault(IsPin);
                if (pin != null) moveableSquares = moveableSquares.Intersect(pin);

                 
                var checks = king.GetChecks();
                foreach (var check in checks)
                {
                    moveableSquares = moveableSquares.Intersect(check.SquaresThatCanBeMovedToInOrderToBreakCheck);
                }
            }
            var moves = moveableSquares.Select(square => new Move() { Destination = square, Piece = this });

            if (this.PieceType == PieceType.Pawn && this.Square.Y == 6) //all moves must be to back row
            {
                moves = moves.SelectMany((move) => PieceType.PromotionTypes.Select(pt => new Move() { Destination = move.Destination, Piece = move.Piece, PromotionType = pt }));
            }

            return moves;

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
﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace ChessEngine
{
    public class Piece
    {
        public Square Square { get; internal set; }
        public PieceType PieceType {get; internal set;}
        public PieceColour PieceColour {get; internal set;}
        public bool HasMoved { get; set; }

        public IEnumerable<Square> GetMoves(bool getThreats)
        {
            if (!getThreats && Square.Game.ColourWhoseTurnItIs != this.PieceColour) return Enumerable.Empty<Square>();
            var moveableSquares = this.PieceType.GetMoves(this).Where(sq => sq != null);
            if (this.PieceType == PieceType.King)
            {
                moveableSquares = moveableSquares.Where(sq => getThreats || !sq.IsThreatenedBy(PieceColour.Opponent)).ToArray();
            }
            else
            {
                if (!getThreats)
                {
                    var king = Square.Game.Pieces.Single(p => p.PieceColour == this.PieceColour && p.PieceType == PieceType.King);
                    var pin = ChessEngine.Directions.Compass.Select(d => king.Square.Walk(d.Item1, d.Item2)).SingleOrDefault(IsPin);
                    if (pin != null) moveableSquares = moveableSquares.Intersect(pin);

                }
            }
            return moveableSquares;
        }
 

        private bool IsPin(IEnumerable<Square> squaresFromKing)
        {
            var piecesOnPath = squaresFromKing.Where(sq => sq != null && sq.Piece != null).Take(2).ToArray();
            if (piecesOnPath.Length == 2)
            {
                var thisPieceIsBetweenKingAndEnemy = piecesOnPath[0].Piece == this;
                var nextPieceIsEnemy = piecesOnPath[1].Piece.PieceColour != this.PieceColour;
                var nextPieceIsPieceThatCanPin = PieceType.PinningTypes.Contains(piecesOnPath[1].Piece.PieceType);
                if (thisPieceIsBetweenKingAndEnemy && nextPieceIsEnemy && nextPieceIsPieceThatCanPin)
                    return true;
            }
            return false;
        }
    }
}
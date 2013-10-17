using System.Collections.Generic;
using System.Linq;

namespace ChessEngine
{
    static class PawnMovement
    {
        public static IEnumerable<Square> GetMoves(Piece pawn) 
        {
            var forwardOne = pawn.Square.Nav(y: pawn.Colour.Direction, allow: sq => sq.Piece == null);
            if (forwardOne != null)
            {
                yield return forwardOne;
                if(pawn.Square.Y == pawn.Colour.PawnRow){
                    yield return forwardOne.Nav(y: pawn.Colour.Direction, allow: sq => sq.Piece == null);
                }
            }
		
            //taking
            yield return pawn.Square.Nav(y: pawn.Colour.Direction, x:-1, allow: sq => sq.Piece.IsOpponentOf(pawn));
            yield return pawn.Square.Nav(y: pawn.Colour.Direction, x: 1, allow: sq => sq.Piece.IsOpponentOf(pawn));
		
            //en passant...
            if (pawn.Square.Y == (pawn.Colour.PawnRow + (pawn.Colour.Direction * 3)))
            {
                var lastMove = pawn.Square.Game.Moves.LastOrDefault();
                if (lastMove != null)
                {
                    var previouslyMovedPiece = lastMove.Piece;
                    if (previouslyMovedPiece.IsOpponentOf(pawn) && previouslyMovedPiece.PieceType == PieceType.Pawn)
                    {
                        var previousMoveOrigin = lastMove.Piece.Square;
                        var previousMoveDestination = lastMove.Destination;
                        if (previousMoveOrigin.Y == previouslyMovedPiece.Colour.PawnRow && previousMoveDestination.Y == pawn.Square.Y)
                        {
                            if (previousMoveDestination == pawn.Square.Nav(x: -1))
                            {
                                yield return pawn.Square.Nav(x: -1, y: pawn.Colour.Direction);
                            }
                            if (previousMoveDestination == pawn.Square.Nav(x: 1))
                            {
                                yield return pawn.Square.Nav(x: 1, y: pawn.Colour.Direction);
                            }

                        }
                    }
                }
            }
        }
    }
}
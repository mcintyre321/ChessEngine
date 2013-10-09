using System.Collections.Generic;
using System.Linq;

namespace ChessEngine
{
    static class PawnMovement
    {
        public static IEnumerable<Square> GetMoves(ChessPiece pawn) 
        {
            var forwardOne = pawn.Square.Nav(y: pawn.PieceColour.Direction, allow: sq => sq.Piece == null);
            if (forwardOne != null)
            {
                yield return forwardOne;
                if(pawn.Square.Y == pawn.PieceColour.PawnRow){
                    yield return forwardOne.Nav(y: pawn.PieceColour.Direction, allow: sq => sq.Piece == null);
                }
            }
		
            //taking
            yield return pawn.Square.Nav(y: pawn.PieceColour.Direction, x:-1, allow: sq => sq.Piece.IsOpponentOf(pawn));
            yield return pawn.Square.Nav(y: pawn.PieceColour.Direction, x: 1, allow: sq => sq.Piece.IsOpponentOf(pawn));
		
            //en passant...
            if (pawn.Square.Y == (pawn.PieceColour.PawnRow + (pawn.PieceColour.Direction * 3)))
            {
                var lastMove = pawn.Square.Board.Moves.LastOrDefault();
                if (lastMove != null)
                {
                    var previouslyMovedPiece = lastMove.Item1;
                    if (previouslyMovedPiece.IsOpponentOf(pawn) && previouslyMovedPiece.PieceType == PieceType.Pawn)
                    {
                        var previousMoveOrigin = lastMove.Item2;
                        var previousMoveDestination = lastMove.Item3;
                        if (previousMoveOrigin.Y == previouslyMovedPiece.PieceColour.PawnRow && previousMoveDestination.Y == pawn.Square.Y)
                        {
                            if (previousMoveDestination == pawn.Square.Nav(x: -1))
                            {
                                yield return pawn.Square.Nav(x: -1, y: pawn.PieceColour.Direction);
                            }
                            if (previousMoveDestination == pawn.Square.Nav(x: 1))
                            {
                                yield return pawn.Square.Nav(x: 1, y: pawn.PieceColour.Direction);
                            }

                        }
                    }
                }
            }
        }
    }
}
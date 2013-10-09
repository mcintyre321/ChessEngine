using System.Collections.Generic;

namespace ChessEngine
{
    static class KnightMovement
    {
        public static IEnumerable<Square> GetMoves(ChessPiece piece) 
        {
            yield return piece.Square.Nav(x:  1, y:  2, allow: piece.CanEnter());
            yield return piece.Square.Nav(x:  2, y:  1, allow: piece.CanEnter());
            yield return piece.Square.Nav(x:  2, y: -1, allow: piece.CanEnter());
            yield return piece.Square.Nav(x:  1, y: -2, allow: piece.CanEnter());
            yield return piece.Square.Nav(x: -1, y: -2, allow: piece.CanEnter());
            yield return piece.Square.Nav(x: -2, y: -1, allow: piece.CanEnter());
            yield return piece.Square.Nav(x: -2, y:  1, allow: piece.CanEnter());
            yield return piece.Square.Nav(x: -1, y:  2, allow: piece.CanEnter());
        }
    }
}
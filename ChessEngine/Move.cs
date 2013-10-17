using System.Linq;

namespace ChessEngine
{
    public class Move
    {
    

        public Piece Piece { get; internal set; }
        public Square Destination { get; internal set; }
        public PieceType PromotionType { get; internal set; }
        public void Apply()
        {
            var move = this;
            var piece = move.Piece;
            var destination = move.Destination;
            if (piece.Colour != this.Piece.Square.Game.ColourWhoseTurnItIs)
            {
                throw new InvalidMoveException("It is the other colours move");
            }
            if (!piece.PotentialMoves().Select(m => m.Destination).Contains(destination))
            {
                throw new InvalidMoveException("That piece cannot move to that square");
            }
            if (destination.Piece != null)
            {
                destination.Piece.Square = null;
            }
            var origin = piece.Square;
            var targetPiece = destination.Piece;
            destination.Piece = piece;
            piece.Square = destination;
            piece.HasMoved = true;
            origin.Piece = null;
            if (piece.PieceType == PieceType.Pawn && targetPiece == null && origin.X != destination.X)
            {
                //must be en-passant
                var actualTarget = piece.Square.Nav(y: piece.Colour.Direction * -1);
                actualTarget.Piece.Square = null;
                actualTarget.Piece = null;
            }

            if (piece.PieceType == PieceType.King && (origin.X - destination.X) == 2)
            { // must be castling
                if (destination.X == 2)
                {
                    var rook = destination.Nav(x: -2).Piece;
                    rook.Square.Piece = null;
                    rook.Square = destination.Nav(x: -1);
                    rook.Square.Piece = rook;
                    rook.HasMoved = true;
                }
                else
                {
                    var rook = destination.Nav(x: 3).Piece;
                    rook.Square.Piece = null;
                    rook.Square = destination.Nav(x: 1);
                    destination.Nav(x: -1).Piece = rook;
                    rook.HasMoved = true;
                }
            }
            if (this.PromotionType != null) move.Piece.PieceType = this.PromotionType;
            this.Piece.Square.Game._moves.Add(move);
            this.Piece.Square.Game.ColourWhoseTurnItIs = this.Piece.Square.Game.ColourWhoseTurnItIs.Opponent;
        }
    }
}
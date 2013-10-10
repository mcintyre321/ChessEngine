using System;
using System.Collections.Generic;
using System.Linq;

namespace ChessEngine
{
    public class PieceType
    {
        internal string Name { get; set; }
        public string Characters { get; set; }
        internal Func<Piece, IEnumerable<Square>> GetMoves { get; set; }
        public override string ToString()
        {
            return Name;
        }
        public PieceType()
        {
        }
        static PieceType()
        {
            Pawn = new PieceType() { Name = "Pawn", Characters = "♙♟", GetMoves = PawnMovement.GetMoves };
            Rook = new PieceType() { Name = "Rook", Characters = "♖♜", GetMoves = RookMovement.GetMoves };
            Knight = new PieceType() { Name = "Knight", Characters = "♘♞", GetMoves = KnightMovement.GetMoves };
            Bishop = new PieceType() { Name = "Bishop", Characters = "♗♝", GetMoves = BishopMovement.GetMoves };
            Queen = new PieceType() { Name = "Queen", Characters = "♕♛", GetMoves = QueenMovement.GetMoves };
            King = new PieceType() { Name = "King", Characters = "♔♚", GetMoves = KingMovement.GetMoves };
        }
        public static PieceType Pawn { get; private set; }
        public static PieceType Rook { get; private set; }
        public static PieceType Knight { get; private set; }
        public static PieceType Bishop { get; private set; }
        public static PieceType Queen { get; private set; }
        public static PieceType King { get; private set; }
        public static IEnumerable<PieceType> PieceTypes { get { return new[] { Pawn, Rook, Knight, Bishop, Queen, King }; } }

        public IEnumerable<PieceType> PinningTypes
        {
            get { return new[] { Rook, Bishop, Queen }; }
        }
    }
}
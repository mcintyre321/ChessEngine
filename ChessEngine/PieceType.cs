using System;
using System.Collections.Generic;
using System.Linq;

namespace ChessEngine
{
    public class PieceType
    {
        internal int[] StartingCols {get;set;}
        internal bool BackRow {get;set;}
        internal string Name {get;set;}
        public string Characters {get;set;}
        internal Func<ChessPiece, IEnumerable<Square>> GetMoves {get; set;}
        public override string ToString(){
            return Name;
        }
        public PieceType(){
            BackRow = true;
        }
        static PieceType()
        {
            Pawn = new PieceType() { Name = "Pawn", Characters = "♙♟", StartingCols = Enumerable.Range(0, 8).ToArray(), BackRow = false, GetMoves = PawnMovement.GetMoves};
            Rook = new PieceType() { Name = "Rook", Characters = "♖♜", StartingCols = new []{0, 7 }, GetMoves = RookMovement.GetMoves};
            Knight = new PieceType() { Name = "Knight", Characters = "♘♞", StartingCols = new []{1, 6 }, GetMoves = KnightMovement.GetMoves };
            Bishop = new PieceType() { Name = "Bishop", Characters = "♗♝", StartingCols = new []{2, 5}, GetMoves = BishopMovement.GetMoves };
            Queen = new PieceType() { Name = "Queen", Characters = "♕♛", StartingCols = new[] {3 }, GetMoves = QueenMovement.GetMoves };
            King = new PieceType() { Name = "King", Characters = "♔♚", StartingCols = new[] {4} , GetMoves = KingMovement.GetMoves } ;
        }
        public static PieceType Pawn { get; private set; }	
        public static PieceType Rook { get; private set; }	
        public static PieceType Knight { get; private set; }	
        public static PieceType Bishop { get; private set; }	
        public static PieceType Queen { get; private set; }	
        public static PieceType King { get; private set; }	
        public static IEnumerable<PieceType> PieceTypes {get{ return new []{ Pawn, Rook, Knight, Bishop, Queen, King };}}
	
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChessEngine
{
    public class ChessGame
    {
        public PieceColour ColourWhoseTurnItIs { get; internal set; }
        public Square[][] Board;
        public ChessGame() : this(InitialLayout, PieceColour.White) { }
        public ChessGame(string layout, PieceColour colourWhoseTurnItIs = null)
        {
            colourWhoseTurnItIs = colourWhoseTurnItIs ?? PieceColour.White;
            ColourWhoseTurnItIs = colourWhoseTurnItIs;
            CreateBoard();
            AddPiecesToBoard(layout);
        }

        private void CreateBoard()
        {
            Board = new Square[8][];

            foreach (var x in Enumerable.Range(0, 8))
            {
                Board[x] = new Square[8];
                foreach (var y in Enumerable.Range(0, 8))
                {
                     Board[x][y] = new Square {X = x, Y = y, Game = this};
                }
            };
        }

        internal readonly List<Move> _moves = new List<Move>();
        public IEnumerable<Move> Moves {get { return _moves; }} 
        private const string InitialLayout =
@"|♜|♞|♝|♛|♚|♝|♞|♜|
|♟|♟|♟|♟|♟|♟|♟|♟|
|＿|＿|＿|＿|＿|＿|＿|＿|
|＿|＿|＿|＿|＿|＿|＿|＿|
|＿|＿|＿|＿|＿|＿|＿|＿|
|＿|＿|＿|＿|＿|＿|＿|＿|
|♙|♙|♙|♙|♙|♙|♙|♙|
|♖|♘|♗|♕|♔|♗|♘|♖|";
        public IEnumerable<Piece> Pieces { get { return Board.SelectMany(array => array).Select(x => x.Piece).Where(p => p != null); } }

        public IEnumerable<Square> Squares
        {
            get { return Board.SelectMany(sq => sq); }
        }

        public GameState State
        {
            get
            {
                if (Pieces.Where(p => p.Colour == ColourWhoseTurnItIs).SelectMany(p => p.PotentialMoves()).Any())
                {
                    return GameState.InProgress;
                }
                var king = Pieces.Single(p => p.Colour == ColourWhoseTurnItIs && p.PieceType == PieceType.King);
                return king.GetChecks().Any() ? GameState.CheckMate : GameState.Stalemate;
            }
        }

        private void AddPiecesToBoard(string layout)
        {
            var data = layout.Split(new[] { Environment.NewLine }, StringSplitOptions.None).Reverse()
                             .Select(line => line.ToCharArray().Where(c => c != '|').ToArray())
                             .ToArray();
            for (var y = 0; y < 8; y++)
                for (var x = 0; x < 8; x++)
                {
                    var character = data[y][x];
                    if (character == '＿') continue;
                    foreach (var col in PieceColour.Colours)

                        foreach (var pieceType in PieceType.PieceTypes)
                        {
                            var pieceChar = col == PieceColour.White
                                                ? pieceType.Characters[0]
                                                : pieceType.Characters[1];
                            if (character == pieceChar)
                            {
                                var square = Board[x][y];
                                square.Piece = new Piece()
                                {
                                    PieceType = pieceType,
                                    Colour = col,
                                    Square = square
                                };
                            }
                        }
                }
        }

        
        public override string ToString()
        {
            var sb = new StringBuilder();

            for (var y = 7; y >= 0; y--)
            {
                sb.Append("|");
                for (var x = 0; x < 8; x++)
                {
                    var piece = Board[x][y].Piece;
                    if (piece == null)
                    {
                        sb.Append('＿');
                    }
                    else
                    {
                        sb.Append(piece.Colour == PieceColour.White ? piece.PieceType.Characters[0] : piece.PieceType.Characters[1]);
                    }
                    sb.Append("|");
                }
                sb.Append(Environment.NewLine);
            }

            return sb.ToString();
        }

        /// <param name="x">0-7</param>
        /// <param name="row">0-7</param>
        internal Square this[int x, int row]
        {
            get { return Board[x][row]; }
        }

        public Square this[string colAndRow]
        {
            get { return this[colAndRow[0], int.Parse(colAndRow[1].ToString())]; }
        }

        /// <param name="column">a-h</param>
        /// <param name="row">1-9</param>
        public Square this[char column, int row]
        {
            get
            {

                return Board[((int)column - 97)][row - 1];
            }
        }

        public IEnumerable<Piece> EnemiesOf(Piece piece)
        {
            return Pieces.Where(p => p.Colour == piece.Colour.Opponent);
        }

        
     
    }

    public enum GameState   
    {
        InProgress,
        Stalemate,
        CheckMate,
    }
}

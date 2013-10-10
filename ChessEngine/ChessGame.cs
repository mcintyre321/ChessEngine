using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            Enumerable.Range(0, 8).ToList().ForEach(x =>
            {
                Board[x] = new Square[8];
                Enumerable.Range(0, 8).ToList().ForEach(y => { Board[x][y] = new Square {X = x, Y = y, Game = this}; });
            });
        }

        internal List<Tuple<Piece, Square, Square>> Moves = new List<Tuple<Piece, Square, Square>>();
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

        private void AddPiecesToBoard(string layout)
        {
            var data = layout.Split(new[] { Environment.NewLine }, StringSplitOptions.None).Reverse()
                             .Select(line => line.Where(c => c != '|').ToArray()).ToArray();
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

        public void RegisterMove(Piece piece, Square destination, PieceType promotionType = null)
        {
            if (piece.Colour != ColourWhoseTurnItIs)
            {
                throw new InvalidMoveException("It is the other colours move");
            }
            if (!piece.Square.GetMoves().Contains(destination))
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
                var actualTarget = piece.Square.Nav(y: piece.Colour.Direction*-1);
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
                    rook.Square = destination.Nav(x: -1);
                    destination.Nav(x: -1).Piece = rook;
                    rook.HasMoved = true;
                }
            }
            Moves.Add(Tuple.Create(piece, origin, destination));
            ColourWhoseTurnItIs = ColourWhoseTurnItIs.Opponent;
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

    }


}

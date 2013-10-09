using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine
{
    public class ChessBoard
    {
        public Square[][] _squares;

        public ChessBoard(string layout = null)
        {
            layout = layout ??
@"|♜|♞|♝|♛|♚|♝|♞|♜|
|♟|♟|♟|♟|♟|♟|♟|♟|
|＿|＿|＿|＿|＿|＿|＿|＿|
|＿|＿|＿|＿|＿|＿|＿|＿|
|＿|＿|＿|＿|＿|＿|＿|＿|
|＿|＿|＿|＿|＿|＿|＿|＿|
|♙|♙|♙|♙|♙|♙|♙|♙|
|♖|♘|♗|♕|♔|♗|♘|♖|";
            _squares = new Square[8][];

            Enumerable.Range(0, 8).ToList().ForEach(x =>
            {
                _squares[x] = new Square[8];
                Enumerable.Range(0, 8).ToList().ForEach(y =>
                {
                    _squares[x][y] = new Square { X = x, Y = y, Board = this };
                });
            });

            AddPiecesToBoard(layout);
        }
        internal List<Tuple<ChessPiece, Square, Square>> Moves = new List<Tuple<ChessPiece, Square, Square>>();
        public IEnumerable<ChessPiece> Pieces { get { return _squares.SelectMany(array => array).Select(x => x.Piece).Where(p => p != null); } }

        public IEnumerable<Square> Squares
        {
            get { return _squares.SelectMany(sq => sq); }
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
                                var square = _squares[x][y];
                                square.Piece = new ChessPiece()
                                {
                                    PieceType = pieceType,
                                    PieceColour = col,
                                    Square = square
                                };
                            }
                        }
                }
        }

        public void RegisterMove(ChessPiece piece, Square destination, PieceType promotionType = null)
        {
            if (piece.PieceColour == (Moves.Select(m => m.Item1.PieceColour).LastOrDefault() ?? PieceColour.Black))
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
                var actualTarget = piece.Square.Nav(y: piece.PieceColour.Direction*-1);
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
                }
                else
                {
                    var rook = destination.Nav(x: 3).Piece;
                    rook.Square.Piece = null;
                    rook.Square = destination.Nav(x: -1);
                    destination.Nav(x: -1).Piece = rook;
                }
            }
            Moves.Add(Tuple.Create(piece, origin, destination));
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            for (var y = 7; y >= 0; y--)
            {
                sb.Append("|");
                for (var x = 0; x < 8; x++)
                {
                    var piece = _squares[x][y].Piece;
                    if (piece == null)
                    {
                        sb.Append('＿');
                    }
                    else
                    {
                        sb.Append(piece.PieceColour == PieceColour.White ? piece.PieceType.Characters[0] : piece.PieceType.Characters[1]);
                    }
                    sb.Append("|");
                }
                sb.Append(Environment.NewLine);
            }

            return sb.ToString();
        }


        public Square this[int x, int y]
        {
            get { return _squares[x][y]; }
        }
    }


}

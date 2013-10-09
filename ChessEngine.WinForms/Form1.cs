using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChessEngine.WinForms
{
    public partial class Form1 : Form
    {
        private ChessBoard _chessBoard;

        public Form1()
        {
            InitializeComponent();
            _chessBoard = new ChessBoard();
            DrawBoard();
        }
        Dictionary<Square, Label> squareToControlLookup = new Dictionary<Square, Label>();
        private void DrawBoard()
        {
            const int squareSize = 50;

            foreach (var square in _chessBoard.Squares)
            {
                var squareControl = new Label()
                {
                    Width = squareSize,
                    Height = squareSize,
                    Left = square.X * squareSize,
                    Top = 7 * squareSize - square.Y * squareSize,
                    BackColor = GetBackColor(square),
                    ForeColor = Color.Black,
                    Font = new Font(FontFamily.GenericSansSerif, 25),

                };
                squareToControlLookup[square] = squareControl;
                this.Controls.Add(squareControl);
                squareControl.Click += (o, e) => HandleClick(square, squareControl);
            }
            RefreshIcons();
        }

        private void RefreshIcons()
        {
            foreach (var pair in squareToControlLookup)
            {
                var square = pair.Key;
                pair.Value.Text = square.Piece == null
                                      ? ""
                                      : square.Piece.PieceColour == PieceColour.White
                                            ? square.Piece.PieceType.Characters[0].ToString()
                                            : square.Piece.PieceType.Characters[1].ToString();
            }
        }

        private static Color GetBackColor(Square square)
        {
            return (square.X + square.Y) % 2 == 0 ? Color.White : Color.Gainsboro;
        }

        private ChessPiece selected;
        private void HandleClick(Square square, Label squareControl)
        {
            if (selected == null)
            {
                var moves = square.GetMoves().ToArray();
                if (moves.Any())
                {
                    selected = square.Piece;
                    foreach (var move in moves)
                    {
                        squareToControlLookup[move].BackColor = Color.LightBlue;
                    }
                }
            }
            else
            {
                var moves = selected.Square.GetMoves().ToArray();
                if (moves.Contains(square))
                {
                    try
                    {
                        _chessBoard.RegisterMove(selected, square);
                        RefreshIcons();
                    }
                    catch (InvalidMoveException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                }
                foreach (var move in moves)
                {
                    squareToControlLookup[move].BackColor = GetBackColor(move);
                }
                selected = null;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

    }
}

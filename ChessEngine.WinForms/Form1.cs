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
        private ChessGame _chessGame;

        public Form1()
        {
            InitializeComponent();
            _chessGame = new ChessGame();
            DrawBoard();
        }
        Dictionary<Square, Label> squareToControlLookup = new Dictionary<Square, Label>();
        private void DrawBoard()
        {
            const int squareSize = 50;

            foreach (var square in _chessGame.Squares)
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
                squareControl.Click += (o, e) => HandleClick(square);
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
        private void HandleClick(Square square)
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
                        _chessGame.RegisterMove(selected, square);
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
            this.Pasted += (o, ev) =>
            {
                _chessGame = new ChessGame(ev.ClipboardText);
                this.Controls.Clear();
                this.squareToControlLookup.Clear();
                DrawBoard();
            };
        }

        public class ClipboardEventArgs : EventArgs
        {
            public string ClipboardText { get; set; }
            public ClipboardEventArgs(string clipboardText)
            {
                ClipboardText = clipboardText;
            }
        }
        public event EventHandler<ClipboardEventArgs> Pasted;

        private const int WM_PASTE = 0x0302;
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_PASTE)
            {
                var evt = Pasted;
                if (evt != null)
                {
                    evt(this, new ClipboardEventArgs(Clipboard.GetText()));
                }
            }

            base.WndProc(ref m);
        }



    }
}

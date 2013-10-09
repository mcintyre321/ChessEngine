using System;
using System.Linq;
using NUnit.Framework;

namespace ChessEngine.Tests
{
    public class Tests
    {
        private ChessBoard chessBoard;

        [SetUp]
        public void CreateChessBoard()
        {
            chessBoard = new ChessBoard();
        }

        [Test]
        public void CheckBoardLayout()
        {
            var layout = @"
|♜|♞|♝|♛|♚|♝|♞|♜|
|♟|♟|♟|♟|♟|♟|♟|♟|
|＿|＿|＿|＿|＿|＿|＿|＿|
|＿|＿|＿|＿|＿|＿|＿|＿|
|＿|＿|＿|＿|＿|＿|＿|＿|
|＿|＿|＿|＿|＿|＿|＿|＿|
|♙|♙|♙|♙|♙|♙|♙|♙|
|♖|♘|♗|♕|♔|♗|♘|♖|";
            Assert.AreEqual(chessBoard.ToString().Trim(), layout.Trim());
        }

        [Test]
        public void PawnsCanThreaten()
        {
            var layout = @"
|♜|♞|♝|♛|♚|♝|♞|♜|
|♟|♟|♟|♟|＿|♟|♟|♟|
|＿|＿|＿|＿|＿|＿|＿|＿|
|＿|＿|＿|＿|♟|＿|＿|＿|
|＿|＿|＿|♙|＿|＿|＿|＿|
|＿|＿|＿|＿|＿|＿|＿|＿|
|♙|♙|♙|＿|♙|♙|♙|♙|
|♖|♘|♗|♕|♔|♗|♘|♖|";
            var chessBoard = new ChessBoard(layout);
            Assert.True(chessBoard[3, 3].GetMoves().Contains(chessBoard[4, 4]));
        }
    }
}
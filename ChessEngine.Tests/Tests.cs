using System;
using System.Linq;
using NUnit.Framework;

namespace ChessEngine.Tests
{
    public class Tests
    {
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
            Assert.AreEqual(new ChessGame().ToString().Trim(), layout.Trim());
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
            var chessBoard = new ChessGame(layout);
            Assert.True(chessBoard[3, 3].Piece.PotentialMoves().Contains(chessBoard[4, 4]));
        }

        [Test]
        public void PinnedRookCanOnlyMoveInThePinnedPlane()
        {
            var layout = @"
|＿|＿|＿|♜|♚|＿|＿|＿|
|＿|＿|＿|＿|＿|＿|＿|＿|
|＿|＿|＿|＿|＿|＿|＿|＿|
|＿|＿|＿|＿|＿|＿|＿|＿|
|＿|＿|＿|＿|＿|＿|＿|＿|
|＿|＿|＿|♖|＿|＿|＿|＿|
|＿|＿|＿|＿|＿|＿|＿|＿|
|＿|＿|＿|♔|＿|＿|＿|＿|";
            var chessBoard = new ChessGame(layout);
            var squaresRookCanGoTo = new []{"d2", "d4", "d5", "d6", "d7", "d8"}.Select(coord => chessBoard[coord]);
            CollectionAssert.AreEquivalent(squaresRookCanGoTo, chessBoard["d3"].Piece.PotentialMoves());
        }

        [Test]
        public void BishopMovesInDiagonal()
        {
            var layout = @"
|＿|＿|＿|♜|♚|＿|＿|＿|
|＿|＿|＿|＿|＿|＿|＿|＿|
|＿|＿|＿|＿|＿|＿|＿|＿|
|＿|＿|＿|＿|＿|＿|＿|＿|
|＿|＿|＿|＿|＿|＿|＿|＿|
|＿|＿|＿|♗|＿|＿|＿|＿|
|＿|＿|＿|＿|＿|＿|＿|＿|
|＿|＿|＿|＿|♔|＿|＿|＿|";
            var chessBoard = new ChessGame(layout);


            var northEast = new[] { "b1", "c2", "e4", "f5", "g6", "h7" }.Select(coord => chessBoard[coord]);
            var southEast = new[] { "a6", "b5", "c4", "e2", "f1", }.Select(coord => chessBoard[coord]);
            var squaresRookCanGoTo = northEast.Concat(southEast);
            CollectionAssert.AreEquivalent(squaresRookCanGoTo, chessBoard["d3"].Piece.PotentialMoves());
        }

        [Test]
        public void RookMovesInPlane()
        {
            var layout = @"
|＿|＿|＿|♜|♚|＿|＿|＿|
|＿|＿|＿|＿|＿|＿|＿|＿|
|＿|＿|＿|＿|＿|＿|＿|＿|
|＿|＿|＿|＿|＿|＿|＿|＿|
|＿|＿|＿|＿|＿|＿|＿|＿|
|＿|＿|＿|♖|＿|＿|＿|＿|
|＿|＿|＿|＿|＿|＿|＿|＿|
|＿|＿|＿|＿|♔|＿|＿|＿|";
            var chessBoard = new ChessGame(layout);


            var vertical = new[] { 0, 1, 3, 4, 5, 6, 7 }.Select(row => chessBoard[3, row]);
            var horizontal = new[] { 0, 1, 2, 4, 5, 6, 7 }.Select(col => chessBoard[col, 2]);
            var squaresRookCanGoTo = vertical
                .Concat(horizontal);
            CollectionAssert.AreEquivalent(squaresRookCanGoTo, chessBoard["d3"].Piece.PotentialMoves());
        }

        [Test]
        public void QueenRespectsCheck()
        {
            var layout = @"
|＿|＿|＿|♜|♚|＿|＿|＿|
|＿|＿|＿|＿|＿|＿|＿|＿|
|＿|＿|＿|＿|＿|＿|＿|＿|
|＿|＿|＿|♕|♜|＿|＿|＿|
|＿|＿|＿|＿|＿|＿|＿|＿|
|＿|＿|＿|＿|＿|＿|＿|＿|
|＿|＿|＿|＿|＿|＿|＿|＿|
|＿|＿|＿|＿|♔|＿|＿|＿|";
            var chessBoard = new ChessGame(layout);

            var squaresThatWouldBlockCheck = new[] { "e4", "e5" }.Select(coord => chessBoard[coord]);
            CollectionAssert.AreEquivalent(squaresThatWouldBlockCheck, chessBoard["d5"].Piece.PotentialMoves());

        }
        [Test]
        public void KingRespectsCheck()
        {
            var layout = @"
|＿|＿|＿|＿|♚|＿|＿|＿|
|＿|＿|＿|＿|＿|＿|＿|＿|
|＿|＿|＿|＿|＿|＿|＿|＿|
|＿|＿|＿|♕|♜|＿|＿|＿|
|＿|＿|＿|＿|＿|＿|＿|＿|
|＿|＿|＿|＿|＿|＿|＿|＿|
|＿|＿|＿|＿|＿|＿|＿|＿|
|＿|＿|＿|＿|♔|＿|＿|＿|";
            var chessBoard = new ChessGame(layout);

            var safeSquares = new[] { "d1", "d2", "f1", "f2" }.Select(coord => chessBoard[coord]).ToArray();
            CollectionAssert.AreEquivalent(safeSquares, chessBoard["e1"].Piece.PotentialMoves());

        }

    }
}
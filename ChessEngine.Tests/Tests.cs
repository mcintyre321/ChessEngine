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
            //When a game is created
            var game = new ChessGame()
            //Then the board should be laid out correctly
            var layout = @"
|♜|♞|♝|♛|♚|♝|♞|♜|
|♟|♟|♟|♟|♟|♟|♟|♟|
|＿|＿|＿|＿|＿|＿|＿|＿|
|＿|＿|＿|＿|＿|＿|＿|＿|
|＿|＿|＿|＿|＿|＿|＿|＿|
|＿|＿|＿|＿|＿|＿|＿|＿|
|♙|♙|♙|♙|♙|♙|♙|♙|
|♖|♘|♗|♕|♔|♗|♘|♖|";
            Assert.AreEqual(game.ToString().Trim(), layout.Trim());
        }

        [Test]
        public void PawnsCanThreaten()
        {
            //Given a pawn threatens another piece
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
            //When the pawn's moves are listed
            var moves = chessBoard[3, 3].Piece.PotentialMoves();
            
            //The other pieces square is available
            Assert.True(moves.Select(move => move.Destination).Contains(chessBoard[4, 4]));
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
            CollectionAssert.AreEquivalent(squaresRookCanGoTo, chessBoard["d3"].Piece.PotentialMoves().Select(m => m.Destination));
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
            CollectionAssert.AreEquivalent(squaresRookCanGoTo, chessBoard["d3"].Piece.PotentialMoves().Select(m => m.Destination));
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
            CollectionAssert.AreEquivalent(squaresRookCanGoTo, chessBoard["d3"].Piece.PotentialMoves().Select(m => m.Destination));
        }

        [Test]
        public void KnightChecksAreRespected()
        {
            var layout = @"
|＿|＿|＿|＿|♚|＿|＿|＿|
|＿|＿|＿|＿|＿|＿|＿|＿|
|＿|＿|＿|＿|＿|＿|＿|＿|
|＿|＿|＿|♕|＿|＿|＿|＿|
|＿|＿|＿|＿|＿|＿|＿|＿|
|＿|＿|＿|＿|＿|＿|＿|＿|
|＿|＿|♞|＿|＿|＿|＿|＿|
|＿|＿|＿|＿|♔|＿|＿|＿|";
            var chessBoard = new ChessGame(layout);

            CollectionAssert.AreEquivalent(Enumerable.Empty<Square>(), chessBoard["d5"].Piece.PotentialMoves().Select(m => m.Destination));
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
            CollectionAssert.AreEquivalent(squaresThatWouldBlockCheck, chessBoard["d5"].Piece.PotentialMoves().Select(m => m.Destination));

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
            CollectionAssert.AreEquivalent(safeSquares, chessBoard["e1"].Piece.PotentialMoves().Select(m => m.Destination));
        }

        [Test]
        public void CanCastlePawn()
        {
            var layout = @"
|＿|＿|＿|＿|♚|＿|＿|＿|
|＿|＿|♙|＿|＿|＿|＿|＿|
|＿|＿|＿|＿|＿|＿|＿|＿|
|＿|＿|＿|＿|＿|＿|＿|＿|
|＿|＿|＿|＿|＿|＿|＿|＿|
|＿|＿|＿|＿|＿|＿|＿|＿|
|＿|＿|＿|＿|＿|＿|＿|＿|
|＿|＿|＿|＿|♔|＿|＿|＿|";
            var chessBoard = new ChessGame(layout);

            var moves = chessBoard["c7"].Piece.PotentialMoves();
            Assert.AreEqual(4, moves.Count());
            Assert.True(moves.All(m => m.PromotionType != null));
            moves.Single(m => m.PromotionType == PieceType.Rook).Apply();
            Assert.AreEqual(PieceType.Rook, chessBoard["c8"].Piece.PieceType);
        }

        [Test]
        public void CanDetectCheckmate()
        {
            var layout = @"
|＿|＿|♖|＿|♚|＿|＿|＿|
|＿|＿|♖|＿|＿|＿|＿|＿|
|＿|＿|＿|＿|＿|＿|＿|＿|
|＿|＿|＿|＿|＿|＿|＿|＿|
|＿|＿|＿|＿|＿|＿|＿|＿|
|＿|＿|＿|＿|＿|＿|＿|＿|
|＿|＿|＿|＿|＿|＿|＿|＿|
|＿|＿|＿|＿|♔|＿|＿|＿|";
            var chessBoard = new ChessGame(layout, PieceColour.Black);
            var moves = chessBoard["e87"].Piece.PotentialMoves();
            Assert.AreEqual(0, moves.Count());
            Assert.AreEqual(GameState.CheckMate, chessBoard.State);
        }

        [Test]
        public void CanDetectStalemate()
        {
            var layout = @"
|＿|＿|＿|＿|♚|＿|＿|＿|
|＿|＿|♖|＿|＿|＿|＿|＿|
|＿|＿|＿|♖|＿|♖|＿|＿|
|＿|＿|＿|＿|＿|＿|＿|＿|
|＿|＿|＿|＿|＿|＿|＿|＿|
|＿|＿|＿|＿|＿|＿|＿|＿|
|＿|＿|＿|＿|＿|＿|＿|＿|
|＿|＿|＿|＿|♔|＿|＿|＿|";
            var chessBoard = new ChessGame(layout, PieceColour.Black);
            var moves = chessBoard["e87"].Piece.PotentialMoves();
            Assert.AreEqual(0, moves.Count());
            Assert.AreEqual(GameState.Stalemate, chessBoard.State);
        }
    }
}

using System;

namespace ChessEngine
{
    public class InvalidMoveException : Exception{
        public InvalidMoveException(string message) :base(message){
        }
    }
}
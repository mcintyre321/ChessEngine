using System.Collections.Generic;
using System.Linq;

namespace ChessEngine
{
    public class Square
    {
        public int X{get; internal set;}
        public int Y{get; internal set;}
        public Piece Piece { get; internal set; }
        public ChessGame Game { get; internal set; }
        
        public override string ToString()
        {
            return "[" + ((char) (X + 97)) + (Y + 1).ToString() + "]";
        }
    }
}
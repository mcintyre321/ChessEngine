using System.Collections.Generic;
using System.Linq;

namespace ChessEngine
{
    public class PieceColour
    {
        static PieceColour()
        {
            White = new PieceColour() {BackRow = 0, PawnRow = 1, Direction = 1};
            Black = new PieceColour() {BackRow = 7, PawnRow = 6, Direction = -1};
        }

        internal int BackRow { get; set; }
        internal int PawnRow { get; set; }
        internal int Direction { get; set; }
        public static PieceColour White { get; private set; }
        public static PieceColour Black { get; private set; }

        public static IEnumerable<PieceColour> Colours
        {
            get
            {
                yield return White;
                yield return Black;
            }
        }

        public PieceColour Opponent
        {
            get { return this == PieceColour.White ? PieceColour.Black : PieceColour.White; }
        }

        public override string ToString()
        {
            return this == PieceColour.White ? "White" : "Black";
        }
    }
}

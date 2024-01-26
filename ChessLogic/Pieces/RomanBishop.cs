namespace ChessLogic
{
    public class RomanBishop : Piece
    {
        public override PieceType Type => PieceType.RomanBishop;
        public override Player Color { get; }
        
        private static readonly Direction[] dirs = new Direction[]
        {
            Direction.NorthWest,
            Direction.NorthEast,
            Direction.SouthWest,
            Direction.SouthEast
        };
        
        public RomanBishop(Player color)
        {
            Color = color;
        }
        public override Piece Copy()
        {
            RomanBishop copy = new RomanBishop(Color);
            copy.HasMoved = HasMoved;
            return copy;
        }
        
        private static IEnumerable<Position> PotentialToPositions(Position from)
        {
            foreach (Direction vDir in new Direction[] { Direction.North, Direction.South })
            {
                foreach (Direction hDir in new Direction[] { Direction.West, Direction.East })
                {
                    yield return from + 2 * vDir + hDir; // 2 up/down, 1 left/right
                    yield return from + 2 * hDir + vDir; // 2 left/right, 1 up/down
                }
            }
        }

        private IEnumerable<Position> MovePositions(Position from, Board board)
        {
            return PotentialToPositions(from).Where(pos => Board.IsInside(pos)
                                                           && (board.IsEmpty(pos) || board[pos].Color != Color));
        }

        public override IEnumerable<Move> GetMoves(Position from, Board board)
        {
            return MovePositionsInDirs(from, board, dirs).Select(to => new NormalMove(from, to))
                .Concat(MovePositions(from, board).Select(to => new NormalMove(from, to)));
        }
    }
}

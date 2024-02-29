namespace ChessLogic
{
    public class Horseman : Piece
    {
        public override PieceType Type => PieceType.Horseman;

        public override Player Color { get; }

        private static readonly Direction[] dirs = new Direction[]
        {
            Direction.North,
            Direction.South,
            Direction.East,
            Direction.West,
            Direction.NorthWest,
            Direction.NorthEast,
            Direction.SouthWest,
            Direction.SouthEast
        };

        public Horseman(Player color)
        {
            Color = color;
        }

        public override Piece Copy()
        {
            Horseman copy = new Horseman(Color);
            copy.HasMoved = HasMoved;
            return copy;
        }

        private IEnumerable<Position> MovePositions(Position from, Board board)
        {
            foreach (Direction dir in dirs)
            {
                Position to = from + dir;

                if (!Board.IsInside(to)) // square is outside the board
                {
                    continue;
                }

                if (board.IsEmpty(to) || board[to].Color != Color) // square is empty or occupied by an enemy piece
                {
                    yield return to;
                }
            }
        }

        public override IEnumerable<Move> GetMoves(Position from, Board board)
        {
            foreach (Position to in MovePositions(from, board))
            {
                yield return new NormalMove(from, to);
            }
        }
    }
}

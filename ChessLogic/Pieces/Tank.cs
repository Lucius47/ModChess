namespace ChessLogic
{
    public class Tank : Piece
    {
        public override PieceType Type => PieceType.Tank;
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
        
        public Tank(Player color)
        {
            Color = color;
        }
        public override Piece Copy()
        {
            Tank copy = new Tank(Color);
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
        
        private IEnumerable<Position> RangedPositions(Position from, Board board)
        {
            foreach (Direction dir in dirs)
            {
                if (dir == Direction.NorthWest || dir == Direction.NorthEast
                                               || dir == Direction.SouthWest || dir == Direction.SouthEast)
                {
                    continue;
                }
                
                Position to = from + (2 * dir);

                if (!Board.IsInside(to)) // square is outside the board
                {
                    continue;
                }

                Position midPos = from + dir;
                if (!board.IsEmpty(midPos)) // can't shoot over pieces
                {
                    continue;
                }

                if (!board.IsEmpty(to) && board[to].Color != Color) // square is occupied by an enemy piece
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

            foreach (Position to in RangedPositions(from, board))
            {
                yield return new RangedMove(from, to);
            }
        }
    }
}

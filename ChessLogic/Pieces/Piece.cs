namespace ChessLogic
{
    public abstract class Piece
    {
        public abstract PieceType Type { get; }
        public abstract Player Color { get; }
        public bool HasMoved { get; set; } = false;

        public abstract Piece Copy();

        public abstract IEnumerable<Move> GetMoves(Position pos, Board board);

        protected IEnumerable<Position> MovePositionsInDir(Position from, Board board, Direction dir)
        {
            for (Position pos = from + dir; Board.IsInside(pos); pos += dir)
            {
                if (board.IsEmpty(pos)) // Empty square
                {
                    yield return pos;
                    continue;
                }
                
                Piece piece = board[pos];

                if (piece.Color != Color) // Enemy piece
                {
                    yield return pos;
                }

                // Friendly piece and this piece cannot jump
                yield break;
            }
        }

        protected IEnumerable<Position> MovePositionsInDirs(Position from, Board board, Direction[] dirs)
        {
            return dirs.SelectMany(dir => MovePositionsInDir(from, board, dir));
        }

        public virtual bool CanCaptureOpponentKing(Position from, Board board) // to detect check
        {
            return GetMoves(from, board).Any(move =>
            {
                Piece piece = board[move.ToPos];
                return piece != null && piece.Type == PieceType.King; // no need to check color, because it is checked in GetMoves
            });
        }
    }
}

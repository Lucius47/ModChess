namespace ChessLogic
{
    public class King : Piece
    {
        // mod
        public bool IsRomanKing { get; set; } = false;
        public bool IsEgyptianKing { get; set; } = false;
        // mod


        public override PieceType Type => PieceType.King;

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

        public King(Player color)
        {
            Color = color;
        }

        private static bool IsUnmovedRook(Position pos, Board board) // for castling move
        {
            if (board.IsEmpty(pos))
            {
                return false;
            }

            Piece piece = board[pos];
            return piece.Type == PieceType.Rook && !piece.HasMoved; // no need to check color or piece type
        }

        private static bool AllEmpty(IEnumerable<Position> positions, Board board) // for castling move
        {
            return positions.All(pos => board.IsEmpty(pos)); // all positions are empty
        }

        private bool CanCastleKingSide(Position from, Board board) // for castling move
        {
            if (HasMoved)
            {
                return false;
            }

            Position rookPos = new Position(from.Row, 7); // rook position
            Position[] betweenPositions = new Position[] { new(from.Row, 5), new(from.Row, 6) }; // positions between king and rook

            return IsUnmovedRook(rookPos, board) && AllEmpty(betweenPositions, board);
        }

        private bool CanCastleQueenSide(Position from, Board board) // for castling move
        {
            if (HasMoved)
            {
                return false;
            }

            Position rookPos = new Position(from.Row, 0); // rook position
            Position[] betweenPositions = new Position[] { new(from.Row, 1), new(from.Row, 2), new(from.Row, 3) }; // positions between king and rook

            return IsUnmovedRook(rookPos, board) && AllEmpty(betweenPositions, board);
        }

        public override Piece Copy()
        {
            King copy = new King(Color);
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

                if (IsRomanKing) // for Roman King
                {
                    // Your king can move 1 extra space in the up/down/left/right direction(s)
                    if (board.IsEmpty(to) && (board.IsEmpty(to + dir) || board[to + dir].Color != Color))
                    {
                        if (dir == Direction.North || dir == Direction.South || dir == Direction.East || dir == Direction.West)
                        {
                            yield return to + dir;
                        }
                    }
                }

                if (IsEgyptianKing) // for Egyptian King
                {
                    if (dir == Direction.NorthWest || dir == Direction.SouthWest || dir == Direction.NorthEast || dir == Direction.SouthEast)
                    {
                        if (board.IsEmpty(to) && (board.IsEmpty(to + dir) || board[to + dir].Color != Color))
                        {
                            yield return to + dir;
                        }
                    }
                }
            }
        }

        public override IEnumerable<Move> GetMoves(Position from, Board board)
        {
            foreach (Position to in MovePositions(from, board))
            {
                yield return new NormalMove(from, to);
            }

            if (CanCastleKingSide(from, board))
            {
                yield return new Castle(MoveType.CastleKS, from);
            }

            if (CanCastleQueenSide(from, board))
            {
                yield return new Castle(MoveType.CastleQS, from);
            }
        }

        public override bool CanCaptureOpponentKing(Position from, Board board)
        {
            // for castling move
            return MovePositions(from, board).Any(to =>
            {
                Piece piece = board[to];
                return piece != null && piece.Type == PieceType.King;
            });
        }
    }
}

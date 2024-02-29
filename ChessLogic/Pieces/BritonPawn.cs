namespace ChessLogic
{
    public class BritonPawn : Piece
    {
        // Your pawns can attack diagonally and 2 spaces in front of them. Your pawns don’t move when they attack
        public override PieceType Type => PieceType.BritonPawn;

        public override Player Color { get; }

        private readonly Direction forward;

        public BritonPawn(Player color)
        {
            Color = color;

            if (color == Player.White)
            {
                forward = Direction.North;
            }
            else
            {
                forward = Direction.South;
            }
        }

        public override Piece Copy()
        {
            BritonPawn copy = new BritonPawn(Color);
            copy.HasMoved = HasMoved;
            return copy;
        }

        private static bool CanMoveTo(Position pos, Board board)
        {
            return Board.IsInside(pos) && board.IsEmpty(pos);
        }

        private bool CanCaptureAt(Position pos, Board board)
        {
            if (!Board.IsInside(pos) || board.IsEmpty(pos)) // square is outside of board or empty
            {
                return false;
            }
            
            return board[pos].Color != Color; // true if square is occupied by enemy
        }

        public static IEnumerable<Move> PromotionMoves(Position from, Position to)
        {
            yield return new PawnPromotion(from, to, PieceType.Knight);
            yield return new PawnPromotion(from, to, PieceType.Bishop);
            yield return new PawnPromotion(from, to, PieceType.Rook);
            yield return new PawnPromotion(from, to, PieceType.Queen);
        }

        private IEnumerable<Move> ForwardMoves(Position from, Board board)
        {
            Position oneMovePos = from + forward; // one square forward

            if (CanMoveTo(oneMovePos, board))
            {
                if (oneMovePos.Row == 0 || oneMovePos.Row == 7) // check if pawn can promote
                {
                    foreach (Move promMove in PromotionMoves(from, oneMovePos))
                    {
                        yield return promMove;
                    }
                }
                else
                {
                    yield return new NormalMove(from, oneMovePos);
                }

                Position twoMovePos = oneMovePos + forward; // two squares forward, no need to check for promotion

                if (!HasMoved && CanMoveTo(twoMovePos, board)) // pawn hasn't moved yet
                {
                    yield return new DoublePawn(from, twoMovePos);
                }

                if (CanCaptureAt(twoMovePos, board)) // can attack two squares forward
                {
                    yield return new RangedMove(from, twoMovePos);
                }
            }
        }

        private IEnumerable<Move> DiagonalMoves(Position from, Board board)
        {
            foreach (Direction dir in new Direction[] { Direction.West, Direction.East })
            {
                Position to = from + forward + dir; // one square forward and one square to the left/right

                if (to == board.GetPawnSkipPosition(Color.Opponent())) // if the toPos was skipped
                                                                       // by an opponent pawn in the previous move
                {
                    yield return new EnPassant(from, to);
                }

                else if (CanCaptureAt(to, board))
                {
                    yield return new RangedMove(from, to);
                }
            }
        }

        public override IEnumerable<Move> GetMoves(Position from, Board board)
        {
            return ForwardMoves(from, board).Concat(DiagonalMoves(from, board)); // all legal forward moves and diagonal moves
        }

        public override bool CanCaptureOpponentKing(Position from, Board board)
        {
            // check only if a diagonal move can capture the opponent king
            return DiagonalMoves(from, board).Any(move =>
            {
                Piece piece = board[move.ToPos];
                return piece != null && piece.Type == PieceType.King;
            });
        }
    }
}

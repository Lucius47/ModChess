namespace ChessLogic
{
    public class Castle : Move
    {
        public override MoveType Type { get; }
        public override Position FromPos { get; }
        public override Position ToPos { get; }

        private readonly Direction kingMoveDir;
        private readonly Position rookFromPos;
        private readonly Position rookToPos;

        public Castle(MoveType type, Position kingPos)
        {
            Type = type;
            FromPos = kingPos;

            if (type == MoveType.CastleKS)
            {
                kingMoveDir = Direction.East;
                ToPos = new Position(kingPos.Row, 6);
                rookFromPos = new Position(kingPos.Row, 7);
                rookToPos = new Position(kingPos.Row, 5);
            }
            else if (type == MoveType.CastleQS)
            {
                kingMoveDir = Direction.West;
                ToPos = new Position(kingPos.Row, 2);
                rookFromPos = new Position(kingPos.Row, 0);
                rookToPos = new Position(kingPos.Row, 3);
            }
        }

        public override bool Execute(Board board)
        {
            // move king and rook
            new NormalMove(FromPos, ToPos).Execute(board);
            new NormalMove(rookFromPos, rookToPos).Execute(board);

            return false; // for the 50move rule. castling never moves a pawn or captures a piece.
        }

        public override bool IsLegal(Board board)
        {
            Player player = board[FromPos].Color; // get the player who is making the move

            if (board.IsInCheck(player)) // if the player is in check
            {
                return false; // then they can't castle
            }

            // check if the king will move through or to a check
            Board boardCopy = board.Copy();
            Position kingPosInCopy = FromPos;

            for (int i = 0; i < 2; i++) // move the king twice in a copy of the board
            {
                new NormalMove(kingPosInCopy, kingPosInCopy + kingMoveDir).Execute(boardCopy);
                kingPosInCopy += kingMoveDir;

                if (boardCopy.IsInCheck(player))
                {
                    return false;
                }
            }

            return true; // can castle
        }
    }
}

namespace ChessLogic
{
    public class DoublePawn : Move
    {
        public override MoveType Type => MoveType.DoublePawn;
        public override Position FromPos { get; }
        public override Position ToPos { get; }

        private readonly Position skippedPosition; // en passant position

        public DoublePawn(Position from, Position to)
        {
            FromPos = from;
            ToPos = to; // from + 2 * forward

            skippedPosition = new Position((from.Row + to.Row) / 2, from.Column); // square btw from and to
        }

        public override void Execute(Board board)
        {
            Player player = board[FromPos].Color;
            board.SetPawnSkipPosition(player, skippedPosition);
            new NormalMove(FromPos, ToPos).Execute(board);
        }
    }
}

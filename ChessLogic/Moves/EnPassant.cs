namespace ChessLogic
{
    public class EnPassant : Move
    {
        public override MoveType Type => MoveType.EnPassant;
        public override Position FromPos { get; }
        public override Position ToPos { get; }

        private readonly Position capturedPos; // en passant position

        public EnPassant(Position from, Position to)
        {
            FromPos = from;
            ToPos = to;

            // capturedPos must be in the row we are moving from (FromPos)
            // and in the column we are moving to (ToPos)
            capturedPos = new Position(from.Row, to.Column);
        }

        public override bool Execute(Board board)
        {
            new NormalMove(FromPos, ToPos).Execute(board);
            board[capturedPos] = null; // capture the pawn

            return true; // for the 50move rule. always moves a pawn and captures
        }
    }
}

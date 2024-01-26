namespace ChessLogic
{
    public abstract class Move
    {
        public abstract MoveType Type { get; }
        public abstract Position FromPos { get; }
        public abstract Position ToPos { get; }

        public abstract bool Execute(Board board); // true if a piece was captured, or a pawn was moved. (for the 50move rule)

        public virtual bool IsLegal(Board board) // true if executing this move doesn't put the current player in check
        {
            // we need to check if the move results in check when executed
            // so we need to execute the move on a copy of the board

            Player player = board[FromPos].Color; // get the player who is making the move
            Board boardCopy = board.Copy(); // make a copy of the board
            Execute(boardCopy); // and execute the move on the copy
            return !boardCopy.IsInCheck(player); // return true if the player's king is not in check after the move.

            // note: this code is not very efficient, so it shouldn't be used when an AI is calculating moves

            // note: this method is virtual because some moves (like castling) need to override it
        }
    }
}

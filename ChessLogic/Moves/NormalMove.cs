﻿namespace ChessLogic
{
    public class NormalMove : Move
    {
        public override MoveType Type => MoveType.Normal;
        public override Position FromPos { get; }
        public override Position ToPos { get; }

        public NormalMove(Position fromPos, Position toPos)
        {
            FromPos = fromPos;
            ToPos = toPos;
        }

        public override bool Execute(Board board)
        {
            Piece piece = board[FromPos];
            bool capture = !board.IsEmpty(ToPos); // captures if the ToPos is not empty.
            board[ToPos] = piece;
            board[FromPos] = null;
            piece.HasMoved = true;

            return capture || piece.Type == PieceType.Pawn; // for the 50move rule. if it's a capture move or a pawn is moved
        }
    }
}

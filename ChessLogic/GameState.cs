namespace ChessLogic
{
    public class GameState
    {
        public Board Board { get; }
        public Player CurrentPlayer { get; private set; }
        public Result Result { get; private set; } = null;

        public GameState(Player player, Board board)
        {
            CurrentPlayer = player;
            Board = board;
        }

        public IEnumerable<Move> LegalMovesForPiece(Position pos) // all legal moves for a piece at this position
                                                                  // the piece itself doesn't know where it is on the board.
        {
            if (Board.IsEmpty(pos) || Board[pos].Color != CurrentPlayer) // empty, or not my piece
            {
                return Enumerable.Empty<Move>();
            }

            Piece piece = Board[pos]; // is my piece
            IEnumerable<Move> moveCandidates = piece.GetMoves(pos, Board); // get all moves for this piece

            return moveCandidates.Where(move => move.IsLegal(Board)); // return only legal moves
        }

        public void MakeMove(Move move) // No need to check if move is legal, because we will only call it with legal moves
        {
            Board.SetPawnSkipPosition(CurrentPlayer, null); // can only en passant on the next turn after
                                                            // a double pawn move

            move.Execute(Board);
            CurrentPlayer = CurrentPlayer.Opponent();
            CheckForGameOver();
        }

        public IEnumerable<Move> AllLegalMovesFor(Player player)
        {
            // all moves player can make
            IEnumerable<Move> moveCandidates = Board.PiecePositionsFor(player).SelectMany(pos =>
            {
                Piece piece = Board[pos];
                return piece.GetMoves(pos, Board);
            });

            // filter out illegal moves
            return moveCandidates.Where(move => move.IsLegal(Board));
        }

        public void CheckForGameOver() // called after every move
        {
            if (!AllLegalMovesFor(CurrentPlayer).Any()) // no legal moves for current player
            {
                if (Board.IsInCheck(CurrentPlayer)) // current player is in check, so checkmate
                {
                    Result = Result.Win(CurrentPlayer.Opponent()); // other player wins by checkmate
                }
                else // stalemate
                {
                    Result = Result.Draw(EndReason.Stalemate);
                }
            }
        }

        public bool IsGameOver()
        {
            return Result != null; // game is over if there is a result
        }
    }
}

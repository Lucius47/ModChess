namespace ChessLogic
{
    public class GameState
    {
        public Board Board { get; }
        public Player CurrentPlayer { get; private set; }
        public Result Result { get; private set; } = null;
        
        public int noCaptureOrPawnMoves = 0; // Counter to keep track of the moves for the 50move rule.
        private string stateString;

        private readonly Dictionary<string, int> stateHistory = new(); // how many times a given state has occurred

        public GameState(Player player, Board board)
        {
            CurrentPlayer = player;
            Board = board;

            stateString = new StateString(CurrentPlayer, board).ToString();
            stateHistory[stateString] = 1;
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

            bool captureOrPawn = move.Execute(Board);

            if (captureOrPawn)
            {
                noCaptureOrPawnMoves = 0;
                stateHistory.Clear(); // if a capture was made, or a pawn was moved, we can never encounter the 
                                      // same state again.
            }
            else
            {
                noCaptureOrPawnMoves++;
            }

            CurrentPlayer = CurrentPlayer.Opponent();

            UpdateStateString();

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
            else if (Board.InsufficientMaterial())
            {
                Result = Result.Draw(EndReason.InsufficientMaterial);
            }
            else if (FiftyMoveRule())
            {
                Result = Result.Draw(EndReason.FiftyMoveRule);
            }
            else if (ThreefoldRepetition())
            {
                Result = Result.Draw(EndReason.ThreefoldRepetition);
            }
        }

        public bool IsGameOver()
        {
            return Result != null; // game is over if there is a result
        }

        private bool FiftyMoveRule()
        {
            // in chess rules, 50 moves in the 50move rule refers to full moves. 1 white move + 1 black move = 1 full move
            int fullMoves = noCaptureOrPawnMoves / 2;
            return fullMoves == 50;
        }

        private void UpdateStateString()
        {
            stateString = new StateString(CurrentPlayer, Board).ToString();

            if (!stateHistory.ContainsKey(stateString))
            {
                stateHistory[stateString] = 1;
            }
            else
            {
                stateHistory[stateString]++;
            }
        }

        private bool ThreefoldRepetition()
        {
            return stateHistory[stateString] == 3;
        }
    }
}

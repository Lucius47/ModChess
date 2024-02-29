namespace ChessLogic
{
    public class Board
    {
        private readonly Piece[,] pieces = new Piece[8, 8];

        private static Civilizations whiteCiv = Civilizations.Default;
        private static Civilizations blackCiv = Civilizations.Default;

        private readonly Dictionary<Player, Position> pawnSkipPositions = new() // en passant squares
        {
            { Player.White, null },
            { Player.Black, null }
        };

        public Piece this[int row, int col]
        {
            get { return pieces[row, col]; }
            set { pieces[row, col] = value; }
        }

        public Piece this[Position pos]
        {
            get { return this[pos.Row, pos.Column]; }
            set { this[pos.Row, pos.Column] = value; }
        }

        // en passant
        public Position GetPawnSkipPosition(Player player) // get en passant square for a player
        {
            return pawnSkipPositions[player];
        }

        public void SetPawnSkipPosition(Player player, Position pos) // set en passant square for a player
        {
            pawnSkipPositions[player] = pos;
        }
        // end en passant

        public static Board Initial(Civilizations _whiteCiv = Civilizations.Default, Civilizations _blackCiv = Civilizations.Default)
        {
            Board board = new Board();

            whiteCiv = _whiteCiv;
            blackCiv = _blackCiv;

            board.AddStartPieces();
            return board;
        }

        private void AddStartPieces()
        {
            switch (whiteCiv)
            {
                case Civilizations.Default:
                    AddDefaultStartPieces(Player.White);
                    break;
                case Civilizations.HolyRomanEmpire:
                    AddRomanStartPieces(Player.White);
                    break;
                case Civilizations.China:
                    AddChineseStartPieces(Player.White);
                    break;
                case Civilizations.Italy:
                    AddItalianStartPieces(Player.White);
                    break;
                case Civilizations.Huns:
                    AddHunsStartPieces(Player.White);
                    break;
                case Civilizations.Vikings:
                    AddVikingStartPieces(Player.White);
                    break;
                case Civilizations.Britons:
                    AddBritonStartPieces(Player.White);
                    break;
                case Civilizations.Egypt:
                    AddEgyptStartPieces(Player.White);
                    break;
                default:
                    AddDefaultStartPieces(Player.White);
                    break;
            }

            switch (blackCiv)
            {
                case Civilizations.Default:
                    AddDefaultStartPieces(Player.Black);
                    break;
                case Civilizations.HolyRomanEmpire:
                    AddRomanStartPieces(Player.Black);
                    break;
                case Civilizations.China:
                    AddChineseStartPieces(Player.Black);
                    break;
                case Civilizations.Italy:
                    AddItalianStartPieces(Player.Black);
                    break;
                case Civilizations.Huns:
                    AddHunsStartPieces(Player.Black);
                    break;
                case Civilizations.Vikings:
                    AddVikingStartPieces(Player.Black);
                    break;
                case Civilizations.Britons:
                    AddBritonStartPieces(Player.Black);
                    break;
                case Civilizations.Egypt:
                    AddEgyptStartPieces(Player.Black);
                    break;
                default:
                    AddDefaultStartPieces(Player.Black);
                    break;
            }
        }

        private void AddDefaultStartPieces(Player color)
        {
            var row = color == Player.White ? 7 : 0;
            var pawnRow = color == Player.White ? 6 : 1;

            this[row, 0] = new Rook(color);
            this[row, 1] = new Knight(color);
            this[row, 2] = new Bishop(color);

            this[row, 3] = new Queen(color);
            this[row, 4] = new King(color);

            this[row, 5] = new Bishop(color);
            this[row, 6] = new Knight(color);
            this[row, 7] = new Rook(color);

            for (int c = 0; c < 8; c++)
            {
                this[pawnRow, c] = new Pawn(color);
            }
        }

        private void AddRomanStartPieces(Player color)
        {
            var row = color == Player.White ? 7 : 0;
            var pawnRow = color == Player.White ? 6 : 1;

            this[row, 0] = new Rook(color);
            this[row, 1] = new RomanBishop(color);
            this[row, 2] = new RomanBishop(color);

            this[row, 3] = new Queen(color);
            var king = new King(color)
            {
                IsRomanKing = true
            };
            this[row, 4] = king;

            this[row, 5] = new RomanBishop(color);
            this[row, 6] = new RomanBishop(color);
            this[row, 7] = new Rook(color);

            for (int c = 0; c < 8; c++)
            {
                this[pawnRow, c] = new Pawn(color);
            }
        }

        private void AddChineseStartPieces(Player color)
        {
            var row = color == Player.White ? 7 : 0;
            var pawnRow = color == Player.White ? 6 : 1;
            var thirdRow = color == Player.White ? 5 : 2;

            this[row, 0] = new Rook(color);
            this[row, 1] = new Knight(color);
            this[row, 2] = new Bishop(color);

            this[row, 3] = new Queen(color);
            this[row, 4] = new King(color);

            this[row, 5] = new Bishop(color);
            this[row, 6] = new Knight(color);
            this[row, 7] = new Rook(color);

            for (int c = 0; c < 8; c++)
            {
                this[pawnRow, c] = new Pawn(color);

                if (c is not (0 or 1 or 6 or 7))
                {
                    this[thirdRow, c] = new Pawn(color);
                }
            }
        }

        private void AddItalianStartPieces(Player color)
        {
            var row = color == Player.White ? 7 : 0;
            var pawnRow = color == Player.White ? 6 : 1;

            this[row, 0] = new Rook(color);
            this[row, 1] = new Tank(color);
            this[row, 2] = new Bishop(color);

            this[row, 3] = new Queen(color);
            this[row, 4] = new King(color);

            this[row, 5] = new Bishop(color);
            this[row, 6] = new Tank(color);
            this[row, 7] = new Rook(color);

            for (int c = 0; c < 8; c++)
            {
                this[pawnRow, c] = new Pawn(color);
            }
        }

        private void AddHunsStartPieces(Player color)
        {
            var row = color == Player.White ? 7 : 0;
            var pawnRow = color == Player.White ? 6 : 1;

            this[row, 0] = new Rook(color);
            this[row, 1] = new Horseman(color);
            this[row, 2] = new Horseman(color);

            this[row, 3] = new Horseman(color);
            this[row, 4] = new King(color);

            this[row, 5] = new Horseman(color);
            this[row, 6] = new Horseman(color);
            this[row, 7] = new Rook(color);

            for (int c = 0; c < 8; c++)
            {
                this[pawnRow, c] = new Horseman(color);
            }
        }

        private void AddVikingStartPieces(Player color)
        {
            var row = color == Player.White ? 7 : 0;
            var pawnRow = color == Player.White ? 6 : 1;

            this[row, 0] = new Rook(color);
            this[row, 1] = new Knight(color);
            this[row, 2] = new Bishop(color);

            this[row, 3] = new Queen(color);
            this[row, 4] = new King(color);

            this[row, 5] = new Bishop(color);
            this[row, 6] = new Knight(color);
            this[row, 7] = new Rook(color);

            for (int c = 0; c < 8; c++)
            {
                this[pawnRow, c] = new VikingPawn(color);
            }
        }

        private void AddBritonStartPieces(Player color)
        {
            var row = color == Player.White ? 7 : 0;
            var pawnRow = color == Player.White ? 6 : 1;

            this[row, 0] = new Rook(color);
            this[row, 1] = new Knight(color);
            this[row, 2] = new Bishop(color);

            this[row, 3] = new Queen(color);
            this[row, 4] = new King(color);

            this[row, 5] = new Bishop(color);
            this[row, 6] = new Knight(color);
            this[row, 7] = new Rook(color);

            for (int c = 0; c < 8; c++)
            {
                this[pawnRow, c] = new BritonPawn(color);
            }
        }

        private void AddEgyptStartPieces(Player color)
        {
            var row = color == Player.White ? 7 : 0;
            var pawnRow = color == Player.White ? 6 : 1;

            this[row, 0] = new Rook(color);
            this[row, 1] = new Bishop(color);
            this[row, 2] = new Bishop(color);

            this[row, 3] = new Bishop(color);
            var king = new King(color)
            {
                IsEgyptianKing = true
            };
            this[row, 4] = king;

            this[row, 5] = new Bishop(color);
            this[row, 6] = new Bishop(color);
            this[row, 7] = new Rook(color);

            for (int c = 0; c < 8; c++)
            {
                this[pawnRow, c] = new Pawn(color);
            }
        }

        public static bool IsInside(Position pos)
        {
            return pos.Row >= 0 && pos.Row < 8 && pos.Column >= 0 && pos.Column < 8;
        }

        public bool IsEmpty(Position pos)
        {
            return this[pos] == null;
        }

        private IEnumerable<Position> PiecePositions() // get all positions with pieces
        {
            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    Position pos = new Position(r, c);
                    if (!IsEmpty(pos))
                    {
                        yield return pos;
                    }
                }
            }
        }

        public IEnumerable<Position> PiecePositionsFor(Player player) // get all positions with pieces of a certain color
        {
            return PiecePositions().Where(pos => this[pos].Color == player);
        }

        public bool IsInCheck(Player player)
        {
            return PiecePositionsFor(player.Opponent()).Any(pos =>
            {
                Piece piece = this[pos];
                return piece.CanCaptureOpponentKing(pos, this);
            });
        }

        public Board Copy() // we need this to filter out moves that would leave the current player's king in check
        {
            Board copy = new Board();

            foreach (Position pos in PiecePositions())
            {
                copy[pos] = this[pos].Copy();
            }

            return copy;
        }

        public Counting CountPieces() // doesn't have to be public. But might be useful when computer controlled players are added
        {
            Counting counting = new Counting();

            foreach (Position pos in PiecePositions())
            {
                Piece piece = this[pos];
                counting.Increment(piece.Color, piece.Type);
            }

            return counting;
        }

        public bool InsufficientMaterial()
        {
            Counting counting = CountPieces();

            return IsKingVKing(counting) || IsKingBishopVKing(counting) || 
                IsKingKnightVKing(counting) || IsKingBishopVKingBishop(counting);
        }

        private static bool IsKingVKing(Counting counting)
        {
            // if there are only 2 pieces, they must both be kings
            return counting.TotalCount == 2;
        }

        private static bool IsKingBishopVKing(Counting counting)
        {
            // if there are 3 pieces and one of them is a black or white bishop.
            return counting.TotalCount == 3 && (counting.White(PieceType.Bishop) == 1 || counting.Black(PieceType.Bishop) == 1);
        }

        private static bool IsKingKnightVKing(Counting counting)
        {
            // if there are 3 pieces and one of them is a black or white knight.
            return counting.TotalCount == 3 && (counting.White(PieceType.Knight) == 1 || counting.Black(PieceType.Knight) == 1);
        }

        private bool IsKingBishopVKingBishop(Counting counting)
        {
            // insufficient if both bishops are on the same color square
            if (counting.TotalCount != 4)
            {
                return false;
            }

            if (counting.White(PieceType.Bishop) != 1 || counting.Black(PieceType.Bishop) != 1)
            {
                return false;
            }

            // there are two bishops and two kings

            // check if bishops are on the squares of same color.
            Position wBishopPos = FindPiece(Player.White, PieceType.Bishop);
            Position bBishopPos = FindPiece(Player.Black, PieceType.Bishop);

            return wBishopPos.SquareColor() == bBishopPos.SquareColor();
        }

        private Position FindPiece(Player color, PieceType type)
        {
            // return first piece of color and type found
            return PiecePositionsFor(color).First(pos => this[pos].Type == type);
        }


        // threefold repetition rule
        private bool IsUnmovedKingAndRook(Position kingPos, Position rookPos)
        {
            if (IsEmpty(kingPos) || IsEmpty(rookPos))
            {
                // one of the pieces have moved.
                return false;
            }

            Piece king = this[kingPos];
            Piece rook = this[rookPos];

            return king.Type == PieceType.King && rook.Type == PieceType.Rook // if indeed the king and rook are there (no need for it though)
                && !king.HasMoved && !rook.HasMoved; // and none of them have moved.
        }

        public bool CastleRightsKS(Player player)
        {
            return player switch
            {
                Player.White => IsUnmovedKingAndRook(new Position(7, 4), new Position(7, 7)), // start positions of king and rook
                Player.Black => IsUnmovedKingAndRook(new Position(0, 4), new Position(0, 7)),
                _ => false
            };
        }

        public bool CastleRightsQS(Player player)
        {
            return player switch
            {
                Player.White => IsUnmovedKingAndRook(new Position(7, 4), new Position(7, 0)),
                Player.Black => IsUnmovedKingAndRook(new Position(0, 4), new Position(0, 0)),
                _ => false
            };
        }

        private bool HasPawnInPosition(Player player, Position[] pawnPositions, Position skipPos)
        {
            // if the given player has a pawn which can move to skipPos.
            foreach (Position pos in pawnPositions.Where(IsInside)) // if pos is inside the board
            {
                Piece piece = this[pos];
                if (piece == null || piece.Color != player || piece.Type != PieceType.Pawn)
                {
                    continue;
                }

                // there is a pawn with the correct color and position to capture the skipPos

                // check if that move doesn't leave the king in check.
                EnPassant move = new EnPassant(pos, skipPos);
                if (move.IsLegal(this))
                {
                    return true;
                }
            }

            return false;
        }

        public bool CanCaptureEnPassant(Player player)
        {
            Position skipPos = GetPawnSkipPosition(player.Opponent()); // en passant target square

            if (skipPos == null)
            {
                // the opponent did not move a pawn two squares in the previous move
                return false;
            }

            // do we have a pawn that can capture en passant
            Position[] pawnPositions = player switch
            {
                // positions from where a pawn can move to the en passant square
                Player.White => new Position[] { skipPos + Direction.SouthWest, skipPos + Direction.SouthEast },
                Player.Black => new Position[] { skipPos + Direction.NorthWest, skipPos + Direction.NorthEast },
                _ => Array.Empty<Position>()
            };

            return HasPawnInPosition(player, pawnPositions, skipPos);
        }

        // end threefold repetition rule
    }
}

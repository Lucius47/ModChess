using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using ChessLogic;

namespace ChessUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Image[,] pieceImages = new Image[8, 8];
        private readonly Rectangle[,] highlights = new Rectangle[8, 8];
        private readonly Dictionary<Position, Move> moveCache = new Dictionary<Position, Move>(); // legal moves for the selected piece

        private GameState gameState;
        private Position selectedPos = null; // selected piece

        public MainWindow()
        {
            InitializeComponent();
            InitializeBoard();

            MainMenu mainMenu = new MainMenu();
            MenuContainer.Content = mainMenu;

            foreach (var civ in Enum.GetValues(typeof(Civilizations)))
            {
                mainMenu.Player1Selection.Items.Add(civ.ToString());
                mainMenu.Player2Selection.Items.Add(civ.ToString());
            }

            mainMenu.OptionSelected += option =>
            {
                MenuContainer.Content = null;
                
                if (option == Option.Exit)
                {
                    Application.Current.Shutdown();
                }
                else if (option == Option.Restart)
                {
                    gameState = new GameState(Player.White, Board.Initial(
                        (Civilizations)mainMenu.Player1Selection.SelectedIndex,
                        (Civilizations)mainMenu.Player2Selection.SelectedIndex
                        ));
                    DrawBoard(gameState.Board);
                    SetCursor(gameState.CurrentPlayer);
                }
            };
        }

        private void InitializeBoard()
        {
            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    Image image = new Image();
                    pieceImages[r, c] = image;
                    PieceGrid.Children.Add(image);

                    Rectangle highlight = new Rectangle();
                    highlights[r, c] = highlight;
                    HighlightGrid.Children.Add(highlight); // highlights are transparent by default
                }
            }
        }

        private void DrawBoard(Board board)
        {
            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    Piece piece = board[r, c];
                    pieceImages[r, c].Source = Images.GetImage(piece);
                }
            }
        }

        private void BoardGrid_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (IsMenuOnScreen()) // ignore mouse events when a menu is open.
            {
                return;
            }

            Point point = e.GetPosition(BoardGrid);
            Position pos = ToSquarePosition(point);

            if (selectedPos == null)  // when the first square is selected
            {
                OnFromPositionSelected(pos);
            }
            else // when the second square is selected
            {
                OnToPositionSelected(pos);
            }
        }

        private Position ToSquarePosition(Point point) // get board position from mouse position
        {
            double squareSize = BoardGrid.ActualWidth / 8;

            int row = (int)(point.Y / squareSize);
            int column = (int)(point.X / squareSize);
            return new Position(row, column);
        }

        private void OnFromPositionSelected(Position pos)
        {
            IEnumerable<Move> moves = gameState.LegalMovesForPiece(pos); // empty if square is empty or piece cannot be moved

            if (moves.Any())
            {
                selectedPos = pos;
                CacheMoves(moves);
                ShowHighlights();
            }
        }

        private void OnToPositionSelected(Position pos)
        {
            selectedPos = null;
            HideHighlights();

            if (moveCache.TryGetValue(pos, out Move move))
            {
                if (move.Type == MoveType.PawnPromotion) // if pawn promotion move
                {
                    HandlePromotion(move.FromPos, move.ToPos); // cannot handle the move directly because we need to show the promotion menu
                }
                else // normal move
                {
                    HandleMove(move);
                }
            }
        }

        private void HandlePromotion(Position from, Position to)
        {
            // show the pawn at the promotion position and wait for the user to select a piece
            pieceImages[to.Row, to.Column].Source = Images.GetImage(gameState.CurrentPlayer, PieceType.Pawn);
            // hide the pawn from the original position
            pieceImages[from.Row, from.Column].Source = null;

            PromotionMenu promotionMenu = new PromotionMenu(gameState.CurrentPlayer);
            MenuContainer.Content = promotionMenu; // this will make it visible on the screen

            promotionMenu.PieceSelected += type =>
            {
                MenuContainer.Content = null; // hide the menu
                Move promMove = new PawnPromotion(from, to, type);
                HandleMove(promMove);
            };
        }

        private void HandleMove(Move move)
        {
            gameState.MakeMove(move);
            DrawBoard(gameState.Board);
            SetCursor(gameState.CurrentPlayer);

            if (gameState.IsGameOver())
            {
                ShowGameOver();
            }
        }

        private void CacheMoves(IEnumerable<Move> moves)
        {
            moveCache.Clear();

            foreach (Move move in moves)
            {
                moveCache[move.ToPos] = move;
            }
        }

        private void ShowHighlights()
        {
            Color color = Color.FromArgb(150, 125, 255, 125);

            foreach (Position to in moveCache.Keys)
            {
                highlights[to.Row, to.Column].Fill = new SolidColorBrush(color);
            }
        }

        private void HideHighlights()
        {
            foreach (Position to in moveCache.Keys)
            {
                highlights[to.Row, to.Column].Fill = Brushes.Transparent;
            }
        }

        private void SetCursor(Player player)
        {
            if (player == Player.White)
            {
                Cursor = ChessCursors.WhiteCursor;
            }
            else
            {
                Cursor = ChessCursors.BlackCursor;
            }
        }

        private bool IsMenuOnScreen()
        {
            return MenuContainer.Content != null;
        }

        private void ShowGameOver()
        {
            GameOverMenu gameOverMenu = new GameOverMenu(gameState);
            MenuContainer.Content = gameOverMenu;

            gameOverMenu.OptionSelected += option =>
            {
                if (option == Option.Restart)
                {
                    MenuContainer.Content = null;
                    RestartGame();
                }
                else // if quit button is pressed
                {
                    Application.Current.Shutdown();
                }
            };
        }

        private void RestartGame()
        {
            selectedPos = null; // it is possible that a piece is selected when the game is restarted

            HideHighlights();
            moveCache.Clear();
            
            MainMenu mainMenu = new MainMenu();
            MenuContainer.Content = mainMenu;

            foreach (var civ in Enum.GetValues(typeof(Civilizations)))
            {
                mainMenu.Player1Selection.Items.Add(civ.ToString());
                mainMenu.Player2Selection.Items.Add(civ.ToString());
            }

            mainMenu.OptionSelected += option =>
            {
                MenuContainer.Content = null;
                
                if (option == Option.Exit)
                {
                    Application.Current.Shutdown();
                }
                else if (option == Option.Restart)
                {
                    gameState = new GameState(Player.White, Board.Initial(
                        (Civilizations)mainMenu.Player1Selection.SelectedIndex,
                        (Civilizations)mainMenu.Player2Selection.SelectedIndex
                    ));
                    DrawBoard(gameState.Board);
                    SetCursor(gameState.CurrentPlayer);
                }
            };
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (!IsMenuOnScreen() && e.Key == System.Windows.Input.Key.Escape)
            {
                ShowPauseMenu();
            }
        }

        private void ShowPauseMenu()
        {
            PauseMenu pauseMenu = new PauseMenu();
            MenuContainer.Content = pauseMenu;

            pauseMenu.OptionSelected += option =>
            {
                MenuContainer.Content = null;

                if (option == Option.Restart)
                {
                    RestartGame();
                }
            };
        }

        // AI
        private void HandleAIMove()
        {

        }
        // end AI
    }
}
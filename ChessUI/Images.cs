using System.Windows.Media;
using System.Windows.Media.Imaging;
using ChessLogic;

namespace ChessUI
{
    public static class Images
    {
        private static readonly Dictionary<PieceType, ImageSource> whiteSources = new()
        {
            {PieceType.Pawn, LoadImage("Assets/PawnW.png") },
            {PieceType.VikingPawn, LoadImage("Assets/PawnW.png") },
            {PieceType.BritonPawn, LoadImage("Assets/PawnW.png") },
            {PieceType.Rook, LoadImage("Assets/RookW.png") },
            {PieceType.Knight, LoadImage("Assets/KnightW.png") },
            {PieceType.Horseman, LoadImage("Assets/HorsemanW.png") },
            {PieceType.Tank, LoadImage("Assets/TankW.png") },
            {PieceType.Bishop, LoadImage("Assets/BishopW.png") },
            {PieceType.RomanBishop, LoadImage("Assets/BishopW.png") },
            {PieceType.Queen, LoadImage("Assets/QueenW.png") },
            {PieceType.King, LoadImage("Assets/KingW.png") }
        };

        private static readonly Dictionary<PieceType, ImageSource> blackSources = new()
        {
            {PieceType.Pawn, LoadImage("Assets/PawnB.png") },
            {PieceType.VikingPawn, LoadImage("Assets/PawnB.png") },
            {PieceType.BritonPawn, LoadImage("Assets/PawnB.png") },
            {PieceType.Rook, LoadImage("Assets/RookB.png") },
            {PieceType.Knight, LoadImage("Assets/KnightB.png") },
            {PieceType.Horseman, LoadImage("Assets/HorsemanB.png") },
            {PieceType.Tank, LoadImage("Assets/TankB.png") },
            {PieceType.Bishop, LoadImage("Assets/BishopB.png") },
            {PieceType.RomanBishop, LoadImage("Assets/BishopB.png") },
            {PieceType.Queen, LoadImage("Assets/QueenB.png") },
            {PieceType.King, LoadImage("Assets/KingB.png") }
        };

        private static ImageSource LoadImage(string filepath)
        {
            return new BitmapImage(new Uri(filepath, UriKind.Relative));
        }

        public static ImageSource GetImage(Player color, PieceType type)
        {
            return color switch
            {
                Player.White => whiteSources[type],
                Player.Black => blackSources[type],
                _ => null, //throw new ArgumentException("Invalid color")
            };
        }

        public static ImageSource GetImage(Piece piece)
        {
            if (piece == null)
            {
                return null;
            }

            return GetImage(piece.Color, piece.Type);
        }
    }
}

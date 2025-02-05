using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Tetris.Grid;
using Tetris.Grid.Blocks;
using Tetris.Models;

namespace Tetris
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ImageSource[] _titleImages =
        [
            new BitmapImage(new Uri("Assets/TileEmpty.png",UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileCyan.png",UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileBlue.png",UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileOrange.png",UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileYellow.png",UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileGreen.png",UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TilePurple.png",UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileRed.png",UriKind.Relative)),
        ];

        private readonly ImageSource[] _blockImages =
        [
            new BitmapImage(new Uri("Assets/Block-Empty.png",UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-I.png",UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-J.png",UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-L.png",UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-O.png",UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-S.png",UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-T.png",UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-Z.png",UriKind.Relative)),
        ];

        private readonly Image[,] _imageControl;

        private GameState _gameState = new();
        public MainWindow()
        {
            InitializeComponent();
            _imageControl = SetupGameCanvas(_gameState.GameGrid);
        }

        private Image[,] SetupGameCanvas(GameGrid grid)
        {
            Image[,] imageControls = new Image[grid.Rows, grid.Columns];
            int cellSize = 25;

            for (int r = 0; r < grid.Rows; r++)
            {
                for (int c = 0; c < grid.Columns; c++)
                {
                    Image imageControl = new()
                    {
                        Width = cellSize,
                        Height = cellSize,
                    };

                    Canvas.SetTop(imageControl, (r - 2) * cellSize);
                    Canvas.SetLeft(imageControl, c * cellSize);
                    GameCanvas.Children.Add(imageControl);
                    imageControls[r, c] = imageControl;
                }
            }
            return imageControls;
        }

        private void DrawGrid(GameGrid grid)
        {
            for (int r = 0; r < grid.Rows; r++)
            {
                for (int c = 0; c < grid.Columns; c++)
                {
                    int id = grid[r, c];
                    _imageControl[r, c].Source = _titleImages[id];
                }
            }
        }

        private void DrawBlock(Block block)
        {
            foreach (Position p in block.TitlePositions())
            {
                _imageControl[p.Row, p.Column].Source = _titleImages[block.Id];
            }
        }


        private void Draw(GameState gameState)
        {
            DrawGrid(gameState.GameGrid);
            DrawBlock(gameState.CurrentBlock);
        }

        private async Task GameLoop()
        {
            Draw(_gameState);

            while (!_gameState.GameOver)
            {
                await Task.Delay(500);
                _gameState.MoveBlockDown();
                Draw(_gameState);
            }

            GameOverMenu.Visibility = Visibility.Visible;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (_gameState.GameOver)
            {
                return;
            }

            switch (e.Key)
            {
                case Key.Left:
                    _gameState.MoveBlockLeft();
                    break;
                case Key.Right:
                    _gameState.MoveBlockRight();
                    break;
                case Key.Down:
                    _gameState.MoveBlockDown();
                    break;
                case Key.Up:
                    _gameState.RotateBlockCW();
                    break;
                case Key.Z:
                    _gameState.RotateBlockCCW();
                    break;
                default:
                    return;
            }

            Draw(_gameState);
        }

        private async void GameCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            await GameLoop();
        }

        private async void PlayAgain_Click(object sender, RoutedEventArgs e)
        {
            _gameState = new GameState();
            GameOverMenu.Visibility = Visibility.Hidden;
            await GameLoop();
        }
    }
}
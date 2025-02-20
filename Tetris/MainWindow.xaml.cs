﻿using System.Windows;
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
        private readonly ImageSource[] _tileImages =
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

        private readonly Image[,] _imageControls;
        private readonly int _maxDelay = 1000;
        private readonly int _minDelay = 75;
        private readonly int _delayDecrease = 75;

        private GameState _gameState = new();
        public MainWindow()
        {
            InitializeComponent();
            _imageControls = SetupGameCanvas(_gameState.GameGrid);
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

                    Canvas.SetTop(imageControl, (r - 2) * cellSize + 10);
                    Canvas.SetLeft(imageControl, c * cellSize);
                    GameCanvas.Children.Add(imageControl);
                    imageControls[r, c] = imageControl;
                }
            }
            return imageControls;
        }

        #region Draws
        private void DrawGrid(GameGrid grid)
        {
            for (int r = 0; r < grid.Rows; r++)
            {
                for (int c = 0; c < grid.Columns; c++)
                {
                    int id = grid[r, c];
                    _imageControls[r, c].Opacity = 1;
                    _imageControls[r, c].Source = _tileImages[id];
                }
            }
        }

        private void DrawBlock(Block block)
        {
            foreach(Position p in block.TitlePositions())
            {
                _imageControls[p.Row, p.Column].Opacity = 1;
                _imageControls[p.Row, p.Column].Source = _tileImages[block.Id];
            }
        }

        private void DrawNextBlock(BlockQueue blockQueue)
        {
            Block next = blockQueue.NextBlock;

            NextBlock.Source = _blockImages[next.Id];
        }

        private void DrawHeldBlock (Block heldBlock)
        {
            if(heldBlock == null)
            {
                HoldImage.Source = _blockImages[0];
            }
            else
            {
                HoldImage.Source = _blockImages[heldBlock.Id];
            }
        }

        private void DrawGhostBlock (Block ghostBlock)
        {
            int dropDistance = _gameState.BlockDropDistance();

            foreach(Position p in ghostBlock.TitlePositions())
            {
                _imageControls[p.Row + dropDistance, p.Column].Opacity = 0.25;
                _imageControls[p.Row + dropDistance,p.Column].Source = _tileImages[ghostBlock.Id];
            }

        }

        private void Draw(GameState gameState)
        {
            DrawGrid(gameState.GameGrid);
            DrawGhostBlock(gameState.CurrentBlock);
            DrawBlock(gameState.CurrentBlock);
            DrawNextBlock(gameState.BlockQueue);
            DrawHeldBlock(gameState.HeldBlock);
            ScoreText.Text = $"Score: {gameState.Score}";

        }

        #endregion
        private async Task GameLoop()
        {
            Draw(_gameState);

            while (!_gameState.GameOver)
            {
                int delay = Math.Max(_minDelay, _maxDelay - (_gameState.Score * _delayDecrease));
                await Task.Delay(delay);
                _gameState.MoveBlockDown();
                Draw(_gameState);
            }

            GameOverMenu.Visibility = Visibility.Visible;
            FinalScoreTotal.Text = $"Score: {_gameState.Score}";
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
                case Key.Z:
                    _gameState.RotateBlockCW();
                    break;
                case Key.X:
                    _gameState.RotateBlockCCW();
                    break;
                case Key.C:
                    _gameState.HoldBlock();
                    break;
                case Key.V:
                    _gameState.DropBlock();
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
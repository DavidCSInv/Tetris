using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Tetris.Grid.Blocks;
using Tetris.Models;

namespace Tetris.Grid
{
    public class GameState
    {
        public GameGrid GameGrid { get; }
        public BlockQueue BlockQueue { get; }
        public bool GameOver { get; private set; }
        private Block _currentBlock;
        public int Score;
        public Block HeldBlock { get; private set; }
        public bool CanHold {  get; private set; }
        public Block CurrentBlock 
        {
            get => _currentBlock;
            private set
            {
                _currentBlock = value;
                _currentBlock.Reset();

                for(int i = 0; i < 2; i++)
                {
                    _currentBlock.Move(1, 0);

                    if (!BlockFits())
                    {
                        _currentBlock.Move(-1,0);
                    }
                }
            }
        }

        public GameState()
        {
            GameGrid = new GameGrid(22, 10);
            BlockQueue = new BlockQueue();
            CurrentBlock = BlockQueue.GetAndUpdate();
            CanHold = true;
        }
        
        private bool BlockFits()
        {
            foreach(Position p in CurrentBlock.TitlePositions())
            {
                if (!GameGrid.IsEmpty(p.Row, p.Column))
                {
                    return false;
                }
            }
            return true;
        }

        public void HoldBlock()
        {
            if (!CanHold)
            {
                return;
            }

            if(HeldBlock == null)
            {
                HeldBlock = CurrentBlock;
                CurrentBlock = BlockQueue.GetAndUpdate();
            }
            else
            {
                Block temp = CurrentBlock;
                CurrentBlock = HeldBlock;
                HeldBlock = temp;
            }

            CanHold = false;
        }

        #region Block Movement
        public void RotateBlockCW() 
        {
            CurrentBlock.RotationCW();
            if (!BlockFits())
            {
                if (!TryWallKick(CurrentBlock))
                {
                    CurrentBlock.RotationCCW(); 
                }
            }
        }
        public void RotateBlockCCW()
        {
            CurrentBlock.RotationCCW();

            if (!BlockFits())
            {
                if (!TryWallKick(CurrentBlock))
                {
                    CurrentBlock.RotationCW(); 
                }
            }
        }
        public void MoveBlockLeft()
        {
            CurrentBlock.Move(0, -1);

            if (!BlockFits())
            {
                CurrentBlock.Move(0, 1);
            }
        }
        public void MoveBlockRight()
        {
            CurrentBlock.Move(0, 1);

            if (!BlockFits())
            {
                CurrentBlock.Move(0, -1);
            }
        }

        public void MoveBlockDown()
        {
            CurrentBlock.Move(1, 0);

            if (!BlockFits())
            {
                CurrentBlock.Move(-1, 0);
                PlaceBlock();
            }
        }

        #endregion
        private bool IsGameOver()
        {
            return !(GameGrid.IsRowEmpty(0) && GameGrid.IsRowEmpty(1));
        }

        private void PlaceBlock()
        {
            foreach (Position p in CurrentBlock.TitlePositions())
            {
                GameGrid[p.Row, p.Column] = CurrentBlock.Id;
            }

            Score+= GameGrid.ClearFullRows();

            if (IsGameOver())
            {
                GameOver = true;
            }
            else
            {
                CurrentBlock = BlockQueue.GetAndUpdate();
                CanHold = true;
            }
        }

        private int TitleDropDistance(Position p)
        {
            int drop = 0;

            while(GameGrid.IsEmpty(p.Row + drop + 1, p.Column))
            {
                drop++;
            }

            return drop;
        }

        public int BlockDropDistance()
        {
            int drop = GameGrid.Rows;

            foreach(Position p in CurrentBlock.TitlePositions())
            {
                drop = Math.Min(drop, TitleDropDistance(p));
            }
            return drop;
        }

        public void DropBlock()
        {
            CurrentBlock.Move(BlockDropDistance(), 0);
            PlaceBlock();
        }

        private bool TryWallKick(Block block)
        {
            int[] offsets = { -1, 1, -2, 2 };

            foreach (int offset in offsets)
            {
                block.Move(0, offset); 

                if (BlockFits()) 
                {
                    return true;
                }

                block.Move(0, -offset); 
            }

            return false; 
        }
    }
}

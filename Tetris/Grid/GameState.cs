﻿using System;
using System.Collections.Generic;
using System.Linq;
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

        private Block currentBlock;

        public Block CurrentBlock 
        {
            get => currentBlock;
            private set
            {
                currentBlock = value;
                currentBlock.Reset();
            }
        }

        public GameState()
        {
            GameGrid = new GameGrid(22, 10);
            BlockQueue = new BlockQueue();
            CurrentBlock = BlockQueue.GetAndUpdate();
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

        public void RotateBlockCW() 
        {
            CurrentBlock.RotationCW();
            if (!BlockFits())
            {
                CurrentBlock.RotationCW();
            }
        }
        public void RotateBlockCCW()
        {
            CurrentBlock.RotationCCW();
            if (!BlockFits())
            {
                CurrentBlock.RotationCCW();
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

        private bool IsGameOver()
        {
            return !(GameGrid.IsRowEmpty(0) && GameGrid.IsRowFull(1));
        }

        private void PlaceBlock()
        {
            foreach (Position p in CurrentBlock.TitlePositions())
            {
                GameGrid[p.Row, p.Column] = CurrentBlock.id;
            }

            GameGrid.ClearFullRows();

            if (IsGameOver())
            {
                GameOver = true;
            }
            else
            {
                CurrentBlock = BlockQueue.GetAndUpdate();
            }
        }
        private void MoveBlockDown() 
        {
            CurrentBlock.Move(1, 0);

            if (!BlockFits())
            {
                CurrentBlock.Move(-1, 0);
                PlaceBlock();
            }
        }
    }
}

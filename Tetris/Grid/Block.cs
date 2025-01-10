using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris.Models
{
    public abstract class Block
    {
        protected abstract Position[][] Titles { get; }
        protected abstract Position StartOffSet { get; }
        public abstract int id { get; }
        private int rotationState;
        private Position offset;

        public Block()
        {
            offset = new Position(StartOffSet.Row, StartOffSet.Column);
        }

        public IEnumerable<Position> TitlePositions()
        {
            foreach (Position P in Titles[rotationState])
            {
                yield return new Position(P.Row + offset.Row, P.Column + offset.Column);
            }
        }
        //Não necessariamente o player vai rotacionar sentido horario por isso é void
        public void RotationCW()
        {
            rotationState = (rotationState + 1) % Titles.Length;
        }

        public void RotationCCW()
        {
            if (rotationState == 0)
            {
                rotationState = Titles.Length - 1;
            }
            else
            {
                rotationState--;
            }
        }

        public void Move(int rows, int columns)
        {
            offset.Row += rows;
            offset.Column += columns;
        }

        public void Reset()
        {
            rotationState = 0;
            offset = 0;
        }
    }
}

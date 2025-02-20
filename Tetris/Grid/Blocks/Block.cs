using Tetris.Models;

namespace Tetris.Grid.Blocks
{
    public abstract class Block
    {
        protected abstract Position[][] Tiles { get; }
        protected abstract Position StartOffSet { get; }
        public abstract int Id { get; }
        private int _rotationState;
        private readonly Position _offset;

        public Block()
        {
            _offset = new Position(StartOffSet.Row, StartOffSet.Column);
        }

        public IEnumerable<Position> TitlePositions()
        {
            foreach (Position P in Tiles[_rotationState])
            {
                yield return new Position(P.Row + _offset.Row, P.Column + _offset.Column);
            }
        }
        //Não necessariamente o player vai rotacionar sentido horario por isso é void
        public void RotationCW()
        {
            _rotationState = (_rotationState + 1) % Tiles.Length;
        }

        public void RotationCCW()
        {
            if (_rotationState == 0)
            {
                _rotationState = Tiles.Length - 1;
            }
            else
            {
                _rotationState--;
            }
        }

        public void Move(int rows, int columns)
        {
            _offset.Row += rows;
            _offset.Column += columns;
        }

        public void Reset()
        {
            _rotationState = 0;
            _offset.Row = StartOffSet.Row;
            _offset.Column = StartOffSet.Column;
        }
    }
}

using Tetris.Models;

namespace Tetris.Grid.Blocks
{
    public class OBlock : Block
    {
        private readonly Position[][] titles =
        [
            [new (0,0),new (0,1),new (1,0),new (1,1)]
        ];
        public override int id => 4;

        protected override Position StartOffSet => new(0, 4);

        protected override Position[][] Titles => titles;
    }
}

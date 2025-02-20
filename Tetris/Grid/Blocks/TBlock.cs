using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tetris.Models;

namespace Tetris.Grid.Blocks
{
    public class TBlock : Block
    {
        private readonly Position[][] tiles =
        [
            [new (0,1),new (1,0),new(1,1),new Position(1,2)],
            [new (0,1),new (1,1),new(1,2),new Position(2,1)],
            [new (1,0),new (1,1),new(1,2),new Position(2,1)],
            [new (0,1),new (1,0),new(1,1),new Position(2,1)]
        ];
        public override int Id => 6;

        protected override Position[][] Tiles => tiles;

        protected override Position StartOffSet => new (0,3);
    }
}

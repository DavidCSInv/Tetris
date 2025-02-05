using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tetris.Models;

namespace Tetris.Grid.Blocks
{
    public class SBlock : Block
    {
        private readonly Position[][] titles =
        [
            [new (0,1),new (0,2),new(1,0),new Position(1,1)],
            [new (0,1),new (1,1),new(1,2),new Position(2,2)],
            [new (1,1),new (1,2),new(2,0),new Position(2,1)],
            [new (0,0),new (1,0),new(1,1),new Position(2,1)]
        ];

        public override int Id => 5;

        protected override Position[][] Titles => titles;

        protected override Position StartOffSet => new (0,3);
    }
}

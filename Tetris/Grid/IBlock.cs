﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris.Models
{
    public class IBlock : Block
    {

        private readonly Position[][] titles = new Position[][]
        {
            new Position[]{ new (1, 0), new (1, 1),new(1,2),new Position(1,3)},
            new Position[]{ new (0, 2), new (1, 2),new(2,2),new Position(2,3)},
            new Position[]{ new (2, 0), new (2,1), new(2,2),new Position(2,3)},
            new Position[]{ new (0, 1), new (1, 1),new(2,1),new Position(3,1)}
        };

        public override int id => 1;

        protected override Position StartOffSet => new Position(-1,3);

        protected override Position[][] Titles => Titles;
    }
}
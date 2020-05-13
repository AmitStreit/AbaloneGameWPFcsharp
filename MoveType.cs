using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaloneGameWPF
{
    class MoveType
    {
        public PieceType[,] borad = new PieceType[Settings.BORAD_ARRAY_SIZE,Settings.BORAD_ARRAY_SIZE];//לוח משחק 
        public float score;//ניקוד 
    }
}

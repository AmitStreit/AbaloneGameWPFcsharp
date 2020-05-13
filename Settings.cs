using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaloneGameWPF
{
    class Settings
    {
        public const int BORAD_ARRAY_SIZE = 9;//the array is 2 dimantanal so every size of it is 9 (9*9)
        public const int CANVAS_SIZE = 600;//each of the canvas sides is 600 pixels (600p*600p)
        public const int PIECE_SIZE = 65;//the size in pixels of the pice on the canvas  (65p*65p)
        public const bool TESTING_MODE = false;//האים מצב בדיקה פועל
        public const int NUM_OF_EJECTED_PIECES_TO_WIN = 6; //the amount of balls each player needs to eject in oreder to win 
        public static bool IS_BOT_ENABLED = false;// האם משחקים נגד בוט 
        public const int DEFAULT_NUM_OF_PIECES = 14; //the number of starting balls per player
    }
}

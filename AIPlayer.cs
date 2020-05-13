using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Security.Cryptography;

namespace AbaloneGameWPF
{
    /// <summary>
    /// מחלקת הבוט
    /// </summary>
    class AIPlayer
    {
        public PieceType[,] borad;//לוח המשחק 
        public PieceType[,] dynamic_borad;//לוח המשחק שניתן לשינוי 
        public static int[,] distance_table = new int[Settings.BORAD_ARRAY_SIZE, Settings.BORAD_ARRAY_SIZE]
        {
            { -1,-1,-1,-1,4,4,4,4,4},
            { -1,-1,-1,4,3,3,3,3,4},
            { -1,-1,4,3,2,2,2,3,4},

            { -1,4,3,2,1,1,2,3,4},
            { 4,3,2,1,0,1,2,3,4},
            { 4,3,2,1,1,2,3,4,-1},

            { 4,3,2,2,2,3,4,-1,-1},
            { 4,3,3,3,3,4,-1,-1,-1},
            { 4,4,4,4,4,-1,-1,-1,-1},
        };//טבלת המרחקים 
        PieceType rivel = PieceType.white;//שחקן יריב
        PieceType friendly = PieceType.black;//שחקן ידידותי
        PieceType friendly_selected = PieceType.selected_black_bot;//שחקן ידידתי נבחר 

        /// <summary>
        /// פעולה בונה 
        /// </summary>
        public AIPlayer()
        {

        }

        /// <summary>
        /// פעולה זו נקראת כאשר זה הוא התור של הבוט והוא צריך לבצע צעד 
        /// </summary>
        /// <param name="original_game_borad">לוח המשחק המקורי </param>
        /// <param name="frienfly_num">מספר החיילים הידידותיים </param>
        /// <param name="rivel_num">מספר היריבים </param>
        public void Make_Move(ref PieceType[,] original_game_borad, int frienfly_num, int rivel_num)//unfinished
        {
            bool isfirst = true;
            MoveType best_move = new MoveType();
            MoveType possible_move = new MoveType();

            PieceType[,] dynamic_borad = new PieceType[Settings.BORAD_ARRAY_SIZE, Settings.BORAD_ARRAY_SIZE];
            Copy_Arrays(ref original_game_borad, ref dynamic_borad);
            for (int i = 0; i < Settings.BORAD_ARRAY_SIZE; i++)
            {
                for (int j = 0; j < Settings.BORAD_ARRAY_SIZE; j++)
                {
                    if (dynamic_borad[i, j] == friendly || dynamic_borad[i, j] == friendly_selected)
                    {
                        if (isfirst)
                        {
                            isfirst = false;
                            best_move = Get_Best_Move_Of_Piece(ref dynamic_borad, j, i);
                        }
                        else
                        {
                            possible_move = Get_Best_Move_Of_Piece(ref dynamic_borad, j, i);
                            if (possible_move.score > best_move.score)
                            {
                                best_move.score = possible_move.score;
                                Copy_Arrays(ref possible_move.borad, ref best_move.borad);
                            }
                        }
                        Copy_Arrays(ref original_game_borad, ref dynamic_borad);
                    }
                }
            }
            Copy_Arrays(ref best_move.borad, ref original_game_borad);
        }

        /// <summary>
        /// פעולה זו מחזירה את הצעד הטוב ביותר לחייל ספוציפי 
        /// </summary>
        /// <param name="og_borad">לוח מקורי </param>
        /// <param name="x">שיעור ה איקס של החייל </param>
        /// <param name="y">שיעור הוואי של החייל </param>
        /// <returns>פעולה זו מחזירה את הצעד הטוב ביותר לחייל ספוציפי </returns>
        public MoveType Get_Best_Move_Of_Piece(ref PieceType[,] og_borad, int x, int y)
        {
            MoveType bestM = new MoveType();
            MoveType possible = new MoveType();
            bestM = Check_All_Moves_Of_One(ref og_borad, x, y);
            possible = Check_All_Moves_Of_Two(ref og_borad, x, y);
            if (possible.score > bestM.score)
            {
                bestM.score = possible.score;
                Copy_Arrays(ref possible.borad, ref bestM.borad);
            }
            possible = Check_All_Moves_Of_Three(ref og_borad, x, y);
            if (possible.score > bestM.score)
            {
                bestM.score = possible.score;
                Copy_Arrays(ref possible.borad, ref bestM.borad);
            }
            possible = Check_All_Shifts_Of_Two(ref og_borad, x, y);
            if (possible.score > bestM.score)
            {
                bestM.score = possible.score;
                Copy_Arrays(ref possible.borad, ref bestM.borad);
            }
            possible = Check_All_Shifts_Of_Three(ref og_borad, x, y);
            if (possible.score > bestM.score)
            {
                bestM.score = possible.score;
                Copy_Arrays(ref possible.borad, ref bestM.borad);
            }
            possible = Check_All_Eets_Of_Two(ref og_borad, x, y);
            if (possible.score > bestM.score)
            {
                bestM.score = possible.score;
                Copy_Arrays(ref possible.borad, ref bestM.borad);
            }
            possible = Check_All_Eets_Of_Three(ref og_borad, x, y);
            if (possible.score > bestM.score)
            {
                bestM.score = possible.score;
                Copy_Arrays(ref possible.borad, ref bestM.borad);
            }
            return bestM;
        }

        /// <summary>
        /// פעולה זו בודקת את כול הצעדים האפשריים של חייל אחד 
        /// </summary>
        /// <param name="unchengeable_borad">לוח מקורי </param>
        /// <param name="x">נצ חייל </param>
        /// <param name="y">נצ חייל</param>
        /// <returns>הצעד הטוב ביותר שמצאה </returns>
        public MoveType Check_All_Moves_Of_One(ref PieceType[,] unchengeable_borad, int x, int y)
        {
            float current_score;
            MoveType goodest = new MoveType();
            goodest.score = -10000;
            PieceType[,] destroyable_array = new PieceType[Settings.BORAD_ARRAY_SIZE, Settings.BORAD_ARRAY_SIZE];
            Copy_Arrays(ref unchengeable_borad, ref destroyable_array);

            if (y != 0)//1
            {
                if (destroyable_array[y - 1, x] == PieceType.free)
                {
                    destroyable_array[y - 1, x] = friendly_selected;
                    destroyable_array[y, x] = PieceType.free;
                    current_score = Evaluate_Borad(destroyable_array, 0, 0);
                    if (current_score > goodest.score)
                    {
                        goodest.score = current_score;
                        Copy_Arrays(ref destroyable_array, ref goodest.borad);
                    }
                    Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                }
            }//1
            if (y != 0 && x != 8)//2
            {
                if (destroyable_array[y - 1, x + 1] == PieceType.free)
                {
                    destroyable_array[y - 1, x + 1] = friendly_selected;
                    destroyable_array[y, x] = PieceType.free;
                    current_score = Evaluate_Borad(destroyable_array, 0, 0);
                    if (current_score > goodest.score)
                    {
                        goodest.score = current_score;
                        Copy_Arrays(ref destroyable_array, ref goodest.borad);
                    }
                    Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                }
            }//2
            if (x != 8)//3
            {
                if (y != 0 && x != 8)//2
                {
                    if (destroyable_array[y, x + 1] == PieceType.free)
                    {
                        destroyable_array[y, x + 1] = friendly_selected;
                        destroyable_array[y, x] = PieceType.free;
                        current_score = Evaluate_Borad(destroyable_array, 0, 0);
                        if (current_score > goodest.score)
                        {
                            goodest.score = current_score;
                            Copy_Arrays(ref destroyable_array, ref goodest.borad);
                        }
                        Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                    }
                }
            }//3
            if (y != 8)//4
            {
                if (destroyable_array[y + 1, x] == PieceType.free)
                {
                    destroyable_array[y + 1, x] = friendly_selected;
                    destroyable_array[y, x] = PieceType.free;
                    current_score = Evaluate_Borad(destroyable_array, 0, 0);
                    if (current_score > goodest.score)
                    {
                        goodest.score = current_score;
                        Copy_Arrays(ref destroyable_array, ref goodest.borad);
                    }
                    Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                }
            }//4
            if (y != 8 && x != 0)//5
            {
                if (destroyable_array[y + 1, x - 1] == PieceType.free)
                {
                    destroyable_array[y + 1, x - 1] = friendly_selected;
                    destroyable_array[y, x] = PieceType.free;
                    current_score = Evaluate_Borad(destroyable_array, 0, 0);
                    if (current_score > goodest.score)
                    {
                        goodest.score = current_score;
                        Copy_Arrays(ref destroyable_array, ref goodest.borad);
                    }
                    Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                }
            }//5
            if (x != 0)//6
            {
                if (destroyable_array[y, x - 1] == PieceType.free)
                {
                    destroyable_array[y, x - 1] = friendly_selected;
                    destroyable_array[y, x] = PieceType.free;
                    current_score = Evaluate_Borad(destroyable_array, 0, 0);
                    if (current_score > goodest.score)
                    {
                        goodest.score = current_score;
                        Copy_Arrays(ref destroyable_array, ref goodest.borad);
                    }
                    Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                }
            }//5
            return goodest;
        }//finished with blue

        /// <summary>
        /// פעולה זו בודקת את כול הצעדים האפשריים של 2 
        /// </summary>
        /// <param name="unchengeable_borad">לוח מקורי </param>
        /// <param name="x">נצ חייל </param>
        /// <param name="y">נצ חייל</param>
        /// <returns>הצעד הכי טוב שהיא מצאה</returns>
        public MoveType Check_All_Moves_Of_Two(ref PieceType[,] unchengeable_borad, int x, int y)
        {
            int x2, y2;
            float current_score;
            MoveType goodest = new MoveType();
            goodest.score = -10000;
            PieceType[,] destroyable_array = new PieceType[Settings.BORAD_ARRAY_SIZE, Settings.BORAD_ARRAY_SIZE];
            Copy_Arrays(ref unchengeable_borad, ref destroyable_array);

            //this is flat case 
            if (x != 0)
            {
                if (destroyable_array[y, x - 1] == friendly)//left movment is possible
                {
                    if (x >= 2)
                    {
                        if (destroyable_array[y, x - 2] == PieceType.free)//flat left
                        {
                            //move is possible. make it and check it
                            destroyable_array[y, x - 2] = friendly_selected;
                            destroyable_array[y, x - 1] = friendly_selected;
                            destroyable_array[y, x] = PieceType.free;
                            current_score = Evaluate_Borad(destroyable_array, 0, 0);
                            if (current_score > goodest.score)
                            {
                                goodest.score = current_score;
                                Copy_Arrays(ref destroyable_array, ref goodest.borad);
                            }
                            Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                        }//flat left
                    }
                    if (x != 8)
                    {
                        if (destroyable_array[y, x + 1] == PieceType.free)//flat right
                        {
                            //move is possible. make it and check it
                            destroyable_array[y, x + 1] = friendly_selected;
                            destroyable_array[y, x] = friendly_selected;
                            destroyable_array[y, x - 1] = PieceType.free;
                            current_score = Evaluate_Borad(destroyable_array, 0, 0);
                            if (current_score > goodest.score)
                            {
                                goodest.score = current_score;
                                Copy_Arrays(ref destroyable_array, ref goodest.borad);
                            }
                            Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                        }//flat right
                    }
                    if (y != 0)
                    {
                        if (destroyable_array[y - 1, x] == PieceType.free)//if upmid is free
                        {
                            if (destroyable_array[y - 1, x - 1] == PieceType.free)//upleft
                            {
                                //move is possible. make it and check it
                                destroyable_array[y - 1, x] = friendly_selected;
                                destroyable_array[y - 1, x - 1] = friendly_selected;
                                destroyable_array[y, x] = PieceType.free;
                                destroyable_array[y, x - 1] = PieceType.free;

                                current_score = Evaluate_Borad(destroyable_array, 0, 0);
                                if (current_score > goodest.score)
                                {
                                    goodest.score = current_score;
                                    Copy_Arrays(ref destroyable_array, ref goodest.borad);
                                }
                                Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                            }//upleft

                            if (x != 8)
                            {
                                if (destroyable_array[y - 1, x + 1] == PieceType.free)//upright
                                {
                                    //move is possible. make it and check it
                                    destroyable_array[y - 1, x] = friendly_selected;
                                    destroyable_array[y - 1, x + 1] = friendly_selected;
                                    destroyable_array[y, x] = PieceType.free;
                                    destroyable_array[y, x - 1] = PieceType.free;

                                    current_score = Evaluate_Borad(destroyable_array, 0, 0);
                                    if (current_score > goodest.score)
                                    {
                                        goodest.score = current_score;
                                        Copy_Arrays(ref destroyable_array, ref goodest.borad);
                                    }
                                    Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                                }//upright
                            }
                        }//if up mid is free
                    }
                    if (y != 8)
                    {
                        if (destroyable_array[y + 1, x - 1] == PieceType.free)//if down mid is free
                        {
                            if (x >= 2)
                            {
                                if (destroyable_array[y + 1, x - 2] == PieceType.free)//downleft
                                {
                                    //move is possible. make it and check it
                                    destroyable_array[y + 1, x - 1] = friendly_selected;
                                    destroyable_array[y + 1, x - 2] = friendly_selected;
                                    destroyable_array[y, x] = PieceType.free;
                                    destroyable_array[y, x - 1] = PieceType.free;

                                    current_score = Evaluate_Borad(destroyable_array, 0, 0);
                                    if (current_score > goodest.score)
                                    {
                                        goodest.score = current_score;
                                        Copy_Arrays(ref destroyable_array, ref goodest.borad);
                                    }
                                    Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                                }//downleft
                            }

                            if (destroyable_array[y + 1, x] == PieceType.free)//downright
                            {
                                //move is possible. make it and check it
                                destroyable_array[y + 1, x - 1] = friendly_selected;
                                destroyable_array[y + 1, x] = friendly_selected;
                                destroyable_array[y, x] = PieceType.free;
                                destroyable_array[y, x - 1] = PieceType.free;

                                current_score = Evaluate_Borad(destroyable_array, 0, 0);
                                if (current_score > goodest.score)
                                {
                                    goodest.score = current_score;
                                    Copy_Arrays(ref destroyable_array, ref goodest.borad);
                                }
                                Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                            }//downright

                        }//if down mid is free
                    }
                }//finished
            }//left movment mybe possible finished
            if (x != 8)
            {
                if (destroyable_array[y, x + 1] == friendly)//right movment is possible
                {
                    if (x != 0)
                    {
                        if (destroyable_array[y, x - 1] == PieceType.free)//flat left
                        {
                            //move is possible. make it and check it
                            destroyable_array[y, x - 1] = friendly_selected;
                            destroyable_array[y, x] = friendly_selected;
                            destroyable_array[y, x + 1] = PieceType.free;
                            current_score = Evaluate_Borad(destroyable_array, 0, 0);
                            if (current_score > goodest.score)
                            {
                                goodest.score = current_score;
                                Copy_Arrays(ref destroyable_array, ref goodest.borad);
                            }
                            Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                        }//flat left
                    }
                    if (x <= 6)
                    {
                        if (destroyable_array[y, x + 2] == PieceType.free)//flat right
                        {
                            //move is possible. make it and check it
                            destroyable_array[y, x + 2] = friendly_selected;
                            destroyable_array[y, x + 1] = friendly_selected;
                            destroyable_array[y, x] = PieceType.free;
                            current_score = Evaluate_Borad(destroyable_array, 0, 0);
                            if (current_score > goodest.score)
                            {
                                goodest.score = current_score;
                                Copy_Arrays(ref destroyable_array, ref goodest.borad);
                            }
                            Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                        }//flat right
                    }
                    if (y != 0)
                    {
                        if (destroyable_array[y - 1, x + 1] == PieceType.free)//if upmid is free
                        {
                            if (x != 0)
                            {
                                if (destroyable_array[y - 1, x - 1] == PieceType.free)//upleft
                                {
                                    //move is possible. make it and check it
                                    destroyable_array[y - 1, x - 1] = friendly_selected;
                                    destroyable_array[y - 1, x + 1] = friendly_selected;
                                    destroyable_array[y, x] = PieceType.free;
                                    destroyable_array[y, x + 1] = PieceType.free;

                                    current_score = Evaluate_Borad(destroyable_array, 0, 0);
                                    if (current_score > goodest.score)
                                    {
                                        goodest.score = current_score;
                                        Copy_Arrays(ref destroyable_array, ref goodest.borad);
                                    }
                                    Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                                }//upleft
                            }
                            if (x <= 6)
                            {
                                if (destroyable_array[y - 1, x + 2] == PieceType.free)//upright
                                {
                                    //move is possible. make it and check it
                                    destroyable_array[y - 1, x + 2] = friendly_selected;
                                    destroyable_array[y - 1, x + 1] = friendly_selected;
                                    destroyable_array[y, x] = PieceType.free;
                                    destroyable_array[y, x + 1] = PieceType.free;

                                    current_score = Evaluate_Borad(destroyable_array, 0, 0);
                                    if (current_score > goodest.score)
                                    {
                                        goodest.score = current_score;
                                        Copy_Arrays(ref destroyable_array, ref goodest.borad);
                                    }
                                    Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                                }//upright
                            }
                        }//if up mid is free
                    }
                    if (y != 8)
                    {
                        if (destroyable_array[y + 1, x] == PieceType.free)//if down mid is free
                        {
                            if (x != 0)
                            {
                                if (destroyable_array[y + 1, x - 1] == PieceType.free)//downleft
                                {
                                    //move is possible. make it and check it
                                    destroyable_array[y + 1, x - 1] = friendly_selected;
                                    destroyable_array[y + 1, x] = friendly_selected;
                                    destroyable_array[y, x] = PieceType.free;
                                    destroyable_array[y, x + 1] = PieceType.free;

                                    current_score = Evaluate_Borad(destroyable_array, 0, 0);
                                    if (current_score > goodest.score)
                                    {
                                        goodest.score = current_score;
                                        Copy_Arrays(ref destroyable_array, ref goodest.borad);
                                    }
                                    Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                                }//downleft
                            }
                            if (x != 8)
                            {
                                if (destroyable_array[y + 1, x + 1] == PieceType.free)//downright
                                {
                                    //move is possible. make it and check it
                                    destroyable_array[y + 1, x + 1] = friendly_selected;
                                    destroyable_array[y + 1, x] = friendly_selected;
                                    destroyable_array[y, x] = PieceType.free;
                                    destroyable_array[y, x + 1] = PieceType.free;

                                    current_score = Evaluate_Borad(destroyable_array, 0, 0);
                                    if (current_score > goodest.score)
                                    {
                                        goodest.score = current_score;
                                        Copy_Arrays(ref destroyable_array, ref goodest.borad);
                                    }
                                    Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                                }//downright
                            }
                        }//if down mid is free
                    }
                }
            }//right movement mybe possible finished

            //this is down case
            if (y != 0)
            {
                if (destroyable_array[y - 1, x] == friendly)//left movment is possible fin
                {
                    y2 = y - 1;
                    x2 = x;
                    if (y >= 2)
                    {
                        if (destroyable_array[y - 2, x] == PieceType.free)//flat left
                        {
                            //move is possible. make it and check it
                            destroyable_array[y - 2, x] = friendly_selected;
                            destroyable_array[y - 1, x] = friendly_selected;
                            destroyable_array[y, x] = PieceType.free;
                            current_score = Evaluate_Borad(destroyable_array, 0, 0);
                            if (current_score > goodest.score)
                            {
                                goodest.score = current_score;
                                Copy_Arrays(ref destroyable_array, ref goodest.borad);
                            }
                            Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                        }//flat left
                    }
                    if (y != 8)
                    {
                        if (destroyable_array[y + 1, x] == PieceType.free)//flat right
                        {
                            //move is possible. make it and check it
                            destroyable_array[y + 1, x] = friendly_selected;
                            destroyable_array[y, x] = friendly_selected;
                            destroyable_array[y2, x2] = PieceType.free;
                            current_score = Evaluate_Borad(destroyable_array, 0, 0);
                            if (current_score > goodest.score)
                            {
                                goodest.score = current_score;
                                Copy_Arrays(ref destroyable_array, ref goodest.borad);
                            }
                            Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                        }//flat right
                    }
                    if (x != 8)
                    {
                        if (destroyable_array[y - 1, x + 1] == PieceType.free)//if upmid is free
                        {
                            if (y2 != 0 && x2 != 8)
                            {
                                if (destroyable_array[y2 - 1, x2 + 1] == PieceType.free)//upleft
                                {
                                    //move is possible. make it and check it
                                    destroyable_array[y2 - 1, x2 + 1] = friendly_selected;
                                    destroyable_array[y - 1, x + 1] = friendly_selected;
                                    destroyable_array[y, x] = PieceType.free;
                                    destroyable_array[y2, x2] = PieceType.free;

                                    current_score = Evaluate_Borad(destroyable_array, 0, 0);
                                    if (current_score > goodest.score)
                                    {
                                        goodest.score = current_score;
                                        Copy_Arrays(ref destroyable_array, ref goodest.borad);
                                    }
                                    Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                                }//upleft
                            }
                            if (x != 8)
                            {
                                if (destroyable_array[y, x + 1] == PieceType.free)//upright
                                {
                                    //move is possible. make it and check it
                                    destroyable_array[y, x + 1] = friendly_selected;
                                    destroyable_array[y - 1, x + 1] = friendly_selected;
                                    destroyable_array[y, x] = PieceType.free;
                                    destroyable_array[y2, x2] = PieceType.free;

                                    current_score = Evaluate_Borad(destroyable_array, 0, 0);
                                    if (current_score > goodest.score)
                                    {
                                        goodest.score = current_score;
                                        Copy_Arrays(ref destroyable_array, ref goodest.borad);
                                    }
                                    Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                                }//upright
                            }
                        }//if up mid is free
                    }
                    if (x != 0)
                    {
                        if (destroyable_array[y, x - 1] == PieceType.free)//if down mid is free
                        {
                            if (y2 >= 0 && y2 <= 8 && x2 != 0)
                            {
                                if (destroyable_array[y2, x2 - 1] == PieceType.free)//downleft
                                {
                                    //move is possible. make it and check it
                                    destroyable_array[y2, x2 - 1] = friendly_selected;
                                    destroyable_array[y, x - 1] = friendly_selected;
                                    destroyable_array[y, x] = PieceType.free;
                                    destroyable_array[y2, x2] = PieceType.free;

                                    current_score = Evaluate_Borad(destroyable_array, 0, 0);
                                    if (current_score > goodest.score)
                                    {
                                        goodest.score = current_score;
                                        Copy_Arrays(ref destroyable_array, ref goodest.borad);
                                    }
                                    Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                                }//downleft
                            }
                            if (y != 8)
                            {
                                if (destroyable_array[y + 1, x - 1] == PieceType.free)//downright
                                {
                                    //move is possible. make it and check it
                                    destroyable_array[y + 1, x - 1] = friendly_selected;
                                    destroyable_array[y, x - 1] = friendly_selected;
                                    destroyable_array[y, x] = PieceType.free;
                                    destroyable_array[y2, x2] = PieceType.free;

                                    current_score = Evaluate_Borad(destroyable_array, 0, 0);
                                    if (current_score > goodest.score)
                                    {
                                        goodest.score = current_score;
                                        Copy_Arrays(ref destroyable_array, ref goodest.borad);
                                    }
                                    Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                                }//downright
                            }
                        }//if down mid is free
                    }
                }//finished
            }//left movment mybe possible finished
            if (y != 8)
            {
                if (destroyable_array[y + 1, x] == friendly)//right movment is possible
                {
                    y2 = y + 1;
                    x2 = x;
                    if (y != 0)
                    {
                        if (destroyable_array[y - 1, x] == PieceType.free)//flat left
                        {
                            //move is possible. make it and check it
                            destroyable_array[y - 1, x] = friendly_selected;
                            destroyable_array[y, x] = friendly_selected;
                            destroyable_array[y2, x2] = PieceType.free;
                            current_score = Evaluate_Borad(destroyable_array, 0, 0);
                            if (current_score > goodest.score)
                            {
                                goodest.score = current_score;
                                Copy_Arrays(ref destroyable_array, ref goodest.borad);
                            }
                            Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                        }//flat left
                    }
                    if (y2 != 8)
                    {
                        if (destroyable_array[y2 + 1, x2] == PieceType.free)//flat right
                        {
                            //move is possible. make it and check it
                            destroyable_array[y2 + 1, x2] = friendly_selected;
                            destroyable_array[y2, x2] = friendly_selected;
                            destroyable_array[y, x] = PieceType.free;
                            current_score = Evaluate_Borad(destroyable_array, 0, 0);
                            if (current_score > goodest.score)
                            {
                                goodest.score = current_score;
                                Copy_Arrays(ref destroyable_array, ref goodest.borad);
                            }
                            Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                        }//flat right
                    }
                    if (x != 8)
                    {
                        if (destroyable_array[y, x + 1] == PieceType.free)//if upmid is free
                        {
                            if (y != 0)
                            {
                                if (destroyable_array[y - 1, x + 1] == PieceType.free)//upleft
                                {
                                    //move is possible. make it and check it
                                    destroyable_array[y - 1, x + 1] = friendly_selected;
                                    destroyable_array[y, x + 1] = friendly_selected;
                                    destroyable_array[y, x] = PieceType.free;
                                    destroyable_array[y2, x2] = PieceType.free;

                                    current_score = Evaluate_Borad(destroyable_array, 0, 0);
                                    if (current_score > goodest.score)
                                    {
                                        goodest.score = current_score;
                                        Copy_Arrays(ref destroyable_array, ref goodest.borad);
                                    }
                                    Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                                }//upleft
                            }
                            if (y2 <= 8 && y2 >= 0 && x2 != 8)
                            {
                                if (destroyable_array[y2, x2 + 1] == PieceType.free)//upright
                                {
                                    //move is possible. make it and check it
                                    destroyable_array[y2, x2 + 1] = friendly_selected;
                                    destroyable_array[y, x + 1] = friendly_selected;
                                    destroyable_array[y, x] = PieceType.free;
                                    destroyable_array[y2, x2] = PieceType.free;

                                    current_score = Evaluate_Borad(destroyable_array, 0, 0);
                                    if (current_score > goodest.score)
                                    {
                                        goodest.score = current_score;
                                        Copy_Arrays(ref destroyable_array, ref goodest.borad);
                                    }
                                    Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                                }//upright
                            }
                        }//if up mid is free
                    }
                    if (y != 8 && x != 0)
                    {
                        if (destroyable_array[y + 1, x - 1] == PieceType.free)//if down mid is free
                        {
                            if (destroyable_array[y, x - 1] == PieceType.free)//downleft
                            {
                                //move is possible. make it and check it
                                destroyable_array[y, x - 1] = friendly_selected;
                                destroyable_array[y + 1, x - 1] = friendly_selected;
                                destroyable_array[y, x] = PieceType.free;
                                destroyable_array[y2, x2] = PieceType.free;

                                current_score = Evaluate_Borad(destroyable_array, 0, 0);
                                if (current_score > goodest.score)
                                {
                                    goodest.score = current_score;
                                    Copy_Arrays(ref destroyable_array, ref goodest.borad);
                                }
                                Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                            }//downleft

                            if (y2 != 8 && x2 != 0)
                            {
                                if (destroyable_array[y2 + 1, x2 - 1] == PieceType.free)//downright
                                {
                                    //move is possible. make it and check it
                                    destroyable_array[y2 + 1, x2 - 1] = friendly_selected;
                                    destroyable_array[y + 1, x - 1] = friendly_selected;
                                    destroyable_array[y, x] = PieceType.free;
                                    destroyable_array[y2, x2] = PieceType.free;

                                    current_score = Evaluate_Borad(destroyable_array, 0, 0);
                                    if (current_score > goodest.score)
                                    {
                                        goodest.score = current_score;
                                        Copy_Arrays(ref destroyable_array, ref goodest.borad);
                                    }
                                    Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                                }//downright
                            }
                        }//if down mid is free
                    }
                }
            }//right movement mybe possible finished

            //this is up case
            if (y != 8 && x != 0)
            {
                if (destroyable_array[y + 1, x - 1] == friendly)//left movment is possible fin
                {
                    y2 = y + 1;
                    x2 = x - 1;
                    if (y2 != 8 && x2 != 0)
                    {
                        if (destroyable_array[y2 + 1, x2 - 1] == PieceType.free)//flat left
                        {
                            //move is possible. make it and check it
                            destroyable_array[y2 + 1, x2 - 1] = friendly_selected;
                            destroyable_array[y2, x2] = friendly_selected;
                            destroyable_array[y, x] = PieceType.free;
                            current_score = Evaluate_Borad(destroyable_array, 0, 0);
                            if (current_score > goodest.score)
                            {
                                goodest.score = current_score;
                                Copy_Arrays(ref destroyable_array, ref goodest.borad);
                            }
                            Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                        }//flat left
                    }
                    if (y != 0 && x != 8)
                    {
                        if (destroyable_array[y - 1, x + 1] == PieceType.free)//flat right
                        {
                            //move is possible. make it and check it
                            destroyable_array[y - 1, x + 1] = friendly_selected;
                            destroyable_array[y, x] = friendly_selected;
                            destroyable_array[y2, x2] = PieceType.free;
                            current_score = Evaluate_Borad(destroyable_array, 0, 0);
                            if (current_score > goodest.score)
                            {
                                goodest.score = current_score;
                                Copy_Arrays(ref destroyable_array, ref goodest.borad);
                            }
                            Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                        }//flat right
                    }
                    if (x != 0)
                    {
                        if (destroyable_array[y, x - 1] == PieceType.free)//if upmid is free
                        {
                            if (x2 != 0)
                            {
                                if (destroyable_array[y2, x2 - 1] == PieceType.free)//upleft
                                {
                                    //move is possible. make it and check it
                                    destroyable_array[y2, x2 - 1] = friendly_selected;
                                    destroyable_array[y, x - 1] = friendly_selected;
                                    destroyable_array[y, x] = PieceType.free;
                                    destroyable_array[y2, x2] = PieceType.free;

                                    current_score = Evaluate_Borad(destroyable_array, 0, 0);
                                    if (current_score > goodest.score)
                                    {
                                        goodest.score = current_score;
                                        Copy_Arrays(ref destroyable_array, ref goodest.borad);
                                    }
                                    Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                                }//upleft
                            }
                            if (y != 0)
                            {
                                if (destroyable_array[y - 1, x] == PieceType.free)//upright
                                {
                                    //move is possible. make it and check it
                                    destroyable_array[y - 1, x] = friendly_selected;
                                    destroyable_array[y, x - 1] = friendly_selected;
                                    destroyable_array[y, x] = PieceType.free;
                                    destroyable_array[y2, x2] = PieceType.free;

                                    current_score = Evaluate_Borad(destroyable_array, 0, 0);
                                    if (current_score > goodest.score)
                                    {
                                        goodest.score = current_score;
                                        Copy_Arrays(ref destroyable_array, ref goodest.borad);
                                    }
                                    Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                                }//upright
                            }
                        }//if up mid is free
                    }
                    if (y != 8)
                    {
                        if (destroyable_array[y + 1, x] == PieceType.free)//if down mid is free
                        {
                            if (y2 != 8)
                            {
                                if (destroyable_array[y2 + 1, x2] == PieceType.free)//downleft
                                {
                                    //move is possible. make it and check it
                                    destroyable_array[y2 + 1, x2] = friendly_selected;
                                    destroyable_array[y + 1, x] = friendly_selected;
                                    destroyable_array[y, x] = PieceType.free;
                                    destroyable_array[y2, x2] = PieceType.free;

                                    current_score = Evaluate_Borad(destroyable_array, 0, 0);
                                    if (current_score > goodest.score)
                                    {
                                        goodest.score = current_score;
                                        Copy_Arrays(ref destroyable_array, ref goodest.borad);
                                    }
                                    Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                                }//downleft
                            }
                            if (x != 8)
                            {
                                if (destroyable_array[y, x + 1] == PieceType.free)//downright
                                {
                                    //move is possible. make it and check it
                                    destroyable_array[y, x + 1] = friendly_selected;
                                    destroyable_array[y + 1, x] = friendly_selected;
                                    destroyable_array[y, x] = PieceType.free;
                                    destroyable_array[y2, x2] = PieceType.free;

                                    current_score = Evaluate_Borad(destroyable_array, 0, 0);
                                    if (current_score > goodest.score)
                                    {
                                        goodest.score = current_score;
                                        Copy_Arrays(ref destroyable_array, ref goodest.borad);
                                    }
                                    Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                                }//downright
                            }
                        }//if down mid is free
                    }
                }//finished
            }//left movment mybe possible finished
            if (y != 0 && x != 8)
            {
                if (destroyable_array[y - 1, x + 1] == friendly)//right movment is possible
                {
                    y2 = y - 1;
                    x2 = x + 1;
                    if (y != 8 && x != 0)
                    {
                        if (destroyable_array[y + 1, x - 1] == PieceType.free)//flat left
                        {
                            //move is possible. make it and check it
                            destroyable_array[y + 1, x - 1] = friendly_selected;
                            destroyable_array[y, x] = friendly_selected;
                            destroyable_array[y2, x2] = PieceType.free;
                            current_score = Evaluate_Borad(destroyable_array, 0, 0);
                            if (current_score > goodest.score)
                            {
                                goodest.score = current_score;
                                Copy_Arrays(ref destroyable_array, ref goodest.borad);
                            }
                            Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                        }//flat left
                    }
                    if (y2 != 0 && x2 != 8)
                    {
                        if (destroyable_array[y2 - 1, x2 + 1] == PieceType.free)//flat right
                        {
                            //move is possible. make it and check it
                            destroyable_array[y2 - 1, x2 + 1] = friendly_selected;
                            destroyable_array[y2, x2] = friendly_selected;
                            destroyable_array[y, x] = PieceType.free;
                            current_score = Evaluate_Borad(destroyable_array, 0, 0);
                            if (current_score > goodest.score)
                            {
                                goodest.score = current_score;
                                Copy_Arrays(ref destroyable_array, ref goodest.borad);
                            }
                            Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                        }//flat right
                    }
                    if (y != 0)
                    {
                        if (destroyable_array[y - 1, x] == PieceType.free)//if upmid is free
                        {
                            if (x != 0)
                            {
                                if (destroyable_array[y, x - 1] == PieceType.free)//upleft
                                {
                                    //move is possible. make it and check it
                                    destroyable_array[y, x - 1] = friendly_selected;
                                    destroyable_array[y - 1, x] = friendly_selected;
                                    destroyable_array[y, x] = PieceType.free;
                                    destroyable_array[y2, x2] = PieceType.free;

                                    current_score = Evaluate_Borad(destroyable_array, 0, 0);
                                    if (current_score > goodest.score)
                                    {
                                        goodest.score = current_score;
                                        Copy_Arrays(ref destroyable_array, ref goodest.borad);
                                    }
                                    Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                                }//upleft
                            }
                            if (y2 != 0)
                            {
                                if (destroyable_array[y2 - 1, x2] == PieceType.free)//upright
                                {
                                    //move is possible. make it and check it
                                    destroyable_array[y2 - 1, x2] = friendly_selected;
                                    destroyable_array[y - 1, x] = friendly_selected;
                                    destroyable_array[y, x] = PieceType.free;
                                    destroyable_array[y2, x2] = PieceType.free;

                                    current_score = Evaluate_Borad(destroyable_array, 0, 0);
                                    if (current_score > goodest.score)
                                    {
                                        goodest.score = current_score;
                                        Copy_Arrays(ref destroyable_array, ref goodest.borad);
                                    }
                                    Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                                }//upright
                            }
                        }//if up mid is free
                    }
                    if (x != 8)
                    {
                        if (destroyable_array[y, x + 1] == PieceType.free)//if down mid is free
                        {
                            if (y != 8)
                            {
                                if (destroyable_array[y + 1, x] == PieceType.free)//downleft
                                {
                                    //move is possible. make it and check it
                                    destroyable_array[y + 1, x] = friendly_selected;
                                    destroyable_array[y, x + 1] = friendly_selected;
                                    destroyable_array[y, x] = PieceType.free;
                                    destroyable_array[y2, x2] = PieceType.free;

                                    current_score = Evaluate_Borad(destroyable_array, 0, 0);
                                    if (current_score > goodest.score)
                                    {
                                        goodest.score = current_score;
                                        Copy_Arrays(ref destroyable_array, ref goodest.borad);
                                    }
                                    Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                                }//downleft
                            }
                            if (x2 != 8)
                            {
                                if (destroyable_array[y2, x2 + 1] == PieceType.free)//downright
                                {
                                    //move is possible. make it and check it
                                    destroyable_array[y2, x2 + 1] = friendly_selected;
                                    destroyable_array[y, x + 1] = friendly_selected;
                                    destroyable_array[y, x] = PieceType.free;
                                    destroyable_array[y2, x2] = PieceType.free;

                                    current_score = Evaluate_Borad(destroyable_array, 0, 0);
                                    if (current_score > goodest.score)
                                    {
                                        goodest.score = current_score;
                                        Copy_Arrays(ref destroyable_array, ref goodest.borad);
                                    }
                                    Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                                }//downright
                            }
                        }//if down mid is free
                    }
                }
            }//right movement mybe possible finished

            return goodest;
        }//finished with blue

        /// <summary>
        /// פעולה זו בודקת את כול הצעדים האפשריים של 3
        /// </summary>
        /// <param name="unchengeable_borad">לוח מקורי </param>
        /// <param name="x">נצ חייל</param>
        /// <param name="y">נצ חייל </param>
        /// <returns>הצעד הכי טוב שמצאה </returns>
        public MoveType Check_All_Moves_Of_Three(ref PieceType[,] unchengeable_borad, int x, int y)
        {
            int x2, y2, x3, y3;
            float current_score;
            MoveType goodest = new MoveType();
            goodest.score = -10000;
            PieceType[,] destroyable_array = new PieceType[Settings.BORAD_ARRAY_SIZE, Settings.BORAD_ARRAY_SIZE];
            Copy_Arrays(ref unchengeable_borad, ref destroyable_array);

            //this is flat case 
            if (x >= 2) //flat left
            {
                if (destroyable_array[y, x - 1] == friendly)//right movment is possible
                {
                    y2 = y;
                    x2 = x - 1;
                    if (destroyable_array[y, x - 2] == friendly)
                    {
                        y3 = y;
                        x3 = x - 2;
                        if (x3 != 0)
                        {
                            if (destroyable_array[y3, x3 - 1] == PieceType.free)//flat left
                            {
                                //move is possible. make it and check it
                                destroyable_array[y3, x3 - 1] = friendly_selected;
                                destroyable_array[y3, x3] = friendly_selected;
                                destroyable_array[y2, x2] = friendly_selected;
                                destroyable_array[y, x] = PieceType.free;
                                current_score = Evaluate_Borad(destroyable_array, 0, 0);
                                if (current_score > goodest.score)
                                {
                                    goodest.score = current_score;
                                    Copy_Arrays(ref destroyable_array, ref goodest.borad);
                                }
                                Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                            }//flat left
                        }
                        if (x != 8)
                        {
                            if (destroyable_array[y, x + 1] == PieceType.free)//flat right
                            {
                                //move is possible. make it and check it
                                destroyable_array[y, x + 1] = friendly_selected;
                                destroyable_array[y, x] = friendly_selected;
                                destroyable_array[y2, x2] = friendly_selected;
                                destroyable_array[y3, x3] = PieceType.free;
                                current_score = Evaluate_Borad(destroyable_array, 0, 0);
                                if (current_score > goodest.score)
                                {
                                    goodest.score = current_score;
                                    Copy_Arrays(ref destroyable_array, ref goodest.borad);
                                }
                                Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                            }//flat right
                        }
                        if (y != 0 && y2 != 0)
                        {
                            if (destroyable_array[y - 1, x] == PieceType.free && destroyable_array[y2 - 1, x2] == PieceType.free)//if upmid is free
                            {
                                if (y3 != 0)
                                {
                                    if (destroyable_array[y3 - 1, x3] == PieceType.free)//upleft
                                    {
                                        //move is possible. make it and check it
                                        destroyable_array[y - 1, x] = friendly_selected;
                                        destroyable_array[y2 - 1, x2] = friendly_selected;
                                        destroyable_array[y3 - 1, x3] = friendly_selected;
                                        destroyable_array[y, x] = PieceType.free;
                                        destroyable_array[y2, x2] = PieceType.free;
                                        destroyable_array[y3, x3] = PieceType.free;
                                        current_score = Evaluate_Borad(destroyable_array, 0, 0);
                                        if (current_score > goodest.score)
                                        {
                                            goodest.score = current_score;
                                            Copy_Arrays(ref destroyable_array, ref goodest.borad);
                                        }
                                        Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                                    }//upleft
                                }
                                if (x != 8)
                                {
                                    if (destroyable_array[y - 1, x + 1] == PieceType.free)//upright
                                    {
                                        //move is possible. make it and check it
                                        destroyable_array[y - 1, x + 1] = friendly_selected;
                                        destroyable_array[y2 - 1, x2 + 1] = friendly_selected;
                                        destroyable_array[y3 - 1, x3 + 1] = friendly_selected;
                                        destroyable_array[y, x] = PieceType.free;
                                        destroyable_array[y2, x2] = PieceType.free;
                                        destroyable_array[y3, x3] = PieceType.free;
                                        current_score = Evaluate_Borad(destroyable_array, 0, 0);
                                        if (current_score > goodest.score)
                                        {
                                            goodest.score = current_score;
                                            Copy_Arrays(ref destroyable_array, ref goodest.borad);
                                        }
                                        Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                                    }//upright
                                }
                            }//if up mid is free
                        }
                        if (y != 8 && y2 != 8 && x != 0 && x2 != 0)
                        {
                            if (destroyable_array[y + 1, x - 1] == PieceType.free && destroyable_array[y2 + 1, x2 - 1] == PieceType.free)//if down mid is free
                            {
                                if (y3 != 8 && x3 != 0)
                                {
                                    if (destroyable_array[y3 + 1, x3 - 1] == PieceType.free)//downleft
                                    {
                                        //move is possible. make it and check it
                                        destroyable_array[y + 1, x - 1] = friendly_selected;
                                        destroyable_array[y2 + 1, x2 - 1] = friendly_selected;
                                        destroyable_array[y3 + 1, x3 - 1] = friendly_selected;
                                        destroyable_array[y, x] = PieceType.free;
                                        destroyable_array[y2, x2] = PieceType.free;
                                        destroyable_array[y3, x3] = PieceType.free;
                                        current_score = Evaluate_Borad(destroyable_array, 0, 0);
                                        if (current_score > goodest.score)
                                        {
                                            goodest.score = current_score;
                                            Copy_Arrays(ref destroyable_array, ref goodest.borad);
                                        }
                                        Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                                    }//downleft
                                }
                                if (y != 8)
                                {
                                    if (destroyable_array[y + 1, x] == PieceType.free)//downright
                                    {
                                        //move is possible. make it and check it
                                        destroyable_array[y + 1, x] = friendly_selected;
                                        destroyable_array[y2 + 1, x2] = friendly_selected;
                                        destroyable_array[y3 + 1, x3] = friendly_selected;
                                        destroyable_array[y, x] = PieceType.free;
                                        destroyable_array[y2, x2] = PieceType.free;
                                        destroyable_array[y3, x3] = PieceType.free;

                                        current_score = Evaluate_Borad(destroyable_array, 0, 0);
                                        if (current_score > goodest.score)
                                        {
                                            goodest.score = current_score;
                                            Copy_Arrays(ref destroyable_array, ref goodest.borad);
                                        }
                                        Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                                    }//downright
                                }
                            }//if down mid is free
                        }
                    }
                }

            }//left movement mybe possible finished
            if (x <= 6) //flat left
            {
                if (destroyable_array[y, x + 1] == friendly)//right movment is possible
                {
                    y2 = y;
                    x2 = x + 1;
                    if (destroyable_array[y, x + 2] == friendly)
                    {
                        y3 = y;
                        x3 = x + 2;
                        if (x3 != 8)
                        {
                            if (destroyable_array[y3, x3 + 1] == PieceType.free)//flat left
                            {
                                //move is possible. make it and check it
                                destroyable_array[y3, x3 + 1] = friendly_selected;
                                destroyable_array[y3, x3] = friendly_selected;
                                destroyable_array[y2, x2] = friendly_selected;
                                destroyable_array[y, x] = PieceType.free;
                                current_score = Evaluate_Borad(destroyable_array, 0, 0);
                                if (current_score > goodest.score)
                                {
                                    goodest.score = current_score;
                                    Copy_Arrays(ref destroyable_array, ref goodest.borad);
                                }
                                Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                            }//flat left
                        }
                        if (x != 0)
                        {
                            if (destroyable_array[y, x - 1] == PieceType.free)//flat right
                            {
                                //move is possible. make it and check it
                                destroyable_array[y, x - 1] = friendly_selected;
                                destroyable_array[y, x] = friendly_selected;
                                destroyable_array[y, x + 1] = friendly_selected;
                                destroyable_array[y3, x3] = PieceType.free;
                                current_score = Evaluate_Borad(destroyable_array, 0, 0);
                                if (current_score > goodest.score)
                                {
                                    goodest.score = current_score;
                                    Copy_Arrays(ref destroyable_array, ref goodest.borad);
                                }
                                Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                            }//flat right
                        }
                        if (y != 0 && y2 != 0 && x != 8 && x2 != 8)
                        {
                            if (destroyable_array[y - 1, x + 1] == PieceType.free && destroyable_array[y2 - 1, x2 + 1] == PieceType.free)//if upmid is free
                            {
                                if (y != 0)
                                {
                                    if (destroyable_array[y - 1, x] == PieceType.free)//upleft
                                    {
                                        //move is possible. make it and check it
                                        destroyable_array[y - 1, x] = friendly_selected;
                                        destroyable_array[y2 - 1, x2] = friendly_selected;
                                        destroyable_array[y3 - 1, x3] = friendly_selected;
                                        destroyable_array[y, x] = PieceType.free;
                                        destroyable_array[y2, x2] = PieceType.free;
                                        destroyable_array[y3, x3] = PieceType.free;
                                        current_score = Evaluate_Borad(destroyable_array, 0, 0);
                                        if (current_score > goodest.score)
                                        {
                                            goodest.score = current_score;
                                            Copy_Arrays(ref destroyable_array, ref goodest.borad);
                                        }
                                        Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                                    }//upleft
                                }
                                if (y3 != 0 && x3 != 8)
                                {
                                    if (destroyable_array[y3 - 1, x3 + 1] == PieceType.free)//upright
                                    {
                                        //move is possible. make it and check it
                                        destroyable_array[y - 1, x + 1] = friendly_selected;
                                        destroyable_array[y2 - 1, x2 + 1] = friendly_selected;
                                        destroyable_array[y3 - 1, x3 + 1] = friendly_selected;
                                        destroyable_array[y, x] = PieceType.free;
                                        destroyable_array[y2, x2] = PieceType.free;
                                        destroyable_array[y3, x3] = PieceType.free;
                                        current_score = Evaluate_Borad(destroyable_array, 0, 0);
                                        if (current_score > goodest.score)
                                        {
                                            goodest.score = current_score;
                                            Copy_Arrays(ref destroyable_array, ref goodest.borad);
                                        }
                                        Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                                    }//upright
                                }
                            }//if up mid is free
                        }
                        if (y != 8 && y2 != 8)
                        {
                            if (destroyable_array[y + 1, x] == PieceType.free && destroyable_array[y2 + 1, x2] == PieceType.free)//if down mid is free
                            {
                                if (y != 8 && x != 0)
                                {
                                    if (destroyable_array[y + 1, x - 1] == PieceType.free)//downleft
                                    {
                                        //move is possible. make it and check it
                                        destroyable_array[y + 1, x - 1] = friendly_selected;
                                        destroyable_array[y2 + 1, x2 - 1] = friendly_selected;
                                        destroyable_array[y3 + 1, x3 - 1] = friendly_selected;
                                        destroyable_array[y, x] = PieceType.free;
                                        destroyable_array[y2, x2] = PieceType.free;
                                        destroyable_array[y3, x3] = PieceType.free;
                                        current_score = Evaluate_Borad(destroyable_array, 0, 0);
                                        if (current_score > goodest.score)
                                        {
                                            goodest.score = current_score;
                                            Copy_Arrays(ref destroyable_array, ref goodest.borad);
                                        }
                                        Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                                    }//downleft
                                }
                                if (y3 != 8)
                                {
                                    if (destroyable_array[y3 + 1, x3] == PieceType.free)//downright
                                    {
                                        //move is possible. make it and check it
                                        destroyable_array[y + 1, x] = friendly_selected;
                                        destroyable_array[y2 + 1, x2] = friendly_selected;
                                        destroyable_array[y3 + 1, x3] = friendly_selected;
                                        destroyable_array[y, x] = PieceType.free;
                                        destroyable_array[y2, x2] = PieceType.free;
                                        destroyable_array[y3, x3] = PieceType.free;
                                        current_score = Evaluate_Borad(destroyable_array, 0, 0);
                                        if (current_score > goodest.score)
                                        {
                                            goodest.score = current_score;
                                            Copy_Arrays(ref destroyable_array, ref goodest.borad);
                                        }
                                        Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                                    }//downright
                                }
                            }//if down mid is free
                        }
                    }
                }

            }//right movement mybe possible finished

            //this is down case
            if (y >= 2) //flat left
            {
                if (destroyable_array[y - 1, x] == friendly)//right movment is possible
                {
                    y2 = y - 1;
                    x2 = x;
                    if (destroyable_array[y - 2, x] == friendly)
                    {
                        y3 = y - 2;
                        x3 = x;
                        if (y3 != 0)
                        {
                            if (destroyable_array[y3 - 1, x3] == PieceType.free)//flat left
                            {
                                //move is possible. make it and check it
                                destroyable_array[y3 - 1, x3] = friendly_selected;
                                destroyable_array[y3, x3] = friendly_selected;
                                destroyable_array[y2, x2] = friendly_selected;
                                destroyable_array[y, x] = PieceType.free;
                                current_score = Evaluate_Borad(destroyable_array, 0, 0);
                                if (current_score > goodest.score)
                                {
                                    goodest.score = current_score;
                                    Copy_Arrays(ref destroyable_array, ref goodest.borad);
                                }
                                Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                            }//flat left
                        }
                        if (y != 8)
                        {
                            if (destroyable_array[y + 1, x] == PieceType.free)//flat right
                            {
                                //move is possible. make it and check it
                                destroyable_array[y + 1, x] = friendly_selected;
                                destroyable_array[y, x] = friendly_selected;
                                destroyable_array[y - 1, x] = friendly_selected;
                                destroyable_array[y3, x3] = PieceType.free;
                                current_score = Evaluate_Borad(destroyable_array, 0, 0);
                                if (current_score > goodest.score)
                                {
                                    goodest.score = current_score;
                                    Copy_Arrays(ref destroyable_array, ref goodest.borad);
                                }
                                Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                            }//flat right
                        }
                        if (y != 0 && y2 != 0 && x != 8 && x2 != 8)
                        {
                            if (destroyable_array[y - 1, x + 1] == PieceType.free && destroyable_array[y2 - 1, x2 + 1] == PieceType.free)//if upmid is free
                            {
                                if (y3 != 0 && x3 != 8)
                                {
                                    if (destroyable_array[y3 - 1, x3 + 1] == PieceType.free)//upleft
                                    {
                                        //move is possible. make it and check it
                                        destroyable_array[y - 1, x + 1] = friendly_selected;
                                        destroyable_array[y2 - 1, x2 + 1] = friendly_selected;
                                        destroyable_array[y3 - 1, x3 + 1] = friendly_selected;
                                        destroyable_array[y, x] = PieceType.free;
                                        destroyable_array[y2, x2] = PieceType.free;
                                        destroyable_array[y3, x3] = PieceType.free;
                                        current_score = Evaluate_Borad(destroyable_array, 0, 0);
                                        if (current_score > goodest.score)
                                        {
                                            goodest.score = current_score;
                                            Copy_Arrays(ref destroyable_array, ref goodest.borad);
                                        }
                                        Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                                    }//upleft
                                }
                                if (x != 8)
                                {
                                    if (destroyable_array[y, x + 1] == PieceType.free)//upright
                                    {
                                        //move is possible. make it and check it
                                        destroyable_array[y, x + 1] = friendly_selected;
                                        destroyable_array[y2, x2 + 1] = friendly_selected;
                                        destroyable_array[y3, x3 + 1] = friendly_selected;
                                        destroyable_array[y, x] = PieceType.free;
                                        destroyable_array[y2, x2] = PieceType.free;
                                        destroyable_array[y3, x3] = PieceType.free;
                                        current_score = Evaluate_Borad(destroyable_array, 0, 0);
                                        if (current_score > goodest.score)
                                        {
                                            goodest.score = current_score;
                                            Copy_Arrays(ref destroyable_array, ref goodest.borad);
                                        }
                                        Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                                    }//upright
                                }
                            }//if up mid is free
                        }
                        if (x != 0 && x2 != 0)
                        {
                            if (destroyable_array[y, x - 1] == PieceType.free && destroyable_array[y2, x2 - 1] == PieceType.free)//if down mid is free
                            {
                                if (x3 != 0)
                                {
                                    if (destroyable_array[y3, x3 - 1] == PieceType.free)//downleft
                                    {
                                        //move is possible. make it and check it
                                        destroyable_array[y, x - 1] = friendly_selected;
                                        destroyable_array[y2, x2 - 1] = friendly_selected;
                                        destroyable_array[y3, x3 - 1] = friendly_selected;
                                        destroyable_array[y, x] = PieceType.free;
                                        destroyable_array[y2, x2] = PieceType.free;
                                        destroyable_array[y3, x3] = PieceType.free;
                                        current_score = Evaluate_Borad(destroyable_array, 0, 0);
                                        if (current_score > goodest.score)
                                        {
                                            goodest.score = current_score;
                                            Copy_Arrays(ref destroyable_array, ref goodest.borad);
                                        }
                                        Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                                    }//downleft
                                }
                                if (y != 8 && x != 0)
                                {
                                    if (destroyable_array[y + 1, x - 1] == PieceType.free)//downright
                                    {
                                        //move is possible. make it and check it
                                        destroyable_array[y + 1, x - 1] = friendly_selected;
                                        destroyable_array[y2 + 1, x2 - 1] = friendly_selected;
                                        destroyable_array[y3 + 1, x3 - 1] = friendly_selected;
                                        destroyable_array[y, x] = PieceType.free;
                                        destroyable_array[y2, x2] = PieceType.free;
                                        destroyable_array[y3, x3] = PieceType.free;
                                        current_score = Evaluate_Borad(destroyable_array, 0, 0);
                                        if (current_score > goodest.score)
                                        {
                                            goodest.score = current_score;
                                            Copy_Arrays(ref destroyable_array, ref goodest.borad);
                                        }
                                        Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                                    }//downright
                                }
                            }//if down mid is free
                        }
                    }
                }

            }//left movement mybe possible finished
            if (y <= 6) //flat left
            {
                if (destroyable_array[y + 1, x] == friendly)//right movment is possible
                {
                    y2 = y + 1;
                    x2 = x;
                    if (destroyable_array[y + 2, x] == friendly)
                    {
                        y3 = y + 2;
                        x3 = x;
                        if (y3 != 8)
                        {
                            if (destroyable_array[y3 + 1, x3] == PieceType.free)//flat left
                            {
                                //move is possible. make it and check it
                                destroyable_array[y3 + 1, x3] = friendly_selected;
                                destroyable_array[y3, x3] = friendly_selected;
                                destroyable_array[y2, x2] = friendly_selected;
                                destroyable_array[y, x] = PieceType.free;
                                current_score = Evaluate_Borad(destroyable_array, 0, 0);
                                if (current_score > goodest.score)
                                {
                                    goodest.score = current_score;
                                    Copy_Arrays(ref destroyable_array, ref goodest.borad);
                                }
                                Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                            }//flat left
                        }
                        if (y != 0)
                        {
                            if (destroyable_array[y - 1, x] == PieceType.free)//flat right
                            {
                                //move is possible. make it and check it
                                destroyable_array[y - 1, x] = friendly_selected;
                                destroyable_array[y, x] = friendly_selected;
                                destroyable_array[y + 1, x] = friendly_selected;
                                destroyable_array[y3, x3] = PieceType.free;
                                current_score = Evaluate_Borad(destroyable_array, 0, 0);
                                if (current_score > goodest.score)
                                {
                                    goodest.score = current_score;
                                    Copy_Arrays(ref destroyable_array, ref goodest.borad);
                                }
                                Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                            }//flat right
                        }
                        if (x != 8 && x2 != 8)
                        {
                            if (destroyable_array[y, x + 1] == PieceType.free && destroyable_array[y2, x2 + 1] == PieceType.free)//if upmid is free
                            {
                                if (x3 != 8)
                                {
                                    if (destroyable_array[y3, x3 + 1] == PieceType.free)//upleft
                                    {
                                        //move is possible. make it and check it
                                        destroyable_array[y, x + 1] = friendly_selected;
                                        destroyable_array[y2, x2 + 1] = friendly_selected;
                                        destroyable_array[y3, x3 + 1] = friendly_selected;
                                        destroyable_array[y, x] = PieceType.free;
                                        destroyable_array[y2, x2] = PieceType.free;
                                        destroyable_array[y3, x3] = PieceType.free;
                                        current_score = Evaluate_Borad(destroyable_array, 0, 0);
                                        if (current_score > goodest.score)
                                        {
                                            goodest.score = current_score;
                                            Copy_Arrays(ref destroyable_array, ref goodest.borad);
                                        }
                                        Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                                    }//upleft
                                }
                                if (y != 0 && x != 8)
                                {
                                    if (destroyable_array[y - 1, x + 1] == PieceType.free)//upright
                                    {
                                        //move is possible. make it and check it
                                        destroyable_array[y - 1, x + 1] = friendly_selected;
                                        destroyable_array[y2 - 1, x2 + 1] = friendly_selected;
                                        destroyable_array[y3 - 1, x3 + 1] = friendly_selected;
                                        destroyable_array[y, x] = PieceType.free;
                                        destroyable_array[y2, x2] = PieceType.free;
                                        destroyable_array[y3, x3] = PieceType.free;
                                        current_score = Evaluate_Borad(destroyable_array, 0, 0);
                                        if (current_score > goodest.score)
                                        {
                                            goodest.score = current_score;
                                            Copy_Arrays(ref destroyable_array, ref goodest.borad);
                                        }
                                        Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                                    }//upright
                                }
                            }//if up mid is free
                        }
                        if (x != 0 && x2 != 0 && y != 8 && y2 != 8)
                        {
                            if (destroyable_array[y + 1, x - 1] == PieceType.free && destroyable_array[y2 + 1, x2 - 1] == PieceType.free)//if down mid is free
                            {
                                if (x3 != 0 && y3 != 8)
                                {
                                    if (destroyable_array[y3 + 1, x3 - 1] == PieceType.free)//downleft
                                    {
                                        //move is possible. make it and check it
                                        destroyable_array[y + 1, x - 1] = friendly_selected;
                                        destroyable_array[y2 + 1, x2 - 1] = friendly_selected;
                                        destroyable_array[y3 + 1, x3 - 1] = friendly_selected;
                                        destroyable_array[y, x] = PieceType.free;
                                        destroyable_array[y2, x2] = PieceType.free;
                                        destroyable_array[y3, x3] = PieceType.free;
                                        current_score = Evaluate_Borad(destroyable_array, 0, 0);
                                        if (current_score > goodest.score)
                                        {
                                            goodest.score = current_score;
                                            Copy_Arrays(ref destroyable_array, ref goodest.borad);
                                        }
                                        Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                                    }//downleft
                                }
                                if (x != 0)
                                {
                                    if (destroyable_array[y, x - 1] == PieceType.free)//downright
                                    {
                                        //move is possible. make it and check it
                                        destroyable_array[y, x - 1] = friendly_selected;
                                        destroyable_array[y2, x2 - 1] = friendly_selected;
                                        destroyable_array[y3, x3 - 1] = friendly_selected;
                                        destroyable_array[y, x] = PieceType.free;
                                        destroyable_array[y2, x2] = PieceType.free;
                                        destroyable_array[y3, x3] = PieceType.free;
                                        current_score = Evaluate_Borad(destroyable_array, 0, 0);
                                        if (current_score > goodest.score)
                                        {
                                            goodest.score = current_score;
                                            Copy_Arrays(ref destroyable_array, ref goodest.borad);
                                        }
                                        Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                                    }//downright
                                }
                            }//if down mid is free
                        }
                    }
                }

            }//right movement mybe possible finished

            //this is up case 
            if (y <= 6 && x >= 2) //flat left
            {
                if (destroyable_array[y + 1, x - 1] == friendly)//right movment is possible
                {
                    y2 = y + 1;
                    x2 = x - 1;
                    if (destroyable_array[y + 2, x - 2] == friendly)
                    {
                        y3 = y + 2;
                        x3 = x - 2;
                        if (y3 != 8 && x3 != 0)
                        {
                            if (destroyable_array[y3 + 1, x3 - 1] == PieceType.free)//flat left
                            {
                                //move is possible. make it and check it
                                destroyable_array[y3 + 1, x3 - 1] = friendly_selected;
                                destroyable_array[y3, x3] = friendly_selected;
                                destroyable_array[y2, x2] = friendly_selected;
                                destroyable_array[y, x] = PieceType.free;
                                current_score = Evaluate_Borad(destroyable_array, 0, 0);
                                if (current_score > goodest.score)
                                {
                                    goodest.score = current_score;
                                    Copy_Arrays(ref destroyable_array, ref goodest.borad);
                                }
                                Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                            }//flat left
                        }
                        if (y != 0 && x != 8)
                        {
                            if (destroyable_array[y - 1, x + 1] == PieceType.free)//flat right
                            {
                                //move is possible. make it and check it
                                destroyable_array[y - 1, x + 1] = friendly_selected;
                                destroyable_array[y, x] = friendly_selected;
                                destroyable_array[y2, x2] = friendly_selected;
                                destroyable_array[y3, x3] = PieceType.free;
                                current_score = Evaluate_Borad(destroyable_array, 0, 0);
                                if (current_score > goodest.score)
                                {
                                    goodest.score = current_score;
                                    Copy_Arrays(ref destroyable_array, ref goodest.borad);
                                }
                                Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                            }//flat right
                        }
                        if (x != 0 && x2 != 0)
                        {
                            if (destroyable_array[y, x - 1] == PieceType.free && destroyable_array[y2, x2 - 1] == PieceType.free)//if upmid is free
                            {
                                if (x3 != 0)
                                {
                                    if (destroyable_array[y3, x3 - 1] == PieceType.free)//upleft
                                    {
                                        //move is possible. make it and check it
                                        destroyable_array[y, x - 1] = friendly_selected;
                                        destroyable_array[y2, x2 - 1] = friendly_selected;
                                        destroyable_array[y3, x3 - 1] = friendly_selected;
                                        destroyable_array[y, x] = PieceType.free;
                                        destroyable_array[y2, x2] = PieceType.free;
                                        destroyable_array[y3, x3] = PieceType.free;
                                        current_score = Evaluate_Borad(destroyable_array, 0, 0);
                                        if (current_score > goodest.score)
                                        {
                                            goodest.score = current_score;
                                            Copy_Arrays(ref destroyable_array, ref goodest.borad);
                                        }
                                        Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                                    }//upleft
                                }
                                if (y != 0)
                                {
                                    if (destroyable_array[y - 1, x] == PieceType.free)//upright
                                    {
                                        //move is possible. make it and check it
                                        destroyable_array[y - 1, x] = friendly_selected;
                                        destroyable_array[y2 - 1, x2] = friendly_selected;
                                        destroyable_array[y3 - 1, x3] = friendly_selected;
                                        destroyable_array[y, x] = PieceType.free;
                                        destroyable_array[y2, x2] = PieceType.free;
                                        destroyable_array[y3, x3] = PieceType.free;
                                        current_score = Evaluate_Borad(destroyable_array, 0, 0);
                                        if (current_score > goodest.score)
                                        {
                                            goodest.score = current_score;
                                            Copy_Arrays(ref destroyable_array, ref goodest.borad);
                                        }
                                        Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                                    }//upright
                                }
                            }//if up mid is free
                        }
                        if (y != 8 && y2 != 8)
                        {
                            if (destroyable_array[y + 1, x] == PieceType.free && destroyable_array[y2 + 1, x2] == PieceType.free)//if down mid is free
                            {
                                if (y3 != 8)
                                {
                                    if (destroyable_array[y3 + 1, x3] == PieceType.free)//downleft
                                    {
                                        //move is possible. make it and check it
                                        destroyable_array[y + 1, x] = friendly_selected;
                                        destroyable_array[y2 + 1, x2] = friendly_selected;
                                        destroyable_array[y3 + 1, x3] = friendly_selected;
                                        destroyable_array[y, x] = PieceType.free;
                                        destroyable_array[y2, x2] = PieceType.free;
                                        destroyable_array[y3, x3] = PieceType.free;
                                        current_score = Evaluate_Borad(destroyable_array, 0, 0);
                                        if (current_score > goodest.score)
                                        {
                                            goodest.score = current_score;
                                            Copy_Arrays(ref destroyable_array, ref goodest.borad);
                                        }
                                        Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                                    }//downleft
                                }
                                if (x != 8)
                                {
                                    if (destroyable_array[y, x + 1] == PieceType.free)//downright
                                    {
                                        //move is possible. make it and check it
                                        destroyable_array[y, x + 1] = friendly_selected;
                                        destroyable_array[y2, x2 + 1] = friendly_selected;
                                        destroyable_array[y3, x3 + 1] = friendly_selected;
                                        destroyable_array[y, x] = PieceType.free;
                                        destroyable_array[y2, x2] = PieceType.free;
                                        destroyable_array[y3, x3] = PieceType.free;
                                        current_score = Evaluate_Borad(destroyable_array, 0, 0);
                                        if (current_score > goodest.score)
                                        {
                                            goodest.score = current_score;
                                            Copy_Arrays(ref destroyable_array, ref goodest.borad);
                                        }
                                        Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                                    }//downright
                                }
                            }//if down mid is free
                        }
                    }
                }

            }//left movement mybe possible finished
            if (y >= 2 && x <= 6) //flat left
            {
                if (destroyable_array[y - 1, x + 1] == friendly)//right movment is possible
                {
                    y2 = y - 1;
                    x2 = x + 1;
                    if (destroyable_array[y - 2, x + 2] == friendly)
                    {
                        y3 = y - 2;
                        x3 = x + 2;
                        if (y3 != 0 && x3 != 8)
                        {
                            if (destroyable_array[y3 - 1, x3 + 1] == PieceType.free)//flat left
                            {
                                //move is possible. make it and check it
                                destroyable_array[y3 - 1, x3 + 1] = friendly_selected;
                                destroyable_array[y3, x3] = friendly_selected;
                                destroyable_array[y2, x2] = friendly_selected;
                                destroyable_array[y, x] = PieceType.free;
                                current_score = Evaluate_Borad(destroyable_array, 0, 0);
                                if (current_score > goodest.score)
                                {
                                    goodest.score = current_score;
                                    Copy_Arrays(ref destroyable_array, ref goodest.borad);
                                }
                                Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                            }//flat left
                        }
                        if (y != 8 && x != 0)
                        {
                            if (destroyable_array[y + 1, x - 1] == PieceType.free)//flat right
                            {
                                //move is possible. make it and check it
                                destroyable_array[y + 1, x - 1] = friendly_selected;
                                destroyable_array[y, x] = friendly_selected;
                                destroyable_array[y2, x2] = friendly_selected;
                                destroyable_array[y3, x3] = PieceType.free;
                                current_score = Evaluate_Borad(destroyable_array, 0, 0);
                                if (current_score > goodest.score)
                                {
                                    goodest.score = current_score;
                                    Copy_Arrays(ref destroyable_array, ref goodest.borad);
                                }
                                Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                            }//flat right
                        }
                        if (x != 8 && x2 != 8)
                        {
                            if (destroyable_array[y, x + 1] == PieceType.free && destroyable_array[y2, x2 + 1] == PieceType.free)//if upmid is free
                            {
                                if (x3 != 8)
                                {
                                    if (destroyable_array[y3, x3 + 1] == PieceType.free)//upleft
                                    {
                                        //move is possible. make it and check it
                                        destroyable_array[y, x + 1] = friendly_selected;
                                        destroyable_array[y2, x2 + 1] = friendly_selected;
                                        destroyable_array[y3, x3 + 1] = friendly_selected;
                                        destroyable_array[y, x] = PieceType.free;
                                        destroyable_array[y2, x2] = PieceType.free;
                                        destroyable_array[y3, x3] = PieceType.free;
                                        current_score = Evaluate_Borad(destroyable_array, 0, 0);
                                        if (current_score > goodest.score)
                                        {
                                            goodest.score = current_score;
                                            Copy_Arrays(ref destroyable_array, ref goodest.borad);
                                        }
                                        Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                                    }//upleft
                                }
                                if (y != 8)
                                {
                                    if (destroyable_array[y + 1, x] == PieceType.free)//upright
                                    {
                                        //move is possible. make it and check it
                                        destroyable_array[y + 1, x] = friendly_selected;
                                        destroyable_array[y2 + 1, x2] = friendly_selected;
                                        destroyable_array[y3 + 1, x3] = friendly_selected;
                                        destroyable_array[y, x] = PieceType.free;
                                        destroyable_array[y2, x2] = PieceType.free;
                                        destroyable_array[y3, x3] = PieceType.free;
                                        current_score = Evaluate_Borad(destroyable_array, 0, 0);
                                        if (current_score > goodest.score)
                                        {
                                            goodest.score = current_score;
                                            Copy_Arrays(ref destroyable_array, ref goodest.borad);
                                        }
                                        Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                                    }//upright
                                }
                            }//if up mid is free
                        }
                        if (y != 0 && y2 != 0)
                        {
                            if (destroyable_array[y - 1, x] == PieceType.free && destroyable_array[y2 - 1, x2] == PieceType.free)//if down mid is free
                            {
                                if (y3 != 0)
                                {
                                    if (destroyable_array[y3 - 1, x3] == PieceType.free)//downleft
                                    {
                                        //move is possible. make it and check it
                                        destroyable_array[y - 1, x] = friendly_selected;
                                        destroyable_array[y2 - 1, x2] = friendly_selected;
                                        destroyable_array[y3 - 1, x3] = friendly_selected;
                                        destroyable_array[y, x] = PieceType.free;
                                        destroyable_array[y2, x2] = PieceType.free;
                                        destroyable_array[y3, x3] = PieceType.free;
                                        current_score = Evaluate_Borad(destroyable_array, 0, 0);
                                        if (current_score > goodest.score)
                                        {
                                            goodest.score = current_score;
                                            Copy_Arrays(ref destroyable_array, ref goodest.borad);
                                        }
                                        Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                                    }//downleft
                                }
                                if (x != 0)
                                {
                                    if (destroyable_array[y, x - 1] == PieceType.free)//downright
                                    {
                                        //move is possible. make it and check it
                                        destroyable_array[y, x - 1] = friendly_selected;
                                        destroyable_array[y2, x2 - 1] = friendly_selected;
                                        destroyable_array[y3, x3 - 1] = friendly_selected;
                                        destroyable_array[y, x] = PieceType.free;
                                        destroyable_array[y2, x2] = PieceType.free;
                                        destroyable_array[y3, x3] = PieceType.free;
                                        current_score = Evaluate_Borad(destroyable_array, 0, 0);
                                        if (current_score > goodest.score)
                                        {
                                            goodest.score = current_score;
                                            Copy_Arrays(ref destroyable_array, ref goodest.borad);
                                        }
                                        Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                                    }//downright
                                }
                            }//if down mid is free
                        }
                    }
                }

            }//right movement mybe possible finished

            return goodest;
        }//finished with blue

        /// <summary>
        /// פעולה זו בודקת את כול ההזות של 2
        /// </summary>
        /// <param name="unchengeable_borad">לוח מקורי </param>
        /// <param name="x">נצ חייל </param>
        /// <param name="y">נצ חייל</param>
        /// <returns>הצעד הכי טוב שמצאה </returns>
        public MoveType Check_All_Shifts_Of_Two(ref PieceType[,] unchengeable_borad, int x, int y)
        {
            float current_score;
            MoveType goodest = new MoveType();
            goodest.score = -10000;
            PieceType[,] destroyable_array = new PieceType[Settings.BORAD_ARRAY_SIZE, Settings.BORAD_ARRAY_SIZE];
            Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
            //flat case 
            if (x >= 3)
            {
                if (destroyable_array[y, x - 1] == friendly)//left movment is possible
                {
                    if (destroyable_array[y, x - 2] == rivel)
                    {
                        if (destroyable_array[y, x - 3] == PieceType.free)
                        {
                            // move is possible.make it and check it
                            destroyable_array[y, x - 3] = rivel;
                            destroyable_array[y, x - 2] = friendly_selected;
                            destroyable_array[y, x - 1] = friendly_selected;
                            destroyable_array[y, x] = PieceType.free;
                            current_score = Evaluate_Borad(destroyable_array, 0, 1);
                            if (current_score > goodest.score)
                            {
                                goodest.score = current_score;
                                Copy_Arrays(ref destroyable_array, ref goodest.borad);
                            }
                            Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                        }
                    }
                }
            }//left psuh mybe possible finished
            if (x <= 5)
            {
                if (destroyable_array[y, x + 1] == friendly)//left movment is possible
                {
                    if (destroyable_array[y, x + 2] == rivel)
                    {
                        if (destroyable_array[y, x + 3] == PieceType.free)
                        {
                            // move is possible.make it and check it
                            destroyable_array[y, x + 3] = rivel;
                            destroyable_array[y, x + 2] = friendly_selected;
                            destroyable_array[y, x + 1] = friendly_selected;
                            destroyable_array[y, x] = PieceType.free;
                            current_score = Evaluate_Borad(destroyable_array, 0, 1);
                            if (current_score > goodest.score)
                            {
                                goodest.score = current_score;
                                Copy_Arrays(ref destroyable_array, ref goodest.borad);
                            }
                            Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                        }
                    }
                }
            }//right movement mybe possible finished

            //down case
            if (y >= 3)
            {
                if (destroyable_array[y - 1, x] == friendly)//left movment is possible
                {
                    if (destroyable_array[y - 2, x] == rivel)
                    {
                        if (destroyable_array[y - 3, x] == PieceType.free)
                        {
                            // move is possible.make it and check it
                            destroyable_array[y - 3, x] = rivel;
                            destroyable_array[y - 2, x] = friendly_selected;
                            destroyable_array[y - 1, x] = friendly_selected;
                            destroyable_array[y, x] = PieceType.free;
                            current_score = Evaluate_Borad(destroyable_array, 0, 1);
                            if (current_score > goodest.score)
                            {
                                goodest.score = current_score;
                                Copy_Arrays(ref destroyable_array, ref goodest.borad);
                            }
                            Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                        }
                    }
                }
            }//left psuh mybe possible finished
            if (y <= 5)
            {
                if (destroyable_array[y + 1, x] == friendly)//left movment is possible
                {
                    if (destroyable_array[y + 2, x] == rivel)
                    {
                        if (destroyable_array[y + 3, x] == PieceType.free)
                        {
                            // move is possible.make it and check it
                            destroyable_array[y + 3, x] = rivel;
                            destroyable_array[y + 2, x] = friendly_selected;
                            destroyable_array[y + 1, x] = friendly_selected;
                            destroyable_array[y, x] = PieceType.free;
                            current_score = Evaluate_Borad(destroyable_array, 0, 1);
                            if (current_score > goodest.score)
                            {
                                goodest.score = current_score;
                                Copy_Arrays(ref destroyable_array, ref goodest.borad);
                            }
                            Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                        }
                    }
                }
            }//right psuh mybe possible finished

            //up case
            if (x >= 3 && y <= 5)
            {
                if (destroyable_array[y + 1, x - 1] == friendly)//left movment is possible
                {
                    if (destroyable_array[y + 2, x - 2] == rivel)
                    {
                        if (destroyable_array[y + 3, x - 3] == PieceType.free)
                        {
                            // move is possible.make it and check it
                            destroyable_array[y + 3, x - 3] = rivel;
                            destroyable_array[y + 2, x - 2] = friendly_selected;
                            destroyable_array[y + 1, x - 1] = friendly_selected;
                            destroyable_array[y, x] = PieceType.free;
                            current_score = Evaluate_Borad(destroyable_array, 0, 1);
                            if (current_score > goodest.score)
                            {
                                goodest.score = current_score;
                                Copy_Arrays(ref destroyable_array, ref goodest.borad);
                            }
                            Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                        }
                    }
                }
            }//left psuh mybe possible finished   
            if (y >= 3 && x <= 5)
            {
                if (destroyable_array[y - 1, x + 1] == friendly)//left movment is possible
                {
                    if (destroyable_array[y - 2, x + 2] == rivel)
                    {
                        if (destroyable_array[y - 3, x + 3] == PieceType.free)
                        {
                            // move is possible.make it and check it
                            destroyable_array[y - 3, x + 3] = rivel;
                            destroyable_array[y - 2, x + 2] = friendly_selected;
                            destroyable_array[y - 1, x + 1] = friendly_selected;
                            destroyable_array[y, x] = PieceType.free;
                            current_score = Evaluate_Borad(destroyable_array, 0, 1);
                            if (current_score > goodest.score)
                            {
                                goodest.score = current_score;
                                Copy_Arrays(ref destroyable_array, ref goodest.borad);
                            }
                            Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                        }
                    }
                }
            }//right psuh mybe possible finished

            return goodest;
        }//finished with blue and checked

        /// <summary>
        /// פעולה זו בודקת את כול ההזות של 3
        /// </summary>
        /// <param name="unchengeable_borad">לוח מקורי </param>
        /// <param name="x">נצ חייל</param>
        /// <param name="y">נצ חייל</param>
        /// <returns>הצעד הכי טוב שמצאה </returns>
        public MoveType Check_All_Shifts_Of_Three(ref PieceType[,] unchengeable_borad, int x, int y)
        {
            float current_score;
            MoveType goodest = new MoveType();
            goodest.score = -10000;
            PieceType[,] destroyable_array = new PieceType[Settings.BORAD_ARRAY_SIZE, Settings.BORAD_ARRAY_SIZE];
            Copy_Arrays(ref unchengeable_borad, ref destroyable_array);

            //flat case
            if (x >= 4)
            {
                if (destroyable_array[y, x - 1] == friendly)
                {
                    if (destroyable_array[y, x - 2] == friendly)
                    {
                        if (destroyable_array[y, x - 3] == rivel)
                        {
                            if (destroyable_array[y, x - 4] == PieceType.free)//move one
                            {
                                // move is possible.make it and check it
                                destroyable_array[y, x - 4] = rivel;
                                destroyable_array[y, x - 3] = friendly_selected;
                                destroyable_array[y, x - 2] = friendly_selected;
                                destroyable_array[y, x - 1] = friendly_selected;
                                destroyable_array[y, x] = PieceType.free;
                                current_score = Evaluate_Borad(destroyable_array, 0, 1);
                                if (current_score > goodest.score)
                                {
                                    goodest.score = current_score;
                                    Copy_Arrays(ref destroyable_array, ref goodest.borad);
                                }
                                Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                            }
                            if (destroyable_array[y, x - 4] == rivel)//move two
                            {
                                if (x >= 5)
                                {
                                    if (destroyable_array[y, x - 5] == PieceType.free)
                                    {
                                        // move is possible.make it and check it
                                        destroyable_array[y, x - 5] = rivel;
                                        destroyable_array[y, x - 3] = friendly_selected;
                                        destroyable_array[y, x - 2] = friendly_selected;
                                        destroyable_array[y, x - 1] = friendly_selected;
                                        destroyable_array[y, x] = PieceType.free;
                                        current_score = Evaluate_Borad(destroyable_array, 0, 2);
                                        if (current_score > goodest.score)
                                        {
                                            goodest.score = current_score;
                                            Copy_Arrays(ref destroyable_array, ref goodest.borad);
                                        }
                                        Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                                    }
                                }
                            }
                        }
                    }
                }
            }//left psuh mybe possible finished
            if (x <= 4)
            {
                if (destroyable_array[y, x + 1] == friendly)
                {
                    if (destroyable_array[y, x + 2] == friendly)
                    {
                        if (destroyable_array[y, x + 3] == rivel)
                        {
                            if (destroyable_array[y, x + 4] == PieceType.free)//move one
                            {
                                // move is possible.make it and check it
                                destroyable_array[y, x + 4] = rivel;
                                destroyable_array[y, x + 3] = friendly_selected;
                                destroyable_array[y, x + 2] = friendly_selected;
                                destroyable_array[y, x + 1] = friendly_selected;
                                destroyable_array[y, x] = PieceType.free;
                                current_score = Evaluate_Borad(destroyable_array, 0, 1);
                                if (current_score > goodest.score)
                                {
                                    goodest.score = current_score;
                                    Copy_Arrays(ref destroyable_array, ref goodest.borad);
                                }
                                Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                            }
                            if (destroyable_array[y, x + 4] == rivel)//move two
                            {
                                if (x <= 3)
                                {
                                    if (destroyable_array[y, x + 5] == PieceType.free)
                                    {
                                        // move is possible.make it and check it
                                        destroyable_array[y, x + 5] = rivel;
                                        destroyable_array[y, x + 3] = friendly_selected;
                                        destroyable_array[y, x + 2] = friendly_selected;
                                        destroyable_array[y, x + 1] = friendly_selected;
                                        destroyable_array[y, x] = PieceType.free;
                                        current_score = Evaluate_Borad(destroyable_array, 0, 2);
                                        if (current_score > goodest.score)
                                        {
                                            goodest.score = current_score;
                                            Copy_Arrays(ref destroyable_array, ref goodest.borad);
                                        }
                                        Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                                    }
                                }
                            }
                        }
                    }
                }
            }//right psuh mybe possible finished

            //down case 
            if (y >= 4)
            {
                if (destroyable_array[y - 1, x] == friendly)
                {
                    if (destroyable_array[y - 2, x] == friendly)
                    {
                        if (destroyable_array[y - 3, x] == rivel)
                        {
                            if (destroyable_array[y - 4, x] == PieceType.free)//move one
                            {
                                // move is possible.make it and check it
                                destroyable_array[y - 4, x] = rivel;
                                destroyable_array[y - 3, x] = friendly_selected;
                                destroyable_array[y - 2, x] = friendly_selected;
                                destroyable_array[y - 1, x] = friendly_selected;
                                destroyable_array[y, x] = PieceType.free;
                                current_score = Evaluate_Borad(destroyable_array, 0, 1);
                                if (current_score > goodest.score)
                                {
                                    goodest.score = current_score;
                                    Copy_Arrays(ref destroyable_array, ref goodest.borad);
                                }
                                Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                            }
                            if (destroyable_array[y - 4, x] == rivel)//move two
                            {
                                if (y >= 5)
                                {
                                    if (destroyable_array[y - 5, x] == PieceType.free)
                                    {
                                        // move is possible.make it and check it
                                        destroyable_array[y - 5, x] = rivel;
                                        destroyable_array[y - 3, x] = friendly_selected;
                                        destroyable_array[y - 2, x] = friendly_selected;
                                        destroyable_array[y - 1, x] = friendly_selected;
                                        destroyable_array[y, x] = PieceType.free;
                                        current_score = Evaluate_Borad(destroyable_array, 0, 2);
                                        if (current_score > goodest.score)
                                        {
                                            goodest.score = current_score;
                                            Copy_Arrays(ref destroyable_array, ref goodest.borad);
                                        }
                                        Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                                    }
                                }
                            }
                        }
                    }
                }
            }//left psuh mybe possible finished
            if (y <= 4)
            {
                if (destroyable_array[y + 1, x] == friendly)
                {
                    if (destroyable_array[y + 2, x] == friendly)
                    {
                        if (destroyable_array[y + 3, x] == rivel)
                        {
                            if (destroyable_array[y + 4, x] == PieceType.free)//move one
                            {
                                // move is possible.make it and check it
                                destroyable_array[y + 4, x] = rivel;
                                destroyable_array[y + 3, x] = friendly_selected;
                                destroyable_array[y + 2, x] = friendly_selected;
                                destroyable_array[y + 1, x] = friendly_selected;
                                destroyable_array[y, x] = PieceType.free;
                                current_score = Evaluate_Borad(destroyable_array, 0, 1);
                                if (current_score > goodest.score)
                                {
                                    goodest.score = current_score;
                                    Copy_Arrays(ref destroyable_array, ref goodest.borad);
                                }
                                Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                            }
                            if (destroyable_array[y + 4, x] == rivel)//move two
                            {
                                if (y <= 3)
                                {
                                    if (destroyable_array[y + 5, x] == PieceType.free)
                                    {
                                        // move is possible.make it and check it
                                        destroyable_array[y + 5, x] = rivel;
                                        destroyable_array[y + 3, x] = friendly_selected;
                                        destroyable_array[y + 2, x] = friendly_selected;
                                        destroyable_array[y + 1, x] = friendly_selected;
                                        destroyable_array[y, x] = PieceType.free;
                                        current_score = Evaluate_Borad(destroyable_array, 0, 2);
                                        if (current_score > goodest.score)
                                        {
                                            goodest.score = current_score;
                                            Copy_Arrays(ref destroyable_array, ref goodest.borad);
                                        }
                                        Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                                    }
                                }
                            }
                        }
                    }
                }
            }//right psuh mybe possible finished

            //up case
            if (y <= 4 && x >= 4)
            {
                if (destroyable_array[y + 1, x - 1] == friendly)
                {
                    if (destroyable_array[y + 2, x - 2] == friendly)
                    {
                        if (destroyable_array[y + 3, x - 3] == rivel)
                        {
                            if (destroyable_array[y + 4, x - 4] == PieceType.free)//move one
                            {
                                // move is possible.make it and check it
                                destroyable_array[y + 4, x - 4] = rivel;
                                destroyable_array[y + 3, x - 3] = friendly_selected;
                                destroyable_array[y + 2, x - 2] = friendly_selected;
                                destroyable_array[y + 1, x - 1] = friendly_selected;
                                destroyable_array[y, x] = PieceType.free;
                                current_score = Evaluate_Borad(destroyable_array, 0, 1);
                                if (current_score > goodest.score)
                                {
                                    goodest.score = current_score;
                                    Copy_Arrays(ref destroyable_array, ref goodest.borad);
                                }
                                Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                            }
                            if (destroyable_array[y + 4, x - 4] == rivel)//move two
                            {
                                if (y <= 3 && x >= 5)
                                {
                                    if (destroyable_array[y + 5, x - 5] == PieceType.free)
                                    {
                                        // move is possible.make it and check it
                                        destroyable_array[y + 5, x - 5] = rivel;
                                        destroyable_array[y + 3, x - 3] = friendly_selected;
                                        destroyable_array[y + 2, x - 2] = friendly_selected;
                                        destroyable_array[y + 1, x - 1] = friendly_selected;
                                        destroyable_array[y, x] = PieceType.free;
                                        current_score = Evaluate_Borad(destroyable_array, 0, 2);
                                        if (current_score > goodest.score)
                                        {
                                            goodest.score = current_score;
                                            Copy_Arrays(ref destroyable_array, ref goodest.borad);
                                        }
                                        Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                                    }
                                }
                            }
                        }
                    }
                }
            }//left psuh mybe possible finished
            if (x <= 4 && y >= 4)
            {
                if (destroyable_array[y - 1, x + 1] == friendly)
                {
                    if (destroyable_array[y - 2, x + 2] == friendly)
                    {
                        if (destroyable_array[y - 3, x + 3] == rivel)
                        {
                            if (destroyable_array[y - 4, x + 4] == PieceType.free)//move one
                            {
                                // move is possible.make it and check it
                                destroyable_array[y - 4, x + 4] = rivel;
                                destroyable_array[y - 3, x + 3] = friendly_selected;
                                destroyable_array[y - 2, x + 2] = friendly_selected;
                                destroyable_array[y - 1, x + 1] = friendly_selected;
                                destroyable_array[y, x] = PieceType.free;
                                current_score = Evaluate_Borad(destroyable_array, 0, 1);
                                if (current_score > goodest.score)
                                {
                                    goodest.score = current_score;
                                    Copy_Arrays(ref destroyable_array, ref goodest.borad);
                                }
                                Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                            }
                            if (destroyable_array[y - 4, x + 4] == rivel)//move two
                            {
                                if (x <= 3 && y >= 5)
                                {
                                    if (destroyable_array[y - 5, x + 5] == PieceType.free)
                                    {
                                        // move is possible.make it and check it
                                        destroyable_array[y - 5, x + 5] = rivel;
                                        destroyable_array[y - 3, x + 3] = friendly_selected;
                                        destroyable_array[y - 2, x + 2] = friendly_selected;
                                        destroyable_array[y - 1, x + 1] = friendly_selected;
                                        destroyable_array[y, x] = PieceType.free;
                                        current_score = Evaluate_Borad(destroyable_array, 0, 2);
                                        if (current_score > goodest.score)
                                        {
                                            goodest.score = current_score;
                                            Copy_Arrays(ref destroyable_array, ref goodest.borad);
                                        }
                                        Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                                    }
                                }
                            }
                        }
                    }
                }
            }//right psuh mybe possible finished

            return goodest;
        }//finished with blue and checked

        /// <summary>
        /// פעולה זו בודקת את כול האכילות של 2 
        /// </summary>
        /// <param name="unchengeable_borad">לוח מקורי</param>
        /// <param name="x">נצ חייל </param>
        /// <param name="y">נצ חייל </param>
        /// <returns>הצעד הכי טוב שמצאה </returns>
        public MoveType Check_All_Eets_Of_Two(ref PieceType[,] unchengeable_borad, int x, int y)
        {
            float current_score;
            MoveType goodest = new MoveType();
            goodest.score = -10000;
            PieceType[,] destroyable_array = new PieceType[Settings.BORAD_ARRAY_SIZE, Settings.BORAD_ARRAY_SIZE];
            Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
            Graphics graphics = new Graphics();
            //flat case
            if (x >= 2)
            {
                if (destroyable_array[y, x - 1] == friendly)
                {
                    if (destroyable_array[y, x - 2] == rivel)
                    {
                        if (graphics.Get_First_Index_Of_Row(y) == x - 2)
                        {
                            // move is possible.make it and check it
                            destroyable_array[y, x - 2] = friendly_selected;
                            destroyable_array[y, x - 1] = friendly_selected;
                            destroyable_array[y, x] = PieceType.free;
                            current_score = Evaluate_Borad(destroyable_array, 1, 0);
                            if (current_score > goodest.score)
                            {
                                goodest.score = current_score;
                                Copy_Arrays(ref destroyable_array, ref goodest.borad);
                            }
                            Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                        }
                    }
                }
            }//left 
            if (x <= 6)
            {
                if (destroyable_array[y, x + 1] == friendly)
                {
                    if (destroyable_array[y, x + 2] == rivel)
                    {
                        if (graphics.Get_Last_Index_Of_Row(y) == x + 2)
                        {
                            // move is possible.make it and check it
                            destroyable_array[y, x + 2] = friendly_selected;
                            destroyable_array[y, x + 1] = friendly_selected;
                            destroyable_array[y, x] = PieceType.free;
                            current_score = Evaluate_Borad(destroyable_array, 1, 0);
                            if (current_score > goodest.score)
                            {
                                goodest.score = current_score;
                                Copy_Arrays(ref destroyable_array, ref goodest.borad);
                            }
                            Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                        }
                    }
                }
            }//right

            //down case
            if (y >= 2)
            {
                if (destroyable_array[y - 1, x] == friendly)
                {
                    if (destroyable_array[y - 2, x] == rivel)
                    {
                        if (y - 2 <= 4)
                        {
                            if (graphics.Get_First_Index_Of_Row(y - 2) == x || y - 2 == 0)
                            {
                                // move is possible.make it and check it
                                destroyable_array[y - 2, x] = friendly_selected;
                                destroyable_array[y - 1, x] = friendly_selected;
                                destroyable_array[y, x] = PieceType.free;
                                current_score = Evaluate_Borad(destroyable_array, 1, 0);
                                if (current_score > goodest.score)
                                {
                                    goodest.score = current_score;
                                    Copy_Arrays(ref destroyable_array, ref goodest.borad);
                                }
                                Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                            }
                        }
                    }
                }
            }//left 
            if (y <= 6)
            {
                if (destroyable_array[y + 1, x] == friendly)
                {
                    if (destroyable_array[y + 2, x] == rivel)
                    {
                        if (y + 2 >= 4)
                        {
                            if (graphics.Get_Last_Index_Of_Row(y + 2) == x || y + 2 == 8)
                            {
                                // move is possible.make it and check it
                                destroyable_array[y + 2, x] = friendly_selected;
                                destroyable_array[y + 1, x] = friendly_selected;
                                destroyable_array[y, x] = PieceType.free;
                                current_score = Evaluate_Borad(destroyable_array, 1, 0);
                                if (current_score > goodest.score)
                                {
                                    goodest.score = current_score;
                                    Copy_Arrays(ref destroyable_array, ref goodest.borad);
                                }
                                Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                            }
                        }
                    }
                }
            }//right

            //up case 
            if (y >= 2 && x <= 6)
            {
                if (destroyable_array[y - 1, x + 1] == friendly)
                {
                    if (destroyable_array[y - 2, x + 2] == rivel)
                    {
                        if (y - 2 <= 4)
                        {
                            if (graphics.Get_Last_Index_Of_Row(y - 2) == x + 2 || y - 2 == 0)
                            {
                                // move is possible.make it and check it
                                destroyable_array[y - 2, x + 2] = friendly_selected;
                                destroyable_array[y - 1, x + 1] = friendly_selected;
                                destroyable_array[y, x] = PieceType.free;
                                current_score = Evaluate_Borad(destroyable_array, 1, 0);
                                if (current_score > goodest.score)
                                {
                                    goodest.score = current_score;
                                    Copy_Arrays(ref destroyable_array, ref goodest.borad);
                                }
                                Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                            }
                        }
                    }
                }
            }//left 
            if (y <= 6 && x >= 2)
            {
                if (destroyable_array[y + 1, x - 1] == friendly)
                {
                    if (destroyable_array[y + 2, x - 2] == rivel)
                    {
                        if (y + 2 >= 4)
                        {
                            if (graphics.Get_First_Index_Of_Row(y + 2) == x - 2 || y + 2 == 0)
                            {
                                // move is possible.make it and check it
                                destroyable_array[y + 2, x - 2] = friendly_selected;
                                destroyable_array[y + 1, x - 1] = friendly_selected;
                                destroyable_array[y, x] = PieceType.free;
                                current_score = Evaluate_Borad(destroyable_array, 1, 0);
                                if (current_score > goodest.score)
                                {
                                    goodest.score = current_score;
                                    Copy_Arrays(ref destroyable_array, ref goodest.borad);
                                }
                                Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                            }
                        }
                    }
                }
            }//right

            return goodest;
        }//finishjed with blue and checked

        /// <summary>
        /// פעולה זו בודקת את כול האיכלות של 3
        /// </summary>
        /// <param name="unchengeable_borad">לוח מקורי </param>
        /// <param name="x">נצ חייל</param>
        /// <param name="y">צה חייל </param>
        /// <returns>הצעד הכי טוב שמצאה </returns>
        public MoveType Check_All_Eets_Of_Three(ref PieceType[,] unchengeable_borad, int x, int y)//finished with blue and half checked
        {
            float current_score;
            MoveType goodest = new MoveType();
            goodest.score = -10000;
            PieceType[,] destroyable_array = new PieceType[Settings.BORAD_ARRAY_SIZE, Settings.BORAD_ARRAY_SIZE];
            Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
            Graphics graphics = new Graphics();

            //flat case 
            if (x >= 3)
            {
                if (destroyable_array[y, x - 1] == friendly)
                {
                    if (destroyable_array[y, x - 2] == friendly)
                    {
                        if (destroyable_array[y, x - 3] == rivel)
                        {
                            if (graphics.Get_First_Index_Of_Row(y) == x - 3)
                            {
                                //make move of one
                                destroyable_array[y, x - 3] = friendly_selected;
                                destroyable_array[y, x - 2] = friendly_selected;
                                destroyable_array[y, x - 1] = friendly_selected;
                                destroyable_array[y, x] = PieceType.free;
                                current_score = Evaluate_Borad(destroyable_array, 1, 0);
                                if (current_score > goodest.score)
                                {
                                    goodest.score = current_score;
                                    Copy_Arrays(ref destroyable_array, ref goodest.borad);
                                }
                                Copy_Arrays(ref unchengeable_borad, ref destroyable_array);

                            }
                            if (x >= 4)
                            {
                                if (destroyable_array[y, x - 4] == rivel)
                                {
                                    if (graphics.Get_First_Index_Of_Row(y) == x - 4)
                                    {
                                        //make move of two 
                                        destroyable_array[y, x - 3] = friendly_selected;
                                        destroyable_array[y, x - 2] = friendly_selected;
                                        destroyable_array[y, x - 1] = friendly_selected;
                                        destroyable_array[y, x] = PieceType.free;
                                        current_score = Evaluate_Borad(destroyable_array, 1, 1);
                                        if (current_score > goodest.score)
                                        {
                                            goodest.score = current_score;
                                            Copy_Arrays(ref destroyable_array, ref goodest.borad);
                                        }
                                        Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                                    }
                                }
                            }
                        }
                    }
                }
            }//left
            if (x <= 5)
            {
                if (destroyable_array[y, x + 1] == friendly)
                {
                    if (destroyable_array[y, x + 2] == friendly)
                    {
                        if (destroyable_array[y, x + 3] == rivel)
                        {
                            if (graphics.Get_Last_Index_Of_Row(y) == x + 3)
                            {
                                //make move of one
                                destroyable_array[y, x + 3] = friendly_selected;
                                destroyable_array[y, x + 2] = friendly_selected;
                                destroyable_array[y, x + 1] = friendly_selected;
                                destroyable_array[y, x] = PieceType.free;
                                current_score = Evaluate_Borad(destroyable_array, 1, 0);
                                if (current_score > goodest.score)
                                {
                                    goodest.score = current_score;
                                    Copy_Arrays(ref destroyable_array, ref goodest.borad);
                                }
                                Copy_Arrays(ref unchengeable_borad, ref destroyable_array);

                            }
                            if (x <= 4)
                            {
                                if (destroyable_array[y, x + 4] == rivel)
                                {
                                    if (graphics.Get_Last_Index_Of_Row(y) == x + 4)
                                    {
                                        //make move of two 
                                        destroyable_array[y, x + 3] = friendly_selected;
                                        destroyable_array[y, x + 2] = friendly_selected;
                                        destroyable_array[y, x + 1] = friendly_selected;
                                        destroyable_array[y, x] = PieceType.free;
                                        current_score = Evaluate_Borad(destroyable_array, 1, 1);
                                        if (current_score > goodest.score)
                                        {
                                            goodest.score = current_score;
                                            Copy_Arrays(ref destroyable_array, ref goodest.borad);
                                        }
                                        Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                                    }
                                }
                            }
                        }
                    }
                }
            }//right

            //down case 
            if (y >= 3)
            {
                if (destroyable_array[y - 1, x] == friendly)
                {
                    if (destroyable_array[y - 2, x] == friendly)
                    {
                        if (destroyable_array[y - 3, x] == rivel)
                        {
                            if (graphics.Get_First_Index_Of_Row(y - 3) == x || y - 3 == 0)
                            {
                                //make move of one
                                destroyable_array[y - 3, x] = friendly_selected;
                                destroyable_array[y - 2, x] = friendly_selected;
                                destroyable_array[y - 1, x] = friendly_selected;
                                destroyable_array[y, x] = PieceType.free;
                                current_score = Evaluate_Borad(destroyable_array, 1, 0);
                                if (current_score > goodest.score)
                                {
                                    goodest.score = current_score;
                                    Copy_Arrays(ref destroyable_array, ref goodest.borad);
                                }
                                Copy_Arrays(ref unchengeable_borad, ref destroyable_array);

                            }
                            if (y >= 4)
                            {
                                if (destroyable_array[y - 4, x] == rivel)
                                {
                                    if (graphics.Get_First_Index_Of_Row(y - 4) == x || y - 4 == 0)
                                    {
                                        //make move of two 
                                        destroyable_array[y - 3, x] = friendly_selected;
                                        destroyable_array[y - 2, x] = friendly_selected;
                                        destroyable_array[y - 1, x] = friendly_selected;
                                        destroyable_array[y, x] = PieceType.free;
                                        current_score = Evaluate_Borad(destroyable_array, 1, 1);
                                        if (current_score > goodest.score)
                                        {
                                            goodest.score = current_score;
                                            Copy_Arrays(ref destroyable_array, ref goodest.borad);
                                        }
                                        Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                                    }
                                }
                            }
                        }
                    }
                }
            }//left
            if (y <= 5)
            {
                if (destroyable_array[y + 1, x] == friendly)
                {
                    if (destroyable_array[y + 2, x] == friendly)
                    {
                        if (destroyable_array[y + 3, x] == rivel)
                        {
                            if (graphics.Get_Last_Index_Of_Row(y + 3) == x || y + 3 == 8)
                            {
                                //make move of one
                                destroyable_array[y + 3, x] = friendly_selected;
                                destroyable_array[y + 2, x] = friendly_selected;
                                destroyable_array[y + 1, x] = friendly_selected;
                                destroyable_array[y, x] = PieceType.free;
                                current_score = Evaluate_Borad(destroyable_array, 1, 0);
                                if (current_score > goodest.score)
                                {
                                    goodest.score = current_score;
                                    Copy_Arrays(ref destroyable_array, ref goodest.borad);
                                }
                                Copy_Arrays(ref unchengeable_borad, ref destroyable_array);

                            }
                            if (y <= 4)
                            {
                                if (destroyable_array[y + 4, x] == rivel)
                                {
                                    if (graphics.Get_Last_Index_Of_Row(y + 4) == x || y + 4 == 8)
                                    {
                                        //make move of two 
                                        destroyable_array[y + 3, x] = friendly_selected;
                                        destroyable_array[y + 2, x] = friendly_selected;
                                        destroyable_array[y + 1, x] = friendly_selected;
                                        destroyable_array[y, x] = PieceType.free;
                                        current_score = Evaluate_Borad(destroyable_array, 1, 1);
                                        if (current_score > goodest.score)
                                        {
                                            goodest.score = current_score;
                                            Copy_Arrays(ref destroyable_array, ref goodest.borad);
                                        }
                                        Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                                    }
                                }
                            }
                        }
                    }
                }
            }//right

            //up case 
            if (y <= 5 && x >= 3)
            {
                if (destroyable_array[y + 1, x - 1] == friendly)
                {
                    if (destroyable_array[y + 2, x - 2] == friendly)
                    {
                        if (destroyable_array[y + 3, x - 3] == rivel)
                        {
                            if (graphics.Get_First_Index_Of_Row(y + 3) == x - 3 || y + 3 == 8)
                            {
                                //make move of one
                                destroyable_array[y + 3, x - 3] = friendly_selected;
                                destroyable_array[y + 2, x - 2] = friendly_selected;
                                destroyable_array[y + 1, x - 1] = friendly_selected;
                                destroyable_array[y, x] = PieceType.free;
                                current_score = Evaluate_Borad(destroyable_array, 1, 0);
                                if (current_score > goodest.score)
                                {
                                    goodest.score = current_score;
                                    Copy_Arrays(ref destroyable_array, ref goodest.borad);
                                }
                                Copy_Arrays(ref unchengeable_borad, ref destroyable_array);

                            }
                            if (y <= 4 && x >= 4)
                            {
                                if (destroyable_array[y + 4, x - 4] == rivel)
                                {
                                    if (graphics.Get_First_Index_Of_Row(y + 4) == x - 4 || y + 4 == 8)
                                    {
                                        //make move of two 
                                        destroyable_array[y + 3, x - 3] = friendly_selected;
                                        destroyable_array[y + 2, x - 2] = friendly_selected;
                                        destroyable_array[y + 1, x - 1] = friendly_selected;
                                        destroyable_array[y, x] = PieceType.free;
                                        current_score = Evaluate_Borad(destroyable_array, 1, 1);
                                        if (current_score > goodest.score)
                                        {
                                            goodest.score = current_score;
                                            Copy_Arrays(ref destroyable_array, ref goodest.borad);
                                        }
                                        Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                                    }
                                }
                            }
                        }
                    }
                }
            }//left
            if (x <= 5 && y >= 3)
            {
                if (destroyable_array[y - 1, x + 1] == friendly)
                {
                    if (destroyable_array[y - 2, x + 2] == friendly)
                    {
                        if (destroyable_array[y - 3, x + 3] == rivel)
                        {
                            if (graphics.Get_Last_Index_Of_Row(y - 3) == x + 3 || y - 3 == 8)
                            {
                                //make move of one
                                destroyable_array[y - 3, x + 3] = friendly_selected;
                                destroyable_array[y - 2, x + 2] = friendly_selected;
                                destroyable_array[y - 1, x + 1] = friendly_selected;
                                destroyable_array[y, x] = PieceType.free;
                                current_score = Evaluate_Borad(destroyable_array, 1, 0);
                                if (current_score > goodest.score)
                                {
                                    goodest.score = current_score;
                                    Copy_Arrays(ref destroyable_array, ref goodest.borad);
                                }
                                Copy_Arrays(ref unchengeable_borad, ref destroyable_array);

                            }
                            if (y >= 4 && x <= 4)
                            {
                                if (destroyable_array[y - 4, x + 4] == rivel)
                                {
                                    if (graphics.Get_Last_Index_Of_Row(y - 4) == x + 4 || y - 4 == 8)
                                    {
                                        //make move of two 
                                        destroyable_array[y - 3, x + 3] = friendly_selected;
                                        destroyable_array[y - 2, x + 2] = friendly_selected;
                                        destroyable_array[y - 1, x + 1] = friendly_selected;
                                        destroyable_array[y, x] = PieceType.free;
                                        current_score = Evaluate_Borad(destroyable_array, 1, 1);
                                        if (current_score > goodest.score)
                                        {
                                            goodest.score = current_score;
                                            Copy_Arrays(ref destroyable_array, ref goodest.borad);
                                        }
                                        Copy_Arrays(ref unchengeable_borad, ref destroyable_array);
                                    }
                                }
                            }
                        }
                    }
                }
            }//right

            return goodest;
        }//finished with blue 


        #region evaluation_mathoods

        /// <summary>
        /// פעולה המעריכה את הלוח 
        /// </summary>
        /// <param name="game_borad">לוח המשחק </param>
        /// <param name="ejected_white">כמות החיילים הלבנים שהוצאו מהלוח </param>
        /// <param name="moved_white">כמות החיילים הלבנים שנדחפו במהלך </param>
        /// <returns>ניקוד הלוח</returns>
        public float Evaluate_Borad(PieceType[,] game_borad, int ejected_white, int moved_white)
        {
            float score = 0;

            int num_of_ejected_white = ejected_white;//max 1 min 0 max
            float normalized_num_of_ejected_white = 10 * num_of_ejected_white;
            int wight_1 = 100;

            int num_of_moved_white = moved_white;//max 2 min 0 max y = 5x
            float normalized_num_of_moved_white = 5 * num_of_moved_white;
            int wight_2 = 5;

            int num_of_nodes = Find_Num_Of_Nodes(game_borad); //max 14 min 1 minimiz y = -4x + 14
            float normalized_num_of_nodes = -4 * num_of_nodes + 14;//max 10 min 0
            int wight_3 = 1;

            float avg_distance_from_middle = Find_Avg_Distance_From_Middle(game_borad); //max 4 min 1.428 y = -3.88 * x +15.55 minimz
            float normalized_avg_distance_from_middle = (float)((-3.88) * (avg_distance_from_middle) + 15.55);//max 10 min 0
            int wight_4 = 1;

            int num_of_pieces_at_risk_of_ejection = Find_Num_Of_Pieces_At_Risk_Of_Eat(game_borad); //max 14 min 0 y = -5/7*x + 10 minimz           
            //float normalized_num_of_pieces_at_risk_of_ejection = (-5 / 7) * num_of_pieces_at_risk_of_ejection + 10;//max 10 min 0
            float normalized_num_of_pieces_at_risk_of_ejection = num_of_pieces_at_risk_of_ejection;
            int wight_5 = -100;




            //if (moved_white == 2)
            //{
            //    wight = 200;
            //}

            score = (wight_1 * normalized_num_of_ejected_white) + (wight_2 * normalized_num_of_moved_white) + (wight_3 * normalized_num_of_nodes) +
                (wight_4 * normalized_avg_distance_from_middle) + (wight_5 * normalized_num_of_pieces_at_risk_of_ejection);

            //score = (normalized_num_of_pieces_at_risk_of_ejection * wight_1);

            //score = nodes_score_table[num_of_nodes] - avg_distance_from_middle + wight + (1000 * ejected_white) - (num_of_pieces_at_risk_of_ejection * 100);
            return score;
        }

        /// <summary>
        /// פעולה שמוצאת את מספר הקוצות שלהם מחולק 
        /// </summary>
        /// <param name="game_borad">לוח המשחק </param>
        /// <returns>מספר הקבוצות </returns>
        private int Find_Num_Of_Nodes(PieceType[,] game_borad)
        {
            int nodes_num = 0;
            PieceType[,] distractible_borad = new PieceType[Settings.BORAD_ARRAY_SIZE, Settings.BORAD_ARRAY_SIZE];
            Copy_Arrays(ref game_borad, ref distractible_borad);
            for (int i = 0; i < Settings.BORAD_ARRAY_SIZE; i++)
            {
                for (int j = 0; j < Settings.BORAD_ARRAY_SIZE; j++)
                {
                    if (distractible_borad[i, j] == friendly || distractible_borad[i, j] == friendly_selected)
                    {
                        nodes_num++;
                        Node_Num_Rec(distractible_borad, j, i);
                    }
                }
            }
            Copy_Arrays(ref game_borad, ref distractible_borad);
            return nodes_num;
        }
        /// <summary>
        /// פעולת עזר 
        /// </summary>
        /// <param name="borad"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void Node_Num_Rec(PieceType[,] borad, int x, int y)
        {
            if (borad[y, x] == friendly || borad[y, x] == friendly_selected)
            {
                borad[y, x] = PieceType.visited;
                if (y != 0)//1
                {
                    Node_Num_Rec(borad, x, y - 1);
                }
                if (y != 0 && x != 8)//2
                {
                    Node_Num_Rec(borad, x + 1, y - 1);
                }
                if (x != 8)//3
                {
                    Node_Num_Rec(borad, x + 1, y);
                }
                if (y != 8)//4
                {
                    Node_Num_Rec(borad, x, y + 1);
                }
                if (y != 8 && x != 0)//5
                {
                    Node_Num_Rec(borad, x - 1, y + 1);
                }
                if (x != 0)//6
                {
                    Node_Num_Rec(borad, x - 1, y);
                }
            }
        }

        /// <summary>
        /// פעולה שמחזירה את המרחק הממוצע של כול החיילים מהמרכז 
        /// </summary>
        /// <param name="game_borad">לוח המשחק</param>
        /// <returns>פעולה שמחזירה את המרחק הממוצע של כול החיילים מהמרכז </returns>
        private float Find_Avg_Distance_From_Middle(PieceType[,] game_borad)//without obsticals
        {
            int num_of_balls = 0;
            float avg = 0;
            for (int i = 0; i < Settings.BORAD_ARRAY_SIZE; i++)
            {
                for (int j = 0; j < Settings.BORAD_ARRAY_SIZE; j++)
                {
                    if (game_borad[i, j] == friendly || game_borad[i, j] == friendly_selected)
                    {
                        num_of_balls++;
                        avg = avg + Get_Distance_Of_Point_From_Middle(i, j);
                    }
                }
            }
            return avg / num_of_balls;
        }
        /// <summary>
        /// פעולת עזר
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private float Get_Distance_Of_Point_From_Middle(int x, int y)
        {
            return distance_table[x, y];
        }

        /// <summary>
        /// פעולה המחזירה את מספר החיילים של הבוט הנמצאים בסיכון
        /// </summary>
        /// <param name="game_borad">לוח המחשק /param>
        /// <returns>פעולה המחזירה את מספר החיילים של הבוט הנמצאים בסיכון</returns>
        private int Find_Num_Of_Pieces_At_Risk_Of_Eat(PieceType[,] game_borad)
        {
            int num_of_balls = 0;
            for (int i = 0; i < Settings.BORAD_ARRAY_SIZE; i++)
            {
                for (int j = 0; j < Settings.BORAD_ARRAY_SIZE; j++)
                {
                    if (game_borad[i, j] == friendly || game_borad[i, j] == friendly_selected)
                    {
                        if (Is_Piece_At_Risk_Of_Eat(game_borad, j, i))
                        {
                            num_of_balls++;
                        }
                    }
                }
            }
            return num_of_balls;
        }

        /// <summary>
        /// פעולת עזר 
        /// </summary>
        /// <param name="game_borad"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private bool Is_Piece_At_Risk_Of_Eat(PieceType[,] game_borad, int x, int y)
        {
            bool condition = false;
            Graphics graphics = new Graphics();
            if (Get_Distance_Of_Point_From_Middle(x, y) == 4)
            {
                //1
                if (y == 8 || (y >= 4 && graphics.Get_Last_Index_Of_Row(y) == x))
                {
                    if (game_borad[y - 1, x] == friendly || game_borad[y - 1, x] == friendly_selected)
                    {
                        if (game_borad[y - 2, x] == rivel)
                        {
                            if (game_borad[y - 3, x] == rivel)
                            {
                                if (game_borad[y - 4, x] == rivel)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                    else if (game_borad[y - 1, x] == rivel)
                    {
                        if (game_borad[y - 2, x] == rivel)
                        {
                            return true;
                        }
                    }
                }
                //2
                if (y == 8 || (y >= 4 && graphics.Get_First_Index_Of_Row(y) == x))
                {
                    if (game_borad[y - 1, x + 1] == friendly || game_borad[y - 1, x + 1] == friendly_selected)
                    {
                        if (game_borad[y - 2, x + 2] == rivel)
                        {
                            if (game_borad[y - 3, x + 3] == rivel)
                            {
                                if (game_borad[y - 4, x + 4] == rivel)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                    else if (game_borad[y - 1, x + 1] == rivel)
                    {
                        if (game_borad[y - 2, x + 2] == rivel)
                        {
                            return true;
                        }
                    }
                }
                //3
                if (graphics.Get_First_Index_Of_Row(y) == x)
                {
                    if (game_borad[y, x + 1] == friendly || game_borad[y, x + 1] == friendly_selected)
                    {
                        if (game_borad[y, x + 2] == rivel)
                        {
                            if (game_borad[y, x + 3] == rivel)
                            {
                                if (game_borad[y, x + 4] == rivel)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                    else if (game_borad[y, x + 1] == rivel)
                    {
                        if (game_borad[y, x + 2] == rivel)
                        {
                            return true;
                        }
                    }
                }
                //4
                if (y == 0 || (y <= 4 && graphics.Get_First_Index_Of_Row(y) == x))
                {
                    if (game_borad[y + 1, x] == friendly || game_borad[y + 1, x] == friendly_selected)
                    {
                        if (game_borad[y + 2, x] == rivel)
                        {
                            if (game_borad[y + 3, x] == rivel)
                            {
                                if (game_borad[y + 4, x] == rivel)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                    else if (game_borad[y + 1, x] == rivel)
                    {
                        if (game_borad[y + 2, x] == rivel)
                        {
                            return true;
                        }
                    }
                }
                //5
                if (y == 0 || (y <= 4 && graphics.Get_Last_Index_Of_Row(y) == x))
                {
                    if (game_borad[y + 1, x - 1] == friendly || game_borad[y + 1, x - 1] == friendly_selected)
                    {
                        if (game_borad[y + 2, x - 2] == rivel)
                        {
                            if (game_borad[y + 3, x - 3] == rivel)
                            {
                                if (game_borad[y + 4, x - 4] == rivel)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                    else if (game_borad[y + 1, x - 1] == rivel)
                    {
                        if (game_borad[y + 2, x - 2] == rivel)
                        {
                            return true;
                        }
                    }
                }
                //6
                if (graphics.Get_Last_Index_Of_Row(y) == x)
                {
                    if (game_borad[y, x - 1] == friendly || game_borad[y, x - 1] == friendly_selected)
                    {
                        if (game_borad[y, x - 2] == rivel)
                        {
                            if (game_borad[y, x - 3] == rivel)
                            {
                                if (game_borad[y, x - 4] == rivel)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                    else if (game_borad[y, x - 1] == rivel)
                    {
                        if (game_borad[y, x - 2] == rivel)
                        {
                            return true;
                        }
                    }
                }
            }
            return condition;
        }
        #endregion


        #region small_mathoods

        /// <summary>
        /// פעולה שמעתיקה מערכים
        /// </summary>
        /// <param name="source">מערך המקור </param>
        /// <param name="destination">מערך היעד </param>
        public void Copy_Arrays(ref PieceType[,] source, ref PieceType[,] destination)
        {
            for (int i = 0; i < Settings.BORAD_ARRAY_SIZE; i++)
            {
                for (int j = 0; j < Settings.BORAD_ARRAY_SIZE; j++)
                {
                    destination[i, j] = source[i, j];
                }
            }
        }
        #endregion
    }
}

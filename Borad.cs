using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

using System.Windows;

namespace AbaloneGameWPF
{
    /// <summary>
    /// מחלקת לוח 
    /// </summary>
    class Borad
    {
        public PieceType[,] borad;//לוח המשחק 
        public Graphics graphics;//הצהרה על מחלקת גרפיקה 
        public int num_of_selected_pices = 0;// מספר החיילים שנבחרו 
        public Point[] previous_clicks;//הצהרה על מערך הלחיצות הקודמות
        public TwoPeiceFormationType Formation2;//הצהרה על פורמציה 

        //turn
        public bool turn = true;//true = white,false = black
        public PieceType friendly = PieceType.white;//חיילים ידידותיים 
        public PieceType rival = PieceType.black;//חיילי היריב

        //game
        public int black_balls_ejected = 0;//מספר החיילים השחורים שנפלטו מהלוח
        public int white_balls_ejected = 0;//מספר החיילים הלבנים שנפלטו מהלוח
        public bool is_game_ended = false;//האם משחק הסתיים 

        public int left_move_num = 0;//כמות החיילים שנדחפים בהזזה שמאלה
        public int right_move_num = 0;//כמות החיילים שנדחפים בהזזה ימינה
        public int left_eat_num = 0;//כמות החיילים שיוצאים בהזזה שמאלה
        public int right_eat_num = 0;//כמות החיילים שיוצאים בהזזה ימינה

        //ai
        public AIPlayer bot = new AIPlayer();//מחלקת בוט 

        /// <summary>
        /// פעולה בונה 
        /// </summary>
        public Borad()
        {
            Initialize_Game_Array();
            Initialize_Clicked_Array();
            is_game_ended = false;
            black_balls_ejected = 0;
            white_balls_ejected = 0;
            left_move_num = 0;
            right_move_num = 0;
            left_eat_num = 0;
            right_eat_num = 0;
        }

        /// <summary>
        /// פעולה המאתחלת את לוח המשחק 
        /// </summary>
        internal void Initialize_Game_Array()
        {
            //array of the game borad using piecetype(unusable,free,black,white)
            PieceType[,] array = new PieceType[Settings.BORAD_ARRAY_SIZE, Settings.BORAD_ARRAY_SIZE]
            {

            { PieceType.unusable,PieceType.unusable,PieceType.unusable, PieceType.unusable,PieceType.black,PieceType.black, PieceType.black,PieceType.black,PieceType.black},
            { PieceType.unusable,PieceType.unusable,PieceType.unusable, PieceType.black,PieceType.black,PieceType.black, PieceType.black,PieceType.black,PieceType.black},
            { PieceType.unusable,PieceType.unusable,PieceType.free, PieceType.free,PieceType.black,PieceType.black, PieceType.black,PieceType.free,PieceType.free},

            { PieceType.unusable,PieceType.free,PieceType.free, PieceType.free,PieceType.free,PieceType.free, PieceType.free,PieceType.free,PieceType.free},
            { PieceType.free,PieceType.free,PieceType.free, PieceType.free,PieceType.free,PieceType.free, PieceType.free,PieceType.free,PieceType.free},
            { PieceType.free,PieceType.free, PieceType.free,PieceType.free,PieceType.free, PieceType.free,PieceType.free,PieceType.free,PieceType.unusable},

            { PieceType.free, PieceType.free,PieceType.white,PieceType.white, PieceType.white,PieceType.free,PieceType.free,PieceType.unusable,PieceType.unusable},
            { PieceType.white,PieceType.white,PieceType.white, PieceType.white,PieceType.white,PieceType.white,PieceType.unusable,PieceType.unusable,PieceType.unusable},
            { PieceType.white,PieceType.white, PieceType.white,PieceType.white,PieceType.white,PieceType.unusable,PieceType.unusable,PieceType.unusable, PieceType.unusable},

            };

            //for (int i = 0; i < Settings.BORAD_ARRAY_SIZE; i++)
            //{
            //    for (int j = 0; j < Settings.BORAD_ARRAY_SIZE; j++)
            //    {
            //        array[i, j] = PieceType.black;
            //    }

            //}

            //{

            //{ PieceType.unusable,PieceType.unusable,PieceType.unusable, PieceType.unusable,PieceType.black,PieceType.free, PieceType.free,PieceType.free,PieceType.free},
            //{ PieceType.unusable,PieceType.unusable,PieceType.unusable, PieceType.black,PieceType.black,PieceType.free, PieceType.free,PieceType.free,PieceType.free},
            //{ PieceType.unusable,PieceType.unusable,PieceType.black, PieceType.black,PieceType.black,PieceType.free, PieceType.free,PieceType.free,PieceType.free},

            //{ PieceType.unusable,PieceType.white,PieceType.black, PieceType.black,PieceType.black,PieceType.free, PieceType.free,PieceType.free,PieceType.free},
            //{ PieceType.white,PieceType.white,PieceType.black, PieceType.black,PieceType.black,PieceType.free, PieceType.free,PieceType.free,PieceType.free},
            //{ PieceType.white,PieceType.white, PieceType.black,PieceType.black,PieceType.free, PieceType.free,PieceType.free,PieceType.free,PieceType.unusable},

            //{ PieceType.white, PieceType.white,PieceType.black,PieceType.free, PieceType.white,PieceType.free,PieceType.free,PieceType.unusable,PieceType.unusable},
            //{ PieceType.white,PieceType.white,PieceType.free, PieceType.free,PieceType.free,PieceType.free,PieceType.unusable,PieceType.unusable,PieceType.unusable},
            //{ PieceType.white,PieceType.free, PieceType.free,PieceType.free,PieceType.free,PieceType.unusable,PieceType.unusable,PieceType.unusable, PieceType.unusable},

            //};
            borad = array;
        }

        /// <summary>
        /// פעולה המאתחלת את מערך הלחיצות
        /// </summary>
        internal void Initialize_Clicked_Array()
        {
            System.Windows.Point[] array = new System.Windows.Point[3];
            previous_clicks = array;
            Update_Score_And_Turns();
        }

        /// <summary>
        /// פעולה המציירת את לוח המשחק 
        /// </summary>
        /// <param name="game_canvas"></param>
        public void Draw_Borad(Canvas game_canvas)
        {
            graphics = new Graphics(game_canvas);
            graphics.Clear_Game_Canvas();
            graphics.Drew_Canvas_Background();
            graphics.Drew_Pieces_From_Array(borad);
        }

        /// <summary>
        /// פעולה המטפלת  באירוע הלחיצה על הלוח 
        /// </summary>
        /// <param name="game_canvas"> קנבס המשחק </param>
        /// <param name="click_point"> נקודת הלחיצה במערך</param>
        public void Canvas_Clicked(Canvas game_canvas, System.Windows.Point click_point)//add else to move makers 
        {
            int x, y;
            graphics = new Graphics(game_canvas);
            Point array_point = new Point();
            Point t_array_point = new Point();
            Point t_array_point2 = new Point();
            Point t_array_point3 = new Point();
            //מציאת מקום הלחיצה במערך 
            array_point = graphics.Pixel_Point_To_Array_Point(click_point);
            //במידה ונלחץ מקום במערך נכנסים 
            if (is_game_ended)
            {
                //restart game
                if (array_point.X != -1 && array_point.Y != -1)
                {
                    x = (int)array_point.X;
                    y = (int)array_point.Y;

                    if (borad[y, x] == PieceType.eatable_white || borad[y, x] == PieceType.eatable_black)
                    {
                        borad[y, x] = PieceType.free;
                        Draw_Borad(game_canvas);
                    }
                }
            }
            else if (array_point.X != -1 && array_point.Y != -1)//gets in if a point on borad was pressed 
            {
                x = (int)array_point.X;
                y = (int)array_point.Y;
                //בחירה במקום ריק
                if (borad[y, x] == PieceType.black || borad[y, x] == PieceType.white || borad[y, x] == PieceType.option || borad[y, x] == PieceType.movable_black || borad[y, x] == PieceType.movable_white || borad[y, x] == PieceType.eatable_black || borad[y, x] == PieceType.eatable_white)
                {
                    switch (num_of_selected_pices)
                    {
                        //first click
                        case 0:
                            previous_clicks[0] = array_point;
                            num_of_selected_pices++;
                            if (borad[y, x] == friendly)
                            {
                                if (friendly == PieceType.white)
                                {
                                    borad[y, x] = PieceType.selected_white;
                                }
                                else
                                {
                                    borad[y, x] = PieceType.selected_black;
                                }
                                Clean_Bot_Selection();
                                Draw_Moves_For_One(game_canvas, array_point);
                            }
                            else
                            {
                                num_of_selected_pices--;
                            }
                            break;

                        //second click
                        case 1:
                            previous_clicks[1] = array_point;
                            num_of_selected_pices++;
                            if (borad[y, x] == PieceType.option)//בחירה באופציה 
                            {
                                borad[y, x] = borad[(int)previous_clicks[0].Y, (int)previous_clicks[0].X];
                                borad[(int)previous_clicks[0].Y, (int)previous_clicks[0].X] = PieceType.free;
                                num_of_selected_pices = 0;
                                Clean_Array();
                                Move_Was_Made(game_canvas);
                            }
                            //בחירה באותו חייל 
                            else if (borad[y, x] == friendly)
                            {
                                t_array_point.Y = previous_clicks[0].Y;
                                t_array_point.X = previous_clicks[0].X;
                                if (Is_Neighbors(array_point, t_array_point))
                                {
                                    borad[y, x] = borad[(int)previous_clicks[0].Y, (int)previous_clicks[0].X];
                                    Clean_Array_Options();
                                    Draw_Moves_For_Two(t_array_point, array_point);
                                    Draw_Shifts_For_Two(t_array_point, array_point, Formation2);
                                    Draw_Eats_For_Two(t_array_point, array_point, Formation2);

                                }
                                else
                                {
                                    num_of_selected_pices--;
                                }
                            }
                            //בחירה בחייל לחוץ
                            else if (borad[y, x] == PieceType.selected_black || borad[y, x] == PieceType.selected_white)
                            {
                                num_of_selected_pices--;
                            }
                            else
                            {
                                num_of_selected_pices--;
                            }
                            break;

                        //thired click
                        case 2:
                            previous_clicks[2] = array_point;
                            num_of_selected_pices++;
                            //בחירה באופציה
                            if (borad[y, x] == PieceType.option)
                            {
                                Make_Move_For_Two(previous_clicks[0], previous_clicks[1], Formation2, array_point);
                                num_of_selected_pices = 0;
                                Clean_Array();
                                Move_Was_Made(game_canvas);
                            }
                            //בחירה בחייל מאותה הקבוצה שלא לחוץ
                            else if (borad[y, x] == friendly)
                            {
                                t_array_point.Y = previous_clicks[0].Y;
                                t_array_point.X = previous_clicks[0].X;

                                t_array_point2.Y = previous_clicks[1].Y;
                                t_array_point2.X = previous_clicks[1].X;

                                if (Is_possible_for_three(t_array_point, t_array_point2, Formation2, array_point))
                                {
                                    //make clicked peice seleced 
                                    borad[y, x] = borad[(int)previous_clicks[0].Y, (int)previous_clicks[0].X];
                                    Clean_Array_Options();
                                    Draw_Moves_For_Three(t_array_point, t_array_point2, Formation2, array_point);
                                    Draw_Shifts_For_Three(t_array_point, t_array_point2, Formation2, array_point);
                                    Draw_Eats_For_Three(t_array_point, t_array_point2, Formation2, array_point);
                                }
                                else
                                {
                                    num_of_selected_pices--;
                                }
                            }
                            //בחירה בחייל שאפשר להזיז
                            else if (borad[y, x] == PieceType.movable_black || borad[y, x] == PieceType.movable_white)
                            {
                                t_array_point.Y = previous_clicks[0].Y;
                                t_array_point.X = previous_clicks[0].X;

                                t_array_point2.Y = previous_clicks[1].Y;
                                t_array_point2.X = previous_clicks[1].X;

                                Make_Movable_Move_For_Two(t_array_point, t_array_point2, Formation2, array_point);
                                Clean_Array();
                                Move_Was_Made(game_canvas);
                                num_of_selected_pices = 0;
                            }
                            //בחירה בחייל שאפשר לאכול
                            else if (borad[y, x] == PieceType.eatable_black || borad[y, x] == PieceType.eatable_white)
                            {
                                t_array_point.Y = previous_clicks[0].Y;
                                t_array_point.X = previous_clicks[0].X;

                                t_array_point2.Y = previous_clicks[1].Y;
                                t_array_point2.X = previous_clicks[1].X;

                                Make_Eat_For_Two(t_array_point, t_array_point2, Formation2, array_point);
                                Clean_Array();
                                Move_Was_Made(game_canvas);
                                num_of_selected_pices = 0;
                            }
                            //בחירה בחייל לחוץ
                            else if (borad[y, x] == PieceType.selected_black || borad[y, x] == PieceType.selected_white)
                            {
                                num_of_selected_pices--;
                            }
                            else
                            {
                                num_of_selected_pices--;
                            }
                            break;

                        //forth click
                        case 3:
                            num_of_selected_pices++;
                            //בחירה באופציה
                            if (borad[y, x] == PieceType.option)
                            {
                                t_array_point.Y = previous_clicks[0].Y;
                                t_array_point.X = previous_clicks[0].X;

                                t_array_point2.Y = previous_clicks[1].Y;
                                t_array_point2.X = previous_clicks[1].X;

                                t_array_point3.Y = previous_clicks[2].Y;
                                t_array_point3.X = previous_clicks[2].X;

                                Make_Moves_For_Three(t_array_point, t_array_point2, t_array_point3, Formation2, array_point);
                                num_of_selected_pices = 0;
                                Clean_Array();
                                Move_Was_Made(game_canvas);
                            }
                            else if (borad[y, x] == PieceType.movable_black || borad[y, x] == PieceType.movable_white)
                            {
                                t_array_point.Y = previous_clicks[0].Y;
                                t_array_point.X = previous_clicks[0].X;

                                t_array_point2.Y = previous_clicks[1].Y;
                                t_array_point2.X = previous_clicks[1].X;

                                t_array_point3.Y = previous_clicks[2].Y;
                                t_array_point3.X = previous_clicks[2].X;
                                Make_Movable_Move_For_Three(t_array_point, t_array_point2, t_array_point3, Formation2, array_point);
                                Clean_Array();
                                Move_Was_Made(game_canvas);
                                num_of_selected_pices = 0;
                            }
                            else if (borad[y, x] == PieceType.eatable_black || borad[y, x] == PieceType.eatable_white)
                            {
                                t_array_point.Y = previous_clicks[0].Y;
                                t_array_point.X = previous_clicks[0].X;

                                t_array_point2.Y = previous_clicks[1].Y;
                                t_array_point2.X = previous_clicks[1].X;

                                t_array_point3.Y = previous_clicks[2].Y;
                                t_array_point3.X = previous_clicks[2].X;
                                Make_Eat_For_Three(t_array_point, t_array_point2, t_array_point3, Formation2, array_point);
                                Clean_Array();
                                Move_Was_Made(game_canvas);
                                num_of_selected_pices = 0;
                            }
                            //בחירה בחייל לא לחוץ
                            else if (borad[y, x] == friendly)
                            {
                                num_of_selected_pices--;
                            }
                            //בחירה בחייל לחוץ
                            else if (borad[y, x] == PieceType.selected_black || borad[y, x] == PieceType.selected_white)
                            {
                                num_of_selected_pices--;
                            }
                            else
                            {
                                num_of_selected_pices--;
                            }
                            break;
                    }
                }
                else
                {
                    Clean_Array();
                    num_of_selected_pices = 0;
                }
                if (is_game_ended == false)
                {
                    Update_Score_And_Turns();
                    Draw_Borad(game_canvas);
                }
                else if (is_game_ended)
                {

                }

            }
        }

        /// <summary>
        /// פעולה המציירת צעדים לאחד 
        /// </summary>
        /// <param name="game_canvas">קנבס המשחק </param>
        /// <param name="clicked_array">נקודת הלחיצה במערך</param>
        public void Draw_Moves_For_One(Canvas game_canvas, Point clicked_array)//this have security checks
        {
            int x, y;
            x = (int)clicked_array.X;
            y = (int)clicked_array.Y;
            if (y != 0)
            {
                if (borad[y - 1, x] == PieceType.free)
                {
                    borad[y - 1, x] = PieceType.option;
                }
            }
            if (y != 0 && x != 8)
            {
                if (borad[y - 1, x + 1] == PieceType.free)
                {
                    borad[y - 1, x + 1] = PieceType.option;
                }
            }
            if (x != 8)
            {
                if (borad[y, x + 1] == PieceType.free)
                {
                    borad[y, x + 1] = PieceType.option;
                }
            }
            if (x != 0)
            {
                if (borad[y, x - 1] == PieceType.free)
                {
                    borad[y, x - 1] = PieceType.option;
                }
            }
            if (y != 8 && x != 0)
            {
                if (borad[y + 1, x - 1] == PieceType.free)
                {
                    borad[y + 1, x - 1] = PieceType.option;
                }
            }
            if (y != 8)
            {
                if (borad[y + 1, x] == PieceType.free)
                {
                    borad[y + 1, x] = PieceType.option;
                }
            }

            Draw_Borad(game_canvas);
        }
        /// <summary>
        /// פעולה המציירת צעדים לשניים 
        /// </summary>
        /// <param name="array_point1"> נקודת לחיצה במערך 1</param>
        /// <param name="array_point2">נקודת לחציה במערך 2 </param>
        public void Draw_Moves_For_Two(Point array_point1, Point array_point2)//this have security checks 
        {
            int x1 = (int)array_point1.X, x2 = (int)array_point2.X, y1 = (int)array_point1.Y, y2 = (int)array_point2.Y;

            if (y1 == y2) //flat formation
            {
                if (x1 > x2)//puts them in order
                {
                    Switch_Points(ref array_point1, ref array_point2);
                    x1 = (int)array_point1.X;
                    x2 = (int)array_point2.X;
                    y1 = (int)array_point1.Y;
                    y2 = (int)array_point2.Y;
                }
                //y1-,x1+,x2+
                if (y1 != 0 && x1 != 8)
                {
                    if (borad[y1 - 1, x1 + 1] == PieceType.free)//updates up
                    {
                        if (borad[y1 - 1, x1] == PieceType.free)//upleft
                        {
                            borad[y1 - 1, x1 + 1] = PieceType.option;
                            borad[y1 - 1, x1] = PieceType.option;
                        }
                        if (x2 != 8)
                        {
                            if (borad[y1 - 1, x2 + 1] == PieceType.free)//upright
                            {
                                borad[y1 - 1, x1 + 1] = PieceType.option;
                                borad[y1 - 1, x2 + 1] = PieceType.option;
                            }
                        }
                    }
                }
                //y1+,x1-
                if (y1 != 8)
                {
                    if (borad[y1 + 1, x1] == PieceType.free)//down
                    {
                        if (x1 != 0)
                        {
                            if (borad[y1 + 1, x1 - 1] == PieceType.free)//downleft
                            {
                                borad[y1 + 1, x1] = PieceType.option;
                                borad[y1 + 1, x1 - 1] = PieceType.option;
                            }
                        }
                        if (borad[y1 + 1, x2] == PieceType.free)//downright
                        {
                            borad[y1 + 1, x1] = PieceType.option;
                            borad[y1 + 1, x2] = PieceType.option;
                        }
                    }
                }
                //x2+
                if (x2 != 8)
                {
                    if (borad[y1, x2 + 1] == PieceType.free)//left
                    {
                        borad[y1, x2 + 1] = PieceType.option;
                    }
                }
                //x1-
                if (x1 != 0)
                {
                    if (borad[y1, x1 - 1] == PieceType.free)//right
                    {
                        borad[y1, x1 - 1] = PieceType.option;
                    }
                }
                Formation2 = TwoPeiceFormationType.flat;
            }
            else
            {
                if (x1 == x2) //down formation
                {
                    if (y1 > y2)//puts tham in order
                    {
                        Switch_Points(ref array_point1, ref array_point2);
                        x1 = (int)array_point1.X;
                        x2 = (int)array_point2.X;
                        y1 = (int)array_point1.Y;
                        y2 = (int)array_point2.Y;
                    }
                    //x1+,y1-x2+
                    if (x1 != 8)
                    {
                        if (borad[y1, x1 + 1] == PieceType.free)//side up
                        {
                            if (y1 != 0)
                            {
                                if (borad[y1 - 1, x1 + 1] == PieceType.free)
                                {
                                    borad[y1, x1 + 1] = PieceType.option;
                                    borad[y1 - 1, x1 + 1] = PieceType.option;
                                }
                            }
                            if (x2 != 8)
                            {
                                if (borad[y2, x2 + 1] == PieceType.free)
                                {
                                    borad[y1, x1 + 1] = PieceType.option;
                                    borad[y2, x2 + 1] = PieceType.option;
                                }
                            }
                        }
                    }
                    //y1+,x1-,x2-
                    if (y1 != 8 && x1 != 0)
                    {
                        if (borad[y1 + 1, x1 - 1] == PieceType.free)//side down
                        {
                            if (borad[y1, x1 - 1] == PieceType.free)
                            {
                                borad[y1 + 1, x1 - 1] = PieceType.option;
                                borad[y1, x1 - 1] = PieceType.option;
                            }
                            if (x2 != 0 && y2 != 8)
                            {
                                if (borad[y2 + 1, x2 - 1] == PieceType.free)
                                {
                                    borad[y1 + 1, x1 - 1] = PieceType.option;
                                    borad[y2 + 1, x2 - 1] = PieceType.option;
                                }
                            }

                        }
                    }
                    //y1-
                    if (y1 != 0)
                    {
                        if (borad[y1 - 1, x1] == PieceType.free)//side down
                        {
                            borad[y1 - 1, x1] = PieceType.option;
                        }
                    }
                    //y2+
                    if (y2 != 8)
                    {
                        if (borad[y2 + 1, x2] == PieceType.free)//side down
                        {
                            borad[y2 + 1, x2] = PieceType.option;
                        }
                    }
                    Formation2 = TwoPeiceFormationType.down;

                }
                else//up formation
                {
                    if (y1 < y2)//puts them in order
                    {
                        Switch_Points(ref array_point1, ref array_point2);
                        x1 = (int)array_point1.X;
                        x2 = (int)array_point2.X;
                        y1 = (int)array_point1.Y;
                        y2 = (int)array_point2.Y;
                    }
                    //x1+,y1+,x2+
                    if (x1 != 8)
                    {
                        if (borad[y1, x1 + 1] == PieceType.free)//side down
                        {
                            if (y1 != 8)
                            {
                                if (borad[y1 + 1, x1] == PieceType.free)
                                {
                                    borad[y1, x1 + 1] = PieceType.option;
                                    borad[y1 + 1, x1] = PieceType.option;
                                }
                            }
                            if (x2 != 8)
                            {
                                if (borad[y2, x2 + 1] == PieceType.free)
                                {
                                    borad[y1, x1 + 1] = PieceType.option;
                                    borad[y2, x2 + 1] = PieceType.option;
                                }
                            }
                        }
                    }
                    //y1-,x1-,y2-
                    if (y1 != 0)
                    {
                        if (borad[y1 - 1, x1] == PieceType.free)//side down
                        {
                            if (x1 != 0)
                            {
                                if (borad[y1, x1 - 1] == PieceType.free)
                                {
                                    borad[y1 - 1, x1] = PieceType.option;
                                    borad[y1, x1 - 1] = PieceType.option;
                                }
                            }
                            if (y2 != 0)
                            {
                                if (borad[y2 - 1, x2] == PieceType.free)
                                {
                                    borad[y1 - 1, x1] = PieceType.option;
                                    borad[y2 - 1, x2] = PieceType.option;
                                }
                            }

                        }
                    }
                    //y1+,x1-
                    if (y1 != 8 && x1 != 0)
                    {
                        if (borad[y1 + 1, x1 - 1] == PieceType.free)//side down
                        {
                            borad[y1 + 1, x1 - 1] = PieceType.option;
                        }
                    }
                    //y2-,x2+
                    if (y2 != 0 && x2 != 8)
                    {
                        if (borad[y2 - 1, x2 + 1] == PieceType.free)//side down
                        {
                            borad[y2 - 1, x2 + 1] = PieceType.option;
                        }
                    }

                    Formation2 = TwoPeiceFormationType.up;
                }
            }
        }

        /// <summary>
        /// פעולה המבצעת צעד לשתיים
        /// </summary>
        /// <param name="point1">נקודת לחציה במערך 1 </param>
        /// <param name="point2">נקודת לחיצה במערך 2</param>
        /// <param name="formation2"> סוג הפורמציה של הלחיצות</param>
        /// <param name="array_point">נקודת לחיצה של אופצייה </param>
        public void Make_Move_For_Two(Point point1, Point point2, TwoPeiceFormationType formation2, Point array_point)//finished
        {
            int x1 = (int)point1.X, x2 = (int)point2.X, y1 = (int)point1.Y, y2 = (int)point2.Y, x3 = (int)array_point.X, y3 = (int)array_point.Y;
            if (formation2 == TwoPeiceFormationType.flat)//flat finished
            {
                if (point1.X > point2.X)
                {
                    Switch_Points(ref point1, ref point2);
                    x1 = (int)point1.X;
                    x2 = (int)point2.X;
                    y1 = (int)point1.Y;
                    y2 = (int)point2.Y;
                }

                //peice 1

                if (y1 == y3 + 1 && x1 == x3)//case 1 of 1
                {
                    borad[y1 - 1, x1] = borad[y1, x1];
                    borad[y1, x1] = PieceType.free;
                    borad[y1 - 1, x1 + 1] = borad[y2, x2];
                    borad[y2, x2] = PieceType.free;
                }

                if (y1 == y3 + 1 && x1 == x3 - 1)//case 2 of 1
                {
                    if (borad[y1 - 1, x1] == PieceType.option)
                    {
                        borad[y1 - 1, x1] = borad[y1, x1];
                        borad[y1, x1] = PieceType.free;
                        borad[y1 - 1, x1 + 1] = borad[y2, x2];
                        borad[y2, x2] = PieceType.free;
                    }
                    else
                    {
                        borad[y1 - 1, x1 + 1] = borad[y1, x1];
                        borad[y1, x1] = PieceType.free;
                        borad[y2 - 1, x2 + 1] = borad[y2, x2];
                        borad[y2, x2] = PieceType.free;
                    }

                }

                if (y1 == y3 - 1 && x1 == x3)//case 4 of 1
                {
                    if (borad[y1 + 1, x1 - 1] == PieceType.option)
                    {
                        borad[y1 + 1, x1 - 1] = borad[y1, x1];
                        borad[y1, x1] = PieceType.free;
                        borad[y1 + 1, x1] = borad[y2, x2];
                        borad[y2, x2] = PieceType.free;
                    }
                    else
                    {
                        borad[y1 + 1, x1] = borad[y1, x1];
                        borad[y1, x1] = PieceType.free;
                        borad[y2 + 1, x2] = borad[y2, x2];
                        borad[y2, x2] = PieceType.free;
                    }
                }

                if (y1 == y3 - 1 && x1 == x3 + 1)//case 5 of 1
                {
                    borad[y1 + 1, x1 - 1] = borad[y1, x1];
                    borad[y1, x1] = PieceType.free;
                    borad[y1 + 1, x1] = borad[y2, x2];
                    borad[y2, x2] = PieceType.free;
                }

                if (y1 == y3 && x1 == x3 + 1)//case 6 of 1
                {
                    borad[y1, x1 - 1] = borad[y2, x2];
                    borad[y2, x2] = PieceType.free;
                }

                //piece 2

                if (y2 == y3 + 1 && x2 == x3 - 1)//case 2 of 2
                {
                    borad[y1 - 1, x1 + 1] = borad[y1, x1];
                    borad[y1, x1] = PieceType.free;
                    borad[y2 - 1, x2 + 1] = borad[y2, x2];
                    borad[y2, x2] = PieceType.free;
                }

                if (y2 == y3 && x2 == x3 - 1)//case 3 of 2
                {
                    borad[y2, x2 + 1] = borad[y1, x1];
                    borad[y1, x1] = PieceType.free;
                }

                if (y2 == y3 - 1 && x2 == x3)//case 4 of 2
                {
                    borad[y1 + 1, x1] = borad[y1, x1];
                    borad[y1, x1] = PieceType.free;
                    borad[y2 + 1, x2] = borad[y2, x2];
                    borad[y2, x2] = PieceType.free;
                }
            }
            else if (formation2 == TwoPeiceFormationType.down)//down finished
            {
                if (point1.Y > point2.Y)
                {
                    Switch_Points(ref point1, ref point2);
                    x1 = (int)point1.X;
                    x2 = (int)point2.X;
                    y1 = (int)point1.Y;
                    y2 = (int)point2.Y;
                }

                //peice 1

                if (y1 == y3 + 1 && x1 == x3)//case 1 of 1 fin
                {
                    borad[y1 - 1, x1] = borad[y2, x2];
                    borad[y2, x2] = PieceType.free;

                    //borad[y1 - 1, x1] = borad[y1, x1];
                    //borad[y1, x1] = PieceType.free;
                    //borad[y1 - 1, x1 + 1] = borad[y2, x2];
                    //borad[y2, x2] = PieceType.free;
                }

                if (y1 == y3 + 1 && x1 == x3 - 1)//case 2 of 1 // fin
                {
                    borad[y1 - 1, x1 + 1] = borad[y1, x1];
                    borad[y1, x1] = PieceType.free;
                    borad[y1, x1 + 1] = borad[y2, x2];
                    borad[y2, x2] = PieceType.free;
                }

                if (y1 == y3 && x1 == x3 - 1)//case 3 of 1 //fin
                {

                    if (borad[y1 - 1, x1 + 1] == PieceType.option)
                    {
                        borad[y1 - 1, x1 + 1] = borad[y1, x1];
                        borad[y1, x1] = PieceType.free;
                        borad[y1, x1 + 1] = borad[y2, x2];
                        borad[y2, x2] = PieceType.free;
                    }
                    else
                    {
                        borad[y1, x1 + 1] = borad[y1, x1];
                        borad[y1, x1] = PieceType.free;
                        borad[y2, x2 + 1] = borad[y2, x2];
                        borad[y2, x2] = PieceType.free;
                    }
                }

                if (y1 == y3 - 1 && x1 == x3 + 1)//case 5 of 1 //fin
                {
                    if (borad[y1, x1 - 1] == PieceType.option)
                    {
                        borad[y1, x1 - 1] = borad[y1, x1];
                        borad[y1, x1] = PieceType.free;
                        borad[y1 + 1, x1 - 1] = borad[y2, x2];
                        borad[y2, x2] = PieceType.free;
                    }
                    else
                    {
                        borad[y1 + 1, x1 - 1] = borad[y1, x1];
                        borad[y1, x1] = PieceType.free;
                        borad[y2 + 1, x2 - 1] = borad[y2, x2];
                        borad[y2, x2] = PieceType.free;
                    }
                }

                if (y1 == y3 && x1 == x3 + 1)//case 6 of 1// fin
                {
                    borad[y1, x1 - 1] = borad[y1, x1];
                    borad[y1, x1] = PieceType.free;
                    borad[y1 + 1, x1 - 1] = borad[y2, x2];
                    borad[y2, x2] = PieceType.free;
                }

                //piece 2

                //if (y2 == y3 + 1 && x2 == x3 - 1)//case 2 of 2
                //{
                //    borad[y1 - 1, x1 + 1] = borad[y1, x1];
                //    borad[y1, x1] = PieceType.free;
                //    borad[y2 - 1, x2 + 1] = borad[y2, x2];
                //    borad[y2, x2] = PieceType.free;
                //}

                if (y2 == y3 && x2 == x3 - 1)//case 3 of 2 // fin
                {
                    borad[y1, x1 + 1] = borad[y1, x1];
                    borad[y1, x1] = PieceType.free;
                    borad[y2, x2 + 1] = borad[y2, x2];
                    borad[y2, x2] = PieceType.free;

                }

                if (y2 == y3 - 1 && x2 == x3)//case 4 of 2 //fin
                {
                    borad[y2 + 1, x2] = borad[y1, x1];
                    borad[y1, x1] = PieceType.free;
                }

                if (y2 == y3 - 1 && x2 == x3 + 1)//case 5 of 2 // fin
                {
                    borad[y1 + 1, x1 - 1] = borad[y1, x1];
                    borad[y1, x1] = PieceType.free;
                    borad[y2 + 1, x2 - 1] = borad[y2, x2];
                    borad[y2, x2] = PieceType.free;
                }





            }
            else if (formation2 == TwoPeiceFormationType.up)//up finished
            {
                if (point1.Y < point2.Y)
                {
                    Switch_Points(ref point1, ref point2);
                    x1 = (int)point1.X;
                    x2 = (int)point2.X;
                    y1 = (int)point1.Y;
                    y2 = (int)point2.Y;
                }
                if (y1 == y3 + 1 && x1 == x3)//case 1 of 1
                {
                    if (borad[y1, x1 - 1] == PieceType.option)
                    {
                        borad[y1, x1 - 1] = borad[y1, x1];
                        borad[y1, x1] = PieceType.free;
                        borad[y1 - 1, x1] = borad[y2, x2];
                        borad[y2, x2] = PieceType.free;
                    }
                    else
                    {
                        borad[y1 - 1, x1] = borad[y1, x1];
                        borad[y1, x1] = PieceType.free;
                        borad[y2 - 1, x2] = borad[y2, x2];
                        borad[y2, x2] = PieceType.free;
                    }
                }
                if (y1 == y3 && x1 == x3 - 1)//case 3 of 1
                {

                    if (borad[y1 + 1, x1] == PieceType.option)
                    {
                        borad[y1 + 1, x1] = borad[y1, x1];
                        borad[y1, x1] = PieceType.free;
                        borad[y1, x1 + 1] = borad[y2, x2];
                        borad[y2, x2] = PieceType.free;
                    }
                    else
                    {
                        borad[y1, x1 + 1] = borad[y1, x1];
                        borad[y1, x1] = PieceType.free;
                        borad[y2, x2 + 1] = borad[y2, x2];
                        borad[y2, x2] = PieceType.free;
                    }
                }
                if (y1 == y3 - 1 && x1 == x3)//case 4 of 1
                {
                    borad[y1 + 1, x1] = borad[y1, x1];
                    borad[y1, x1] = PieceType.free;
                    borad[y1, x1 + 1] = borad[y2, x2];
                    borad[y2, x2] = PieceType.free;
                }
                if (y1 == y3 - 1 && x1 == x3 + 1)//case 5 of 1
                {
                    borad[y1 + 1, x1 - 1] = borad[y2, x2];
                    borad[y2, x2] = PieceType.free;
                }

                if (y1 == y3 && x1 == x3 + 1)//case 6 of 1
                {
                    borad[y1, x1 - 1] = borad[y1, x1];
                    borad[y1, x1] = PieceType.free;
                    borad[y1 - 1, x1] = borad[y2, x2];
                    borad[y2, x2] = PieceType.free;
                }

                //piece 2

                if (y2 == y3 + 1 && x2 == x3)//case 1 of 2
                {
                    borad[y1 - 1, x1] = borad[y1, x1];
                    borad[y1, x1] = PieceType.free;
                    borad[y2 - 1, x2] = borad[y2, x2];
                    borad[y2, x2] = PieceType.free;
                }

                if (y2 == y3 + 1 && x2 == x3 - 1)//case 2 of 2
                {
                    borad[y2 - 1, x2 + 1] = borad[y1, x1];
                    borad[y1, x1] = PieceType.free;
                }

                if (y2 == y3 && x2 == x3 - 1)//case 3 of 2
                {
                    borad[y1, x1 + 1] = borad[y1, x1];
                    borad[y1, x1] = PieceType.free;
                    borad[y2, x2 + 1] = borad[y2, x2];
                    borad[y2, x2] = PieceType.free;
                }


            }
        }

        /// <summary>
        /// פעולה המציירת את הצעדים האפשריים של שלוש חיילים 
        /// </summary>
        /// <param name="point1">נקודה 1</param>
        /// <param name="point2">נקודה 2</param>
        /// <param name="formation">סוג הפורמציה </param>
        /// <param name="array_point"> נקודה 3</param>
        public void Draw_Moves_For_Three(Point point1, Point point2, TwoPeiceFormationType formation, Point array_point)//finished with security checks
        {
            int x1 = (int)point1.X, x2 = (int)point2.X, x3 = (int)array_point.X, y1 = (int)point1.Y, y2 = (int)point2.Y, y3 = (int)array_point.Y;
            //flat
            if (formation == TwoPeiceFormationType.flat)//yes security
            {
                if (x1 > x2)//puts them in order
                {
                    Switch_Points(ref point1, ref point2);
                    x1 = (int)point1.X;
                    x2 = (int)point2.X;
                    y1 = (int)point1.Y;
                    y2 = (int)point2.Y;
                }
                if (x3 < x2)
                {
                    Switch_Points(ref point1, ref array_point);
                    Switch_Points(ref point2, ref array_point);
                    x1 = (int)point1.X;
                    x2 = (int)point2.X;
                    x3 = (int)array_point.X;
                    y1 = (int)point1.Y;
                    y2 = (int)point2.Y;
                    y3 = (int)array_point.Y;
                }
                //orders the point in point1,point2,array_point
                //

                if (y1 != 0 && x1 != 8)
                {
                    if (borad[y1 - 1, x1 + 1] == PieceType.free)//up
                    {
                        if (y2 != 0 && x2 != 8)
                        {
                            if (borad[y2 - 1, x2 + 1] == PieceType.free)
                            {
                                if (y1 != 0)
                                {
                                    if (borad[y1 - 1, x1] == PieceType.free)//upleft
                                    {
                                        borad[y1 - 1, x1] = PieceType.option;
                                        borad[y1 - 1, x1 + 1] = PieceType.option;
                                        borad[y2 - 1, x2 + 1] = PieceType.option;

                                    }
                                }
                                if (y3 != 0 && x3 != 8)
                                {
                                    if (borad[y3 - 1, x3 + 1] == PieceType.free)//upright
                                    {
                                        borad[y3 - 1, x3 + 1] = PieceType.option;
                                        borad[y1 - 1, x1 + 1] = PieceType.option;
                                        borad[y2 - 1, x2 + 1] = PieceType.option;
                                    }
                                }

                            }
                        }
                    }
                }
                //
                if (y1 != 8)
                {
                    if (borad[y1 + 1, x1] == PieceType.free)//down
                    {
                        if (y2 != 8)
                        {
                            if (borad[y2 + 1, x2] == PieceType.free)
                            {
                                if (y1 != 8 && x1 != 0)
                                {
                                    if (borad[y1 + 1, x1 - 1] == PieceType.free)//upleft
                                    {
                                        borad[y1 + 1, x1 - 1] = PieceType.option;
                                        borad[y1 + 1, x1] = PieceType.option;
                                        borad[y2 + 1, x2] = PieceType.option;

                                    }
                                }
                                if (y3 != 8)
                                {
                                    if (borad[y3 + 1, x3] == PieceType.free)//upright
                                    {
                                        borad[y3 + 1, x3] = PieceType.option;
                                        borad[y1 + 1, x1] = PieceType.option;
                                        borad[y2 + 1, x2] = PieceType.option;
                                    }
                                }

                            }
                        }
                    }
                }
                //
                if (x1 != 0)
                {
                    if (borad[y1, x1 - 1] == PieceType.free)//left
                    {
                        borad[y1, x1 - 1] = PieceType.option;
                    }
                }
                //
                if (x3 != 8)
                {
                    if (borad[y3, x3 + 1] == PieceType.free)//right
                    {
                        borad[y3, x3 + 1] = PieceType.option;
                    }
                }
            }
            //down
            if (formation == TwoPeiceFormationType.down)//no security
            {
                if (y1 > y2)//puts tham in order
                {
                    Switch_Points(ref point1, ref point2);
                    x1 = (int)point1.X;
                    x2 = (int)point2.X;
                    y1 = (int)point1.Y;
                    y2 = (int)point2.Y;
                }

                if (y3 < y2)
                {
                    Switch_Points(ref point1, ref array_point);
                    Switch_Points(ref point2, ref array_point);
                    x1 = (int)point1.X;
                    x2 = (int)point2.X;
                    x3 = (int)array_point.X;
                    y1 = (int)point1.Y;
                    y2 = (int)point2.Y;
                    y3 = (int)array_point.Y;
                }
                //orders the point in point1,point2,array_point
                //
                if (x1 != 8)
                {
                    if (borad[y1, x1 + 1] == PieceType.free)//up
                    {
                        if (x2 != 8)
                        {
                            if (borad[y2, x2 + 1] == PieceType.free)
                            {
                                if (y1 != 0 && x1 != 8)
                                {
                                    if (borad[y1 - 1, x1 + 1] == PieceType.free)//upleft
                                    {
                                        borad[y1 - 1, x1 + 1] = PieceType.option;
                                        borad[y1, x1 + 1] = PieceType.option;
                                        borad[y2, x2 + 1] = PieceType.option;

                                    }
                                }
                                if (x3 != 8)
                                {
                                    if (borad[y3, x3 + 1] == PieceType.free)//upright
                                    {
                                        borad[y3, x3 + 1] = PieceType.option;
                                        borad[y1, x1 + 1] = PieceType.option;
                                        borad[y2, x2 + 1] = PieceType.option;
                                    }
                                }

                            }
                        }
                    }
                }
                //
                if (y1 != 8 && x1 != 0)
                {
                    if (borad[y1 + 1, x1 - 1] == PieceType.free)//down
                    {
                        if (y2 != 8 && x2 != 0)
                        {
                            if (borad[y2 + 1, x2 - 1] == PieceType.free)
                            {
                                if (x1 != 0)
                                {
                                    if (borad[y1, x1 - 1] == PieceType.free)//upleft
                                    {
                                        borad[y1 + 1, x1 - 1] = PieceType.option;
                                        borad[y2 + 1, x2 - 1] = PieceType.option;
                                        borad[y1, x1 - 1] = PieceType.option;

                                    }
                                }
                                if (y3 != 8 && x3 != 0)
                                {
                                    if (borad[y3 + 1, x3 - 1] == PieceType.free)//upright
                                    {
                                        borad[y1 + 1, x1 - 1] = PieceType.option;
                                        borad[y2 + 1, x2 - 1] = PieceType.option;
                                        borad[y3 + 1, x3 - 1] = PieceType.option;
                                    }
                                }


                            }
                        }
                    }
                }
                //
                if (y1 != 0)
                {
                    if (borad[y1 - 1, x1] == PieceType.free)//left
                    {
                        borad[y1 - 1, x1] = PieceType.option;
                    }
                }
                //
                if (y3 != 8)
                {
                    if (borad[y3 + 1, x3] == PieceType.free)//right
                    {
                        borad[y3 + 1, x3] = PieceType.option;
                    }
                }
            }
            //up
            if (formation == TwoPeiceFormationType.up)//no security
            {
                if (y1 < y2)//puts them in order
                {
                    Switch_Points(ref point1, ref point2);
                    x1 = (int)point1.X;
                    x2 = (int)point2.X;
                    y1 = (int)point1.Y;
                    y2 = (int)point2.Y;
                }
                if (y3 > y2)
                {
                    Switch_Points(ref point1, ref array_point);
                    Switch_Points(ref point2, ref array_point);
                    x1 = (int)point1.X;
                    x2 = (int)point2.X;
                    x3 = (int)array_point.X;
                    y1 = (int)point1.Y;
                    y2 = (int)point2.Y;
                    y3 = (int)array_point.Y;
                }
                //orders the point in point1,point2,array_point
                //
                if (y1 != 0)
                {
                    if (borad[y1 - 1, x1] == PieceType.free)//up
                    {
                        if (y1 != 0)
                        {
                            if (borad[y2 - 1, x2] == PieceType.free)
                            {
                                if (x1 != 0)
                                {
                                    if (borad[y1, x1 - 1] == PieceType.free)//upleft
                                    {
                                        borad[y1 - 1, x1] = PieceType.option;
                                        borad[y2 - 1, x2] = PieceType.option;
                                        borad[y1, x1 - 1] = PieceType.option;
                                    }
                                }
                                if (y3 != 0)
                                {
                                    if (borad[y3 - 1, x3] == PieceType.free)//upright
                                    {
                                        borad[y1 - 1, x1] = PieceType.option;
                                        borad[y2 - 1, x2] = PieceType.option;
                                        borad[y3 - 1, x3] = PieceType.option;
                                    }
                                }

                            }
                        }
                    }
                }
                //
                if (x1 != 8)
                {
                    if (borad[y1, x1 + 1] == PieceType.free)//down
                    {
                        if (x2 != 8)
                        {
                            if (borad[y2, x2 + 1] == PieceType.free)
                            {
                                if (y1 != 8)
                                {
                                    if (borad[y1 + 1, x1] == PieceType.free)//upleft
                                    {
                                        borad[y1, x1 + 1] = PieceType.option;
                                        borad[y2, x2 + 1] = PieceType.option;
                                        borad[y1 + 1, x1] = PieceType.option;
                                    }
                                }
                                if (x3 != 8)
                                {
                                    if (borad[y3, x3 + 1] == PieceType.free)//upright
                                    {
                                        borad[y1, x1 + 1] = PieceType.option;
                                        borad[y2, x2 + 1] = PieceType.option;
                                        borad[y3, x3 + 1] = PieceType.option;
                                    }
                                }

                            }
                        }
                    }
                }
                //
                if (y1 != 8 && x1 != 0)
                {
                    if (borad[y1 + 1, x1 - 1] == PieceType.free)//left
                    {
                        borad[y1 + 1, x1 - 1] = PieceType.option;
                    }
                }
                //
                if (y3 != 0 && x3 != 8)
                {
                    if (borad[y3 - 1, x3 + 1] == PieceType.free)//right
                    {
                        borad[y3 - 1, x3 + 1] = PieceType.option;
                    }
                }

            }
        }

        /// <summary>
        /// פעולה המבצעת את הצעד הנבחר לשלוש חיילים 
        /// </summary>
        /// <param name="point1">נקודה 1</param>
        /// <param name="point2">נקודה 2</param>
        /// <param name="point3">נקודה 3</param>
        /// <param name="formation">סוג הפורמציה </param>
        /// <param name="array_point">נקודת הלחיצה</param>
        public void Make_Moves_For_Three(Point point1, Point point2, Point point3, TwoPeiceFormationType formation, Point array_point)//finished
        {
            int x1 = (int)point1.X,
                y1 = (int)point1.Y,

                x2 = (int)point2.X,
                y2 = (int)point2.Y,

                x3 = (int)point3.X,
                y3 = (int)point3.Y,

                x4 = (int)array_point.X,
                y4 = (int)array_point.Y;


            //case 1 of 1 (y1 - 1 == y4 && x1 == x4)
            //case 2 of 1 (y1 - 1 == y4 && x1 + 1 == x4)
            //case 3 of 1 (y1 == y4 && x1 + 1 == x4)
            //case 4 of 1 (y1 + 1 == y4 && x1 == x4)
            //case 5 of 1 (y1 + 1 == y4 && x1 - 1 == x4)
            //case 6 of 1 (y1 == y4 && x1 - 1 == x4)

            //case 1 of 2 (y2 - 1 == y4 && x2 == x4)
            //case 2 of 2 (y2 - 1 == y4 && x2 + 1 == x4)
            //case 3 of 2 (y2 == y4 && x2 + 1 == x4)
            //case 4 of 2 (y2 + 1 == y4 && x2 == x4)
            //case 5 of 2 (y2 + 1 == y4 && x2 - 1 == x4)
            //case 6 of 2 (y2 == y4 && x2 - 1 == x4)

            //case 1 of 3 (y3 - 1 == y4 && x3 == x4)
            //case 2 of 3 (y3 - 1 == y4 && x3 + 1 == x4)
            //case 3 of 3 (y3 == y4 && x3 + 1 == x4)
            //case 4 of 3 (y3 + 1 == y4 && x3 == x4)
            //case 5 of 3 (y3 + 1 == y4 && x3 - 1 == x4)
            //case 6 of 3 (y3 == y4 && x3 - 1 == x4)



            if (formation == TwoPeiceFormationType.flat)
            {
                if (x1 > x2)//puts them in order
                {
                    Switch_Points(ref point1, ref point2);
                    x1 = (int)point1.X;
                    x2 = (int)point2.X;
                    y1 = (int)point1.Y;
                    y2 = (int)point2.Y;
                }
                if (x3 < x2)
                {
                    Switch_Points(ref point1, ref point3);
                    Switch_Points(ref point2, ref point3);
                    x1 = (int)point1.X;
                    x2 = (int)point2.X;
                    x3 = (int)point3.X;
                    y1 = (int)point1.Y;
                    y2 = (int)point2.Y;
                    y3 = (int)point3.Y;
                }
                //orders the point in point1,point2,array_point


                //peice 1
                if (y1 - 1 == y4 && x1 == x4)//case 1 of 1
                {
                    borad[y1 - 1, x1] = borad[y1, x1];
                    borad[y1, x1] = PieceType.free;

                    borad[y2 - 1, x2] = borad[y2, x2];
                    borad[y2, x2] = PieceType.free;

                    borad[y3 - 1, x3] = borad[y3, x3];
                    borad[y3, x3] = PieceType.free;
                }
                else if (y1 - 1 == y4 && x1 + 1 == x4)//case 2 of 1
                {
                    if (borad[y1 - 1, x1] == PieceType.option)
                    {
                        borad[y1 - 1, x1] = borad[y1, x1];
                        borad[y1, x1] = PieceType.free;

                        borad[y2 - 1, x2] = borad[y2, x2];
                        borad[y2, x2] = PieceType.free;

                        borad[y3 - 1, x3] = borad[y3, x3];
                        borad[y3, x3] = PieceType.free;
                    }
                    else
                    {
                        borad[y1 - 1, x1 + 1] = borad[y1, x1];
                        borad[y1, x1] = PieceType.free;

                        borad[y2 - 1, x2 + 1] = borad[y2, x2];
                        borad[y2, x2] = PieceType.free;

                        borad[y3 - 1, x3 + 1] = borad[y3, x3];
                        borad[y3, x3] = PieceType.free;
                    }
                }
                else if (y1 + 1 == y4 && x1 == x4)//case 4 of 1
                {
                    if (borad[y1 + 1, x1 - 1] == PieceType.option)
                    {
                        //case 5 of 1
                        borad[y1 + 1, x1 - 1] = borad[y1, x1];
                        borad[y1, x1] = PieceType.free;

                        borad[y2 + 1, x2 - 1] = borad[y2, x2];
                        borad[y2, x2] = PieceType.free;

                        borad[y3 + 1, x3 - 1] = borad[y3, x3];
                        borad[y3, x3] = PieceType.free;
                    }
                    else
                    {
                        //case 4 of 3
                        borad[y1 + 1, x1] = borad[y1, x1];
                        borad[y1, x1] = PieceType.free;

                        borad[y2 + 1, x2] = borad[y2, x2];
                        borad[y2, x2] = PieceType.free;

                        borad[y3 + 1, x3] = borad[y3, x3];
                        borad[y3, x3] = PieceType.free;
                    }
                }
                else if (y1 + 1 == y4 && x1 - 1 == x4)//case 5 of 1
                {
                    borad[y1 + 1, x1 - 1] = borad[y1, x1];
                    borad[y1, x1] = PieceType.free;

                    borad[y2 + 1, x2 - 1] = borad[y2, x2];
                    borad[y2, x2] = PieceType.free;

                    borad[y3 + 1, x3 - 1] = borad[y3, x3];
                    borad[y3, x3] = PieceType.free;
                }
                else if (y1 == y4 && x1 - 1 == x4)//case 6 of 1
                {
                    borad[y1, x1 - 1] = borad[y3, x3];
                    borad[y3, x3] = PieceType.free;
                }


                //peice 2
                else if (y2 - 1 == y4 && x2 + 1 == x4)//case 2 of 2
                {
                    if (borad[y3 - 1, x3 + 1] == PieceType.option)
                    {
                        //case 2 of 3
                        borad[y1 - 1, x1 + 1] = borad[y1, x1];
                        borad[y1, x1] = PieceType.free;

                        borad[y2 - 1, x2 + 1] = borad[y2, x2];
                        borad[y2, x2] = PieceType.free;

                        borad[y3 - 1, x3 + 1] = borad[y3, x3];
                        borad[y3, x3] = PieceType.free;
                    }
                    else
                    {
                        //case 1 of 1
                        borad[y1 - 1, x1] = borad[y1, x1];
                        borad[y1, x1] = PieceType.free;

                        borad[y2 - 1, x2] = borad[y2, x2];
                        borad[y2, x2] = PieceType.free;

                        borad[y3 - 1, x3] = borad[y3, x3];
                        borad[y3, x3] = PieceType.free;
                    }
                }
                else if (y2 + 1 == y4 && x2 == x4)//case 4 of 2
                {
                    if (borad[y3 + 1, x3] == PieceType.option)
                    {
                        //csase 4 of 3
                        borad[y1 + 1, x1] = borad[y1, x1];
                        borad[y1, x1] = PieceType.free;

                        borad[y2 + 1, x2] = borad[y2, x2];
                        borad[y2, x2] = PieceType.free;

                        borad[y3 + 1, x3] = borad[y3, x3];
                        borad[y3, x3] = PieceType.free;
                    }
                    else
                    {
                        //case 5 of 1
                        borad[y1 + 1, x1 - 1] = borad[y1, x1];
                        borad[y1, x1] = PieceType.free;

                        borad[y2 + 1, x2 - 1] = borad[y2, x2];
                        borad[y2, x2] = PieceType.free;

                        borad[y3 + 1, x3 - 1] = borad[y3, x3];
                        borad[y3, x3] = PieceType.free;
                    }
                }



                //peice 3
                else if (y3 - 1 == y4 && x3 + 1 == x4)//case 2 of 3
                {
                    borad[y1 - 1, x1 + 1] = borad[y1, x1];
                    borad[y1, x1] = PieceType.free;

                    borad[y2 - 1, x2 + 1] = borad[y2, x2];
                    borad[y2, x2] = PieceType.free;

                    borad[y3 - 1, x3 + 1] = borad[y3, x3];
                    borad[y3, x3] = PieceType.free;
                }
                else if (y3 == y4 && x3 + 1 == x4)//case 3 of 3
                {
                    borad[y3, x3 + 1] = borad[y1, x1];
                    borad[y1, x1] = PieceType.free;
                }
                else if (y3 + 1 == y4 && x3 == x4)//case 4 of 3
                {
                    borad[y1 + 1, x1] = borad[y1, x1];
                    borad[y1, x1] = PieceType.free;

                    borad[y2 + 1, x2] = borad[y2, x2];
                    borad[y2, x2] = PieceType.free;

                    borad[y3 + 1, x3] = borad[y3, x3];
                    borad[y3, x3] = PieceType.free;
                }
            }

            else if (formation == TwoPeiceFormationType.down)
            {
                if (y1 > y2)//puts tham in order
                {
                    Switch_Points(ref point1, ref point2);
                    x1 = (int)point1.X;
                    x2 = (int)point2.X;
                    y1 = (int)point1.Y;
                    y2 = (int)point2.Y;
                }

                if (y3 < y2)
                {
                    Switch_Points(ref point1, ref point3);
                    Switch_Points(ref point2, ref point3);
                    x1 = (int)point1.X;
                    x2 = (int)point2.X;
                    x3 = (int)point3.X;
                    y1 = (int)point1.Y;
                    y2 = (int)point2.Y;
                    y3 = (int)point3.Y;
                }

                //peice 1
                if (y1 - 1 == y4 && x1 == x4) //case 1 of 1 
                {
                    borad[y1 - 1, x1] = borad[y3, x3];
                    borad[y3, x3] = PieceType.free;
                }
                else if (y1 - 1 == y4 && x1 + 1 == x4)//case 2 of 1
                {
                    borad[y1 - 1, x1 + 1] = borad[y1, x1];
                    borad[y1, x1] = PieceType.free;

                    borad[y2 - 1, x2 + 1] = borad[y2, x2];
                    borad[y2, x2] = PieceType.free;

                    borad[y3 - 1, x3 + 1] = borad[y3, x3];
                    borad[y3, x3] = PieceType.free;
                }
                else if (y1 == y4 && x1 + 1 == x4)//case 3 of 1
                {
                    if (borad[y1 - 1, x1 + 1] == PieceType.option)
                    {
                        //case 2 of 1 
                        borad[y1 - 1, x1 + 1] = borad[y1, x1];
                        borad[y1, x1] = PieceType.free;

                        borad[y2 - 1, x2 + 1] = borad[y2, x2];
                        borad[y2, x2] = PieceType.free;

                        borad[y3 - 1, x3 + 1] = borad[y3, x3];
                        borad[y3, x3] = PieceType.free;
                    }
                    else
                    {
                        //case 3 of 3 
                        borad[y1, x1 + 1] = borad[y1, x1];
                        borad[y1, x1] = PieceType.free;

                        borad[y2, x2 + 1] = borad[y2, x2];
                        borad[y2, x2] = PieceType.free;

                        borad[y3, x3 + 1] = borad[y3, x3];
                        borad[y3, x3] = PieceType.free;
                    }
                }
                else if (y1 + 1 == y4 && x1 - 1 == x4)//case 5 of 1
                {
                    if (borad[y1, x1 - 1] == PieceType.option)
                    {
                        //case 6 of 1 
                        borad[y1, x1 - 1] = borad[y1, x1];
                        borad[y1, x1] = PieceType.free;

                        borad[y2, x2 - 1] = borad[y2, x2];
                        borad[y2, x2] = PieceType.free;

                        borad[y3, x3 - 1] = borad[y3, x3];
                        borad[y3, x3] = PieceType.free;
                    }
                    else
                    {
                        //case 5 of 3 
                        borad[y1 + 1, x1 - 1] = borad[y1, x1];
                        borad[y1, x1] = PieceType.free;

                        borad[y2 + 1, x2 - 1] = borad[y2, x2];
                        borad[y2, x2] = PieceType.free;

                        borad[y3 + 1, x3 - 1] = borad[y3, x3];
                        borad[y3, x3] = PieceType.free;
                    }
                }
                else if (y1 == y4 && x1 - 1 == x4)//case 6 of 1
                {
                    borad[y1, x1 - 1] = borad[y1, x1];
                    borad[y1, x1] = PieceType.free;

                    borad[y2, x2 - 1] = borad[y2, x2];
                    borad[y2, x2] = PieceType.free;

                    borad[y3, x3 - 1] = borad[y3, x3];
                    borad[y3, x3] = PieceType.free;
                }

                //peice 2
                else if (y2 == y4 && x2 + 1 == x4)//case 3 of 2
                {
                    if (borad[y3, x3 + 1] == PieceType.option)
                    {
                        //case 3 of 3
                        borad[y1, x1 + 1] = borad[y1, x1];
                        borad[y1, x1] = PieceType.free;

                        borad[y2, x2 + 1] = borad[y2, x2];
                        borad[y2, x2] = PieceType.free;

                        borad[y3, x3 + 1] = borad[y3, x3];
                        borad[y3, x3] = PieceType.free;
                    }
                    else
                    {
                        //case 2 of 1 
                        borad[y1 - 1, x1 + 1] = borad[y1, x1];
                        borad[y1, x1] = PieceType.free;

                        borad[y2 - 1, x2 + 1] = borad[y2, x2];
                        borad[y2, x2] = PieceType.free;

                        borad[y3 - 1, x3 + 1] = borad[y3, x3];
                        borad[y3, x3] = PieceType.free;
                    }
                }
                else if (y2 + 1 == y4 && x2 - 1 == x4)//case 5 of 2
                {
                    if (borad[y3 + 1, x3 - 1] == PieceType.option)
                    {
                        //case 5 of 3 
                        borad[y1 + 1, x1 - 1] = borad[y1, x1];
                        borad[y1, x1] = PieceType.free;

                        borad[y2 + 1, x2 - 1] = borad[y2, x2];
                        borad[y2, x2] = PieceType.free;

                        borad[y3 + 1, x3 - 1] = borad[y3, x3];
                        borad[y3, x3] = PieceType.free;
                    }
                    else
                    {
                        //case 6 of 1
                        borad[y1, x1 - 1] = borad[y1, x1];
                        borad[y1, x1] = PieceType.free;

                        borad[y2, x2 - 1] = borad[y2, x2];
                        borad[y2, x2] = PieceType.free;

                        borad[y3, x3 - 1] = borad[y3, x3];
                        borad[y3, x3] = PieceType.free;
                    }
                }

                //peice 3
                else if (y3 == y4 && x3 + 1 == x4)//case 3 of 3
                {
                    borad[y1, x1 + 1] = borad[y1, x1];
                    borad[y1, x1] = PieceType.free;

                    borad[y2, x2 + 1] = borad[y2, x2];
                    borad[y2, x2] = PieceType.free;

                    borad[y3, x3 + 1] = borad[y3, x3];
                    borad[y3, x3] = PieceType.free;
                }
                else if (y3 + 1 == y4 && x3 == x4)//case 4 of 3
                {
                    borad[y3 + 1, x3] = borad[y1, x1];
                    borad[y1, x1] = PieceType.free;
                }
                else if (y3 + 1 == y4 && x3 - 1 == x4)//case 5 of 3
                {
                    borad[y1 + 1, x1 - 1] = borad[y1, x1];
                    borad[y1, x1] = PieceType.free;

                    borad[y2 + 1, x2 - 1] = borad[y2, x2];
                    borad[y2, x2] = PieceType.free;

                    borad[y3 + 1, x3 - 1] = borad[y3, x3];
                    borad[y3, x3] = PieceType.free;
                }
            }

            else if (formation == TwoPeiceFormationType.up)
            {
                if (y1 < y2)//puts them in order
                {
                    Switch_Points(ref point1, ref point2);
                    x1 = (int)point1.X;
                    x2 = (int)point2.X;
                    y1 = (int)point1.Y;
                    y2 = (int)point2.Y;
                }
                if (y3 > y2)
                {
                    Switch_Points(ref point1, ref point3);
                    Switch_Points(ref point2, ref point3);
                    x1 = (int)point1.X;
                    x2 = (int)point2.X;
                    x3 = (int)point3.X;
                    y1 = (int)point1.Y;
                    y2 = (int)point2.Y;
                    y3 = (int)point3.Y;
                }
                //orders the point in point1,point2,array_point

                //peice 1
                if (y1 - 1 == y4 && x1 == x4) //case 1 of 1 
                {
                    if (borad[y1, x1 - 1] == PieceType.option)
                    {
                        //case 6 of 1
                        borad[y1, x1 - 1] = borad[y1, x1];
                        borad[y1, x1] = PieceType.free;

                        borad[y2, x2 - 1] = borad[y2, x2];
                        borad[y2, x2] = PieceType.free;

                        borad[y3, x3 - 1] = borad[y3, x3];
                        borad[y3, x3] = PieceType.free;
                    }
                    else
                    {
                        //case 1 of 3 
                        borad[y1 - 1, x1] = borad[y1, x1];
                        borad[y1, x1] = PieceType.free;

                        borad[y2 - 1, x2] = borad[y2, x2];
                        borad[y2, x2] = PieceType.free;

                        borad[y3 - 1, x3] = borad[y3, x3];
                        borad[y3, x3] = PieceType.free;
                    }
                }
                else if (y1 == y4 && x1 + 1 == x4)//case 3 of 1
                {
                    if (borad[y1 + 1, x1] == PieceType.option)
                    {
                        //case 4 of 1
                        borad[y1 + 1, x1] = borad[y1, x1];
                        borad[y1, x1] = PieceType.free;

                        borad[y2 + 1, x2] = borad[y2, x2];
                        borad[y2, x2] = PieceType.free;

                        borad[y3 + 1, x3] = borad[y3, x3];
                        borad[y3, x3] = PieceType.free;
                    }
                    else
                    {
                        //case 3 of 3 
                        borad[y1, x1 + 1] = borad[y1, x1];
                        borad[y1, x1] = PieceType.free;

                        borad[y2, x2 + 1] = borad[y2, x2];
                        borad[y2, x2] = PieceType.free;

                        borad[y3, x3 + 1] = borad[y3, x3];
                        borad[y3, x3] = PieceType.free;
                    }
                }
                else if (y1 + 1 == y4 && x1 == x4)//case 4 of 1
                {
                    borad[y1 + 1, x1] = borad[y1, x1];
                    borad[y1, x1] = PieceType.free;

                    borad[y2 + 1, x2] = borad[y2, x2];
                    borad[y2, x2] = PieceType.free;

                    borad[y3 + 1, x3] = borad[y3, x3];
                    borad[y3, x3] = PieceType.free;
                }
                else if (y1 + 1 == y4 && x1 - 1 == x4)//case 5 of 1
                {
                    borad[y1 + 1, x1 - 1] = borad[y3, x3];
                    borad[y3, x3] = PieceType.free;
                }
                else if (y1 == y4 && x1 - 1 == x4)//case 6 of 1
                {
                    borad[y1, x1 - 1] = borad[y1, x1];
                    borad[y1, x1] = PieceType.free;

                    borad[y2, x2 - 1] = borad[y2, x2];
                    borad[y2, x2] = PieceType.free;

                    borad[y3, x3 - 1] = borad[y3, x3];
                    borad[y3, x3] = PieceType.free;
                }

                //peice 2
                if (y2 - 1 == y4 && x2 == x4) //case 1 of 2 
                {
                    if (borad[y3 - 1, x3] == PieceType.option)
                    {
                        //case 1 of 3 
                        borad[y1 - 1, x1] = borad[y1, x1];
                        borad[y1, x1] = PieceType.free;

                        borad[y2 - 1, x2] = borad[y2, x2];
                        borad[y2, x2] = PieceType.free;

                        borad[y3 - 1, x3] = borad[y3, x3];
                        borad[y3, x3] = PieceType.free;
                    }
                    else
                    {
                        //case 6 of 1
                        borad[y1, x1 - 1] = borad[y1, x1];
                        borad[y1, x1] = PieceType.free;

                        borad[y2, x2 - 1] = borad[y2, x2];
                        borad[y2, x2] = PieceType.free;

                        borad[y3, x3 - 1] = borad[y3, x3];
                        borad[y3, x3] = PieceType.free;
                    }
                }
                else if (y2 == y4 && x2 + 1 == x4)//case 3 of 2
                {
                    if (borad[y3, x3 + 1] == PieceType.option)
                    {
                        //case 3 of 3 
                        borad[y1, x1 + 1] = borad[y1, x1];
                        borad[y1, x1] = PieceType.free;

                        borad[y2, x2 + 1] = borad[y2, x2];
                        borad[y2, x2] = PieceType.free;

                        borad[y3, x3 + 1] = borad[y3, x3];
                        borad[y3, x3] = PieceType.free;
                    }
                    else
                    {
                        //case 4 of 1 
                        borad[y1 + 1, x1] = borad[y1, x1];
                        borad[y1, x1] = PieceType.free;

                        borad[y2 + 1, x2] = borad[y2, x2];
                        borad[y2, x2] = PieceType.free;

                        borad[y3 + 1, x3] = borad[y3, x3];
                        borad[y3, x3] = PieceType.free;
                    }
                }

                //peice 3
                if (y3 - 1 == y4 && x3 == x4) //case 1 of 3 
                {
                    borad[y1 - 1, x1] = borad[y1, x1];
                    borad[y1, x1] = PieceType.free;

                    borad[y2 - 1, x2] = borad[y2, x2];
                    borad[y2, x2] = PieceType.free;

                    borad[y3 - 1, x3] = borad[y3, x3];
                    borad[y3, x3] = PieceType.free;
                }
                else if (y3 - 1 == y4 && x3 + 1 == x4)//case 2 of 3
                {
                    borad[y3 - 1, x3 + 1] = borad[y1, x1];
                    borad[y1, x1] = PieceType.free;
                }
                else if (y3 == y4 && x3 + 1 == x4)//case 3 of 3
                {
                    borad[y1, x1 + 1] = borad[y1, x1];
                    borad[y1, x1] = PieceType.free;

                    borad[y2, x2 + 1] = borad[y2, x2];
                    borad[y2, x2] = PieceType.free;

                    borad[y3, x3 + 1] = borad[y3, x3];
                    borad[y3, x3] = PieceType.free;
                }
            }
        }

        /// <summary>
        /// צייר אפשרויות דחיפה לשני חיילים 
        /// </summary>
        /// <param name="point1">נקודה 1</param>
        /// <param name="point2">נקודה 2</param>
        /// <param name="formation">סוג פורמציה </param>
        public void Draw_Shifts_For_Two(Point point1, Point point2, TwoPeiceFormationType formation)//finished with sequrity
        {
            int x1 = (int)point1.X, x2 = (int)point2.X, y1 = (int)point1.Y, y2 = (int)point2.Y;
            if (formation == TwoPeiceFormationType.flat)//flat finished
            {
                if (point1.X > point2.X)
                {
                    Switch_Points(ref point1, ref point2);
                    x1 = (int)point1.X;
                    x2 = (int)point2.X;
                    y1 = (int)point1.Y;
                    y2 = (int)point2.Y;
                }
                //left
                if (x1 >= 2)
                {
                    if (borad[y1, x1 - 1] != PieceType.option)
                    {
                        if (borad[y1, x1 - 1] == rival)
                        {
                            if (borad[y1, x1 - 2] == PieceType.free)
                            {
                                if (rival == PieceType.black)
                                {
                                    borad[y1, x1 - 1] = PieceType.movable_black;
                                }
                                else
                                {
                                    borad[y1, x1 - 1] = PieceType.movable_white;
                                }
                            }

                        }
                    }
                }
                //right
                if (x2 <= 6)
                {
                    if (borad[y2, x2 + 1] != PieceType.option)
                    {
                        if (borad[y2, x2 + 1] == rival)
                        {
                            if (borad[y2, x2 + 2] == PieceType.free)
                            {
                                if (rival == PieceType.black)
                                {
                                    borad[y2, x2 + 1] = PieceType.movable_black;
                                }
                                else
                                {
                                    borad[y2, x2 + 1] = PieceType.movable_white;
                                }
                            }
                        }
                    }
                }
            }
            else if (formation == TwoPeiceFormationType.down)//down
            {
                if (point1.Y > point2.Y)
                {
                    Switch_Points(ref point1, ref point2);
                    x1 = (int)point1.X;
                    x2 = (int)point2.X;
                    y1 = (int)point1.Y;
                    y2 = (int)point2.Y;
                }
                //left
                if (y1 >= 2)
                {
                    if (borad[y1 - 1, x1] != PieceType.option)
                    {
                        if (borad[y1 - 1, x1] == rival)
                        {
                            if (borad[y1 - 2, x1] == PieceType.free)
                            {
                                if (rival == PieceType.black)
                                {
                                    borad[y1 - 1, x1] = PieceType.movable_black;
                                }
                                else
                                {
                                    borad[y1 - 1, x1] = PieceType.movable_white;
                                }
                            }
                        }
                    }
                }
                //right
                if (y2 <= 6)
                {
                    if (borad[y2 + 1, x2] != PieceType.option)
                    {
                        if (borad[y2 + 1, x2] == rival)
                        {
                            if (borad[y2 + 2, x2] == PieceType.free)
                            {
                                if (rival == PieceType.black)
                                {
                                    borad[y2 + 1, x2] = PieceType.movable_black;
                                }
                                else
                                {
                                    borad[y2 + 1, x2] = PieceType.movable_white;
                                }
                            }
                        }
                    }
                }
            }
            else if (formation == TwoPeiceFormationType.up)//up
            {
                if (point1.Y < point2.Y)
                {
                    Switch_Points(ref point1, ref point2);
                    x1 = (int)point1.X;
                    x2 = (int)point2.X;
                    y1 = (int)point1.Y;
                    y2 = (int)point2.Y;
                }
                //left
                if (y1 <= 6 && x1 >= 2)
                {
                    if (borad[y1 + 1, x1 - 1] != PieceType.option)
                    {
                        if (borad[y1 + 1, x1 - 1] == rival)
                        {
                            if (borad[y1 + 2, x1 - 2] == PieceType.free)
                            {
                                if (rival == PieceType.black)
                                {
                                    borad[y1 + 1, x1 - 1] = PieceType.movable_black;
                                }
                                else
                                {
                                    borad[y1 + 1, x1 - 1] = PieceType.movable_white;
                                }
                            }
                        }
                    }
                }
                //right
                if (y2 >= 2 && x2 <= 6)
                {
                    if (borad[y2 - 1, x2 + 1] != PieceType.option)
                    {
                        if (borad[y2 - 1, x2 + 1] == rival)
                        {
                            if (borad[y2 - 2, x2 + 2] == PieceType.free)
                            {
                                if (rival == PieceType.black)
                                {
                                    borad[y2 - 1, x2 + 1] = PieceType.movable_black;
                                }
                                else
                                {
                                    borad[y2 - 1, x2 + 1] = PieceType.movable_white;
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// בצע דחיפה לשני חיילים
        /// </summary>
        /// <param name="point1">נקודה 1</param>
        /// <param name="point2">נקודה 2</param>
        /// <param name="formation">סוג פורמציה </param>
        /// <param name="array_point">נקודת הלחיצה </param>
        public void Make_Movable_Move_For_Two(Point point1, Point point2, TwoPeiceFormationType formation, Point array_point)//finished
        {
            int x1 = (int)point1.X, x2 = (int)point2.X, y1 = (int)point1.Y, y2 = (int)point2.Y, x3 = (int)array_point.X, y3 = (int)array_point.Y;
            if (formation == TwoPeiceFormationType.flat)//flat finished
            {
                if (point1.X > point2.X)
                {
                    Switch_Points(ref point1, ref point2);
                    x1 = (int)point1.X;
                    x2 = (int)point2.X;
                    y1 = (int)point1.Y;
                    y2 = (int)point2.Y;
                }
                //clicked movable ball is on the left 
                if (y1 == y3 && x1 - 1 == x3)
                {
                    borad[y3, x3 - 1] = borad[y3, x3];
                    borad[y1, x1 - 1] = borad[y1, x1];
                    borad[y2, x2 - 1] = borad[y2, x2];
                    borad[y2, x2] = PieceType.free;
                }
                //clicked movable ball in on the right 
                if (y2 == y3 && x2 + 1 == x3)
                {
                    borad[y3, x3 + 1] = borad[y3, x3];
                    borad[y2, x2 + 1] = borad[y2, x2];
                    borad[y1, x1 + 1] = borad[y1, x1];
                    borad[y1, x1] = PieceType.free;
                }
            }
            else if (formation == TwoPeiceFormationType.down)
            {
                if (point1.Y > point2.Y)
                {
                    Switch_Points(ref point1, ref point2);
                    x1 = (int)point1.X;
                    x2 = (int)point2.X;
                    y1 = (int)point1.Y;
                    y2 = (int)point2.Y;
                }
                //clicked movable ball is on the left 
                if (y1 - 1 == y3 && x1 == x3)
                {
                    borad[y3 - 1, x3] = borad[y3, x3];
                    borad[y1 - 1, x1] = borad[y1, x1];
                    borad[y2 - 1, x2] = borad[y2, x2];
                    borad[y2, x2] = PieceType.free;
                }
                //clicked movable ball in on the right 
                if (y2 + 1 == y3 && x2 == x3)
                {
                    borad[y3 + 1, x3] = borad[y3, x3];
                    borad[y2 + 1, x2] = borad[y2, x2];
                    borad[y1 + 1, x1] = borad[y1, x1];
                    borad[y1, x1] = PieceType.free;
                }
            }
            else if (formation == TwoPeiceFormationType.up)
            {
                if (point1.Y < point2.Y)
                {
                    Switch_Points(ref point1, ref point2);
                    x1 = (int)point1.X;
                    x2 = (int)point2.X;
                    y1 = (int)point1.Y;
                    y2 = (int)point2.Y;
                }
                //clicked movable ball is on the left 
                if (y1 + 1 == y3 && x1 - 1 == x3)
                {
                    borad[y3 + 1, x3 - 1] = borad[y3, x3];
                    borad[y1 + 1, x1 - 1] = borad[y1, x1];
                    borad[y2 + 1, x2 - 1] = borad[y2, x2];
                    borad[y2, x2] = PieceType.free;
                }
                //clicked movable ball in on the right 
                if (y2 - 1 == y3 && x2 + 1 == x3)
                {
                    borad[y3 - 1, x3 + 1] = borad[y3, x3];
                    borad[y2 - 1, x2 + 1] = borad[y2, x2];
                    borad[y1 - 1, x1 + 1] = borad[y1, x1];
                    borad[y1, x1] = PieceType.free;
                }
            }
        }

        /// <summary>
        /// צייר דחיפות לשלוש חיילים 
        /// </summary>
        /// <param name="point1">נקודה 1</param>
        /// <param name="point2">נקודה 2</param>
        /// <param name="formation">סוג פורצמציה </param>
        /// <param name="array_point">נקודה 3</param>
        public void Draw_Shifts_For_Three(Point point1, Point point2, TwoPeiceFormationType formation, Point array_point)//finished with security
        {
            int x1 = (int)point1.X, x2 = (int)point2.X, x3 = (int)array_point.X, y1 = (int)point1.Y, y2 = (int)point2.Y, y3 = (int)array_point.Y;

            if (formation == TwoPeiceFormationType.flat)//
            {
                if (x1 > x2)//puts them in order
                {
                    Switch_Points(ref point1, ref point2);
                    x1 = (int)point1.X;
                    x2 = (int)point2.X;
                    y1 = (int)point1.Y;
                    y2 = (int)point2.Y;
                }
                if (x3 < x2)
                {
                    Switch_Points(ref point1, ref array_point);
                    Switch_Points(ref point2, ref array_point);
                    x1 = (int)point1.X;
                    x2 = (int)point2.X;
                    x3 = (int)array_point.X;
                    y1 = (int)point1.Y;
                    y2 = (int)point2.Y;
                    y3 = (int)array_point.Y;
                }
                //left
                if (x1 >= 2)
                {
                    if (borad[y1, x1 - 1] != PieceType.option)
                    {
                        if (borad[y1, x1 - 1] == rival)
                        {
                            if (borad[y1, x1 - 2] == rival)
                            {
                                if (x1 >= 3)
                                {
                                    if (borad[y1, x1 - 3] == PieceType.free)
                                    {
                                        //אפשר להזיז 2 שמאלה
                                        if (rival == PieceType.black)
                                        {
                                            borad[y1, x1 - 1] = PieceType.movable_black;
                                            borad[y1, x1 - 2] = PieceType.movable_black;
                                        }
                                        else
                                        {
                                            borad[y1, x1 - 1] = PieceType.movable_white;
                                            borad[y1, x1 - 2] = PieceType.movable_white;
                                        }
                                        left_move_num = 2;
                                    }
                                }
                            }
                            else if (borad[y1, x1 - 2] == PieceType.free)
                            {
                                //אפשר להזיז 1 שמאלה 
                                if (rival == PieceType.black)
                                {
                                    borad[y1, x1 - 1] = PieceType.movable_black;
                                }
                                else
                                {
                                    borad[y1, x1 - 1] = PieceType.movable_white;
                                }
                                left_move_num = 1;
                            }
                        }
                    }
                }
                //right
                if (x3 <= 6)
                {
                    if (borad[y3, x3 + 1] != PieceType.option)
                    {
                        if (borad[y3, x3 + 1] == rival)
                        {
                            if (borad[y3, x3 + 2] == rival)
                            {
                                if (x3 <= 5)
                                {
                                    if (borad[y3, x3 + 3] == PieceType.free)
                                    {
                                        //אפשר להזיז 2 שמאלה
                                        if (rival == PieceType.black)
                                        {
                                            borad[y3, x3 + 1] = PieceType.movable_black;
                                            borad[y3, x3 + 2] = PieceType.movable_black;
                                        }
                                        else
                                        {
                                            borad[y3, x3 + 1] = PieceType.movable_white;
                                            borad[y3, x3 + 2] = PieceType.movable_white;
                                        }
                                        right_move_num = 2;
                                    }
                                }
                            }
                            else if (borad[y3, x3 + 2] == PieceType.free)
                            {
                                //אפשר להזיז 1 שמאלה 
                                if (rival == PieceType.black)
                                {
                                    borad[y3, x3 + 1] = PieceType.movable_black;
                                }
                                else
                                {
                                    borad[y3, x3 + 1] = PieceType.movable_white;
                                }
                                right_move_num = 1;
                            }
                        }
                    }
                }

            }
            else if (formation == TwoPeiceFormationType.down)
            {
                if (y1 > y2)//puts tham in order
                {
                    Switch_Points(ref point1, ref point2);
                    x1 = (int)point1.X;
                    x2 = (int)point2.X;
                    y1 = (int)point1.Y;
                    y2 = (int)point2.Y;
                }

                if (y3 < y2)
                {
                    Switch_Points(ref point1, ref array_point);
                    Switch_Points(ref point2, ref array_point);
                    x1 = (int)point1.X;
                    x2 = (int)point2.X;
                    x3 = (int)array_point.X;
                    y1 = (int)point1.Y;
                    y2 = (int)point2.Y;
                    y3 = (int)array_point.Y;
                }
                //left
                if (y1 >= 2)
                {
                    if (borad[y1 - 1, x1] != PieceType.option)
                    {
                        if (borad[y1 - 1, x1] == rival)
                        {
                            if (borad[y1 - 2, x1] == rival)
                            {
                                if (y1 >= 3)
                                {
                                    if (borad[y1 - 3, x1] == PieceType.free)
                                    {
                                        //אפשר להזיז 2 שמאלה
                                        if (rival == PieceType.black)
                                        {
                                            borad[y1 - 1, x1] = PieceType.movable_black;
                                            borad[y1 - 2, x1] = PieceType.movable_black;
                                        }
                                        else
                                        {
                                            borad[y1 - 1, x1] = PieceType.movable_white;
                                            borad[y1 - 2, x1] = PieceType.movable_white;
                                        }
                                        left_move_num = 2;
                                    }
                                }
                            }
                            else if (borad[y1 - 2, x1] == PieceType.free)
                            {
                                //אפשר להזיז 1 שמאלה 
                                if (rival == PieceType.black)
                                {
                                    borad[y1 - 1, x1] = PieceType.movable_black;
                                }
                                else
                                {
                                    borad[y1 - 1, x1] = PieceType.movable_white;
                                }
                                left_move_num = 1;
                            }
                        }
                    }
                }
                //right
                if (y3 <= 6)
                {
                    if (borad[y3 + 1, x3] != PieceType.option)
                    {
                        if (borad[y3 + 1, x3] == rival)
                        {
                            if (borad[y3 + 2, x3] == rival)
                            {
                                if (y3 <= 5)
                                {
                                    if (borad[y3 + 3, x3] == PieceType.free)
                                    {
                                        //אפשר להזיז 2 שמאלה
                                        if (rival == PieceType.black)
                                        {
                                            borad[y3 + 1, x3] = PieceType.movable_black;
                                            borad[y3 + 2, x3] = PieceType.movable_black;
                                        }
                                        else
                                        {
                                            borad[y3 + 1, x3] = PieceType.movable_white;
                                            borad[y3 + 2, x3] = PieceType.movable_white;
                                        }
                                        right_move_num = 2;
                                    }
                                }
                            }
                            else if (borad[y3 + 2, x3] == PieceType.free)
                            {
                                //אפשר להזיז 1 שמאלה 
                                if (rival == PieceType.black)
                                {
                                    borad[y3 + 1, x3] = PieceType.movable_black;
                                }
                                else
                                {
                                    borad[y3 + 1, x3] = PieceType.movable_white;
                                }
                                right_move_num = 1;
                            }
                        }
                    }
                }


            }
            else if (formation == TwoPeiceFormationType.up)
            {
                if (y1 < y2)//puts them in order
                {
                    Switch_Points(ref point1, ref point2);
                    x1 = (int)point1.X;
                    x2 = (int)point2.X;
                    y1 = (int)point1.Y;
                    y2 = (int)point2.Y;
                }
                if (y3 > y2)
                {
                    Switch_Points(ref point1, ref array_point);
                    Switch_Points(ref point2, ref array_point);
                    x1 = (int)point1.X;
                    x2 = (int)point2.X;
                    x3 = (int)array_point.X;
                    y1 = (int)point1.Y;
                    y2 = (int)point2.Y;
                    y3 = (int)array_point.Y;
                }
                //left
                if (y1 <= 6 && x1 >= 2)
                {
                    if (borad[y1 + 1, x1 - 1] != PieceType.option)
                    {
                        if (borad[y1 + 1, x1 - 1] == rival)
                        {
                            if (borad[y1 + 2, x1 - 2] == rival)
                            {
                                if (y1 <= 5 && x1 >= 3)
                                {
                                    if (borad[y1 + 3, x1 - 3] == PieceType.free)
                                    {
                                        //אפשר להזיז 2 שמאלה
                                        if (rival == PieceType.black)
                                        {
                                            borad[y1 + 1, x1 - 1] = PieceType.movable_black;
                                            borad[y1 + 2, x1 - 2] = PieceType.movable_black;
                                        }
                                        else
                                        {
                                            borad[y1 + 1, x1 - 1] = PieceType.movable_white;
                                            borad[y1 + 2, x1 - 2] = PieceType.movable_white;
                                        }
                                        left_move_num = 2;
                                    }
                                }
                            }
                            else if (borad[y1 + 2, x1 - 2] == PieceType.free)
                            {
                                //אפשר להזיז 1 שמאלה 
                                if (rival == PieceType.black)
                                {
                                    borad[y1 + 1, x1 - 1] = PieceType.movable_black;
                                }
                                else
                                {
                                    borad[y1 + 1, x1 - 1] = PieceType.movable_white;
                                }
                                left_move_num = 1;
                            }
                        }
                    }
                }
                //right
                if (y3 >= 2 && x3 <= 6)
                {
                    if (borad[y3 - 1, x3 + 1] != PieceType.option)
                    {
                        if (borad[y3 - 1, x3 + 1] == rival)
                        {
                            if (borad[y3 - 2, x3 + 2] == rival)
                            {
                                if (y3 >= 3 && x3 <= 5)
                                {
                                    if (borad[y3 - 3, x3 + 3] == PieceType.free)
                                    {
                                        //אפשר להזיז 2 שמאלה
                                        if (rival == PieceType.black)
                                        {
                                            borad[y3 - 1, x3 + 1] = PieceType.movable_black;
                                            borad[y3 - 2, x3 + 2] = PieceType.movable_black;
                                        }
                                        else
                                        {
                                            borad[y3 - 1, x3 + 1] = PieceType.movable_white;
                                            borad[y3 - 2, x3 + 2] = PieceType.movable_white;
                                        }
                                        right_move_num = 2;
                                    }
                                }
                            }
                            else if (borad[y3 - 2, x3 + 2] == PieceType.free)
                            {
                                //אפשר להזיז 1 שמאלה 
                                if (rival == PieceType.black)
                                {
                                    borad[y3 - 1, x3 + 1] = PieceType.movable_black;
                                }
                                else
                                {
                                    borad[y3 - 1, x3 + 1] = PieceType.movable_white;
                                }
                                right_move_num = 1;
                            }
                        }
                    }
                }

            }
        }

        /// <summary>
        /// בצע דחיפה לשלוש חיילים 
        /// </summary>
        /// <param name="point1">נקודה 1</param>
        /// <param name="point2">נקודה 2</param>
        /// <param name="point3">נקודה 3</param>
        /// <param name="formation">סוג פורמציה </param>
        /// <param name="array_point">נקודת לחיצה </param>
        public void Make_Movable_Move_For_Three(Point point1, Point point2, Point point3, TwoPeiceFormationType formation, Point array_point)//finished 
        {
            int x1 = (int)point1.X, y1 = (int)point1.Y, x2 = (int)point2.X, y2 = (int)point2.Y, x3 = (int)point3.X, y3 = (int)point3.Y, x4 = (int)array_point.X, y4 = (int)array_point.Y;
            if (formation == TwoPeiceFormationType.flat)
            {
                if (x1 > x2)//puts them in order
                {
                    Switch_Points(ref point1, ref point2);
                    x1 = (int)point1.X;
                    x2 = (int)point2.X;
                    y1 = (int)point1.Y;
                    y2 = (int)point2.Y;
                }
                if (x3 < x2)
                {
                    Switch_Points(ref point1, ref point3);
                    Switch_Points(ref point2, ref point3);
                    x1 = (int)point1.X;
                    x2 = (int)point2.X;
                    x3 = (int)point3.X;
                    y1 = (int)point1.Y;
                    y2 = (int)point2.Y;
                    y3 = (int)point3.Y;
                }
                //orders the point in point1,point2,array_point

                //what side was pressed
                if (x4 < x1)//left was pressed
                {
                    if (left_move_num == 2)
                    {
                        borad[y1, x1 - 3] = borad[y1, x1 - 2];
                        borad[y1, x1 - 2] = borad[y1, x1 - 1];
                        borad[y1, x1 - 1] = borad[y1, x1];
                        borad[y1, x1] = borad[y2, x2];
                        borad[y2, x2] = borad[y3, x3];
                        borad[y3, x3] = PieceType.free;
                    }
                    else if (left_move_num == 1)
                    {
                        borad[y1, x1 - 2] = borad[y1, x1 - 1];
                        borad[y1, x1 - 1] = borad[y1, x1];
                        borad[y1, x1] = borad[y2, x2];
                        borad[y2, x2] = borad[y3, x3];
                        borad[y3, x3] = PieceType.free;
                    }
                }
                else if (x4 > x3)//right was pressed 
                {
                    if (right_move_num == 2)
                    {
                        borad[y3, x3 + 3] = borad[y3, x3 + 2];
                        borad[y3, x3 + 2] = borad[y3, x3 + 1];
                        borad[y3, x3 + 1] = borad[y3, x3];
                        borad[y3, x3] = borad[y2, x2];
                        borad[y2, x2] = borad[y1, x1];
                        borad[y1, x1] = PieceType.free;
                    }
                    else if (right_move_num == 1)
                    {
                        borad[y3, x3 + 2] = borad[y3, x3 + 1];
                        borad[y3, x3 + 1] = borad[y3, x3];
                        borad[y3, x3] = borad[y2, x2];
                        borad[y2, x2] = borad[y1, x1];
                        borad[y1, x1] = PieceType.free;
                    }
                }
            }
            else if (formation == TwoPeiceFormationType.down)
            {
                if (y1 > y2)//puts tham in order
                {
                    Switch_Points(ref point1, ref point2);
                    x1 = (int)point1.X;
                    x2 = (int)point2.X;
                    y1 = (int)point1.Y;
                    y2 = (int)point2.Y;
                }

                if (y3 < y2)
                {
                    Switch_Points(ref point1, ref point3);
                    Switch_Points(ref point2, ref point3);
                    x1 = (int)point1.X;
                    x2 = (int)point2.X;
                    x3 = (int)point3.X;
                    y1 = (int)point1.Y;
                    y2 = (int)point2.Y;
                    y3 = (int)point3.Y;
                }
                //what side was pressed
                if (y4 < y1)//left was pressed
                {
                    if (left_move_num == 2)
                    {
                        borad[y1 - 3, x1] = borad[y1 - 2, x1];
                        borad[y1 - 2, x1] = borad[y1 - 1, x1];
                        borad[y1 - 1, x1] = borad[y1, x1];
                        borad[y1, x1] = borad[y2, x2];
                        borad[y2, x2] = borad[y3, x3];
                        borad[y3, x3] = PieceType.free;
                    }
                    else if (left_move_num == 1)
                    {
                        borad[y1 - 2, x1] = borad[y1 - 1, x1];
                        borad[y1 - 1, x1] = borad[y1, x1];
                        borad[y1, x1] = borad[y2, x2];
                        borad[y2, x2] = borad[y3, x3];
                        borad[y3, x3] = PieceType.free;
                    }
                }
                else if (y4 > y3)//right was pressed 
                {
                    if (right_move_num == 2)
                    {
                        borad[y3 + 3, x3] = borad[y3 + 2, x3];
                        borad[y3 + 2, x3] = borad[y3 + 1, x3];
                        borad[y3 + 1, x3] = borad[y3, x3];
                        borad[y3, x3] = borad[y2, x2];
                        borad[y2, x2] = borad[y1, x1];
                        borad[y1, x1] = PieceType.free;
                    }
                    else if (right_move_num == 1)
                    {
                        borad[y3 + 2, x3] = borad[y3 + 1, x3];
                        borad[y3 + 1, x3] = borad[y3, x3];
                        borad[y3, x3] = borad[y2, x2];
                        borad[y2, x2] = borad[y1, x1];
                        borad[y1, x1] = PieceType.free;
                    }
                }

            }
            else if (formation == TwoPeiceFormationType.up)
            {
                if (y1 < y2)//puts them in order
                {
                    Switch_Points(ref point1, ref point2);
                    x1 = (int)point1.X;
                    x2 = (int)point2.X;
                    y1 = (int)point1.Y;
                    y2 = (int)point2.Y;
                }
                if (y3 > y2)
                {
                    Switch_Points(ref point1, ref point3);
                    Switch_Points(ref point2, ref point3);
                    x1 = (int)point1.X;
                    x2 = (int)point2.X;
                    x3 = (int)point3.X;
                    y1 = (int)point1.Y;
                    y2 = (int)point2.Y;
                    y3 = (int)point3.Y;
                }
                //what side was pressed
                if (y4 > y1 && x4 < x1)//left was pressed
                {
                    if (left_move_num == 2)
                    {
                        borad[y1 + 3, x1 - 3] = borad[y1 + 2, x1 - 2];
                        borad[y1 + 2, x1 - 2] = borad[y1 + 1, x1 - 1];
                        borad[y1 + 1, x1 - 1] = borad[y1, x1];
                        borad[y1, x1] = borad[y2, x2];
                        borad[y2, x2] = borad[y3, x3];
                        borad[y3, x3] = PieceType.free;
                    }
                    else if (left_move_num == 1)
                    {
                        borad[y1 + 2, x1 - 2] = borad[y1 + 1, x1 - 1];
                        borad[y1 + 1, x1 - 1] = borad[y1, x1];
                        borad[y1, x1] = borad[y2, x2];
                        borad[y2, x2] = borad[y3, x3];
                        borad[y3, x3] = PieceType.free;
                    }
                }
                else if (y4 < y3 && x4 > x3)//right was pressed 
                {
                    if (right_move_num == 2)
                    {
                        borad[y3 - 3, x3 + 3] = borad[y3 - 2, x3 + 2];
                        borad[y3 - 2, x3 + 2] = borad[y3 - 1, x3 + 1];
                        borad[y3 - 1, x3 + 1] = borad[y3, x3];
                        borad[y3, x3] = borad[y2, x2];
                        borad[y2, x2] = borad[y1, x1];
                        borad[y1, x1] = PieceType.free;
                    }
                    else if (right_move_num == 1)
                    {
                        borad[y3 - 2, x3 + 2] = borad[y3 - 1, x3 + 1];
                        borad[y3 - 1, x3 + 1] = borad[y3, x3];
                        borad[y3, x3] = borad[y2, x2];
                        borad[y2, x2] = borad[y1, x1];
                        borad[y1, x1] = PieceType.free;
                    }
                }

            }
        }

        /// <summary>
        /// צייר אכילות לשתיים
        /// </summary>
        /// <param name="point1">נקודה 1 </param>
        /// <param name="point2">נקודה 2</param>
        /// <param name="formation">סוג פורמציה </param>
        public void Draw_Eats_For_Two(Point point1, Point point2, TwoPeiceFormationType formation)//finished
        {
            int x1 = (int)point1.X, x2 = (int)point2.X, y1 = (int)point1.Y, y2 = (int)point2.Y;
            if (formation == TwoPeiceFormationType.flat)
            {
                if (point1.X > point2.X)
                {
                    Switch_Points(ref point1, ref point2);
                    x1 = (int)point1.X;
                    x2 = (int)point2.X;
                    y1 = (int)point1.Y;
                    y2 = (int)point2.Y;
                }
                //left
                if (x1 != 0)
                {
                    if (borad[y1, x1 - 1] != PieceType.option)
                    {
                        if (borad[y1, x1 - 1] == rival)
                        {
                            if (graphics.Get_First_Index_Of_Row(y1) == x1 - 1)
                            {
                                if (rival == PieceType.black)
                                {
                                    borad[y1, x1 - 1] = PieceType.eatable_black;
                                }
                                else if (rival == PieceType.white)
                                {
                                    borad[y1, x1 - 1] = PieceType.eatable_white;
                                }

                            }
                        }
                    }
                }
                //right
                if (x2 != 8)
                {
                    if (borad[y2, x2 + 1] != PieceType.option)
                    {
                        if (borad[y2, x2 + 1] == rival)
                        {
                            if (graphics.Get_Last_Index_Of_Row(y2) == x2 + 1)
                            {
                                if (rival == PieceType.black)
                                {
                                    borad[y2, x2 + 1] = PieceType.eatable_black;
                                }
                                else if (rival == PieceType.white)
                                {
                                    borad[y2, x2 + 1] = PieceType.eatable_white;
                                }

                            }
                        }
                    }
                }
            }
            else if (formation == TwoPeiceFormationType.down)//down
            {
                if (point1.Y > point2.Y)
                {
                    Switch_Points(ref point1, ref point2);
                    x1 = (int)point1.X;
                    x2 = (int)point2.X;
                    y1 = (int)point1.Y;
                    y2 = (int)point2.Y;
                }
                //left
                if (y1 != 0)
                {
                    if (borad[y1 - 1, x1] != PieceType.option)
                    {
                        if (borad[y1 - 1, x1] == rival)
                        {
                            if (graphics.Get_First_Index_Of_Row(y1 - 1) == x1 || y1 - 1 == 0)
                            {
                                if (rival == PieceType.black)
                                {
                                    borad[y1 - 1, x1] = PieceType.eatable_black;
                                }
                                else if (rival == PieceType.white)
                                {
                                    borad[y1 - 1, x1] = PieceType.eatable_white;
                                }

                            }
                        }
                    }
                }
                //right
                if (y2 != 8)
                {
                    if (borad[y2 + 1, x2] != PieceType.option)
                    {
                        if (borad[y2 + 1, x2] == rival)
                        {
                            if (graphics.Get_Last_Index_Of_Row(y2 + 1) == x2 || y2 + 1 == 8)
                            {
                                if (rival == PieceType.black)
                                {
                                    borad[y2 + 1, x2] = PieceType.eatable_black;
                                }
                                else if (rival == PieceType.white)
                                {
                                    borad[y2 + 1, x2] = PieceType.eatable_white;
                                }

                            }
                        }
                    }
                }
            }
            else if (formation == TwoPeiceFormationType.up)//up
            {
                if (point1.Y < point2.Y)
                {
                    Switch_Points(ref point1, ref point2);
                    x1 = (int)point1.X;
                    x2 = (int)point2.X;
                    y1 = (int)point1.Y;
                    y2 = (int)point2.Y;
                }
                //left
                if (y1 != 8 && x1 != 0)
                {
                    if (borad[y1 + 1, x1 - 1] != PieceType.option)
                    {
                        if (borad[y1 + 1, x1 - 1] == rival)
                        {
                            if (graphics.Get_First_Index_Of_Row(y1 + 1) == x1 - 1 || y1 + 1 == 8)
                            {
                                if (rival == PieceType.black)
                                {
                                    borad[y1 + 1, x1 - 1] = PieceType.eatable_black;
                                }
                                else if (rival == PieceType.white)
                                {
                                    borad[y1 + 1, x1 - 1] = PieceType.eatable_white;
                                }

                            }
                        }
                    }
                }
                //right
                if (y2 != 0 && x2 != 8)
                {
                    if (borad[y2 - 1, x2 + 1] != PieceType.option)
                    {
                        if (borad[y2 - 1, x2 + 1] == rival)
                        {
                            if (graphics.Get_Last_Index_Of_Row(y2 - 1) == x2 + 1 || y2 - 1 == 0)
                            {
                                if (rival == PieceType.black)
                                {
                                    borad[y2 - 1, x2 + 1] = PieceType.eatable_black;
                                }
                                else if (rival == PieceType.white)
                                {
                                    borad[y2 - 1, x2 + 1] = PieceType.eatable_white;
                                }

                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// בצע אכילה לשתיים
        /// </summary>
        /// <param name="point1">נקודה 1</param>
        /// <param name="point2">נקודה 2</param>
        /// <param name="formation">סוג פורמציה </param>
        /// <param name="array_point">נקודת לחיצה </param>
        public void Make_Eat_For_Two(Point point1, Point point2, TwoPeiceFormationType formation, Point array_point)//finished 
        {
            int x1 = (int)point1.X, x2 = (int)point2.X, y1 = (int)point1.Y, y2 = (int)point2.Y, x3 = (int)array_point.X, y3 = (int)array_point.Y;
            if (formation == TwoPeiceFormationType.flat)//flat finished
            {
                if (point1.X > point2.X)
                {
                    Switch_Points(ref point1, ref point2);
                    x1 = (int)point1.X;
                    x2 = (int)point2.X;
                    y1 = (int)point1.Y;
                    y2 = (int)point2.Y;
                }
                if (x3 < x1) //left was clicked
                {
                    borad[y1, x1 - 1] = borad[y1, x1];
                    borad[y1, x1] = borad[y2, x2];
                    borad[y2, x2] = PieceType.free;
                    Eat_Was_Made(rival);
                }
                if (x3 > x2)
                {
                    borad[y2, x2 + 1] = borad[y2, x2];
                    borad[y2, x2] = borad[y1, x1];
                    borad[y1, x1] = PieceType.free;
                    Eat_Was_Made(rival);
                }
            }
            else if (formation == TwoPeiceFormationType.down)
            {
                if (point1.Y > point2.Y)
                {
                    Switch_Points(ref point1, ref point2);
                    x1 = (int)point1.X;
                    x2 = (int)point2.X;
                    y1 = (int)point1.Y;
                    y2 = (int)point2.Y;
                }
                //clicked movable ball is on the left 
                if (y1 - 1 == y3 && x1 == x3)
                {
                    borad[y3, x3] = borad[y1, x1];
                    borad[y2, x2] = PieceType.free;
                    Eat_Was_Made(rival);
                }
                //clicked movable ball in on the right 
                if (y2 + 1 == y3 && x2 == x3)
                {
                    borad[y3, x3] = borad[y1, x1];
                    borad[y1, x1] = PieceType.free;
                    Eat_Was_Made(rival);
                }
            }
            else if (formation == TwoPeiceFormationType.up)
            {
                if (point1.Y < point2.Y)
                {
                    Switch_Points(ref point1, ref point2);
                    x1 = (int)point1.X;
                    x2 = (int)point2.X;
                    y1 = (int)point1.Y;
                    y2 = (int)point2.Y;
                }
                //clicked movable ball is on the left 
                if (y1 + 1 == y3 && x1 - 1 == x3)
                {
                    borad[y3, x3] = borad[y1, x1];
                    borad[y2, x2] = PieceType.free;
                    Eat_Was_Made(rival);
                }
                //clicked movable ball in on the right 
                if (y2 - 1 == y3 && x2 + 1 == x3)
                {
                    borad[y3, x3] = borad[y2, x2];
                    borad[y1, x1] = PieceType.free;
                    Eat_Was_Made(rival);
                }
            }
        }

        /// <summary>
        /// צייר אכילות לשלוש
        /// </summary>
        /// <param name="point1">נקודה 1</param>
        /// <param name="point2">נקודה 2</param>
        /// <param name="formation">פורמציה </param>
        /// <param name="array_point">נקודה 3</param>
        public void Draw_Eats_For_Three(Point point1, Point point2, TwoPeiceFormationType formation, Point array_point)//functional but mybe more sec is needed
        {
            int x1 = (int)point1.X, x2 = (int)point2.X, x3 = (int)array_point.X, y1 = (int)point1.Y, y2 = (int)point2.Y, y3 = (int)array_point.Y;

            if (formation == TwoPeiceFormationType.flat)//
            {
                if (x1 > x2)//puts them in order
                {
                    Switch_Points(ref point1, ref point2);
                    x1 = (int)point1.X;
                    x2 = (int)point2.X;
                    y1 = (int)point1.Y;
                    y2 = (int)point2.Y;
                }
                if (x3 < x2)
                {
                    Switch_Points(ref point1, ref array_point);
                    Switch_Points(ref point2, ref array_point);
                    x1 = (int)point1.X;
                    x2 = (int)point2.X;
                    x3 = (int)array_point.X;
                    y1 = (int)point1.Y;
                    y2 = (int)point2.Y;
                    y3 = (int)array_point.Y;
                }
                //left
                if (x1 != 0)
                {
                    if (borad[y1, x1 - 1] != PieceType.option)
                    {
                        if (borad[y1, x1 - 1] == rival)
                        {
                            if (graphics.Get_First_Index_Of_Row(y1) == x1 - 1)//only one piece is eatable
                            {
                                if (rival == PieceType.black)
                                {
                                    borad[y1, x1 - 1] = PieceType.eatable_black;
                                }
                                else if (rival == PieceType.white)
                                {
                                    borad[y1, x1 - 1] = PieceType.eatable_white;
                                }
                                left_eat_num = 1;
                            }
                            else if (graphics.Get_First_Index_Of_Row(y1) == x1 - 2)//two piece is eatable
                            {
                                if (rival == PieceType.black)
                                {
                                    borad[y1, x1 - 1] = PieceType.eatable_black;
                                    borad[y1, x1 - 2] = PieceType.eatable_black;
                                }
                                else if (rival == PieceType.white)
                                {
                                    borad[y1, x1 - 1] = PieceType.eatable_white;
                                    borad[y1, x1 - 2] = PieceType.eatable_white;
                                }
                                left_eat_num = 2;
                            }
                        }
                    }
                }
                //right
                if (x3 != 8)
                {
                    if (borad[y3, x3 + 1] != PieceType.option)
                    {
                        if (borad[y3, x3 + 1] == rival)
                        {
                            if (graphics.Get_Last_Index_Of_Row(y3) == x3 + 1)//only one piece is eatable
                            {
                                if (rival == PieceType.black)
                                {
                                    borad[y3, x3 + 1] = PieceType.eatable_black;
                                }
                                else if (rival == PieceType.white)
                                {
                                    borad[y3, x3 + 1] = PieceType.eatable_white;
                                }
                                right_eat_num = 1;
                            }
                            else if (graphics.Get_Last_Index_Of_Row(y3) == x3 + 2)//two piece is eatable
                            {
                                if (rival == PieceType.black)
                                {
                                    borad[y3, x3 + 1] = PieceType.eatable_black;
                                    borad[y3, x3 + 2] = PieceType.eatable_black;
                                }
                                else if (rival == PieceType.white)
                                {
                                    borad[y3, x3 + 1] = PieceType.eatable_white;
                                    borad[y3, x3 + 2] = PieceType.eatable_white;
                                }
                                right_eat_num = 2;
                            }
                        }
                    }
                }
            }
            else if (formation == TwoPeiceFormationType.down)
            {
                if (y1 > y2)//puts tham in order
                {
                    Switch_Points(ref point1, ref point2);
                    x1 = (int)point1.X;
                    x2 = (int)point2.X;
                    y1 = (int)point1.Y;
                    y2 = (int)point2.Y;
                }

                if (y3 < y2)
                {
                    Switch_Points(ref point1, ref array_point);
                    Switch_Points(ref point2, ref array_point);
                    x1 = (int)point1.X;
                    x2 = (int)point2.X;
                    x3 = (int)array_point.X;
                    y1 = (int)point1.Y;
                    y2 = (int)point2.Y;
                    y3 = (int)array_point.Y;
                }

                //left
                if (y1 != 0)
                {
                    if (borad[y1 - 1, x1] != PieceType.option)
                    {
                        if (borad[y1 - 1, x1] == rival)
                        {
                            if (graphics.Get_First_Index_Of_Row(y1 - 1) == x1 || y1 - 1 == 0)//only one piece is eatable
                            {
                                if (rival == PieceType.black)
                                {
                                    borad[y1 - 1, x1] = PieceType.eatable_black;
                                }
                                else if (rival == PieceType.white)
                                {
                                    borad[y1 - 1, x1] = PieceType.eatable_white;
                                }
                                left_eat_num = 1;
                            }
                            else if (graphics.Get_First_Index_Of_Row(y1 - 2) == x1 || y1 - 2 == 0)//two piece is eatable
                            {
                                if (rival == PieceType.black)
                                {
                                    borad[y1 - 1, x1] = PieceType.eatable_black;
                                    borad[y1 - 2, x1] = PieceType.eatable_black;
                                }
                                else if (rival == PieceType.white)
                                {
                                    borad[y1 - 1, x1] = PieceType.eatable_white;
                                    borad[y1 - 2, x1] = PieceType.eatable_white;
                                }
                                left_eat_num = 2;
                            }
                        }
                    }
                }
                //right
                if (y3 != 8)
                {
                    if (borad[y3 + 1, x3] != PieceType.option)
                    {
                        if (borad[y3 + 1, x3] == rival)
                        {
                            if (graphics.Get_Last_Index_Of_Row(y3 + 1) == x3 || y3 + 1 == 8)//only one piece is eatable
                            {
                                if (rival == PieceType.black)
                                {
                                    borad[y3 + 1, x3] = PieceType.eatable_black;
                                }
                                else if (rival == PieceType.white)
                                {
                                    borad[y3 + 1, x3] = PieceType.eatable_white;
                                }
                                right_eat_num = 1;
                            }
                            else if (graphics.Get_Last_Index_Of_Row(y3 + 2) == x3 || y3 + 2 == 8)//two piece is eatable
                            {
                                if (rival == PieceType.black)
                                {
                                    borad[y3 + 1, x3] = PieceType.eatable_black;
                                    borad[y3 + 2, x3] = PieceType.eatable_black;
                                }
                                else if (rival == PieceType.white)
                                {
                                    borad[y3 + 1, x3] = PieceType.eatable_white;
                                    borad[y3 + 2, x3] = PieceType.eatable_white;
                                }
                                right_eat_num = 2;
                            }
                        }
                    }
                }

            }
            else if (formation == TwoPeiceFormationType.up)
            {
                if (y1 < y2)//puts them in order
                {
                    Switch_Points(ref point1, ref point2);
                    x1 = (int)point1.X;
                    x2 = (int)point2.X;
                    y1 = (int)point1.Y;
                    y2 = (int)point2.Y;
                }
                if (y3 > y2)
                {
                    Switch_Points(ref point1, ref array_point);
                    Switch_Points(ref point2, ref array_point);
                    x1 = (int)point1.X;
                    x2 = (int)point2.X;
                    x3 = (int)array_point.X;
                    y1 = (int)point1.Y;
                    y2 = (int)point2.Y;
                    y3 = (int)array_point.Y;
                }

                //left
                if (y1 != 8 && x1 != 0)
                {
                    if (borad[y1 + 1, x1 - 1] != PieceType.option)
                    {
                        if (borad[y1 + 1, x1 - 1] == rival)
                        {
                            if (graphics.Get_First_Index_Of_Row(y1 + 1) == x1 - 1 || y1 + 1 == 8)//only one piece is eatable
                            {
                                if (rival == PieceType.black)
                                {
                                    borad[y1 + 1, x1 - 1] = PieceType.eatable_black;
                                }
                                else if (rival == PieceType.white)
                                {
                                    borad[y1 + 1, x1 - 1] = PieceType.eatable_white;
                                }
                                left_eat_num = 1;
                            }
                            else if (graphics.Get_First_Index_Of_Row(y1 + 2) == x1 - 2 || y1 + 2 == 8)//two piece is eatable
                            {
                                if (rival == PieceType.black)
                                {
                                    borad[y1 + 1, x1 - 1] = PieceType.eatable_black;
                                    borad[y1 + 2, x1 - 2] = PieceType.eatable_black;
                                }
                                else if (rival == PieceType.white)
                                {
                                    borad[y1 + 1, x1 - 1] = PieceType.eatable_white;
                                    borad[y1 + 2, x1 - 2] = PieceType.eatable_white;
                                }
                                left_eat_num = 2;
                            }
                        }
                    }
                }
                //right
                if (y3 != 0 && x3 != 8)
                {
                    if (borad[y3 - 1, x3 + 1] != PieceType.option)
                    {
                        if (borad[y3 - 1, x3 + 1] == rival)
                        {
                            if (graphics.Get_Last_Index_Of_Row(y3 - 1) == x3 + 1 || y3 - 1 == 0)//only one piece is eatable
                            {
                                if (rival == PieceType.black)
                                {
                                    borad[y3 - 1, x3 + 1] = PieceType.eatable_black;
                                }
                                else if (rival == PieceType.white)
                                {
                                    borad[y3 - 1, x3 + 1] = PieceType.eatable_white;
                                }
                                right_eat_num = 1;
                            }
                            else if (graphics.Get_Last_Index_Of_Row(y3 - 2) == x3 + 2 || y3 - 2 == 0)//two piece is eatable
                            {
                                if (rival == PieceType.black)
                                {
                                    borad[y3 - 1, x3 + 1] = PieceType.eatable_black;
                                    borad[y3 - 2, x3 + 2] = PieceType.eatable_black;
                                }
                                else if (rival == PieceType.white)
                                {
                                    borad[y3 - 1, x3 + 1] = PieceType.eatable_white;
                                    borad[y3 - 2, x3 + 2] = PieceType.eatable_white;
                                }
                                right_eat_num = 2;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// בצע אכילה לשלוש 
        /// </summary>
        /// <param name="point1">נקודה 1</param>
        /// <param name="point2">נקודה 2 </param>
        /// <param name="point3">נקודה 3</param>
        /// <param name="formation">סוג פורמציה </param>
        /// <param name="array_point">נקודת לחיצה </param>
        public void Make_Eat_For_Three(Point point1, Point point2, Point point3, TwoPeiceFormationType formation, Point array_point)
        {
            int x1 = (int)point1.X, y1 = (int)point1.Y, x2 = (int)point2.X, y2 = (int)point2.Y, x3 = (int)point3.X, y3 = (int)point3.Y, x4 = (int)array_point.X, y4 = (int)array_point.Y;
            if (formation == TwoPeiceFormationType.flat)
            {
                if (x1 > x2)//puts them in order
                {
                    Switch_Points(ref point1, ref point2);
                    x1 = (int)point1.X;
                    x2 = (int)point2.X;
                    y1 = (int)point1.Y;
                    y2 = (int)point2.Y;
                }
                if (x3 < x2)
                {
                    Switch_Points(ref point1, ref point3);
                    Switch_Points(ref point2, ref point3);
                    x1 = (int)point1.X;
                    x2 = (int)point2.X;
                    x3 = (int)point3.X;
                    y1 = (int)point1.Y;
                    y2 = (int)point2.Y;
                    y3 = (int)point3.Y;
                }
                //orders the point in point1,point2,array_point

                //what side was pressed
                if (x4 < x1)//left was pressed
                {
                    if (left_eat_num == 2)
                    {
                        borad[y1, x1 - 1] = borad[y1, x1];
                        borad[y3, x3] = PieceType.free;
                        if (rival == PieceType.black)
                        {
                            black_balls_ejected++;
                        }
                        else
                        {
                            white_balls_ejected++;
                        }
                    }
                    else if (left_eat_num == 1)
                    {
                        borad[y1, x1 - 1] = borad[y1, x1];
                        borad[y3, x3] = PieceType.free;
                        if (rival == PieceType.black)
                        {
                            black_balls_ejected++;
                        }
                        else
                        {
                            white_balls_ejected++;
                        }
                    }
                }
                else if (x4 > x3)//right was pressed 
                {
                    if (right_eat_num == 2)
                    {
                        borad[y3, x3 + 1] = borad[y3, x3];
                        borad[y1, x1] = PieceType.free;
                        if (rival == PieceType.black)
                        {
                            black_balls_ejected++;
                        }
                        else
                        {
                            white_balls_ejected++;
                        }
                    }
                    else if (right_eat_num == 1)
                    {
                        borad[y3, x3 + 1] = borad[y3, x3];
                        borad[y1, x1] = PieceType.free;
                        if (rival == PieceType.black)
                        {
                            black_balls_ejected++;
                        }
                        else
                        {
                            white_balls_ejected++;
                        }
                    }
                }
            }
            else if (formation == TwoPeiceFormationType.down)
            {
                if (y1 > y2)//puts tham in order
                {
                    Switch_Points(ref point1, ref point2);
                    x1 = (int)point1.X;
                    x2 = (int)point2.X;
                    y1 = (int)point1.Y;
                    y2 = (int)point2.Y;
                }

                if (y3 < y2)
                {
                    Switch_Points(ref point1, ref point3);
                    Switch_Points(ref point2, ref point3);
                    x1 = (int)point1.X;
                    x2 = (int)point2.X;
                    x3 = (int)point3.X;
                    y1 = (int)point1.Y;
                    y2 = (int)point2.Y;
                    y3 = (int)point3.Y;
                }

                //what side was pressed
                if (y4 < y1)//left was pressed
                {
                    borad[y1 - 1, x1] = borad[y1, x1];
                    borad[y3, x3] = PieceType.free;
                    if (rival == PieceType.black)
                    {
                        black_balls_ejected++;
                    }
                    else
                    {
                        white_balls_ejected++;
                    }
                }
                else if (y4 > y3)//right was pressed 
                {
                    borad[y3 + 1, x1] = borad[y3, x3];
                    borad[y1, x1] = PieceType.free;
                    if (rival == PieceType.black)
                    {
                        black_balls_ejected++;
                    }
                    else
                    {
                        white_balls_ejected++;
                    }
                }

            }
            else if (formation == TwoPeiceFormationType.up)
            {
                if (y1 < y2)//puts them in order
                {
                    Switch_Points(ref point1, ref point2);
                    x1 = (int)point1.X;
                    x2 = (int)point2.X;
                    y1 = (int)point1.Y;
                    y2 = (int)point2.Y;
                }
                if (y3 > y2)
                {
                    Switch_Points(ref point1, ref point3);
                    Switch_Points(ref point2, ref point3);
                    x1 = (int)point1.X;
                    x2 = (int)point2.X;
                    x3 = (int)point3.X;
                    y1 = (int)point1.Y;
                    y2 = (int)point2.Y;
                    y3 = (int)point3.Y;
                }
                //what side was pressed
                if (y4 > y1 && x4 < x1)//left was pressed
                {
                    borad[y1 + 1, x1 - 1] = borad[y1, x1];
                    borad[y3, x3] = PieceType.free;
                    if (rival == PieceType.black)
                    {
                        black_balls_ejected++;
                    }
                    else
                    {
                        white_balls_ejected++;
                    }
                }
                else if (y4 < y3 && x4 > x3)//right was pressed 
                {
                    borad[y3 - 1, x3 + 1] = borad[y3, x3];
                    borad[y1, x1] = PieceType.free;
                    if (rival == PieceType.black)
                    {
                        black_balls_ejected++;
                    }
                    else
                    {
                        white_balls_ejected++;
                    }
                }
            }

        }



        #region small_mathods
        /// <summary>
        /// פעולה המחליפה בין נקודות 
        /// </summary>
        /// <param name="array_point1">נקודה 1</param>
        /// <param name="array_point2">נקודה 2</param>
        private void Switch_Points(ref Point array_point1, ref Point array_point2)//finished
        {
            Point t = new Point();
            t = array_point2;
            array_point2 = array_point1;
            array_point1 = t;
        }

        /// <summary>
        /// פעולה המנקה את מערך המשחק מזבל
        /// </summary>
        public void Clean_Array()//finished
        {
            //removs options for now leter also selected
            for (int i = 0; i < Settings.BORAD_ARRAY_SIZE; i++)
            {
                for (int j = 0; j < Settings.BORAD_ARRAY_SIZE; j++)
                {
                    if (borad[i, j] == PieceType.option)
                    {
                        borad[i, j] = PieceType.free;
                    }
                    else if (borad[i, j] == PieceType.selected_black)
                    {
                        borad[i, j] = PieceType.black;
                    }
                    else if (borad[i, j] == PieceType.selected_white)
                    {
                        borad[i, j] = PieceType.white;
                    }
                    else if (borad[i, j] == PieceType.movable_black)
                    {
                        borad[i, j] = PieceType.black;
                    }
                    else if (borad[i, j] == PieceType.movable_white)
                    {
                        borad[i, j] = PieceType.white;
                    }
                    else if (borad[i, j] == PieceType.eatable_black)
                    {
                        borad[i, j] = PieceType.black;
                    }
                    else if (borad[i, j] == PieceType.eatable_white)
                    {
                        borad[i, j] = PieceType.white;
                    }

                }
            }
        }

        /// <summary>
        /// פעולה המנקה את המערך מאופציות 
        /// </summary>
        public void Clean_Array_Options()//finished
        {
            for (int i = 0; i < Settings.BORAD_ARRAY_SIZE; i++)
            {
                for (int j = 0; j < Settings.BORAD_ARRAY_SIZE; j++)
                {
                    if (borad[i, j] == PieceType.option)
                    {
                        borad[i, j] = PieceType.free;
                    }
                    if (borad[i, j] == PieceType.movable_black)
                    {
                        borad[i, j] = PieceType.black;
                    }
                    if (borad[i, j] == PieceType.movable_white)
                    {
                        borad[i, j] = PieceType.white;
                    }
                    if (borad[i, j] == PieceType.eatable_black)
                    {
                        borad[i, j] = PieceType.black;
                    }
                    if (borad[i, j] == PieceType.eatable_white)
                    {
                        borad[i, j] = PieceType.white;
                    }

                }
            }
        }

        /// <summary>
        /// פעולה הבודקת האם שני הנקודות שכנים 
        /// </summary>
        /// <param name="clicked_array1"> נקודה 1</param>
        /// <param name="clicked_array2">נקודה 2</param>
        /// <returns>האם שכנים או לא </returns>
        public bool Is_Neighbors(Point clicked_array1, Point clicked_array2)//finished
        {
            bool condision = false;
            int x1 = (int)clicked_array1.X, x2 = (int)clicked_array2.X, y1 = (int)clicked_array1.Y, y2 = (int)clicked_array2.Y;
            if (y1 == y2)
            {
                if (x1 == (x2 + 1) || x1 == (x2 - 1))
                {
                    condision = true;
                }
            }
            else if (y1 == (y2 - 1))
            {
                if (x1 == x2 || x1 == (x2 + 1))
                {
                    condision = true;
                }
            }
            else if (y1 == (y2 + 1))
            {
                if (x1 == x2 || x1 == (x2 - 1))
                {
                    condision = true;
                }
            }
            return condision;
        }

        /// <summary>
        /// פעולה שבודקת האם שלשה מתאפשרת במיקום זה 
        /// </summary>
        /// <param name="point1">נקודה 1</param>
        /// <param name="point2">נקודה 2</param>
        /// <param name="formation">סוג פורמציה </param>
        /// <param name="array_point">נקודה 3</param>
        /// <returns></returns>
        public bool Is_possible_for_three(Point point1, Point point2, TwoPeiceFormationType formation, Point array_point)//finished 
        {
            int x1 = (int)point1.X, x2 = (int)point2.X, x3 = (int)array_point.X, y1 = (int)point1.Y, y2 = (int)point2.Y, y3 = (int)array_point.Y;
            if (formation == TwoPeiceFormationType.flat)
            {
                if (x1 > x2)//puts them in order
                {
                    Switch_Points(ref point1, ref point2);
                    x1 = (int)point1.X;
                    x2 = (int)point2.X;
                    y1 = (int)point1.Y;
                    y2 = (int)point2.Y;
                }
                if (x3 == x1 - 1 && y3 == y1)
                {
                    return true;
                }
                if (x3 == x2 + 1 && y3 == y2)
                {
                    return true;
                }

            }
            if (formation == TwoPeiceFormationType.down)
            {
                if (y1 > y2)//puts tham in order
                {
                    Switch_Points(ref point1, ref point2);
                    x1 = (int)point1.X;
                    x2 = (int)point2.X;
                    y1 = (int)point1.Y;
                    y2 = (int)point2.Y;
                }
                if (x3 == x1 && y3 == y1 - 1)
                {
                    return true;
                }
                if (x3 == x2 && y3 == y2 + 1)
                {
                    return true;
                }
            }
            if (formation == TwoPeiceFormationType.up)
            {
                if (y1 < y2)//puts them in order
                {
                    Switch_Points(ref point1, ref point2);
                    x1 = (int)point1.X;
                    x2 = (int)point2.X;
                    y1 = (int)point1.Y;
                    y2 = (int)point2.Y;
                }
                if (x3 == x1 - 1 && y3 == y1 + 1)
                {
                    return true;
                }
                if (x3 == x2 + 1 && y3 == y2 - 1)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// פעולה הנקראת כאשר מהלך בוצע
        /// </summary>
        /// <param name="game_canvas">קנבס המשחק</param>
        public void Move_Was_Made(Canvas game_canvas)//unfinished
        {
            if (black_balls_ejected == Settings.NUM_OF_EJECTED_PIECES_TO_WIN || white_balls_ejected == Settings.NUM_OF_EJECTED_PIECES_TO_WIN)
            {
                is_game_ended = true;
            }
            if (is_game_ended)
            {
                Game_Has_Ended(game_canvas);
            }
            else
            {
                if (!Settings.IS_BOT_ENABLED)
                {
                    if (turn)//white
                    {
                        turn = false;
                        friendly = PieceType.black;
                        rival = PieceType.white;
                    }
                    else//blace
                    {
                        turn = true;
                        friendly = PieceType.white;
                        rival = PieceType.black;
                    }
                }
                else if (Settings.IS_BOT_ENABLED)
                {
                    int num = 0;
                    bot.Make_Move(ref borad, Settings.DEFAULT_NUM_OF_PIECES - black_balls_ejected, Settings.DEFAULT_NUM_OF_PIECES - white_balls_ejected);
                    for (int i = 0; i < Settings.BORAD_ARRAY_SIZE; i++)
                    {
                        for (int j = 0; j < Settings.BORAD_ARRAY_SIZE; j++)
                        {
                            if (borad[j, i] == PieceType.white)
                            {
                                num++;
                            }
                        }
                    }
                    white_balls_ejected = Settings.DEFAULT_NUM_OF_PIECES - num;
                    //mybe not work 
                    if (black_balls_ejected == Settings.NUM_OF_EJECTED_PIECES_TO_WIN || white_balls_ejected == Settings.NUM_OF_EJECTED_PIECES_TO_WIN)
                    {
                        is_game_ended = true;
                    }
                    if (is_game_ended)
                    {
                        Game_Has_Ended(game_canvas);
                    }
                }
                left_move_num = 0;
                right_move_num = 0;
                left_eat_num = 0;
                right_eat_num = 0;
            }
        }

        /// <summary>
        /// פעולה הנקראת כאשר אכילה בוצעה 
        /// </summary>
        /// <param name="rivel">סוג היריב </param>
        public void Eat_Was_Made(PieceType rivel)
        {
            if (rival == PieceType.black)
            {
                black_balls_ejected++;
            }
            else if (rival == PieceType.white)
            {
                white_balls_ejected++;
            }
        }

        /// <summary>
        /// פעולה במחזירה את שם התור הנכוחי
        /// </summary>
        /// <returns>פעולה במחזירה את שם התור הנכוחי</returns>
        public string Get_Turn_Name()
        {
            if (turn)
            {
                return "White";
            }
            else
            {
                return "Black";
            }
        }

        /// <summary>
        /// פעולה הנקראת כשהמשחק היסתיים
        /// </summary>
        /// <param name="game_canvas">קנבס המשחק</param>
        public void Game_Has_Ended(Canvas game_canvas)
        {
            if (Settings.NUM_OF_EJECTED_PIECES_TO_WIN == black_balls_ejected)
            {
                for (int i = 0; i < Settings.BORAD_ARRAY_SIZE; i++)
                {
                    for (int j = 0; j < Settings.BORAD_ARRAY_SIZE; j++)
                    {
                        if (borad[i, j] == PieceType.black || borad[i, j] == PieceType.selected_black_bot)
                        {
                            borad[i, j] = PieceType.eatable_black;
                        }
                        if (borad[i, j] == PieceType.white || borad[i, j] == PieceType.selected_white_bot)
                        {
                            borad[i, j] = PieceType.movable_white;
                        }
                    }
                }
                MainWindow.currnt_turn_label.Content = "WINNER IS\n" + "White Player" + "";
                MainWindow.black_ejected_label.Content = "Black:\nLOST";
                MainWindow.white_ejected_label.Content = "White:\nWON";
            }
            else if (Settings.NUM_OF_EJECTED_PIECES_TO_WIN == white_balls_ejected)
            {
                for (int i = 0; i < Settings.BORAD_ARRAY_SIZE; i++)
                {
                    for (int j = 0; j < Settings.BORAD_ARRAY_SIZE; j++)
                    {
                        if (borad[i, j] == PieceType.white || borad[i, j] == PieceType.selected_white_bot)
                        {
                            borad[i, j] = PieceType.eatable_white;
                        }
                        if (borad[i, j] == PieceType.black || borad[i, j] == PieceType.selected_black_bot)
                        {
                            borad[i, j] = PieceType.movable_black;
                        }
                    }
                }
                MainWindow.currnt_turn_label.Content = "WINNER IS\n" + "Black Player" + "";
                MainWindow.black_ejected_label.Content = "Black:\nWON";
                MainWindow.white_ejected_label.Content = "White:\nLOST";
            }
            Draw_Borad(game_canvas);
        }

        /// <summary>
        /// פעולה המעדכן את הניקוד והתורות במסך המשחק 
        /// </summary>
        public void Update_Score_And_Turns()
        {
            MainWindow.currnt_turn_label.Content = "Currnt Player:\n" + Get_Turn_Name() + " Player";
            MainWindow.currnt_turn_label.FontSize = 30;
            MainWindow.black_ejected_label.Content = "Black:\n" + black_balls_ejected + "/6";
            MainWindow.black_ejected_label.FontSize = 30;
            MainWindow.white_ejected_label.Content = "White:\n" + white_balls_ejected + "/6";
            MainWindow.white_ejected_label.FontSize = 30;
        }

        /// <summary>
        /// פעולה המנקה את בחירות הבוט מהלוח
        /// </summary>
        public void Clean_Bot_Selection()
        {
            for (int i = 0; i < Settings.BORAD_ARRAY_SIZE; i++)
            {
                for (int j = 0; j < Settings.BORAD_ARRAY_SIZE; j++)
                {
                    if (borad[i, j] == PieceType.selected_black_bot)
                    {
                        borad[i, j] = PieceType.black;
                    }
                    if (borad[i, j] == PieceType.selected_white_bot)
                    {
                        borad[i, j] = PieceType.white;
                    }
                }
            }
        }
    }
    #endregion
}



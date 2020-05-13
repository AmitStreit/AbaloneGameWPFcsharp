using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
namespace AbaloneGameWPF
{
    /// <summary>
    /// מחקלת גרפיקה 
    /// </summary>
    class Graphics
    {
        public static Canvas Game_Canvas;//קנבס המשחק 
        public static Image WIP_Image;//עצם התמונה שעליו עובדים
        public static PixelData[] Index_To_Pixel;//מערך עזר של טיפוס פיקסלמידע

        /// <summary>
        /// פעולה זאת מאתחלת את מערך העזר 
        /// </summary>
        internal void Initialize_ITP()
        {
            Index_To_Pixel = new PixelData[Settings.BORAD_ARRAY_SIZE];

            Index_To_Pixel[0] = new PixelData(38, 136, 5, 4, 8);
            Index_To_Pixel[1] = new PixelData(95, 103, 6, 3, 8);
            Index_To_Pixel[2] = new PixelData(153, 70, 7, 2, 8);
            Index_To_Pixel[3] = new PixelData(210, 37, 8, 1, 8);
            Index_To_Pixel[4] = new PixelData(267, 4, 9, 0, 8);
            Index_To_Pixel[5] = new PixelData(325, 37, 8, 0, 7);
            Index_To_Pixel[6] = new PixelData(382, 70, 7, 0, 6);
            Index_To_Pixel[7] = new PixelData(439, 103, 6, 0, 5);
            Index_To_Pixel[8] = new PixelData(496, 136, 5, 0, 4);
        }

        /// <summary>
        /// פעולה בונה ריקה
        /// </summary>
        public Graphics()
        {

        }

        /// <summary>
        /// פעולה בונה המקשרת את קנבס המחשק למחלקה 
        /// </summary>
        /// <param name="game_canvas"> קנבס המשחק </param>
        public Graphics(Canvas game_canvas)
        {
            Game_Canvas = game_canvas;
            Initialize_ITP();
        }

        /// <summary>
        /// הפעולה מוסיפה את תמונת הרקע
        /// </summary>
        public void Drew_Canvas_Background()
        {
            WIP_Image = new Image();
            BitmapImage bitImg = new BitmapImage(new Uri("Images/abaloneboard.png", UriKind.RelativeOrAbsolute));
            WIP_Image.Source = bitImg;
            WIP_Image.Width = Settings.CANVAS_SIZE;
            WIP_Image.Height = Settings.CANVAS_SIZE;
            Game_Canvas.Children.Add(WIP_Image);
        }

        /// <summary>
        /// הפעולה מנקה את קנבס המשחק מדברים
        /// </summary>
        public void Clear_Game_Canvas()
        {
            Game_Canvas.Children.Clear();
        }

        /// <summary>
        /// הפעולה מציירת את כול החיילים במקומם המתאים 
        /// </summary>
        /// <param name="arr"> מערך החיילים </param>
        public void Drew_Pieces_From_Array(PieceType[,] arr)
        {
            int linePos = 0;
            for (int i = 0; i < Settings.BORAD_ARRAY_SIZE; i++)
            {
                for (int j = 0; j < Settings.BORAD_ARRAY_SIZE; j++)
                {
                    if (arr[i, j] != PieceType.unusable)
                    {
                        if (arr[i, j] == PieceType.black || arr[i, j] == PieceType.white || arr[i, j] == PieceType.option || arr[i, j] == PieceType.selected_black || arr[i, j] == PieceType.selected_white || arr[i, j] == PieceType.movable_black || arr[i, j] == PieceType.movable_white || arr[i, j] == PieceType.eatable_black || arr[i, j] == PieceType.eatable_white || arr[i, j] == PieceType.selected_black_bot || arr[i, j] == PieceType.selected_white_bot)
                        {
                            Drew_Image_On_Canvas(i, arr[i, j], linePos);
                        }
                        linePos++;
                    }
                }
                linePos = 0;
            }
        }

        /// <summary>
        /// הפעולה מציירת תמונה אחת בלוח 
        /// </summary>
        /// <param name="ArrayY">גובה שורה</param>
        /// <param name="Type">סוג ציור</param>
        /// <param name="Pos_In_Line">מיקום בשורה  </param>
        public void Drew_Image_On_Canvas(int ArrayY, PieceType Type, int Pos_In_Line)
        {
            int top, left;

            top = Index_To_Pixel[ArrayY].top;
            left = Index_To_Pixel[ArrayY].first_left + ((Settings.PIECE_SIZE + 1) * Pos_In_Line);

            WIP_Image = new Image();
            BitmapImage bitImg = new BitmapImage(new Uri("/Images/" + Type + "_ball.png", UriKind.RelativeOrAbsolute));
            WIP_Image.Source = bitImg;

            WIP_Image.Width = Settings.PIECE_SIZE;
            WIP_Image.Height = Settings.PIECE_SIZE;

            Canvas.SetTop(WIP_Image, top);
            Canvas.SetLeft(WIP_Image, left);
            Canvas.SetZIndex(WIP_Image, 10);
            Game_Canvas.Children.Add(WIP_Image);
        }

        /// <summary>
        /// פעולה הממירה את נקודת הלחיצה בפיקסלים לנקודה במערך 
        /// </summary>
        /// <param name="pix_point">טיפוס נקודה המכיל את ויי ואת איקס בפיקלים </param>
        /// <returns>טיפוס נקודה המכיל את הנקודה במערך שנלחצה </returns>
        public Point Pixel_Point_To_Array_Point(Point pix_point)
        {
            bool found = false;
            int indx = 0, y = -1, x = -1;
            System.Windows.Point arr_point = new System.Windows.Point(-1, -1);
            if (pix_point.Y >= 36 && pix_point.Y <= 562)//if not in range no piece was clicked
            {
                while (indx < 9 && found == false)//go to every row and check if it is the row 
                {
                    if (pix_point.Y >= (double)Index_To_Pixel[indx].top && pix_point.Y <= (double)Index_To_Pixel[indx].top + 57)
                    {
                        y = indx;
                        found = true;
                    }
                    indx++;
                }
                if (y != -1)//if y is not in arry no need to try and find x
                {
                    arr_point.Y = y;
                    found = false;
                    indx = Index_To_Pixel[y].start_index;
                    if ((pix_point.X >= (double)Index_To_Pixel[y].first_left) && (pix_point.X <= Index_To_Pixel[y].first_left + ((Index_To_Pixel[y].row_length) * (Settings.PIECE_SIZE + 1))))//checks if the pixel x value can even exsist in this line 
                    {
                        while (indx <= Index_To_Pixel[y].end_index && found == false)
                        {
                            if (pix_point.X >= Index_To_Pixel[y].first_left + ((indx - Index_To_Pixel[y].start_index) * (Settings.PIECE_SIZE + 1)) && pix_point.X <= Index_To_Pixel[y].first_left + ((indx + 1 - Index_To_Pixel[y].start_index) * (Settings.PIECE_SIZE + 1)))//checks if the x pixal value fits in this index
                            {
                                x = indx;
                                found = true;
                            }
                            indx++;
                        }
                        arr_point.X = x;
                    }

                }
            }
            return arr_point;
        }

        /// <summary>
        /// מחזיר את האינדקס הראשון של שורה 
        /// </summary>
        /// <param name="rowy">אינקדס השורה </param>
        /// <returns>מחזיר את האינדקס הראשון של שורה </returns>
        public int Get_First_Index_Of_Row(int rowy)
        {
            return Index_To_Pixel[rowy].start_index;
        }

        /// <summary>
        /// מחזיר את האינדקס האחרון של שורה 
        /// </summary>
        /// <param name="rowy">אינקדס השורה </param>
        /// <returns>מחזיר את האינדקס האחרון של שורה </returns>
        public int Get_Last_Index_Of_Row(int rowy)
        {
            return Index_To_Pixel[rowy].end_index;
        }
    }
}

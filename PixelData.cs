using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaloneGameWPF
{
    /// <summary>
    /// מחלקה זאת מכילה את טיפוס המידע המסייע להצגת החיילים במקום המתאים 
    /// </summary>
    class PixelData
    {

        public int top;//the hight of the row 
        public int first_left;//the place of the first piece
        public int row_length;//the length of the row 
        public int start_index;//the smallest index in the row 
        public int end_index; //the biggest index in the row 

        /// <summary>
        /// פעולה בונה המקבלת את כול הנתונים של המחלקה 
        /// </summary>
        /// <param name="top">גובה השורה </param>
        /// <param name="left">המקום השמאלי הראשון</param>
        /// <param name="length">אורך השורה </param>
        /// <param name="stat">אינדקס ההתחלה </param>
        /// <param name="end">אינדקס הסיום</param>
        public PixelData(int top, int left, int length, int stat, int end)
        {
            this.top = top;
            this.first_left = left;
            this.row_length = length;
            this.start_index = stat;
            this.end_index = end;
        }
    }
}

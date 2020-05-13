using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaloneGameWPF
{
    public enum PieceType
    {
        unusable,//מקום לא שמיש 
        free,//מקום שמיש ללא כול חייל בתוכו 
        black,//חייל שחור
        white,//חייל לבן
        selected_black,//חייל שחור בחור
        selected_white,//חייל לבן בחור
        option,//אופציה 
        movable_black,//חייל שחור בר הזזה
        movable_white,//חייל לבן בר הזזה
        eatable_black,//חייל שחור הניתן לאכילה
        eatable_white,//חייל לבן הניתן לאכילה 
        visited,//מקום בו ביקרה הפעולה 
        selected_black_bot,//חייל שחור שהוזז על ידי הבוט 
        selected_white_bot// חייל לבן שהוזז על ידי הבוט 
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    class InputControl
    {   

        public static bool CheckDigitFormat(TextBox tb, KeyPressEventArgs e)
        {
            bool flag = false;
            if (e.KeyChar != 8 && !Char.IsDigit(e.KeyChar) && e.KeyChar != 46 && e.KeyChar != 45)
            {
                flag = true;

            }
            // 小数点的处理
            if (e.KeyChar == 46)
            {
                if (tb.Text.Length <= 0)
                    flag = true;   //小数点不能在第一位
                else
                {
                    // 判断是否是正确的小数格式
                    float f;
                    float oldf;
                    bool b1 = false, b2 = false;
                    b1 = float.TryParse(tb.Text, out oldf);
                    b2 = float.TryParse(tb.Text + e.KeyChar.ToString(), out f);
                    if (b2 == false)
                    {
                        if (b1 == true)
                            flag = true;
                        else
                            flag = false;
                    }
                }
            }
            return flag;
        }
    }
}

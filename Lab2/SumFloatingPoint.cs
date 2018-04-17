using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2
{
    static class SumFloatingPoint
    {
        public static (string def, string binAnsw, float answ) Add(float a1, float a2)
        {
            uint a1_1 = BitConverter.ToUInt32(BitConverter.GetBytes(a1), 0);
            uint a2_2 = BitConverter.ToUInt32(BitConverter.GetBytes(a2), 0);
            
            uint s1 = a1_1 >> 31;
            uint s2 = a2_2 >> 31;
            uint e1 = (a1_1 >> 23) & 0xFF;
            uint e2 = (a2_2 >> 23) & 0xFF;
            uint m1 = (a1_1 & 0x7FFFFF) + 8388608;
            uint m2 = (a2_2 & 0x7FFFFF) + 8388608;

            string def = $"{a1} + {a2}\n\n";
            def += $"a1 = {Convert.ToString(a1_1, 2)}\n";
            def += $"a2 = {Convert.ToString(a2_2, 2)}\n\n";
            def += $"s1 = {s1} e1 = {Convert.ToString(e1, 2)} m1 = {Convert.ToString(m1, 2)}\n";
            def += $"s2 = {s2} e2 = {Convert.ToString(e2, 2)} m2 = {Convert.ToString(m2, 2)}\n";
            
            if (((e1<<23) + m1) < ((e2<<23) + m2))
            {
                uint temp = a1_1;
                a1_1 = a2_2;
                a2_2 = temp;
                temp = e1;
                e1 = e2;
                e2 = temp;
                temp = s1;
                s1 = s2;
                s2 = temp;
                temp = m1;
                m1 = m2;
                m2 = temp;

                def += "\nswap the values, because Abs(a1) < Abs(a2)\n";
                def += $"s1 = {s1} e1 = {Convert.ToString(e1, 2)} m1 = {Convert.ToString(m1, 2)}\n";
                def += $"s2 = {s2} e2 = {Convert.ToString(e2, 2)} m2 = {Convert.ToString(m2, 2)}\n";
            }
            
            uint e3 = e1;
            
            m2 >>= (int)(e1 - e2);
            def += $"\nRight shift m2 by the exponent difference {e1 - e2}\n";
            def += $"s2 = {s2} e2 = {Convert.ToString(e2, 2)} m2 = {Convert.ToString(m2, 2)}\n";
            
            uint m;

            if (s1 == s2)
                m = m1 + m2;
            else
                m = m1 - m2;

            def += $"\nCompute the {(s1==s2?"sum":"difference")} of the mantissas\n";
            def+=  $"s3 = {s1} e3 = {Convert.ToString(e3, 2)} m3 = {Convert.ToString(m, 2)}\n";
            

            if (m >> 23 != 1)
            {
                int len;
                uint tmp = m;
                for (len = 0; tmp != 0; tmp >>= 1, len++) ;

                e3 += (uint)(len - 24);
                if (len > 24)
                    m >>= (len - 24);
                else
                    m <<= (24 - len);

                def += "\nNormalize the resultant\n";
                def += $"s3 = {s1} e3 = {Convert.ToString(e3, 2)} m3 = {Convert.ToString(m, 2)}\n";
            }

            uint answ = (((s1 << 8) + e3)<<23) + (m& 0x7FFFFF);
            float f_answ = BitConverter.ToSingle(BitConverter.GetBytes(answ), 0);

            return (def, Convert.ToString(answ, 2), f_answ);
        }
    }
}
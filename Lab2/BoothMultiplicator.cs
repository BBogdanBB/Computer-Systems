using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2
{
    public static class BoothMultiplicator
    {
        public static (string explanation, string binAnswer, long answer) Multiply(int a, int b)
        {
            int lenA, lenB, modA = Math.Abs(a), modB = Math.Abs(b);
            for (lenA = 1; modA != 0; modA >>= 1, lenA++) ;
            for (lenB = 1; modB != 0; modB >>= 1, lenB++) ;
            return Multiply(a, b, lenA > lenB ? lenA : lenB);
        }

        static (string explanation, string binAnswer, long answer) Multiply(int a, int b, int len)
        {
            int[] A = new int[len], Q = new int[len], M = new int[len], _M = new int[len];
            int Q_1 = 0;
            for (int i = 0; i < len; Q[i] = b & 1, M[i] = a & 1, _M[i] = M[i] == 0 ? 1 : 0, a >>= 1, b >>= 1, i++) ;

            int[] tmp = new int[len];
            tmp[0] = 1;
            Add(_M, tmp);

            StringBuilder def = new StringBuilder(Print(A, Q, M, _M, Q_1) + "\n");
            for (int i = 0; i < len; i++)
            {
                if (Q[0] == 0 && Q_1 == 1)
                {
                    Add(A, M);
                    def.Append("\n" + Print(A, Q, M, _M, Q_1) + " Add");
                }
                else if (Q[0] == 1 && Q_1 == 0)
                {
                    Add(A, _M);
                    def.Append("\n" + Print(A, Q, M, _M, Q_1) + " Sub");
                }
                Shift(A, Q, ref Q_1);
                def.Append("\n" + Print(A, Q, M, _M, Q_1) + " Shift\n");
            }
            string binAnsw = ToBinStr(A) + ToBinStr(Q), temp = "";
            if (A[A.Length - 1] == 1)
                temp = new string('1', 64 - binAnsw.Length);
            else
                temp = new string('0', 64 - binAnsw.Length);
            temp += binAnsw;
            return (def.ToString(), temp, Convert.ToInt64(temp, 2));
        }

        static void Shift(int[] A, int[] Q, ref int Q_1)
        {
            Q_1 = Q[0];
            Array.Copy(Q, 1, Q, 0, Q.Length - 1);
            Q[Q.Length - 1] = A[0];
            Array.Copy(A, 1, A, 0, A.Length - 1);
            A[A.Length - 1] = A[A.Length - 2];
        }
        static void Add(int[] Num1, int[] Num2)
        {
            int Carry = 0;
            for (int i = 0; i < Num1.Length; i++)
            {
                Num1[i] = Carry + Num1[i] + Num2[i];
                if (Num1[i] == 2)
                {
                    Carry = 1;
                    Num1[i] = 0;
                }
                else if (Num1[i] == 3)
                {
                    Carry = 1;
                    Num1[i] = 1;
                }
                else
                    Carry = 0;
            }
        }
        static string Print(int[] A, int[] Q, int[] M, int[] _M, int Q_1)
        {
            return String.Format($"A = {ToBinStr(A)}   Q = {ToBinStr(Q)}  Q-1 = {Convert.ToInt16(Q_1)}  M = {ToBinStr(M)}   _M = {ToBinStr(_M)}");
        }
        static string ToBinStr(int[] arr)
        {
            StringBuilder str = new StringBuilder();
            for (int i = arr.Length - 1; i >= 0; str.Append(Convert.ToInt32(arr[i])), i--) ;
            return str.ToString();
        }
    }
}

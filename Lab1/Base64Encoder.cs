using System;
using System.Text;
using System.IO;

namespace Lab1
{
    public class Base64Encoder
    {
        static string _alphabet = @"ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";
        public static void Encode(string pathFrom, string pathTo)
        {
            int val;
            using (BinaryReader br = new BinaryReader(File.Open(pathFrom, FileMode.Open), Encoding.UTF8))
            {
                using (StreamWriter sr = new StreamWriter(File.OpenWrite(pathTo)))
                {
                    int mod = (int)(br.BaseStream.Length % 3);
                    for (int i = 0; i < br.BaseStream.Length - mod; i+=3)
                    {
                        val = (((br.ReadByte() << 8) + br.ReadByte())<<8) + br.ReadByte();
                        sr.Write(String.Format($"{_alphabet[(val >> 18) & 0x3F]}{_alphabet[(val >> 12) & 0x3F]}{_alphabet[(val >> 6) & 0x3F]}{_alphabet[val & 0x3F]}"));
                    }
                    if(mod == 2)
                    {
                        val = ((br.ReadByte() << 8) + br.ReadByte())<<2;
                        sr.Write(String.Format($"{_alphabet[(val >> 12) & 0x3F]}{_alphabet[(val >> 6) & 0x3F]}{_alphabet[val & 0x3F]}="));
                    }
                    if(mod == 1)
                    {
                        val = br.ReadByte() << 4;
                        sr.Write(String.Format($"{_alphabet[(val >> 6) & 0x3F]}{_alphabet[val & 0x3F]}=="));
                    }
                }
            }
        }

        public static bool CheckCorrect(string path, string encodePath)
        {
            string str1 = Convert.ToBase64String(File.ReadAllBytes(path));
            string str2 = File.ReadAllText(encodePath);
            return str1.Equals(str2);
        }
    }
}

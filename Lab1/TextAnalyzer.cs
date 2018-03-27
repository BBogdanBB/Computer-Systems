using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Lab1
{
    public class TextAnalyzer
    {
        string _path;
        int _length = 0;
        public double AverageEntropy { get; private set; }
        public double Size { get; private set; }
        public Dictionary<char, double> Frequency { get; private set; }
        public TextAnalyzer(string path)
        {
            _path = path ?? throw new ArgumentNullException("path");
            Frequency = new Dictionary<char, double>();
            Analyze();
        }
        private void Analyze()
        {
            using (StreamReader st = new StreamReader(_path, Encoding.Default))
            {
                char s;
                while (!st.EndOfStream)
                {
                    s = (char)st.Read();
                    if (Frequency.Keys.Contains(s))
                        Frequency[s]++;
                    else
                        Frequency.Add(s, 1);
                    _length++;
                }
            }
            Frequency = Frequency.Select(elem => new KeyValuePair<char, double>(elem.Key, 1.0 * elem.Value / _length))
                .OrderBy(elem => elem.Key)
                .ToDictionary(elem => elem.Key, elem => elem.Value);
            AverageEntropy = -Frequency.Sum(elem => elem.Value * Math.Log(elem.Value, 2));
            Size = Math.Ceiling(AverageEntropy * _length / 8);
        }
    }
}
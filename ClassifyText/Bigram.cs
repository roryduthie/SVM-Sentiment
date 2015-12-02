using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassifyText
{
   public class Bigram
    {
        public static List<String> ngrams(int n, String str)
        {
            List<String> ngrams = new List<String>();
            String[] words = str.Split(' '); 
            for (int i = 0; i < words.Length - n + 1; i++)
                ngrams.Add(concat(words, i, i + n));
            return ngrams;
        }

        public static String concat(String[] words, int start, int end)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = start; i < end; i++)
                sb.Append((i > start ? " " : "") + words[i]);
            return sb.ToString();
        }

        public List<String> getNG(String sentence)
        {
            List<String> vocab = new List<String>();
            for (int n = 1; n <= 3; n++) {
                foreach (String ngram in ngrams(n,sentence))
                {
                   vocab.Add(ngram);
                }
            
            }

            return vocab;
        }
    }
}

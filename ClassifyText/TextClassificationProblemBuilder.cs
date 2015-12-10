using libsvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassifyText
{
    class TextClassificationProblemBuilder
    {
        public svm_problem CreateProblem(IEnumerable<string> x, double[] y, IReadOnlyList<string> vocabulary, int bound, int nb)
        {
            int b = bound;
            int n = nb;
            return new svm_problem
            {
                
                y = y,
                x = x.Select(xVector => CreateNode(xVector, vocabulary, b, n)).ToArray(),
                l = y.Length
            };
        }

        public static svm_node[] CreateNode(string x, IReadOnlyList<string> vocabulary, int bound, int nBound)
        {
            var node = new List<svm_node>(vocabulary.Count);
            int sum = 0;
            List<string> allWords = new List<string>();
            x = x.Replace(",", "");
            Bigram b = new Bigram();

            allWords = b.getNG(x);

            string[] words = allWords.ToArray();

            for (int i = 0; i < vocabulary.Count; i++)
            {
                int occurenceCount = words.Count(s => String.Equals(s, vocabulary[i], StringComparison.OrdinalIgnoreCase));
                if (occurenceCount == 0)
                    continue;

                sum = i + 1;
                if(sum > bound && sum < nBound)
                {
                    occurenceCount = -1;
                    sum = 0;
                }
                if(sum > nBound)
                {
                    occurenceCount = 1;
                }

                node.Add(new svm_node
                {
                    index = i + 1,
                    value = occurenceCount
                });
            }

            return node.ToArray();
        }
    }
}

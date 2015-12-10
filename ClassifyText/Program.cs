using DataAccess;
using libsvm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassifyText
{
    class Program
    {

        private static Dictionary<int, string> _predictionDictionary;

        static void Main()
        {
            const string dataFilePath = @"C:\Users\Rory\Desktop\Han.csv";
            List<String> negwords = new List<String>();
            List<String> poswords = new List<String>();
            String negFile = "C:/Users/Rory/Desktop/negative-words.txt";
            String posFile = "C:/Users/Rory/Desktop/positive-words.txt";
            GetNeg(negwords, negFile);
            GetNeg(poswords, posFile);
           
            String[] stopwords = new String[]{"hon.","gentleman","member","friend","lady","a", "about", "above", "above", "across", "after", "afterwards", "again", "against", "all", "almost", "alone", "along", "already", "also","although","always","am","among", "amongst", "amoungst", "amount",  "an", "and", "another", "any","anyhow","anyone","anything","anyway", "anywhere", "are", "around", "as",  "at", "back","be","became", "because","become","becomes", "becoming", "been", "before", "beforehand", "behind", "being", "below", "beside", "besides", "between", "beyond", "bill", "both", "bottom","but", "by", "call", "can", "cannot", "cant", "co", "con", "could", "couldnt", "cry", "de", "describe", "detail", "do", "done", "down", "due", "during", "each", "eg", "eight", "either", "eleven","else", "elsewhere", "empty", "enough", "etc", "even", "ever", "every", "everyone", "everything", "everywhere", "except", "few", "fifteen", "fify", "fill", "find", "fire", "first", "five", "for", "former", "formerly", "forty", "found", "four", "from", "front", "full", "further", "get", "give", "go", "had", "has", "hasnt", "have", "he", "hence", "her", "here", "hereafter", "hereby", "herein", "hereupon", "hers", "herself", "him", "himself", "his", "how", "however", "hundred", "i","ie", "if", "in", "inc", "indeed", "interest", "into", "is", "it", "its", "itself", "keep", "last", "latter", "latterly", "least", "less", "ltd", "made", "many", "may", "me", "meanwhile", "might", "mill", "mine", "more", "moreover", "most", "mostly", "move", "much", "must", "my", "myself", "name", "namely", "neither", "never", "nevertheless", "next", "nine", "no", "nobody", "none", "noone", "nor", "not", "nothing", "now", "nowhere", "of", "off", "often", "on", "once", "one", "only", "onto", "or", "other", "others", "otherwise", "our", "ours", "ourselves", "out", "over", "own","part", "per", "perhaps", "please", "put", "rather", "re", "same", "see", "seem", "seemed", "seeming", "seems", "serious", "several", "she", "should", "show", "side", "since", "sincere", "six", "sixty", "so", "some", "somehow", "someone", "something", "sometime", "sometimes", "somewhere", "still", "such", "system", "take", "ten", "than", "that", "the", "their", "them", "themselves", "then", "thence", "there", "thereafter", "thereby", "therefore", "therein", "thereupon", "these", "they", "thickv", "thin", "third", "this", "those", "though", "three", "through", "throughout", "thru", "thus", "to", "together", "too", "top", "toward", "towards", "twelve", "twenty", "two", "un", "under", "until", "up", "upon", "us", "very", "via", "was", "we", "well", "were", "what", "whatever", "when", "whence", "whenever", "where", "whereafter", "whereas", "whereby", "wherein", "whereupon", "wherever", "whether", "which", "while", "whither", "who", "whoever", "whole", "whom", "whose", "why", "will", "with", "within", "without", "would", "yet", "you", "your", "yours", "yourself", "yourselves", "the"};
            List<String> stop = stopwords.ToList<String>();
            var dataTable = DataTable.New.ReadCsv(dataFilePath);
            List<string> x = dataTable.Rows.Select(row => row["Text"]).ToList();
            double[] y = dataTable.Rows.Select(row => double.Parse(row["IsPos"])).ToArray();

            //var vocab = x.SelectMany(GetWords).Distinct().OrderBy(word => word).ToList();

            Bigram b = new Bigram();
            List<String> v = new List<string>();

            String sent = "";

            for(int i = 0; i < x.Count; i++)
            {
                String c = x[i].ToString();
                
                c = c.Replace(",", "");
                c = c.ToLower();
                String[] sp = c.Split(' ');
                for (int z = 0; z < sp.Length; z++)
                {
                    String word = sp[z];
                    if(stop.Contains(word))
                    {
                        Debug.WriteLine("Stop Word");
                    }
                    else
                    {
                        sent += word;
                        sent += " ";
                    }
                }

                sent = sent.Trim();
                
                v.AddRange(b.getNG(sent));
                

                sent = "";
            }
            int bound = v.Count();
          //v.AddRange(negwords);
            int nBound = v.Count();
          //v.AddRange(poswords);
            //v = v.Distinct().ToList();
            var problemBuilder = new TextClassificationProblemBuilder();
            var problem = problemBuilder.CreateProblem(x, y, v, bound, nBound);

            ProblemHelper.WriteProblem(@"C:\Users\Rory\Desktop\hanData.problem", problem);

            problem = ProblemHelper.ReadProblem(@"C:\Users\Rory\Desktop\hanData.problem");

            const int C = 1;
            var model = new C_SVC(problem, KernelHelper.RadialBasisFunctionKernel(1), C);

            var accuracy = model.GetCrossValidationAccuracy(10);
            Console.WriteLine("Accuracy of the model is {0:P}", accuracy); 

            string userInput;
            _predictionDictionary = new Dictionary<int, string> { { -1, "Negative" }, { 1, "Positive" } };

            do
            {
                userInput = Console.ReadLine();
                var newX = TextClassificationProblemBuilder.CreateNode(userInput, v, bound, nBound);
                var predictedY = model.Predict(newX);

                Console.WriteLine("The prediction is {0}", _predictionDictionary[(int)predictedY]);
                Console.WriteLine(new string('=', 50));

            } while (userInput != "quit");
                                                                              
                                                                              

        }

        private static IEnumerable<string> GetWords(string x)
        {
            return x.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
        }

        public static List<String> GetNeg(List<String> words, String file)
        {
            using(StreamReader r = new StreamReader(file))
            {
                String line;

                while((line = r.ReadLine()) != null)
                {
                    words.Add(line);
                }

            }

            return words;
        }
    }
}

using DataAccess;
using libsvm;
using System;
using System.Collections.Generic;
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
            const string dataFilePath = @"C:\Users\roryduthie\Desktop\Han.csv";

            var dataTable = DataTable.New.ReadCsv(dataFilePath);
            List<string> x = dataTable.Rows.Select(row => row["Text"]).ToList();
            double[] y = dataTable.Rows.Select(row => double.Parse(row["IsPos"])).ToArray();

            //var vocab = x.SelectMany(GetWords).Distinct().OrderBy(word => word).ToList();

            Bigram b = new Bigram();
            List<String> v = new List<string>();

            for(int i = 0; i < x.Count; i++)
            {
                String c = x[i].ToString();
                c = c.Replace(",", "");
                v.AddRange(b.getNG(c));
            }

            var problemBuilder = new TextClassificationProblemBuilder();
            var problem = problemBuilder.CreateProblem(x, y, v);

            ProblemHelper.WriteProblem(@"C:\Users\roryduthie\Desktop\hanData.problem", problem);

            problem = ProblemHelper.ReadProblem(@"C:\Users\roryduthie\Desktop\hanData.problem");

            const int C = 1;
            var model = new C_SVC(problem, KernelHelper.LinearKernel(), C);

            var accuracy = model.GetCrossValidationAccuracy(10);
            Console.WriteLine("Accuracy of the model is {0:P}", accuracy); 

            string userInput;
            _predictionDictionary = new Dictionary<int, string> { { -1, "Negative" }, { 1, "Positive" } };

            do
            {
                userInput = Console.ReadLine();
                var newX = TextClassificationProblemBuilder.CreateNode(userInput, v);
                var predictedY = model.Predict(newX);

                Console.WriteLine("The prediction is {0}", _predictionDictionary[(int)predictedY]);
                Console.WriteLine(new string('=', 50));

            } while (userInput != "quit");
                                                                              
                                                                              

        }

        private static IEnumerable<string> GetWords(string x)
        {
            return x.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}

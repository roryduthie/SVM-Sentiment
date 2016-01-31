using DataAccess;
using libsvm;
using Newtonsoft.Json;
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
        private static List<string> rText;
        private static int[] sID;
        private static int[] tID;

        private static List<String> nText;
        private static int[] nsID;
        private static int[] ntID;

        private static List<string> name;
        private static int[] pID;
        

        
        
        private static List<string> sentC;
        private static List<Sentence> ns = new List<Sentence>();
        private static List<Sent> nss = new List<Sent>();
        private static List<Sentence> ps = new List<Sentence>();
        private static List<Person> pl = new List<Person>();
        private static List<Relationship> rl = new List<Relationship>();
        private static List<String> h79N = new List<String>();
        private static List<String> h79P = new List<String>();

        private static List<String> h83N = new List<String>();
        private static List<String> h83P = new List<String>();

        private static List<String> h87N = new List<String>();
        private static List<String> h87P = new List<String>();


        static void Main()
        {
            const string dataFilePath = @"C:\Users\Rory\Desktop\Han.csv";
            List<String> negwords = new List<String>();
            List<String> poswords = new List<String>();
            sentC = new List<String>();
            //String negFile = "C:/Users/Rory/Desktop/negative-words.txt";
            //String posFile = "C:/Users/Rory/Desktop/positive-words.txt";
            //GetNeg(negwords, negFile);
            //GetNeg(poswords, posFile);
           
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
          // int bound = v.Count();
          //v.AddRange(negwords);
          //  int nBound = v.Count();
          //v.AddRange(poswords);
          //  v = v.Distinct().ToList();
            var problemBuilder = new TextClassificationProblemBuilder();
            var problem = problemBuilder.CreateProblem(x, y, v);

            ProblemHelper.WriteProblem(@"C:\Users\Rory\Desktop\hanData.problem", problem);

            problem = ProblemHelper.ReadProblem(@"C:\Users\Rory\Desktop\hanData.problem");

            const int C = 1;
            var model = new C_SVC(problem, KernelHelper.LinearKernel(), C);

            var accuracy = model.GetCrossValidationAccuracy(10);
            Console.WriteLine("Accuracy of the model is {0:P}", accuracy); 

            string userInput;
            _predictionDictionary = new Dictionary<int, string> { { -1, "Negative" }, { 1, "Positive" } };
            getSent();
            for (int i = 0; i < nText.Count; i++)
            {
                userInput = nText[i].ToString();
                var newX = TextClassificationProblemBuilder.CreateNode(userInput, v);
                var predictedY = model.Predict(newX);

                
                Console.WriteLine("The prediction is {0}", _predictionDictionary[(int)predictedY]);
                String pred = _predictionDictionary[(int)predictedY];
                Console.WriteLine(new string('=', 50));
               sentC.Add(pred);
            }


            addToO();
            getNumbers();
            forPeople();


            using(StreamWriter file = new StreamWriter(@"C:\Users\Rory\Desktop\Peop.csv"))
            {
                file.WriteLine("PersID,Name,Positive,Negative,Party");
                for(int i = 0; i < pl.Count; i++)
                {
                    String line = pl[i].getID().ToString() + "," + pl[i].getName().ToString() + "," + pl[i].getPos().ToString() + "," + pl[i].getNeg().ToString() + "," + pl[i].getParty().ToString();
                    //var json = JsonConvert.SerializeObject(pl[i]);
                    file.WriteLine(line);
                }
            }
           

            using (StreamWriter file = new StreamWriter(@"C:\Users\Rory\Desktop\PositiveSent.csv"))
            {
                file.WriteLine("SpeakerID,TargetID,Text,Sentiment");
                for (int i = 0; i < ps.Count; i++)
                {
                    String line = ps[i].getSID().ToString() + "," + ps[i].getAID().ToString() + "," + ps[i].getText().ToString() + "," + ps[i].getSent().ToString();
                    file.WriteLine(line);
                }
            }

            using (StreamWriter file = new StreamWriter(@"C:\Users\Rory\Desktop\NegativeSent.csv"))
            {
                file.WriteLine("SpeakerID,TargetID,Text,Sentiment");
                for (int i = 0; i < ns.Count; i++)
                {
                    String line = ns[i].getSID().ToString() + "," + ns[i].getAID().ToString() + "," + ns[i].getText().ToString() + "," + ns[i].getSent().ToString();
                    file.WriteLine(line);
                }
            }

            ns.AddRange(ps);
            doSwap();

            var json = "{\"nodes\":";
            json += JsonConvert.SerializeObject(pl);
            json += ",";
            

            var edge = "\"edges\":";
            edge += JsonConvert.SerializeObject(nss);
            edge += "}";

            var fullJSON = json + edge;
            Debug.Write(fullJSON);

            using (StreamWriter file = new StreamWriter(@"C:\Users\Rory\Desktop\Nodes2.JSON"))
            {
                file.Write(fullJSON);
                
            }

            getRels();
            List<Relationship> sl = rl.OrderBy(o=>o.source).ToList();
            var edges = "\"links\":";
            edges += JsonConvert.SerializeObject(sl);
            edges += "}";

            var fullJ = json + edges;

            using (StreamWriter file = new StreamWriter(@"C:\Users\Rory\Desktop\Rels2.JSON"))
            {
                file.Write(fullJ);

            }

        }

        public static void doSwap()
        {
            for (int i = 0; i < ns.Count(); i++)
            {
                String target = "";
                String source = "";
                String sent = "";
                String text = "";

                for (int n = 0; n < pl.Count(); n++)
                {
                    if (ns[i].getAID() == pl[n].getID())
                    {
                        target = pl[n].getName();
                    }
                    if (ns[i].getSID() == pl[n].getID())
                    {                      
                        source = pl[n].getName();
                    }
                }

                sent = ns[i].getSent();
                text = ns[i].getText();


                Sent s = new Sent();
                s.setAID(target);
                s.setSID(source);
                s.setSent(sent);
                s.setText(text);

                nss.Add(s);



            }
        }

        public static String partyAsNum(String party)
        {
            if(party.Equals("Conservative"))
            {
                return "1";
            }
            if (party.Equals("Labour"))
            {
                return "2";
            }
            if (party.Equals("Liberal"))
            {
                return "3";
            }
            if (party.Equals("Scottish National Party"))
            {
                return "4";
            }
            if (party.Equals("SDP"))
            {
                return "5";
            }
            if (party.Equals("Social Democrat"))
            {
                return "5";
            }
            if (party.Equals("Labour Co-operative"))
            {
                return "2";
            }
            if (party.Equals("Official Unionist"))
            {
                return "6";
            }
            if (party.Equals("Democratic Unionist"))
            {
                return "7";
            }
            if (party.Equals("Plaid Cymru"))
            {
                return "8";
            }

            return "9";
        }

        public static void forPeople()
        {
            for(int i =0; i < pl.Count(); i++)
            {
                String name = pl[i].getName();
                String[] pN = name.Split(' ');
                String lN = pN[pN.Length - 1];
                lN = lN.Trim();
                bool check = false;
                if(lN.Equals("Jenkin"))
                {
                    Debug.WriteLine("Jenkin");
                }

                for(int z =0; z < h87N.Count; z++)
                {
                    String nName = h87N[z];
                    String[] sN = nName.Split(' ');
                    String lnN = sN[sN.Length-1];
                    lnN = lnN.Trim();

                    if(lN.Equals(lnN))
                    {
                        String n2 = pN[pN.Length-2];
                        n2 = n2.Trim();
                        if (n2.Equals("Mr.") || n2.Equals("Mr") || n2.Equals("Mrs.") || n2.Equals("Mrs") || n2.Equals("Ms.") || n2.Equals("Miss") || n2.Equals("Ms") || n2.Equals("Lord") || n2.Equals("Dr.") || n2.Equals("Dr") || n2.Equals("Sir"))
                        {
                            Debug.WriteLine("Party: " + h87P[z]);
                            pl[i].setParty(partyAsNum(h87P[z]));
                            check = true;
                            break;
                        }
                        else
                        {
                            String ln2 = sN[sN.Length-2];
                            ln2 = ln2.Trim();
                            if(n2.Equals(ln2))
                            {
                                Debug.WriteLine("Party: " + h87P[z]);
                                pl[i].setParty(partyAsNum(h87P[z]));
                                check = true;
                                break;
                            }
                            else
                            {
                                forPeople1(i);
                                check = true;
                            }
                        }
                    }
                }

                if(!check)
                {
                    forPeople1(i);
                    check = true;
                }
            }
        }

        public static void forPeople1(int i)
        {
           
                String name = pl[i].getName();
                String[] pN = name.Split(' ');
                String lN = pN[pN.Length - 1];
                lN = lN.Trim();
                bool check = false;

                for (int z = 0; z < h83N.Count; z++)
                {
                    String nName = h83N[z];
                    String[] sN = nName.Split(' ');
                    String lnN = sN[sN.Length - 1];
                    lnN = lnN.Trim();

                    if (lN.Equals(lnN))
                    {
                        String n2 = pN[pN.Length - 2];
                        n2 = n2.Trim();
                        if (n2.Equals("Mr.") || n2.Equals("Mrs.") || n2.Equals("Ms.") || n2.Equals("Lord") || n2.Equals("Dr.") || n2.Equals("Sir"))
                        {
                            Debug.WriteLine("Party: " + h83P[z]);
                            Person p = new Person();
                            pl[i].setParty(partyAsNum(h83P[z]));
                            check = true;
                            break;
                        }
                        else
                        {
                            String ln2 = sN[sN.Length - 2];
                            ln2 = ln2.Trim();
                            if (n2.Equals(ln2))
                            {
                                Debug.WriteLine("Party: " + h83P[z]);
                                pl[i].setParty(partyAsNum(h83P[z]));
                                check = true;
                                break;
                            }
                            else
                            {
                                forPeople2(i);
                                check = true;
                            }
                            
                        }
                    }
                }

            if(!check)
            {
                forPeople2(i);
                check = true;
            }
            
        }

        public static void forPeople2(int i)
        {
            
                String name = pl[i].getName();
                String[] pN = name.Split(' ');
                String lN = pN[pN.Length - 1];
                lN = lN.Trim();
                bool check = false;

                for (int z = 0; z < h79N.Count; z++)
                {
                    String nName = h79N[z];
                    String[] sN = nName.Split(' ');
                    String lnN = sN[sN.Length - 1];
                    lnN = lnN.Trim();

                    if (lN.Equals(lnN))
                    {
                        String n2 = pN[pN.Length - 2];
                        n2 = n2.Trim();
                        if (n2.Equals("Mr.") || n2.Equals("Mrs.") || n2.Equals("Ms.") || n2.Equals("Lord") || n2.Equals("Dr.") || n2.Equals("Sir"))
                        {
                            Debug.WriteLine("Party: " + h79P[z]);
                            pl[i].setParty(partyAsNum(h79P[z]));
                            check = true;
                            break;
                        }
                        else
                        {
                            String ln2 = sN[sN.Length - 2];
                            ln2 = ln2.Trim();
                            if (n2.Equals(ln2))
                            {
                                Debug.WriteLine("Party: " + h79P[z]);
                                pl[i].setParty(partyAsNum(h79P[z]));
                                check = true;
                                break;
                            }
                        }
                    }
                }

            if(!check)
            {
                pl[i].setParty(partyAsNum("No Party"));
            }
            
        }

        public static void getRels()
        {
            int count = pl.Count();
            bool check = true;
            for(int i = 0; i < ns.Count(); i++)
            {
                String target = "";
                String source = "";

                for (int n = 0; n < pl.Count(); n++)
                {
                    if(ns[i].getAID() == pl[n].getID())
                    {
                        
                        target = pl[n].getName();
                    }
                    if (ns[i].getSID() == pl[n].getID())
                    {
                        source = pl[n].getName();
                    }
                }
                    
                   

                    
                        
                    
                
                
                String sent = ns[i].getSent();
                count++;
                

                Relationship r = new Relationship();
                r.setAID(target);
                r.setSID(source);
                r.setSent(sent);
                r.setID(3);

                for(int z = 0; z < rl.Count(); z++)
                {
                    if(rl[z].getAID().Equals(target) && rl[z].getSID().Equals(source) && rl[z].getSent().Equals(sent))
                    {
                        Debug.WriteLine("Already in list");
                        check = false;
                        break;
                    }
                    else
                    {
                        check = true;

                    }
                }

                if(check)
                {
                    rl.Add(r);
                    check = false;
                }
                

            }
        }

        public static void getNumbers()
        {
            
            for(int i = 0; i < pID.Length; i++)
            {
                String id = pID[i].ToString();
                int pCount = 0;
                int nCount = 0;
                for(int z = 0; z < ps.Count; z++)
                {
                    String id1 = ps[z].getAID().ToString();
                    if(id.Equals(id1))
                    {
                        pCount++;
                    }
                }
                for(int n = 0; n < ns.Count; n++)
                {
                    String id1 = ns[n].getAID().ToString();
                    if (id.Equals(ns[n].getAID().ToString()))
                    {
                        nCount++;
                    }
                }

                Person p = new Person();
                p.setID(pID[i]);
                p.setName(name[i]);
                p.setNeg(nCount);
                p.setPos(pCount);

                pl.Add(p);

                
            }
            
        }

        public static void getSent()
        {
            const string dataFilePath = @"D:\Docs\StanParse\Positive.csv";
            var dataTable = DataTable.New.ReadCsv(dataFilePath);
            rText = dataTable.Rows.Select(row => row["Text"]).ToList();
            sID = dataTable.Rows.Select(row => int.Parse(row["SpeakerID"])).ToArray();
            tID = dataTable.Rows.Select(row => int.Parse(row["TargetID"])).ToArray();

            const string dataPath = @"D:\Docs\StanParse\Negative.csv";
            var dataT = DataTable.New.ReadCsv(dataPath);
            nText = dataT.Rows.Select(row => row["Text"]).ToList();
            nsID = dataT.Rows.Select(row => int.Parse(row["SpeakerID"])).ToArray();
            ntID = dataT.Rows.Select(row => int.Parse(row["TargetID"])).ToArray();


            const string dataFile = @"D:\Docs\StanParse\People.csv";
            var dataTab = DataTable.New.ReadCsv(dataFile);
            name = dataTab.Rows.Select(row => row["name"]).ToList();
            pID = dataTab.Rows.Select(row => int.Parse(row["PersID"])).ToArray();

            const string h83DP = @"C:\Users\Rory\Desktop\Hans1983.csv";
            var dT = DataTable.New.ReadCsv(h83DP);
            h83N = dT.Rows.Select(row => row["MP"]).ToList();
            h83P = dT.Rows.Select(row => row["Party"]).ToList();

            const string h79DP = @"C:\Users\Rory\Desktop\Hans1979.csv";
            var dT7 = DataTable.New.ReadCsv(h79DP);
            h79N = dT7.Rows.Select(row => row["MP"]).ToList();
            h79P = dT7.Rows.Select(row => row["Party"]).ToList();

            const string h87DP = @"C:\Users\Rory\Desktop\Hans1987.csv";
            var dT8 = DataTable.New.ReadCsv(h87DP);
            h87N = dT8.Rows.Select(row => row["MP"]).ToList();
            h87P = dT8.Rows.Select(row => row["Party"]).ToList();
        }

        public static void addToO()
        {
            for(int i =0; i < rText.Count; i++)
            {
                Sentence s = new Sentence();
                s.setSent(sentC[i].ToString());
                s.setText(rText[i].ToString());

                

               

                s.setAID(tID[i]);
                s.setSID(sID[i]);

                if (sentC[i].ToString().Equals("Negative"))
                {
                    
                    ns.Add(s);
                }
                else
                {
                    ps.Add(s);
                }
                
            }

            
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

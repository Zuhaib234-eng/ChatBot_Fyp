using ChatBot_Fyp.Models.Chatbot;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ChatBot_Fyp.Controllers.Chatbot
{
    public class ChatbotController : Controller
    {
        //Data train
        static List<DataModel> _train = new List<DataModel>();
        public ChatbotController()
        {
            using (StreamReader r = new StreamReader("DataFiles/Data.json"))
            {
                string json = r.ReadToEnd();
                var items = JsonConvert.DeserializeObject<DataListModel>(json);
                _train = items.DataList;
            }
        }
        class ClassInfo
        {
            public ClassInfo(string Quest, List<String> trainDocs)
            {
                Name = Quest;
                var features = trainDocs;
                WordsCount = features.Count();
                WordCount =
                    features.GroupBy(x => x)
                    .ToDictionary(x => x.Key, x => x.Count());
                NumberOfDocs = trainDocs.Count;
            }
            public string Name { get; set; }
            public int WordsCount { get; set; }
            public Dictionary<string, int> WordCount { get; set; }
            public int NumberOfDocs { get; set; }
            public int NumberOfOccurencesInTrainDocs(String word)
            {
                if (WordCount.Keys.Contains(word)) return WordCount[word];
                return 0;
            }
        }

        class Classifier
        {
            List<ClassInfo> _classes;
            int _countOfDocs;
            int _uniqWordsCount;
            public Classifier(List<DataModel> train)
            {
                _classes = train.GroupBy(x => x.Question).Select(g => new ClassInfo(g.Key, g.Select(x => x.Question).ToList())).ToList();
                _countOfDocs = train.Count;
                _uniqWordsCount = train.SelectMany(x => x.Question.Split(' ')).GroupBy(x => x).Count();
            }

            public double IsInClassProbability(string className, string text)
            {
                //var words = text.ExtractFeatures();
                var classResults = _classes
                    .Select(x => new
                    {
                        Result = Math.Pow(Math.E, Calc(x.NumberOfDocs, _countOfDocs, new List<string>(), x.WordsCount, x, _uniqWordsCount)),
                        ClassName = x.Name
                    });


                return classResults.Single(x => x.ClassName == className).Result / classResults.Sum(x => x.Result);
            }

            private static double Calc(double dc, double d, List<String> q, double lc, ClassInfo @class, double v)
            {
                return Math.Log(dc / d) + q.Sum(x => Math.Log((@class.NumberOfOccurencesInTrainDocs(x) + 1) / (v + lc)));
            }
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult BotResponse([FromBody] DataModel model)
        {
            var Answer = _train.Where(s => s.Question.ToLower() == model.Question.ToLower()).FirstOrDefault();
            if (Answer != null)
            {
                return Json(new { status = "success", data = Answer });
            }
            else
            {
                return Json(new { status = "success", data = Answer });
            }
            //using (StreamReader r = new StreamReader("DataFiles/Data.json"))
            //{
            //   // string json = r.ReadToEnd();
            //   // var items = JsonConvert.DeserializeObject<DataListModel>(json);
            //    var Answer = _train.Where(s => s.Question.ToLower() == model.Question.ToLower()).FirstOrDefault();
            //    if (Answer != null)
            //    {
            //        return Json(new { status = "success", data = Answer });
            //    }
            //    else
            //    {
            //        return Json(new { status = "success", data = Answer });
            //    }
            //}
        }
        public IActionResult ComplainForm()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddComplain([FromBody] ComplainModel model)
        {
            //var jsonData = System.IO.File.ReadAllText("DataFiles/Complains.json");
            //var complainList = JsonConvert.DeserializeObject<ComplainData>(jsonData)
            //                      ?? new ComplainData();
            //complainList.Data.Add(model);                        
            //jsonData = JsonConvert.SerializeObject(complainList);
            //System.IO.File.WriteAllText("DataFiles/Complains.json", jsonData);
            ComplainData complainData = new ComplainData();
            using (StreamReader r = new StreamReader("DataFiles/Complains.json"))
            {
                string json = r.ReadToEnd();
                var complains = JsonConvert.DeserializeObject<ComplainData>(json);
                if (complains.Data == null)
                {
                    complains.Data = new List<ComplainModel>();
                    complains.Data.Add(model);
                    complainData = complains;
                }
                else
                {
                    complains.Data.Add(model);
                    complainData = complains;
                }
            }

            using (StreamWriter r = new StreamWriter("DataFiles/Complains.json"))
            {
                var data = JsonConvert.SerializeObject(complainData);
                r.Write(data);
            }
            return Json(new { status = "success" });
        }
    }
}

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
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult BotResponse([FromBody] DataModel model)
        {
            using (StreamReader r = new StreamReader("DataFiles/Data.json"))
            {
                string json = r.ReadToEnd();
                var items = JsonConvert.DeserializeObject<DataListModel>(json);
                var Answer = items.DataList.Where(s => s.Question.ToLower() == model.Question.ToLower()).FirstOrDefault();
                if (Answer != null)
                {
                    return Json(new { status = "success", data = Answer });
                }
                else
                {
                    return Json(new { status = "success", data = Answer });
                }
            }
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

using ChatBot_Fyp.Models.Chatbot;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ChatBot_Fyp.Controllers.Chatbot
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComplainsController : ControllerBase
    {
        [HttpGet]
        public ComplainData GetComplains()
        {
            using (StreamReader r = new StreamReader("DataFiles/Complains.json"))
            {
                string json = r.ReadToEnd();
                var complains = JsonConvert.DeserializeObject<ComplainData>(json);
                return complains;
            }
        }

        [Route("AddComplaint")]
        [HttpPost]
        public Hashtable AddComplaint([FromBody] ComplainModel model)
        {
            Hashtable ht = new Hashtable();
            try
            {
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
                ht["status"] = "success";
                ht["code"] = "00";
                ht["message"] = "";
            }
            catch (Exception ex)
            {
                ht["status"] = "fail";
                ht["code"] = "01";
                ht["message"] = ex.Message;
            }
            return ht;
        }
    }
}

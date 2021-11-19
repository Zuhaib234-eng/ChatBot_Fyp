using ChatBot_Fyp.Models.Chatbot;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
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
    }
}

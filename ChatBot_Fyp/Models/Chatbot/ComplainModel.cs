using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatBot_Fyp.Models.Chatbot
{
    public class ComplainModel : IComplainModel
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public string Email { get; set; }
        public string Complain { get; set; }
    }
    public class ComplainData
    {
        public List<ComplainModel> Data { get; set; }
    }
}

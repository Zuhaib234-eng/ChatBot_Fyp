using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatBot_Fyp.Models.Chatbot
{
    public class DataListModel: IDataListModel
    {
        public List<DataModel> DataList { get; set; }
    }
}

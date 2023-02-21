using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EnzoCommanderSDK.SDK.Models
{

    public class ErrorResponse
    {
        public string code { get; set; }
        public string message { get; set; }
        public Data data { get; set; }
    }

    public class Data
    {
        public int status { get; set; }
        [JsonPropertyName("params")]
        public IDictionary<string,object> _params { get; set; } = new Dictionary<string,object>();
        //public Params _params { get; set; }
        public object[] details { get; set; }
    }

    public class Params
    {
        public string mapping { get; set; }
    }


    //public class Data
    //{
    //    public int status { get; set; }
    //    [JsonProperty("params")]
    //    public List<KeyValuePair<string, object>> _params { get; set; } = new List<KeyValuePair<string, object>>();
    //    public object[] details { get; set; }
    //}

}


//public class Rootobject
//{
//    public string code { get; set; }
//    public string message { get; set; }
//    public Data data { get; set; }
//}

//public class Data
//{
//    public int status { get; set; }
//    public Params _params { get; set; }
//    public object[] details { get; set; }
//}

//public class Params
//{
//    public string mapping { get; set; }
//}

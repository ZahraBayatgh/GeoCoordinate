using Newtonsoft.Json;
using System.Net.Http;

namespace Opeqe.Infrastructure.Toolkits.JsonToolkit
{
    public class JsonContent : StringContent
    {
        public JsonContent(object obj) :
            base(JsonConvert.SerializeObject(obj), System.Text.Encoding.UTF8, "application/json")
        { }
    }

}

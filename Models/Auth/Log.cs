using Dapper.Contrib.Extensions;
using Newtonsoft.Json;

namespace  MyPSG.API.Models.Auth
{
    [Table("tbl_Auth_Logs")]
    public class Log
    {
        public long LogID { get; set;}
        public string Level { get; set;}
        public string ClassName { get; set;}
        public string MethodName { get; set;}
        public string Message { get; set;}
        public string NewValue { get; set;}
        public string OldValue { get; set;}
        public string Exception { get; set;}
        public string CreatedBy { get; set;}
    }

    public static class Logs{
        public static string ToJson<T>(T obj){
            return JsonConvert.SerializeObject(obj);

        }
    }
}
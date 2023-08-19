using System;
using Dapper.Contrib.Extensions;

namespace MyPSG.API.Models
{
    public class BaseModel
    {
        [Write(false)]
        public bool is_active { get; set; } = true;
        public string created_by { get; set; }
        public DateTime? created_date { get; set; } = DateTime.Now;
        public string last_updated_by { get; set; }
        public DateTime? last_updated_date { get; set; }
        public string computer_name { get; set; }
        public DateTime? computer_date { get; set; } = DateTime.Now;
    }
}
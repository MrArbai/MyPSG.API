using System;
using Dapper.Contrib.Extensions;

namespace MyPSG.API.Models
{
    public class BaseModel
    {
        [Write(false)]
        public bool Is_active { get; set; } = true;
        public string Created_by { get; set; }
        public DateTime? Created_date { get; set; } = DateTime.Now;
        public string Last_updated_by { get; set; }
        public DateTime? Last_updated_date { get; set; }
        public string Computer_name { get; set; }
        public DateTime? Computer_date { get; set; } = DateTime.Now;
    }
}
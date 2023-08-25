using System;
using Dapper.Contrib.Extensions;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyPSG.API.Models
{
    public class BaseModel
    {
        [Write(false)]
        [Column("is_active")]
        public bool Is_active { get; set; } = true;
        [Column("created_by")]
        public string Created_by { get; set; }
        [Column("created_date")]
        public DateTime? Created_date { get; set; } = DateTime.Now;
        [Column("last_updated_by")]
        public string Last_updated_by { get; set; }
        [Column("last_updated_date")]
        public DateTime? Last_updated_date { get; set; }
        [Column("computer_name")]
        public string Computer_name { get; set; }
        [Column("computer_date")]
        public DateTime? Computer_date { get; set; } = DateTime.Now;
    }
}
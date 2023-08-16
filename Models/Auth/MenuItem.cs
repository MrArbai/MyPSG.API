namespace MyPSG.API.Models.Auth
{
    public class MenuItem : BaseModel
    {
        public string Item_menu_id { get; set; }
        public string Menu_id { get; set; }
        public int? Grant_id { get; set; }
        public string Keterangan { get; set; }
       

    }
}
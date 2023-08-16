namespace MyPSG.API.Dto.Auth
{
    public class UserRegisterDto
    {
        public string User_id { get; set; }
        public string Role_id { get; set; }
        public int Employee_id { get; set; }
        public string User_name { get; set; }
        public string Password { get; set; }
        public short Status_user { get; set; }
        public string Computer { get; set; }
    }
}
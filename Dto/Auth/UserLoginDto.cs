namespace MyPSG.API.Dto.Auth
{
    public class UserLoginDto
    {
        public string User_id { get; set; }
        public string Password { get; set; }
        public string Computer { get; set; } = "";
        public string App_version { get; set; } = "";
    }
}
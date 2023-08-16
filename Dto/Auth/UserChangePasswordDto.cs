namespace MyPSG.API.Dto.Auth
{
    public class UserChangePasswordDto
    {
        public string User_id { get; set; }
        public string PasswordOld { get; set; }
        public string PasswordNew { get; set; }
    }
}
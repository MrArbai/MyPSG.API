namespace MyPSG.API.Dto.Auth
{
    public class RoleDto
    {
        public string Role_id { get; set; }
        public string Role_name { get; set; }
        public bool? Is_active { get; set; }
        public string Company_id { get; set; }
    }
}
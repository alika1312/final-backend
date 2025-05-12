namespace api.Dtos
{
    public class LoginDto
    {
        public string username { get; set; } = string.Empty;
        public string password { get; set; } = string.Empty;
    }
    public class UpdateManagerUsernameDto
{
    public string? Username { get; set; }
}

}

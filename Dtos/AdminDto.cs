namespace api.Dtos
{
    public class AdminDto
    {

        public int userID { get; set; }
        public string username { get; set; } = string.Empty;
    }

    public class CreateAdminRequestDto
    {
        public int userID { get; set; }
    }

}

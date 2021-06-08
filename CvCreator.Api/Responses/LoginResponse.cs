namespace CvCreator.Api.Responses
{
    public class LoginResponse
    {
        public int Client_ID { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public bool Admin_Permission { get; set; }
    }
}

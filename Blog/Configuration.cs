namespace Blog;

public static class Configuration
{
    //string do hash que será usado no token :0
    public static string JwtKey = "dfsHFDOEUIH9320dasR2384RF80-930UR0J*";
    public static string ApiKeyName = "api_key";
    public static string ApiKey = "kjsflk=240-o=2ot0-43=A";
    public static SmtpConfiguration Smtp = new();
    
    // podemos colocar uma classe dentro de uma classe estática
    public class SmtpConfiguration
    {
        public string Host { get; set; }
        public int Port { get; set; } = 25;
        public string UserName { get; set; }
        public string Password { get; set; }        
    }
    
}
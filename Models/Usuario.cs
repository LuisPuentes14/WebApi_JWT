namespace WebApi.Models
{
    public class Usuario
    {
        public int id { get; set; }
        public string Login { get; set; }
        public string password { get; set; }

        public DateTime feche_create { get; set; }

        public string perfil { get; set; }

    }
}

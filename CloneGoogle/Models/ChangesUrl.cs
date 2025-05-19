using System.ComponentModel.DataAnnotations;

namespace CloneGoogle.Models // Замените YourNamespace на ваш фактический пространство имен
{
    public class ChangesUrl
    {
        public int Id { get; set; }
        public string? OriginUrl { get; set; }

        public string? ShortUrl { get; set; }

        public DateTime CreateUrlTime { get; set; }
        public int ClickUrl { get; set; }
    }
}
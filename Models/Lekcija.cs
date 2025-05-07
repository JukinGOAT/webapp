namespace webapp.Models
{
    public class Lekcija
    {
        public int Id { get; set; }

        
        public string Title { get; set; } = null!;

       
        public string Content { get; set; } = null!;

        
        public int KolegijId { get; set; }

        
        public Kolegij Kolegij { get; set; } = null!;
    }
}

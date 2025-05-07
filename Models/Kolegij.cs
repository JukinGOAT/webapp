namespace webapp.Models
{
    public class Kolegij
    {
        public int Id { get; set; }
        public string Naziv { get; set; }
        public string Opis { get; set; }
        public List<string> Lekcije { get; set; }
    }
}

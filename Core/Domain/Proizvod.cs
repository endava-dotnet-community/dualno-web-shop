namespace Core.Domain
{
    public class Proizvod
    {
        public int Id { get; set; }
        public string Ime { get; set; }
        public string Kategorija { get; set; }
        public decimal Cena { get; set; }
        public string Opis { get; set; }
    }
}
namespace Saitynai.Data.Entities
{
    public class Registration
    {
        public int Id { get; set; }
        public string CarNo { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }

        public Competition Competition { get; set; }

    }
}

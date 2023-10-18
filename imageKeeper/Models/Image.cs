namespace imageKeeper.Models
{
    public class Image
    {

        public int Id { get; set; }
        public string FilePath { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

    }
}

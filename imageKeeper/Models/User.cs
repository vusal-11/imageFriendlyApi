
namespace imageKeeper.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public ICollection<Friendship> Friendships { get; set; }
        private List<Image> _images = new List<Image>();
        public IReadOnlyCollection<Image> Images => _images.AsReadOnly();
    }
}

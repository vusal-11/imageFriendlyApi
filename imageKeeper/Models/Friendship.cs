using System.ComponentModel.DataAnnotations.Schema;

namespace imageKeeper.Models
{
    public class Friendship
    {
        public int Id { get; set; }
        public int UserAId { get; set; }
        public int UserBId { get; set; }

        [ForeignKey("UserAId")]
        public User UserA { get; set; }

        [ForeignKey("UserBId")]
        public User UserB { get; set; }
    }
}

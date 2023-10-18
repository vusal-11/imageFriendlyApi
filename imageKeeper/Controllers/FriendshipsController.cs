using imageKeeper.Data;
using imageKeeper.DTOs;
using imageKeeper.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace imageKeeper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FriendshipsController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;

        public FriendshipsController(UserManager<User> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [Authorize]
        [HttpPost("add")]
        public async Task<IActionResult> AddFriend([FromBody] FriendshipDto friendshipDto)
        {
            var user = await _userManager.GetUserAsync(User);

            // Проверяем, что пользователь существует и не пытается добавить сам себя
            var friendUser = await _userManager.FindByIdAsync(friendshipDto.FriendUserId);
            if (friendUser == null || friendUser.Id == user.Id)
            {
                return BadRequest("Invalid friend user.");
            }

            // Проверяем, что дружба еще не существует
            var existingFriendship = user.Friendships.FirstOrDefault(f => f.UserAId == user.Id && f.UserBId == friendUser.Id);

            if (existingFriendship == null)
            {
                var friendship = new Friendship
                {
                    UserAId = user.Id,
                    UserBId = friendUser.Id
                };

                user.Friendships.Add(friendship);

                // Сохраняем изменения в базе данных
                _context.Users.Update(user);
                _context.SaveChanges();

                return Ok("Friend added successfully");
            }
            else
            {
                return BadRequest("Friendship already exists.");
            }
        }
    }
}

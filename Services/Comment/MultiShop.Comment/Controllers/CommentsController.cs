using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiShop.Comment.Context;
using MultiShop.Comment.Entities;

namespace MultiShop.Comment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CommentsController : ControllerBase
    {
        private readonly CommentContext _commentContext;

        public CommentsController(CommentContext commentContext)
        {
            _commentContext = commentContext;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CommentList()
        {
            var values = await _commentContext.UserComments.ToListAsync();
            return Ok(values);
        }
        
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CommentById(int id)
        {
            var value = await _commentContext.UserComments.FindAsync(id);
            return Ok(value);
        }

        [HttpGet("CommentByProductId")]
        [AllowAnonymous]
        public async Task<IActionResult> CommentByProductId(string productId)
        {
            var value = await _commentContext.UserComments.Where(x => x.ProductId == productId && x.IsApproved)
                .Select(x => new { x.UserCommentId, x.NameSurname, x.ImageUrl, x.CommentDetail, x.Rating, x.CommentDate, x.ProductId, x.IsApproved })
                .ToListAsync();
            return Ok(value);
        }
        
        [HttpPost]
        public async Task<IActionResult> Create(UserComment userComment)
        {
            if (string.IsNullOrWhiteSpace(userComment.ProductId) || string.IsNullOrWhiteSpace(userComment.CommentDetail) ||
                userComment.CommentDetail.Length > 2000 || userComment.Rating is < 1 or > 5)
                return BadRequest("Ürün, yorum ve 1-5 arası puan gerekli.");
            userComment.UserCommentId = 0;
            userComment.IsApproved = false;
            userComment.CommentDate = DateTime.UtcNow;
            userComment.NameSurname = User.FindFirst("name")?.Value ?? "Müşteri";
            userComment.Email = User.FindFirst("email")?.Value ?? string.Empty;
            userComment.ImageUrl = string.Empty;
            await _commentContext.UserComments.AddAsync(userComment);
            await _commentContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(UserComment userComment)
        {
            _commentContext.UserComments.Update(userComment);
            await _commentContext.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var comment = await _commentContext.UserComments.FindAsync(id);
            if (comment is null)
                return NotFound();
            _commentContext.UserComments.Remove(comment);
            await _commentContext.SaveChangesAsync();
            return Ok();
        }
    }
}

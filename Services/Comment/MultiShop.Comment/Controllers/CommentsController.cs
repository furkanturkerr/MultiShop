using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiShop.Comment.Context;
using MultiShop.Comment.Entities;

namespace MultiShop.Comment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly CommentContext _commentContext;

        public CommentsController(CommentContext commentContext)
        {
            _commentContext = commentContext;
        }

        [HttpGet]
        public async Task<IActionResult> CommentList()
        {
            var values = await _commentContext.UserComments.ToListAsync();
            return Ok(values);
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> CommentById(int id)
        {
            var value = await _commentContext.UserComments.FindAsync(id);
            return Ok(value);
        }

        [HttpGet("CommentByProductId")]
        public async Task<IActionResult> CommentByProductId(string productId)
        {
            var value = await _commentContext.UserComments.Where(x=>x.ProductId == productId).ToListAsync();
            return Ok(value);
        }
        
        [HttpPost]
        public async Task<IActionResult> Create(UserComment userComment)
        {
            await _commentContext.UserComments.AddAsync(userComment);
            await _commentContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update(UserComment userComment)
        {
            _commentContext.UserComments.Update(userComment);
            await _commentContext.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var comment = await _commentContext.UserComments.FindAsync(id);
            _commentContext.UserComments.Remove(comment);
            await _commentContext.SaveChangesAsync();
            return Ok();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlogBackend.Data;
using BlogBackend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace BlogBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly BlogContext _context;

        public CommentsController(BlogContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all comments for a specific article.
        /// </summary>
        /// <param name="articleId">The ID of the article.</param>
        [HttpGet("{articleId}")]
        public async Task<ActionResult<IEnumerable<Comment>>> GetComments(string articleId)
        {
            return await _context.Comments.Where(c => c.ArticleId == articleId).ToListAsync();
        }

        /// <summary>
        /// Creates a new comment.
        /// </summary>
        /// <param name="comment">The comment to create.</param>
        [HttpPost]
        public async Task<ActionResult<Comment>> PostComment(Comment comment)
        {
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetComments), new { articleId = comment.ArticleId }, comment);
        }

        /// <summary>
        /// Updates an existing comment.
        /// </summary>
        /// <param name="id">The ID of the comment to update.</param>
        /// <param name="comment">The updated comment.</param>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutComment(string id, Comment comment)
        {
            if (id != comment.Id)
            {
                return BadRequest();
            }

            _context.Entry(comment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Deletes a comment by ID.
        /// </summary>
        /// <param name="id">The ID of the comment to delete.</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(string id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CommentExists(string id)
        {
            return _context.Comments.Any(e => e.Id == id);
        }
    }
}

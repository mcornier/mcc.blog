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
    public class ArticlesController : ControllerBase
    {
        private readonly BlogContext _context;

        public ArticlesController(BlogContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all articles.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Article>>> GetArticles()
        {
            return await _context.Articles.Include(a => a.Comments).ToListAsync();
        }

        /// <summary>
        /// Retrieves a specific article by ID.
        /// </summary>
        /// <param name="id">The ID of the article.</param>
        [HttpGet("{id}")]
        public async Task<ActionResult<Article>> GetArticle(string id)
        {
            var article = await _context.Articles.Include(a => a.Comments).FirstOrDefaultAsync(a => a.Id == id);

            if (article == null)
            {
                return NotFound();
            }

            return article;
        }

        /// <summary>
        /// Creates a new article.
        /// </summary>
        /// <param name="article">The article to create.</param>
        [HttpPost]
        public async Task<ActionResult<Article>> PostArticle(Article article)
        {
            _context.Articles.Add(article);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetArticle), new { id = article.Id }, article);
        }

        /// <summary>
        /// Updates an existing article.
        /// </summary>
        /// <param name="id">The ID of the article to update.</param>
        /// <param name="article">The updated article.</param>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutArticle(string id, Article article)
        {
            if (id != article.Id)
            {
                return BadRequest();
            }

            _context.Entry(article).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArticleExists(id))
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
        /// Deletes an article by ID.
        /// </summary>
        /// <param name="id">The ID of the article to delete.</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArticle(string id)
        {
            var article = await _context.Articles.FindAsync(id);
            if (article == null)
            {
                return NotFound();
            }

            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ArticleExists(string id)
        {
            return _context.Articles.Any(e => e.Id == id);
        }
    }
}

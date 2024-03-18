using BarsantiExplorer.Models;
using BarsantiExplorer.Models.Entities;
using BarsantiExplorer.Models.Requests.Comments;
using BarsantiExplorer.Models.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace BarsantiExplorer.Controllers;

[ApiController]
[Route("api/comments")]
public class CommentsController: BaseController
{
    /// <summary>
    ///  Get filtered comments
    /// </summary>
    /// <response code="200">Returns filtered trips</response>
    [HttpGet("")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(List<Comment>), StatusCodes.Status200OK)]
    public IActionResult getComments([FromQuery] GetCommentsRequest queryParams)
    {
        var comments = DB.Comments
            .Include(c => c.Trip)
            .AsQueryable();

        if (queryParams.TripTitle != null)
        {
            comments = comments.Where(c => c.Trip.Title == queryParams.TripTitle);
        }
        if(queryParams.Sort != null)
        {
            comments = comments.OrderBy(queryParams.Sort);
        }
        var final = comments.Select(c => c.CommentToCommentResponse());

        if (final.ToList().Count == 0)
        {
            return NotFound();
        }
        return Ok(final);
    }

    /// <summary>
    /// Get a comment
    /// </summary>
    /// <response code="200">Returns the comment data</response>
    /// <response code="404">If the comment was not found</response>
    [HttpGet("{id}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(TripResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetComment(int id)
    {
        var comment = DB.Comments
            .Include(c => c.Trip)
            .FirstOrDefault(c => c.Id == id);
        if (comment == null)
        {
            return NotFound();
        }
        return Ok(comment);
    }

    /// <summary>
    /// Delete a comment
    /// </summary>
    /// <response code="200">Returns true if the comment was deleted successfully</response>
    [HttpDelete("{id}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public IActionResult DeleteComment(int id)
    {
        var comment = DB.Comments.Find(id);
        if (comment == null)
        {
            return NotFound();
        }
        comment.DeletedAt = DateTime.Now;
        DB.SaveChanges();
        return Ok(true);
    }

    public CommentsController(BarsantiDbContext context, IConfiguration appSettings) : base(context, appSettings)
    { }
}
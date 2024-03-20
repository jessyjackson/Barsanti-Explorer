using BarsantiExplorer.Models;
using BarsantiExplorer.Models.Entities;
using BarsantiExplorer.Models.Requests.Comments;
using BarsantiExplorer.Models.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace BarsantiExplorer.Controllers;
[ApiController]
[Route("api/comments")]
public class CommentsController : BaseController
{
    public CommentsController(BarsantiDbContext context, IConfiguration appSettings) : base(context, appSettings)
    {
    }

    /// <summary>
    ///  Get filtered comments
    /// </summary>
    /// <response code="200">Returns filtered comments</response>
    [Authorize]
    [HttpGet("")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(List<Comment>), StatusCodes.Status200OK)]
    public IActionResult GetComments([FromQuery] GetCommentsRequest queryParams)
    {
        var comments = DB.Comments
            .AsQueryable();

        if (queryParams.TripId != null)
        {
            comments = comments.Where(c => c.TripId == queryParams.TripId);
        }

        if (queryParams.Sort != null)
        {
            comments = comments.OrderBy(queryParams.Sort);
        }

        return Ok(comments);
    }

    /// <summary>
    /// Get a comment
    /// </summary>
    /// <response code="200">Returns the comment data</response>
    /// <response code="404">If the comment was not found</response>
    [HttpGet("{id}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(CommentResponse), StatusCodes.Status200OK)]
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

        return Ok(comment.MapToCommentResponse());
    }
    /// <summary>
    /// Create a new comment
    /// </summary>
    /// <response code="200">Returns the new comment data</response>  
    [HttpPost("")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(CommentResponse), StatusCodes.Status200OK)]
    public IActionResult CreateComment([FromForm] CreateCommentRequest request)
    {
        var comment = new Comment
        {
            Title = request.Title,
            Text = request.Text,
            TripId = request.TripId,
            CreatedAt = DateTime.Now
        };
        DB.Comments.Add(comment);
        DB.SaveChanges();
        return Ok(comment.CommentToCommentResponse());
    }
    /// <summary>
    /// Accept deny comment
    /// </summary>
    /// <response code="200">Returns the comment data</response> 
    /// <response code="404">If the comment was not found</response>
    [HttpPost("{id}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(CommentResponse), StatusCodes.Status200OK)]
    public IActionResult AcceptComment(int id,[FromForm] AcceptCommentRequest acceptComment)
    {
        var comment = DB.Comments.Find(id);
        if (comment == null)
        {
            return NotFound();
        }
        comment.IsAccepted = acceptComment.IsAccepted;
        DB.SaveChanges();
        return Ok(comment.CommentToCommentResponse());
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


}
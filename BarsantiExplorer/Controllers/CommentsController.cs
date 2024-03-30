using BarsantiExplorer.Models;
using BarsantiExplorer.Models.Entities;
using BarsantiExplorer.Models.Requests.Comments;
using BarsantiExplorer.Models.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Dynamic.Core;
using BarsantiExplorer.Enum;
using BarsantiExplorer.TelegramBot;

namespace BarsantiExplorer.Controllers;

[ApiController]
[Route("api/comments")]
public class CommentsController : BaseController
{
    public CommentsController(BarsantiDbContext context, IConfiguration appSettings,Bot telegramBot) : base(context, appSettings,telegramBot)
    {
    }

    /// <summary>
    ///  Get filtered comments
    /// </summary>
    /// <response code="200">Returns filtered comments</response>
    [HttpGet("")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(List<Comment>), StatusCodes.Status200OK)]
    public IActionResult GetComments([FromQuery] GetCommentsRequest queryParams)
    {
        var comments = DB.Comments
            .Where(el => el.Status == CommentStatus.Approved)
            .Where(el => el.DeletedAt == null);

        if (queryParams.TripId != null)
        {
            comments = comments.Where(c => c.TripId == queryParams.TripId);
        }

        if (queryParams.Page != null && queryParams.Limit != null)
        {
            comments = comments
                .Skip(queryParams.Page.Value * queryParams.Limit.Value)
                .Take(queryParams.Limit.Value);
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
            .Where(c => c.DeletedAt == null)
            .FirstOrDefault(c => c.Id == id);

        if (comment == null)
        {
            return NotFound();
        }
        TelegramBot.DoWork(DB,comment);
        return Ok(comment.MapToCommentResponse());
    }

    /// <summary>
    /// Create a new comment
    /// </summary>
    /// <response code="200">Returns the new comment data</response>  
    [HttpPost("")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(CommentResponse), StatusCodes.Status200OK)]
    public IActionResult CreateComment([FromBody] CreateCommentRequest request)
    {
        var comment = new Comment
        {
            Author = request.Author,
            Rating = request.Rating,
            Text = request.Text,
            TripId = request.TripId,
            CreatedAt = DateTime.Now
        };
        DB.Comments.Add(comment);
        DB.SaveChanges();
        return Ok(comment.MapToCommentResponse());
    }

    /// <summary>
    /// Accept deny comment
    /// </summary>
    /// <response code="200">Returns the comment data</response> 
    /// <response code="404">If the comment was not found</response>
    /// <responde code="400">If the status is invalid</response>
    [HttpPost("{id}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(CommentResponse), StatusCodes.Status200OK)]
    public IActionResult AcceptComment(int id, [FromBody] AcceptCommentRequest acceptComment)
    {
        var comment = DB.Comments.Find(id);
        if (comment == null)
        {
            return NotFound();
        }

        comment.Status = acceptComment.Status;
        DB.SaveChanges();
        return Ok(comment.MapToCommentResponse());
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
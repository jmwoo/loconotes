using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using loconotes.Business.Exceptions;
using loconotes.Models.Note;
using loconotes.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace loconotes.Controllers
{
    [Route("api/notes")]
    [Produces("application/json")]
    public class NotesController : BaseIdentityController
    {
        private readonly INoteService _noteService;

        public NotesController(
            INoteService noteService
        )
        {
            _noteService = noteService;
        }

        [HttpGet("")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var allNotes = await _noteService.GetAll().ConfigureAwait(false);
                return Ok(allNotes);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost("")]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] NoteCreateModel noteCreateModel)
        {
            if (noteCreateModel == null || !TryValidateModel(noteCreateModel))
            {
                return BadRequest(ModelState);
            }

            try
            {
                var note = await _noteService.Create(ApplicationUser, noteCreateModel).ConfigureAwait(false);
                return Created("Note", note);
            }
            catch (ValidationException validationException)
            {
                return BadRequest(validationException.Message);
            }
            catch (ConflictException conflictException)
            {
                return new StatusCodeResult(StatusCodes.Status409Conflict);
            }
        }

		[HttpDelete("")]
		[Authorize]
		public async Task<IActionResult> DeleteAll()
		{
			await _noteService.DeleteAll(ApplicationUser);
			return Ok();
		}

	    [HttpDelete("{id:int}")]
	    [Authorize]
	    public async Task<IActionResult> DeleteNote([FromRoute] int id)
	    {
		    await _noteService.DeleteNote(ApplicationUser, id).ConfigureAwait(false);
		    return Ok();
	    }


		[HttpPost("{id:int}/vote")]
        [Authorize]
        public async Task<IActionResult> Vote([FromRoute] int id, [FromBody] VoteModel voteModel)
        {
            if (voteModel == null || !TryValidateModel(voteModel))
                return BadRequest(ModelState);

            var applicationUser = ApplicationUser;
            voteModel.UserId = applicationUser.Id;

            try
            {
                var note = await _noteService.Vote(applicationUser, id, voteModel).ConfigureAwait(false);
                return Ok(note);
            }
            catch (ConflictException)
            {
                return BadRequest("Invalid vote");
            }
        }

        [HttpPost("nearby")]
        [AllowAnonymous]
        public async Task<IActionResult> Nearby([FromBody] NoteSearchRequest noteSearchRequest)
        {
            if (noteSearchRequest == null || !TryValidateModel(noteSearchRequest))
                return BadRequest();

            var applicationUser = this.ApplicationUser;
            var nearybyNotes = await _noteService.Nearby(applicationUser, noteSearchRequest).ConfigureAwait(false);
            return Ok(nearybyNotes);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using loconotes.Business.Exceptions;
using loconotes.Models.Note;
using loconotes.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace loconotes.Controllers
{
    [Route("api/notes")]
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
            var user = GetApplicationUser();
            var noteToCreate = noteCreateModel.ToNote(user);

            if (!TryValidateModel(noteToCreate))
            {
                return BadRequest(ModelState);
            }

            try
            {
                var note = await _noteService.Create(noteToCreate).ConfigureAwait(false);
                return Created("Note", note);
            }
            catch (ConflictException)
            {
                return new StatusCodeResult(StatusCodes.Status409Conflict);
            }
        }

        [HttpPost("{id:int}/vote")]
        [Authorize]
        public async Task<IActionResult> Vote([FromRoute] int id, [FromBody] VoteModel voteModel)
        {
            var user = GetApplicationUser();
            var note = await _noteService.Vote(id, voteModel).ConfigureAwait(false);
            return Ok(note);
        }

        [HttpPost("nearby")]
        [AllowAnonymous]
        public async Task<IActionResult> Nearby([FromBody] NoteSearchRequest noteSearchRequest)
        {
            if (!TryValidateModel(noteSearchRequest))
                return BadRequest();

            var nearybyNotes = await _noteService.Nearby(noteSearchRequest).ConfigureAwait(false);
            return Ok(nearybyNotes);
        }
    }
}

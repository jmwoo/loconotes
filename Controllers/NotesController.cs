using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using loconotes.Business.Exceptions;
using loconotes.Models;
using loconotes.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace loconotes.Controllers
{
    [Route("api/notes")]
    public class NotesController : BaseController
    {
        private readonly INoteService _noteService;

        public NotesController(
            INoteService noteService
        )
        {
            _noteService = noteService;
        }

        [HttpGet("")]
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

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] NoteCreateModel noteCreateModel)
        {
            var noteToCreate = noteCreateModel.ToNote();

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
        public async Task<IActionResult> Vote([FromRoute] int id, Vote vote)
        {
            var note = await _noteService.Vote(id, vote).ConfigureAwait(false);
            return Ok(note);
        }

        [HttpPost("nearby")]
        public async Task<IActionResult> Nearby([FromBody] NoteSearchRequest noteSearchRequest)
        {
            if (!TryValidateModel(noteSearchRequest))
                return BadRequest();

            var nearybyNotes = await _noteService.Nearby(noteSearchRequest).ConfigureAwait(false);
            return Ok(nearybyNotes);
        }
    }
}

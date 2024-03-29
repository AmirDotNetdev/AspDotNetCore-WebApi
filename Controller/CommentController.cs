using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Comments;
using api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Identity;
using api.Extentions;

namespace api.Controller
{
    [Route("api/comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commnetRepo;
        private readonly IStockRepository _stockRepo;
        private readonly UserManager<AppUser> _userManager;
        public CommentController(ICommentRepository commentRepo, IStockRepository stockRepo, UserManager<AppUser> userManager)
        {
            _commnetRepo = commentRepo;
            _stockRepo = stockRepo;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllComments()
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var comments = await _commnetRepo.GetAllAsync();
            
            var commentDto = comments.Select(x => x.ToCommentDto());
            return Ok(commentDto);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var comment = await _commnetRepo.GetById(id);
            if(comment == null)
            {
                return NotFound();
            }

            return Ok(comment.ToCommentDto());
        }

        [HttpPost("{stockId:int}")]
        public async Task<IActionResult> CreateComment([FromRoute] int stockId, [FromBody] CreateCommentDto commentDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(! await _stockRepo.IsStockExist(stockId))
            {
                return BadRequest("Stock doesnt exist");
            }
            var userName = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(userName);
            var comment = commentDto.ToCommentFromCreateDto(stockId);
            comment.AppUserId = appUser.Id;
            await _commnetRepo.CreateCommentAsync(comment);
            return CreatedAtAction(nameof(GetById), new { id = comment }, comment.ToCommentDto());
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var commentModel = await _commnetRepo.DeleteAsync(id);

            if(commentModel == null)
            {
                return NotFound();
            }

            return Ok(commentModel);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCommentRequestDto updateDto)
        {
            var updateCommentModel = await _commnetRepo.UpdateAsync(id , updateDto.ToCommentFromUpdateDto());
            if(updateCommentModel == null)
            {
                return NotFound("This comment doesnt found");
            }
            return Ok(updateCommentModel);
        }
    }
}
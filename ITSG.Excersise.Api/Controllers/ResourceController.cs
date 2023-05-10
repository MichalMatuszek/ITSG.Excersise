using ITSG.Excersise.Application.Dtos;
using ITSG.Excersise.Application.Resources;
using ITSG.Excersise.Domain.Resources;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITSG.Excersise.Api.Controllers
{
    [Route("api/resources")]
    [ApiController]
    public class ResourceController : ControllerBase
    {
        private readonly IMediator _please;
        private IConfiguration _configuration;

        public ResourceController(IMediator please, IConfiguration configuration)
        {
            _please = please;
            _configuration = configuration;
        }

        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddResource(AddResourceDto request)
        {
            await _please.Send(new AddResourceCommand(request.Name));

            return Ok();
        }

        [HttpDelete, Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteResource(DeleteResourceDto request)
        {
            var result = await _please.Send(new DeleteResourceCommand(request.Id));

            return Ok(result);
        }

        [Route("{resourceId}")]
        [HttpGet]
        public async Task<IActionResult> GetResource(long resourceId)
        {
            var result = await _please.Send(new GetResourceByIdQuery(resourceId));

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [Route("[action]")]
        [HttpPut]
        public async Task<IActionResult> Lock([FromBody]LockResourceDto request)
        {
            LockResult result;
            if (request.IsPermanent)
            {
                result = await _please.Send(new LockResourcePermanentCommand(request.ResourceId, request.Version));
            }
            else
            {
                var blockInMinutes = int.Parse(_configuration["Resources:BlockInMinutes"]);

                result = await _please.Send(new LockResourceCommand(request.ResourceId, blockInMinutes, request.Version));
            }

            return result switch
            {
                LockResult.Success => Ok(),
                LockResult.NotFound => NotFound(),
                LockResult.NotAllowed => BadRequest(),
                LockResult.Conflicted => Conflict(),
                _ => Ok(),
            };
        }

        [Route("[action]")]
        [HttpPut]
        public async Task<IActionResult> Unlock([FromBody]UnlockResourceDto request)
        {
            var res = await _please.Send(new UnlockResourceCommand(request.ResourceId, request.Version));

            return res switch
            {
                LockResult.Success => Ok(),
                LockResult.NotFound => NotFound(),
                LockResult.NotAllowed => BadRequest(),
                LockResult.Conflicted => Conflict(),
                _ => Ok(),
            };
        }
    }
}

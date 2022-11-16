using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class WalksController : Controller {
        private readonly IWalkRepository walkRepository;
        public IMapper Mapper { get; }

        public WalksController(IWalkRepository walkRepository, IMapper mapper) {
            this.walkRepository = walkRepository;
            Mapper = mapper;
        }

        [HttpGet]

        public async Task<IActionResult> GetAllWalksAsync() {
            var walks = await walkRepository.GetAllAsync();

            var walksDto = Mapper.Map<List<Models.DTO.Walk>>(walks);
            return Ok(walksDto);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkAsync")]
        public async Task<IActionResult> GetWalkAsync(Guid id) {
            var walk = await walkRepository.GetAsync(id);

            if (walk == null)
                return NotFound();

            var walkDto = Mapper.Map<Models.DTO.Walk>(walk);

            return Ok(walkDto);
        }

        [HttpPost]
        public async Task<IActionResult> AddWalkAsync([FromBody]Models.DTO.AddWalkRequest addWalkRequest) {
            var walk = Mapper.Map<Models.Domain.Walk>(addWalkRequest);
            walk.Id = Guid.NewGuid();

            walk = await walkRepository.AddAsync(walk);

            var walkDto = Mapper.Map<Models.DTO.Walk>(walk);

            return CreatedAtAction(nameof(GetWalkAsync), new { id = walk.Id }, walkDto);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalkAsync(Guid id) {
            var walk = await walkRepository.DeleteAsync(id);

            if (walk == null)
                return NotFound(id);

            var walkDto = Mapper.Map<Models.DTO.Walk>(walk);

            return Ok(walkDto);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalkAsync([FromRoute]Guid id, [FromBody]Models.DTO.UpdateWalkRequest updateWalkRequest) {
            var walk = Mapper.Map<Models.Domain.Walk>(updateWalkRequest);

            var updatedWalk = await walkRepository.UpdateAsync(id, walk);

            if (updatedWalk == null)
                return NotFound(id);

            return Ok(updatedWalk);
        }
    }
}

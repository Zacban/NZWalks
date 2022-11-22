using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class WalksController : Controller {
        private readonly IWalkRepository walkRepository;
        private readonly IRegionRepository regionRepository;
        private readonly IWalkDifficultyRepository walkDifficultyRepository;

        public IMapper Mapper { get; }

        public WalksController(IWalkRepository walkRepository, IMapper mapper, IRegionRepository regionRepository, IWalkDifficultyRepository walkDifficultyRepository) {
            this.walkRepository = walkRepository;
            Mapper = mapper;
            this.regionRepository = regionRepository;
            this.walkDifficultyRepository = walkDifficultyRepository;
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
            if (!(await ValidateAddWalkAsync(addWalkRequest)))
                return BadRequest(ModelState);

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
            if (!(await ValidateUpdateWalkAsync(updateWalkRequest)))
                return BadRequest(ModelState);

            var walk = Mapper.Map<Models.Domain.Walk>(updateWalkRequest);

            var updatedWalk = await walkRepository.UpdateAsync(id, walk);

            if (updatedWalk == null)
                return NotFound(id);

            return Ok(updatedWalk);
        }

        private async Task<bool> ValidateAddWalkAsync(Models.DTO.AddWalkRequest addWalkRequest) {
            if (addWalkRequest == null) {
                ModelState.AddModelError(nameof(addWalkRequest), "Walk must not be null");
                return false;
            }

            if (string.IsNullOrWhiteSpace(addWalkRequest.Name))
                ModelState.AddModelError(nameof(addWalkRequest.Name), $"{nameof(addWalkRequest.Name)} must not be null, empty or whitespace");

            if (addWalkRequest.Length < 0.0)
                ModelState.AddModelError(nameof(addWalkRequest.Length), $"{nameof(addWalkRequest.Length)} must be larger than 0.0");

            var region = await regionRepository.GetAsync(addWalkRequest.RegionId);
            if (region == null)
                ModelState.AddModelError(nameof(addWalkRequest.RegionId), $"{nameof(addWalkRequest.RegionId)} is not a valid value");

            var walkDifficulty = await walkDifficultyRepository.GetAsync(addWalkRequest.WalkDifficultyId);
            if (walkDifficulty == null)
                ModelState.AddModelError(nameof(addWalkRequest.WalkDifficultyId), $"{addWalkRequest.WalkDifficultyId} is not a valid value");

            return ModelState.ErrorCount == 0;
        }

        private async Task<bool> ValidateUpdateWalkAsync(Models.DTO.UpdateWalkRequest updateWalkRequest) {
            if (updateWalkRequest == null) {
                ModelState.AddModelError(nameof(updateWalkRequest), "Walk must not be null");
                return false;
            }

            if (string.IsNullOrWhiteSpace(updateWalkRequest.Name))
                ModelState.AddModelError(nameof(updateWalkRequest.Name), $"{nameof(updateWalkRequest.Name)} must not be null, empty or whitespace");

            if (updateWalkRequest.Length < 0.0)
                ModelState.AddModelError(nameof(updateWalkRequest.Length), $"{nameof(updateWalkRequest.Length)} must be larger than 0.0");

            var region = await regionRepository.GetAsync(updateWalkRequest.RegionId);
            if (region == null)
                ModelState.AddModelError(nameof(updateWalkRequest.RegionId), $"{nameof(updateWalkRequest.RegionId)} is not a valid value");

            var walkDifficulty = await walkDifficultyRepository.GetAsync(updateWalkRequest.WalkDifficultyId);
            if (walkDifficulty == null)
                ModelState.AddModelError(nameof(updateWalkRequest.WalkDifficultyId), $"{updateWalkRequest.WalkDifficultyId} is not a valid value");

            return ModelState.ErrorCount == 0;
        }
    }
}

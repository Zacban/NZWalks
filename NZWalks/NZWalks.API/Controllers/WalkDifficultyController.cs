using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class WalkDifficultyController : Controller {
        public IWalkDifficultyRepository WalkDifficultyRepository { get; }
        public IMapper Mapper { get; }
        public WalkDifficultyController(IWalkDifficultyRepository walkDifficultyRepository, IMapper mapper) {
            WalkDifficultyRepository = walkDifficultyRepository;
            Mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync() {
            var walkdifficulties = await WalkDifficultyRepository.GetAllAsync();

            var walkDto = Mapper.Map<List<Models.DTO.WalkDifficulty>>(walkdifficulties);

            return Ok(walkDto);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkDifficultyAsync")]
        public async Task<IActionResult> GetWalkDifficultyAsync(Guid id) {
            var walkdifficulty = await WalkDifficultyRepository.GetAsync(id);
            if (walkdifficulty == null)
                return NotFound(id);

            var walkDto = Mapper.Map<Models.DTO.WalkDifficulty>(walkdifficulty);

            return Ok(walkDto);
        }

        [HttpPost]
        public async Task<IActionResult> AddWalkDifficultyAsync(Models.DTO.AddWalkDifficultyRequest addWalkDifficulty) {
            if (!ValidateAddWalkAsync(addWalkDifficulty))
                return BadRequest(ModelState);

            var walk = new Models.Domain.WalkDifficulty() {
                Code = addWalkDifficulty.Code
            };

            walk = await WalkDifficultyRepository.AddAsync(walk);


            return CreatedAtAction(nameof(GetWalkDifficultyAsync), new { id = walk.Id }, walk);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalkDifficultyAsync([FromRoute] Guid id, [FromBody]Models.DTO.UpdateWalkDifficultyRequest updateWalkDifficultyRequest) {
            if (!ValidateUpdateWalkAsync(updateWalkDifficultyRequest))
                return BadRequest(ModelState);

            var walk = new Models.Domain.WalkDifficulty {
                Code = updateWalkDifficultyRequest.Code
            };

            walk = await WalkDifficultyRepository.UpdateAsync(id, walk);
            if (walk == null)
                return NotFound(id);

            var walkDto = Mapper.Map<Models.DTO.WalkDifficulty>(walk);

            return Ok(walkDto);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalkDifficultyAsync(Guid id) {
            var walk = await WalkDifficultyRepository.DeleteAsync(id);

            if (walk == null)
                return NotFound(id);

            var walkDto = Mapper.Map<Models.DTO.WalkDifficulty>(walk);

            return Ok(walkDto);
        }

        private bool ValidateAddWalkAsync(Models.DTO.AddWalkDifficultyRequest walk) {
            if (walk == null) {
                ModelState.AddModelError(nameof(walk), $"{nameof(walk)} must not be null");
                return false;
            }

            if (string.IsNullOrWhiteSpace(walk.Code))
                ModelState.AddModelError(nameof(walk.Code), $"{nameof(walk.Code)} must not be null, empty or white space");

            return ModelState.ErrorCount == 0;
        }

        private bool ValidateUpdateWalkAsync(Models.DTO.UpdateWalkDifficultyRequest walk) {
            if (walk == null) {
                ModelState.AddModelError(nameof(walk), $"{nameof(walk)} must not be null");
                return false;
            }

            if (string.IsNullOrWhiteSpace(walk.Code))
                ModelState.AddModelError(nameof(walk.Code), $"{nameof(walk.Code)} must not be null, empty or white space");

            return ModelState.ErrorCount == 0;
        }
    }
}

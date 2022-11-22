using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class RegionsController : Controller {
        private readonly IRegionRepository regionRepository;
        public IMapper Mapper { get; }

        public RegionsController(IRegionRepository regionRepository, IMapper mapper) {
            this.regionRepository = regionRepository;
            Mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRegionsAsync() {
            var regions = await regionRepository.GetAllAsync();
            var regionsDto = Mapper.Map<List<Models.DTO.Region>>(regions);

            return Ok(regionsDto);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetRegionAsync")]
        public async Task<IActionResult> GetRegionAsync(Guid id) {
            var region = await regionRepository.GetAsync(id);
            if (region == null)
                return NotFound();

            var regionDto = Mapper.Map<Models.DTO.Region>(region);
            return Ok(regionDto);
        }
        
        [HttpPost]
        public async Task<IActionResult> AddRegionAsync(Models.DTO.AddRegionRequest addRegionRequest) {
            if (!ValidateAddRegionRequest(addRegionRequest))
                return BadRequest(ModelState);

            var region = new Region()
            {
                Area = addRegionRequest.Area,
                Code = addRegionRequest.Code,
                Latitude = addRegionRequest.Latitude,
                Longitude = addRegionRequest.Longitude,
                Name = addRegionRequest.Name,
                Population = addRegionRequest.Population
            };

            region = await regionRepository.AddAsync(region);

            var regionDto = Mapper.Map<Models.DTO.Region>(region);

            return CreatedAtAction(nameof(GetRegionAsync), new { id = regionDto.Id }, regionDto);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteRegionAsync(Guid id) {
            var region = await regionRepository.DeleteAsync(id);

            if (region == null)
                return NotFound();
            var regionDto = Mapper.Map<Models.DTO.Region>(region);

            return Ok(regionDto);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateRegionAsync([FromRoute]Guid id, [FromBody]Models.DTO.UpdateRegionRequest updateRegionRequest) {

            if (!ValidateUpdateRegionRequest(updateRegionRequest))
                return BadRequest(ModelState);

            var region = new Region()
            {
                Area = updateRegionRequest.Area,
                Code = updateRegionRequest.Code,
                Latitude = updateRegionRequest.Latitude,
                Longitude = updateRegionRequest.Longitude,
                Name = updateRegionRequest.Name,
                Population = updateRegionRequest.Population
            };
            var updatedRegion = await regionRepository.UpdateAsync(id, region);

            if (updatedRegion == null)
                return NotFound();

            var regionDto = Mapper.Map<Models.DTO.Region>(updatedRegion); ;
            return Ok(regionDto);
        }

        private bool ValidateAddRegionRequest(Models.DTO.AddRegionRequest addRegionRequest) {
            if (string.IsNullOrWhiteSpace(addRegionRequest.Code)) 
                ModelState.AddModelError(nameof(addRegionRequest.Code), $"{nameof(addRegionRequest.Code)} cannot be empty");

            if (string.IsNullOrWhiteSpace(addRegionRequest.Name))
                ModelState.AddModelError(nameof(addRegionRequest.Name), $"{nameof(addRegionRequest.Name)} cannot be empty");

            if (addRegionRequest.Area <= 0)
                ModelState.AddModelError(nameof(addRegionRequest.Area), $"{nameof(addRegionRequest.Area)} must be a positive number");

            if (addRegionRequest.Population < 0)
                ModelState.AddModelError(nameof(addRegionRequest.Population), $"{nameof(addRegionRequest.Population)} must be a positive number");

            return ModelState.ErrorCount == 0;
        }

        private bool ValidateUpdateRegionRequest(Models.DTO.UpdateRegionRequest updateRegionRequest) {
            if (string.IsNullOrWhiteSpace(updateRegionRequest.Code))
                ModelState.AddModelError(nameof(updateRegionRequest.Code), $"{nameof(updateRegionRequest.Code)} cannot be empty");

            if (string.IsNullOrWhiteSpace(updateRegionRequest.Name))
                ModelState.AddModelError(nameof(updateRegionRequest.Name), $"{nameof(updateRegionRequest.Name)} cannot be empty");

            if (updateRegionRequest.Area <= 0)
                ModelState.AddModelError(nameof(updateRegionRequest.Area), $"{nameof(updateRegionRequest.Area)} must be a positive number");

            if (updateRegionRequest.Population < 0)
                ModelState.AddModelError(nameof(updateRegionRequest.Population), $"{nameof(updateRegionRequest.Population)} must be a positive number");

            return ModelState.ErrorCount == 0;
        }
    }
}

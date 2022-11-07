using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class RegionsController : Controller {
        private readonly IRegionRepository regionRepository;

        public RegionsController(IRegionRepository regionRepository, IMapper mapper) {
            this.regionRepository = regionRepository;
            Mapper = mapper;
        }

        public IMapper Mapper { get; }

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
        public async Task<IActionResult> UpdateAsync([FromRoute]Guid id, [FromBody]Models.DTO.UpdateRegionRequest updateRegionRequest) {
            var region = new Models.Domain.Region()
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
    }
}

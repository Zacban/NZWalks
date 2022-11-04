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
        public async Task<IActionResult> GetAllRegions() {
            var regions = await regionRepository.GetAllAsync();
            var regionsDto = Mapper.Map<List<Models.DTO.Region>>(regions);
            //var regionsDto = new List<Models.DTO.Region>();
            //regions.ToList().ForEach(domainregion =>
            //{
            //    var regionDto = new Models.DTO.Region()
            //    {
            //        Id = domainregion.Id,
            //        Area = domainregion.Area,
            //        Code = domainregion.Code,
            //        Latitude = domainregion.Latitude,
            //        Longitude = domainregion.Longitude,
            //        Name = domainregion.Name,
            //        Population = domainregion.Population
            //    };
            //    regionsDto.Add(regionDto);

            //});


            return Ok(regionsDto);
        }
    }
}

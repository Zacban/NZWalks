using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories {
    public class RegionRepository : IRegionRepository {
        private readonly NZWalksDbContext context;

        public RegionRepository(NZWalksDbContext context) {
            this.context = context;
        }

        public async Task<Region> AddAsync(Region region) {
            region.Id = Guid.NewGuid();
            await context.AddAsync(region);
            await context.SaveChangesAsync();

            return region;
        }

        public async Task<Region> DeleteAsync(Guid id) {
            var region = await context.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if (region == null)
                return null;

            context.Regions.Remove(region);
            await context.SaveChangesAsync();

            return region;
        }

        public async Task<IEnumerable<Region>> GetAllAsync() {
            return await context.Regions.ToListAsync();

        }
        public async Task<Region> GetAsync(Guid id) {
            return await context.Regions.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Region> UpdateAsync(Guid id, Region region) {
            var existingRegion = await context.Regions.FirstOrDefaultAsync(x => x.Id == id);

            if (existingRegion == null)
                return null;

            existingRegion.Population = region.Population;
            existingRegion.Name = region.Name;
            existingRegion.Area = region.Area;
            existingRegion.Code = region.Code;
            existingRegion.Latitude = region.Latitude;
            existingRegion.Longitude = region.Longitude;

            await context.SaveChangesAsync();

            return existingRegion;
        }
    }
}

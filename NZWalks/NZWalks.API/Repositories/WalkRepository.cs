using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories {
    public class WalkRepository : IWalkRepository {
        private readonly NZWalksDbContext context;

        public WalkRepository(NZWalksDbContext context) {
            this.context = context;
        }
        public async Task<Walk> AddAsync(Walk walk) {
            walk.Id = Guid.NewGuid();
            await context.AddAsync(walk);
            await context.SaveChangesAsync();

            return walk;
        }

        public async Task<Walk> DeleteAsync(Guid id) {
            var walk = await context.Walks.FirstOrDefaultAsync(x => x.Id == id);
            if (walk == null)
                return null;

            context.Walks.Remove(walk);
            await context.SaveChangesAsync();

            return walk;

        }

        public async Task<IEnumerable<Walk>> GetAllAsync() {
            return await context.Walks
                .Include(x => x.Region)
                .Include(x => x.WalkDifficulty)
                .ToListAsync();
        }

        public async Task<Walk> GetAsync(Guid id) {
            return await context.Walks
                .Include(x => x.Region)
                .Include(x => x.WalkDifficulty)
                .FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task<Walk> UpdateAsync(Guid id, Walk walk) {
            var existingWalk = await context.Walks.FindAsync(id);

            if (existingWalk == null)
                return null;

            existingWalk.WalkDifficulty = walk.WalkDifficulty;
            existingWalk.WalkDifficultyId = walk.WalkDifficultyId;
            existingWalk.Name = walk.Name;
            existingWalk.Length = walk.Length;
            existingWalk.RegionId = walk.RegionId;

            await context.SaveChangesAsync();

            return existingWalk;
        }
    }
}

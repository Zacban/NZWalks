using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Repositories {
    public class WalkDifficultyRepository : IWalkDifficultyRepository {
        private readonly NZWalksDbContext context;

        public WalkDifficultyRepository(NZWalksDbContext context) {
            this.context = context;
        }

        public async Task<WalkDifficulty> AddAsync(WalkDifficulty walkDifficulty) {
            walkDifficulty.Id = Guid.NewGuid();

            await context.AddAsync(walkDifficulty);
            await context.SaveChangesAsync();

            return walkDifficulty;
        }

        public async Task<WalkDifficulty> DeleteAsync(Guid id) {
            var exisisting = await context.WalkDifficulties.FindAsync(id);

            if (exisisting == null)
                return null;

            context.WalkDifficulties.Remove(exisisting);
            await context.SaveChangesAsync();

            return exisisting;
            
        }

        public async Task<IEnumerable<WalkDifficulty>> GetAllAsync() {
            return await context.WalkDifficulties.ToListAsync();
        }

        public async Task<WalkDifficulty> GetAsync(Guid id) {
            return await context.WalkDifficulties.FindAsync(id);
        }

        public async Task<WalkDifficulty> UpdateAsync(Guid id, WalkDifficulty walkDifficulty) {
            var existing = await context.WalkDifficulties.FindAsync(id);

            if (existing == null)
                return null;

            existing.Code = walkDifficulty.Code;

            await context.SaveChangesAsync();

            return existing;
        }
    }
}

﻿using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories {
    public interface IWalkRepository {
        Task<IEnumerable<Walk>> GetAllAsync();
        Task<Walk> GetAsync(Guid id);
        Task<Walk> DeleteAsync(Guid id);
        Task<Walk> UpdateAsync(Guid id, Walk walk);
        Task<Walk> AddAsync(Walk walk);
    }
}

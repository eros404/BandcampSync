﻿using Eros404.BandcampSync.Core.Models;

namespace Eros404.BandcampSync.Core.Services
{
    public interface IBandcampApiService
    {
        Task<Collection?> GetCollectionAsync(int fanId,int count = 100);
        Task<int?> GetFanIdAsync();
    }
}
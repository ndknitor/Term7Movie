﻿using Term7MovieCore.Data.Dto;
using Term7MovieCore.Entities;

namespace Term7MovieRepository.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<CategoryDTO>> GetAllCategory();

    }
}

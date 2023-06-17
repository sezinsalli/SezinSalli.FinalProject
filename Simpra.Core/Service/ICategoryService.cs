﻿using Simpra.Core.Entity;

namespace Simpra.Core.Service
{
    public interface ICategoryService : IBaseService<Category>
    {
        Task<Category> GetCategoryByIdWithProductAsync(int categoryId);

    }
}

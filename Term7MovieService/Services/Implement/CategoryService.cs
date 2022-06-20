using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Term7MovieCore.Data;
using Term7MovieCore.Data.Dto;
using Term7MovieCore.Data.Response;
using Term7MovieCore.Entities;
using Term7MovieRepository.Repositories.Interfaces;
using Term7MovieService.Services.Interface;

namespace Term7MovieService.Services.Implement
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICategoryRepository cateRepository;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            cateRepository = _unitOfWork.CategoryRepository;
        }

        public async Task<CategoryResponse> GetFullCategory()
        {
            IEnumerable<CategoryDTO> list = await cateRepository.GetAllCategory();

            return new CategoryResponse
            {
                Message = Constants.MESSAGE_SUCCESS,
                Categories = list
            };
        }
    }
}

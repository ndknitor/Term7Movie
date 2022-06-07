using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            IEnumerable<Category> rawdata = await cateRepository.GetAllCategory();
            if (rawdata == null)
                return new CategoryResponse { Message = "Can't access data storage" };
            if (!rawdata.Any())
                return new CategoryResponse { Message = "No category given in data storage" };
            CategoryResponse response = new CategoryResponse();
            List<CategoryDTO> result = new List<CategoryDTO>();
            foreach(var category in rawdata)
            {
                CategoryDTO dto = new CategoryDTO();
                dto.Id = category.Id;
                dto.Name = category.Name;
                result.Add(dto);
            }
            response.categories = result;
            response.Message = "Successful";
            return response;
        }
    }
}

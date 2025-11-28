using AutoMapper;
using DAIS.CoreBusiness.Dtos;
using DAIS.CoreBusiness.Interfaces;
using DAIS.DataAccess.Entities;
using DAIS.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAIS.CoreBusiness.Services
{
    public class CategoryService : ICategoryService
    {
        private IGenericRepository<Category> _genericRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryService> _logger;
        public CategoryService(IGenericRepository<Category> genericRepo, IMapper mapper, ILogger<CategoryService> logger)
        {
            _genericRepo = genericRepo;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<CategoryDto> AddCategory(CategoryDto categoryDto)
        {
            _logger.LogInformation("CategoryService:AddCategory:Method Start");
            try
            {
                if (!string.IsNullOrEmpty(categoryDto.CategoryName))
                {
                    categoryDto.CategoryName = categoryDto.CategoryName.ToUpper();
                }
                var category = _mapper.Map<Category>(categoryDto);
                await _genericRepo.Add(category);
            }
            catch (Exception ex) 
            { 
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("CategoryService:AddCategory:Method End");
            return categoryDto;


        }

        public async Task DeleteCategory(Guid id)
        {
            _logger.LogInformation("CategoryService:DeleteCategory:Method Start");
            try
            {
                var category = await _genericRepo.GetById(id);
                await _genericRepo.Remove(category);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("CategoryService:DeleteCategory:Method End");
        }

        public async Task<List<CategoryDto>> GetAllCategory()
        {
            _logger.LogInformation("CategoryService:GetAllCategory:Method Start");

            List<CategoryDto> categoryDtoList = new List<CategoryDto>();
            try
            {

                var categoryList = await _genericRepo.Query()
                 .Include(x => x.Project)
                 .Include(x => x.MaterialType).ToListAsync().ConfigureAwait(false);
                foreach (var category in categoryList)
                {
                    var categoryDto = _mapper.Map<CategoryDto>(category);
                    categoryDto.MaterialTypeName = category.MaterialType.TypeName;
                    categoryDtoList.Add(categoryDto);
                }
                

            }
            catch (Exception ex) 
            {
                _logger.LogError(ex.Message, ex);
                throw ex;

            }
            _logger.LogInformation("CategoryService:GetAllCategory:Method End");
            return categoryDtoList;
        }

        public async Task<CategoryDto> GetCategoryTypeById(Guid id)
        {
            _logger.LogInformation("CategoryService:GetCategoryTypeById:Method Start");
            CategoryDto categoryDto = new CategoryDto();
            try
            {
                var category = await _genericRepo.Query()
                 .Include(x => x.Project)
                 .Include(x => x.MaterialType).FirstOrDefaultAsync(x=>x.Id==id).ConfigureAwait(false);
                // var category = await _genericRepo.GetById(id);
                categoryDto = _mapper.Map<CategoryDto>(category);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("CategoryService:GetCategoryTypeById:Method Start");
            return categoryDto;

        }




        public async Task<CategoryDto> UpdateCategory(CategoryDto categoryDto)
        {
            _logger.LogInformation("CategoryService:UpdateCategory:Method Start");
            try
            {
                
                var category= _mapper.Map<Category>(categoryDto);

                await _genericRepo.Update(category);

            }
            catch (Exception ex) 
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("CategoryService:UpdateCategory:Method End");
            return categoryDto;
        }

        public CategoryDto GetCategoryIdByName(string name)
        {
            _logger.LogInformation("CategoryService:GetCategoryIdByName:Method Start");
            CategoryDto categoryDto = new CategoryDto();
            try
            {
                var category = _genericRepo.Query()
                 .FirstOrDefault(x => x.CategoryName == name);              
                categoryDto = _mapper.Map<CategoryDto>(category);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("CategoryService:GetCategoryIdByName:Method End");
            return categoryDto;
        }

        public async Task<IEnumerable<CategoryDto>> GetCategoriesByMaterialType(Guid typeId)
        {
            return await _genericRepo.Query()
                .Where(x => x.MaterialTypeId == typeId) // use the correct property from Category entity
                .Select(x => new CategoryDto
                {
                    Id = x.Id,
                    CategoryName = x.CategoryName,
                    MaterialTypeId = x.MaterialTypeId
                })
                .ToListAsync();
        }

    }
}

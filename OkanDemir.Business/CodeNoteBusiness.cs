using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OkanDemir.Business.Encyription;
using OkanDemir.Business.Filters;
using OkanDemir.Business.Mapper;
using OkanDemir.Business.Utilities;
using OkanDemir.Data.Repository;
using OkanDemir.Dto;
using OkanDemir.Dto.Validation;
using OkanDemir.Model;
using System.Collections.Generic;

namespace OkanDemir.Business
{
    public class CodeNoteBusiness
    {
        private readonly IRepository<CodeNote> _codeNoteRepository;

        public CodeNoteBusiness(IRepository<CodeNote> _codeNoteRepository)
        {
            this._codeNoteRepository = _codeNoteRepository;
        }

        public DataTableViewModelResult<List<CodeNoteDto>> GetAll(CodeNoteFilterModel filter)
        {
            var response = new DataTableViewModelResult<List<CodeNoteDto>>();
            response.IsSucceeded = true;

            var result = _codeNoteRepository.ListQueryableNoTracking
                .Include(z=>z.CodeCategory)
                .OrderBy(y => y.CreateDate)
                .Select(y => new CodeNoteDto()
                {
                    CodeCategoryName = y.CodeCategory.Title,
                    UserId = y.UserId,
                    Id = y.Id,
                    Title = y.Title,
                    CodeCategoryId = y.CodeCategoryId,
                });

            response.TotalCount = result.Count();
            response.RecordsFiltered = result.AddSearchFilters(filter).Count();
            response.Data = result.AddSearchFilters(filter).AddOrderAndPageFilters(filter).ToList();

            return response;
        }

        public DbOperationResult Create(CodeNoteDto mDto)
        {
            var validCheck = new CodeNoteValidation().Validate(mDto);
            if (!validCheck.IsValid)
            {
                var errors = new List<string>();
                validCheck.Errors.ForEach(x => errors.Add(x.ErrorMessage));
                return new DbOperationResult(false, "Eksik veya hatalı veri girişi", errors);
            }

            try
            {
                var model = ObjectMapper.Mapper.Map<CodeNote>(mDto);
                model.Description = mDto.Description ?? "";
                model.Summary = mDto.Summary ?? "";
                
                var operationResult = _codeNoteRepository.Insert(model);
                if (operationResult != null)
                    return new DbOperationResult(true, "Veri Eklendi");
                else
                    return new DbOperationResult(false, "Veri Eklenemedi");
            }
            catch (Exception ex)
            {
                return new DbOperationResult(false, ex.Message);
            }
        }

        public DbOperationResult Update(CodeNoteDto mDto)
        {
            var validCheck = new CodeNoteValidation().Validate(mDto);
            if (!validCheck.IsValid)
            {
                var errors = new List<string>();
                validCheck.Errors.ForEach(x => errors.Add(x.ErrorMessage));
                return new DbOperationResult(false, "Eksik veya hatalı veri girişi", errors);
            }

            try
            {
                var modelInDb = _codeNoteRepository.ListQueryable
                    .FirstOrDefault(x => x.Id == mDto.Id);

                modelInDb.UpdateDate = DateTime.Now;
                modelInDb.Description = mDto.Description ?? "";
                modelInDb.Summary = mDto.Summary ?? "";
                modelInDb.Code = mDto.Code ?? "";
                modelInDb.Title = mDto.Title ?? "";
                modelInDb.CodeCategoryId = mDto.CodeCategoryId;

                var operationResult = _codeNoteRepository.Update(modelInDb);
                if (operationResult != null)
                    return new DbOperationResult(true, "Veri Güncellendi");
                else
                    return new DbOperationResult(false, "Veri Güncellenemedi");
            }
            catch (Exception ex)
            {
                return new DbOperationResult(false, ex.Message);
            }
        }

        public CodeNoteDto Get(int userId, int id)
        {
            var data = _codeNoteRepository.ListQueryable
                .FirstOrDefault(x => x.Id == id && x.UserId == userId);

            if (data == null)
                return null;

            var model = ObjectMapper.Mapper.Map<CodeNoteDto>(data);
            return model;
        }

        public DbOperationResult Delete(int userId, int id)
        {
            var data = _codeNoteRepository.ListQueryable
                .Where(x => x.UserId == userId && x.Id == id).FirstOrDefault();

            if (data == null)
                return new DbOperationResult(false, "Veri bulunamadı");

            try
            {
                _codeNoteRepository.Delete(data);
                return new DbOperationResult(true, "Veri Silindi");
            }
            catch (Exception ex)
            {
                return new DbOperationResult(false, ex.Message);
            }
        }
    }
}

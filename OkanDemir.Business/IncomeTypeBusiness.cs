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
    public class IncomeTypeBusiness
    {
        private readonly IRepository<IncomeType> _incomeTypeRepository;

        public IncomeTypeBusiness(IRepository<IncomeType> _incomeTypeRepository)
        {
            this._incomeTypeRepository = _incomeTypeRepository;
        }

        public DataTableViewModelResult<List<IncomeTypeDto>> GetAll(IncomeTypeFilterModel filter)
        {
            var response = new DataTableViewModelResult<List<IncomeTypeDto>>();
            response.IsSucceeded = true;

            var result = _incomeTypeRepository.ListQueryableNoTracking
                .OrderBy(y => y.CreateDate)
                .Where(y => !y.IsDeleted)
                .Select(y => new IncomeTypeDto()
                {
                    Id = y.Id,
                    IsActive = y.IsActive,
                    Name = y.Name,
                    Period = y.Period,
                    UserId = y.UserId,
                });

            response.TotalCount = result.Count();
            response.RecordsFiltered = result.AddSearchFilters(filter).Count();
            response.Data = result.AddSearchFilters(filter).AddOrderAndPageFilters(filter).ToList();

            return response;
        }

        public DbOperationResult Create(IncomeTypeDto mDto)
        {
            var validCheck = new IncomeTypeValidation().Validate(mDto);
            if (!validCheck.IsValid)
            {
                var errors = new List<string>();
                validCheck.Errors.ForEach(x => errors.Add(x.ErrorMessage));
                return new DbOperationResult(false, "Eksik veya hatalı veri girişi", errors);
            }

            try
            {
                var model = ObjectMapper.Mapper.Map<IncomeType>(mDto);
                var operationResult = _incomeTypeRepository.Insert(model);
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

        public DbOperationResult Update(IncomeTypeDto mDto)
        {
            var validCheck = new IncomeTypeValidation().Validate(mDto);
            if (!validCheck.IsValid)
            {
                var errors = new List<string>();
                validCheck.Errors.ForEach(x => errors.Add(x.ErrorMessage));
                return new DbOperationResult(false, "Eksik veya hatalı veri girişi", errors);
            }

            try
            {
                var model = ObjectMapper.Mapper.Map<IncomeType>(mDto);
                var operationResult = _incomeTypeRepository.Update(model);
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

        public IncomeTypeDto Get(int userId, int id)
        {
            var data = _incomeTypeRepository.ListQueryable
                .FirstOrDefault(x => x.Id == id && x.UserId == userId && !x.IsDeleted);

            if (data == null)
                return null;

            var model = ObjectMapper.Mapper.Map<IncomeTypeDto>(data);
            return model;
        }

        public DbOperationResult Delete(int userId, int id)
        {
            var data = _incomeTypeRepository.ListQueryable
                .Where(x => x.UserId == userId && x.Id == id && !x.IsDeleted).FirstOrDefault();

            if (data == null)
                return new DbOperationResult(false, "Veri bulunamadı");

            try
            {
                data.IsDeleted = true;
                data.IsActive = false;
                var operationResult = _incomeTypeRepository.Update(data);
                if (operationResult != null)
                    return new DbOperationResult(true, "Veri Silindi");
                else
                    return new DbOperationResult(false, "Veri Silinemedi");
            }
            catch (Exception ex)
            {
                return new DbOperationResult(false, ex.Message);
            }
        }

        public DbOperationResult Active(int userId, int id)
        {
            var data = _incomeTypeRepository.ListQueryable
                .Where(x => x.UserId == userId && x.Id == id).FirstOrDefault();

            if (data == null)
                return new DbOperationResult(false, "Veri bulunamadı");

            try
            {
                data.IsActive = true;
                var operationResult = _incomeTypeRepository.Update(data);
                if (operationResult != null)
                    return new DbOperationResult(true, "Veri aktif olarak işaretlendi");
                else
                    return new DbOperationResult(false, "Veri aktif edilemedi");
            }
            catch (Exception ex)
            {
                return new DbOperationResult(false, ex.Message);
            }
        }

        public DbOperationResult Passive(int userId, int id)
        {
            var data = _incomeTypeRepository.ListQueryable
                .Where(x => x.UserId == userId && x.Id == id).FirstOrDefault();

            if (data == null)
                return new DbOperationResult(false, "Veri bulunamadı");

            try
            {
                data.IsActive = false;
                var operationResult = _incomeTypeRepository.Update(data);
                if (operationResult != null)
                    return new DbOperationResult(true, "Veri pasif olarak işaretlendi");
                else
                    return new DbOperationResult(false, "Veri aktif edilemedi");
            }
            catch (Exception ex)
            {
                return new DbOperationResult(false, ex.Message);
            }
        }

        public List<IncomeTypeDto> GetAllActiveList(int userId)
        {
            var data = _incomeTypeRepository.ListQueryableNoTracking
                .Where(x => x.UserId == userId && x.IsActive && !x.IsDeleted).ToList();

            return ObjectMapper.Mapper.Map<List<IncomeTypeDto>>(data);
        }
    }
}

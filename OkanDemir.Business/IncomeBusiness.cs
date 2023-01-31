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
    public class IncomeBusiness
    {
        private readonly IRepository<Income> _incomeRepository;

        public IncomeBusiness(IRepository<Income> _incomeRepository)
        {
            this._incomeRepository = _incomeRepository;
        }

        public DataTableViewModelResult<List<IncomeDto>> GetAll(IncomeFilterModel filter)
        {
            var response = new DataTableViewModelResult<List<IncomeDto>>();
            response.IsSucceeded = true;

            var result = _incomeRepository.ListQueryableNoTracking
                .Include(z=>z.IncomeType)
                .OrderBy(y => y.CreateDate)
                .Select(y => new IncomeDto()
                {
                    IncomeTypeName = y.IncomeType.Name,
                    Price = y.Price,
                    PaymentDate = y.PaymentDate,
                    HasPayment = y.HasPayment,
                    UserId = y.UserId,
                    FilePath = y.FilePath,
                    IncomeTypeId = y.IncomeTypeId,
                    Id = y.Id,
                });

            response.TotalCount = result.Count();
            response.RecordsFiltered = result.AddSearchFilters(filter).Count();
            response.Data = result.AddSearchFilters(filter).AddOrderAndPageFilters(filter).ToList();

            return response;
        }

        public DbOperationResult Create(IncomeDto mDto)
        {
            var validCheck = new IncomeValidation().Validate(mDto);
            if (!validCheck.IsValid)
            {
                var errors = new List<string>();
                validCheck.Errors.ForEach(x => errors.Add(x.ErrorMessage));
                return new DbOperationResult(false, "Eksik veya hatalı veri girişi", errors);
            }

            try
            {
                var model = ObjectMapper.Mapper.Map<Income>(mDto);
                model.FilePath = mDto.FilePath ?? "";
                var operationResult = _incomeRepository.Insert(model);
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

        public DbOperationResult Update(IncomeDto mDto)
        {
            var validCheck = new IncomeValidation().Validate(mDto);
            if (!validCheck.IsValid)
            {
                var errors = new List<string>();
                validCheck.Errors.ForEach(x => errors.Add(x.ErrorMessage));
                return new DbOperationResult(false, "Eksik veya hatalı veri girişi", errors);
            }

            try
            {
                var modelInDb = _incomeRepository.ListQueryable
                    .FirstOrDefault(x => x.Id == mDto.Id);

                modelInDb.Price = mDto.Price;
                modelInDb.HasPayment = mDto.HasPayment;
                modelInDb.PaymentDate = mDto.PaymentDate;
                modelInDb.IncomeTypeId = mDto.IncomeTypeId;
                modelInDb.UpdateDate = DateTime.Now;

                if (!string.IsNullOrEmpty(mDto.FilePath) && modelInDb.FilePath != mDto.FilePath)
                    modelInDb.FilePath = mDto.FilePath;

                var operationResult = _incomeRepository.Update(modelInDb);
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

        public IncomeDto Get(int userId, int id)
        {
            var data = _incomeRepository.ListQueryable
                .FirstOrDefault(x => x.Id == id && x.UserId == userId);

            if (data == null)
                return null;

            var model = ObjectMapper.Mapper.Map<IncomeDto>(data);
            return model;
        }

        public DbOperationResult Delete(int userId, int id)
        {
            var data = _incomeRepository.ListQueryable
                .Where(x => x.UserId == userId && x.Id == id).FirstOrDefault();

            if (data == null)
                return new DbOperationResult(false, "Veri bulunamadı");

            try
            {
                _incomeRepository.Delete(data);
                return new DbOperationResult(true, "Veri Silindi");
            }
            catch (Exception ex)
            {
                return new DbOperationResult(false, ex.Message);
            }
        }

        public DbOperationResult Payment(int userId, int id)
        {
            var data = _incomeRepository.ListQueryable
                .Where(x => x.UserId == userId && x.Id == id).FirstOrDefault();

            if (data == null)
                return new DbOperationResult(false, "Veri bulunamadı");

            try
            {
                data.HasPayment = true;
                var operationResult = _incomeRepository.Update(data);
                if (operationResult != null)
                    return new DbOperationResult(true, "Veri ödendi olarak işaretlendi");
                else
                    return new DbOperationResult(false, "Veri ödendi olrak işaretlenemedi");
            }
            catch (Exception ex)
            {
                return new DbOperationResult(false, ex.Message);
            }
        }

        public DbOperationResult UnPayment(int userId, int id)
        {
            var data = _incomeRepository.ListQueryable
                .Where(x => x.UserId == userId && x.Id == id).FirstOrDefault();

            if (data == null)
                return new DbOperationResult(false, "Veri bulunamadı");

            try
            {
                data.HasPayment = false;
                var operationResult = _incomeRepository.Update(data);
                if (operationResult != null)
                    return new DbOperationResult(true, "Veri ödenmedi olarak işaretlendi");
                else
                    return new DbOperationResult(false, "Veri ödenmedi olarak işaretlenemedi");
            }
            catch (Exception ex)
            {
                return new DbOperationResult(false, ex.Message);
            }
        }
    }
}

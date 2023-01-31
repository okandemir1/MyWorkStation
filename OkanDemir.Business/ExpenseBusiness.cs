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
    public class ExpenseBusiness
    {
        private readonly IRepository<Expense> _expenseRepository;

        public ExpenseBusiness(IRepository<Expense> _expenseRepository)
        {
            this._expenseRepository = _expenseRepository;
        }

        public DataTableViewModelResult<List<ExpenseDto>> GetAll(ExpenseFilterModel filter)
        {
            var response = new DataTableViewModelResult<List<ExpenseDto>>();
            response.IsSucceeded = true;

            var result = _expenseRepository.ListQueryableNoTracking
                .OrderByDescending(y => y.CreateDate)
                .Select(y => new ExpenseDto()
                {
                    UserId = y.UserId,
                    Title = y.Title,
                    ExpenseDate = y.ExpenseDate,
                    FilePath = y.FilePath,
                    HasPayment = y.HasPayment,
                    Id = y.Id,
                    Price = y.Price,
                    Summary = y.Summary,
                });

            response.TotalCount = result.Count();
            response.RecordsFiltered = result.AddSearchFilters(filter).Count();
            response.Data = result.AddSearchFilters(filter).AddOrderAndPageFilters(filter).ToList();

            return response;
        }

        public DbOperationResult Create(ExpenseDto mDto)
        {
            var validCheck = new ExpenseValidation().Validate(mDto);
            if (!validCheck.IsValid)
            {
                var errors = new List<string>();
                validCheck.Errors.ForEach(x => errors.Add(x.ErrorMessage));
                return new DbOperationResult(false, "Eksik veya hatalı veri girişi", errors);
            }

            try
            {
                var model = ObjectMapper.Mapper.Map<Expense>(mDto);
                model.FilePath = mDto.FilePath ?? "";

                var operationResult = _expenseRepository.Insert(model);
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

        public DbOperationResult Update(ExpenseDto mDto)
        {
            var validCheck = new ExpenseValidation().Validate(mDto);
            if (!validCheck.IsValid)
            {
                var errors = new List<string>();
                validCheck.Errors.ForEach(x => errors.Add(x.ErrorMessage));
                return new DbOperationResult(false, "Eksik veya hatalı veri girişi", errors);
            }

            try
            {
                var modelInDb = _expenseRepository.ListQueryable
                    .FirstOrDefault(x => x.Id == mDto.Id);

                modelInDb.Price = mDto.Price;
                modelInDb.HasPayment = mDto.HasPayment;
                modelInDb.ExpenseDate = mDto.ExpenseDate;
                modelInDb.Title = mDto.Title ?? "";
                modelInDb.Summary = mDto.Summary ?? "";
                modelInDb.UpdateDate = DateTime.Now;

                if (!string.IsNullOrEmpty(mDto.FilePath) && modelInDb.FilePath != mDto.FilePath)
                    modelInDb.FilePath = mDto.FilePath;

                var operationResult = _expenseRepository.Update(modelInDb);
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

        public ExpenseDto Get(int userId, int id)
        {
            var data = _expenseRepository.ListQueryable
                .FirstOrDefault(x => x.Id == id && x.UserId == userId);

            if (data == null)
                return null;

            var model = ObjectMapper.Mapper.Map<ExpenseDto>(data);
            return model;
        }

        public DbOperationResult Delete(int userId, int id)
        {
            var data = _expenseRepository.ListQueryable
                .Where(x => x.UserId == userId && x.Id == id).FirstOrDefault();

            if (data == null)
                return new DbOperationResult(false, "Veri bulunamadı");

            try
            {
                _expenseRepository.Delete(data);
                return new DbOperationResult(true, "Veri Silindi");
            }
            catch (Exception ex)
            {
                return new DbOperationResult(false, ex.Message);
            }
        }

        public DbOperationResult Payment(int userId, int id)
        {
            var data = _expenseRepository.ListQueryable
                .Where(x => x.UserId == userId && x.Id == id).FirstOrDefault();

            if (data == null)
                return new DbOperationResult(false, "Veri bulunamadı");

            try
            {
                data.HasPayment = true;
                var operationResult = _expenseRepository.Update(data);
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
            var data = _expenseRepository.ListQueryable
                .Where(x => x.UserId == userId && x.Id == id).FirstOrDefault();

            if (data == null)
                return new DbOperationResult(false, "Veri bulunamadı");

            try
            {
                data.HasPayment = false;
                var operationResult = _expenseRepository.Update(data);
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

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
    public class SubscriptionBusiness
    {
        private readonly IRepository<Subscription> _subscriptionRepository;

        public SubscriptionBusiness(IRepository<Subscription> _subscriptionRepository)
        {
            this._subscriptionRepository = _subscriptionRepository;
        }

        public DataTableViewModelResult<List<SubscriptionDto>> GetAll(SubscriptionFilterModel filter)
        {
            var response = new DataTableViewModelResult<List<SubscriptionDto>>();
            response.IsSucceeded = true;

            var result = _subscriptionRepository.ListQueryableNoTracking
                .Include(z=>z.SubscriptionType)
                .OrderBy(y => y.CreateDate)
                .Select(y => new SubscriptionDto()
                {
                    SubscriptionTypeName = y.SubscriptionType.Title,
                    HasPayment = y.HasPayment,
                    Id = y.Id,
                    UserId = y.UserId,
                    FilePath = y.FilePath,
                    Price = y.SubscriptionType.Price,
                });

            response.TotalCount = result.Count();
            response.RecordsFiltered = result.AddSearchFilters(filter).Count();
            response.Data = result.AddSearchFilters(filter).AddOrderAndPageFilters(filter).ToList();

            return response;
        }

        public DbOperationResult Create(SubscriptionDto mDto)
        {
            var validCheck = new SubscriptionValidation().Validate(mDto);
            if (!validCheck.IsValid)
            {
                var errors = new List<string>();
                validCheck.Errors.ForEach(x => errors.Add(x.ErrorMessage));
                return new DbOperationResult(false, "Eksik veya hatalı veri girişi", errors);
            }

            try
            {
                var model = ObjectMapper.Mapper.Map<Subscription>(mDto);
                model.FilePath = mDto.FilePath ?? "";

                var operationResult = _subscriptionRepository.Insert(model);
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

        public DbOperationResult Update(SubscriptionDto mDto)
        {
            var validCheck = new SubscriptionValidation().Validate(mDto);
            if (!validCheck.IsValid)
            {
                var errors = new List<string>();
                validCheck.Errors.ForEach(x => errors.Add(x.ErrorMessage));
                return new DbOperationResult(false, "Eksik veya hatalı veri girişi", errors);
            }

            try
            {
                var modelInDb = _subscriptionRepository.ListQueryable
                    .FirstOrDefault(x => x.Id == mDto.Id);

                modelInDb.PaymentDate = mDto.PaymentDate;
                modelInDb.HasPayment = mDto.HasPayment;
                modelInDb.SubscriptionTypeId = mDto.SubscriptionTypeId;
                modelInDb.UpdateDate = DateTime.Now;

                if (!string.IsNullOrEmpty(mDto.FilePath) && modelInDb.FilePath != mDto.FilePath)
                    modelInDb.FilePath = mDto.FilePath;

                var operationResult = _subscriptionRepository.Update(modelInDb);
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

        public SubscriptionDto Get(int userId, int id)
        {
            var data = _subscriptionRepository.ListQueryable
                .FirstOrDefault(x => x.Id == id && x.UserId == userId);

            if (data == null)
                return null;

            var model = ObjectMapper.Mapper.Map<SubscriptionDto>(data);
            return model;
        }

        public DbOperationResult Delete(int userId, int id)
        {
            var data = _subscriptionRepository.ListQueryable
                .Where(x => x.UserId == userId && x.Id == id).FirstOrDefault();

            if (data == null)
                return new DbOperationResult(false, "Veri bulunamadı");

            try
            {
                _subscriptionRepository.Delete(data);
                return new DbOperationResult(true, "Veri Silindi");
            }
            catch (Exception ex)
            {
                return new DbOperationResult(false, ex.Message);
            }
        }

        public DbOperationResult Payment(int userId, int id)
        {
            var data = _subscriptionRepository.ListQueryable
                .Where(x => x.UserId == userId && x.Id == id).FirstOrDefault();

            if (data == null)
                return new DbOperationResult(false, "Veri bulunamadı");

            try
            {
                data.HasPayment = true;
                var operationResult = _subscriptionRepository.Update(data);
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
            var data = _subscriptionRepository.ListQueryable
                .Where(x => x.UserId == userId && x.Id == id).FirstOrDefault();

            if (data == null)
                return new DbOperationResult(false, "Veri bulunamadı");

            try
            {
                data.HasPayment = false;
                var operationResult = _subscriptionRepository.Update(data);
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

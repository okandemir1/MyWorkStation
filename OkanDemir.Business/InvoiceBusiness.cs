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
    public class InvoiceBusiness
    {
        private readonly IRepository<Invoice> _invoiceRepository;

        public InvoiceBusiness(IRepository<Invoice> _invoiceRepository)
        {
            this._invoiceRepository = _invoiceRepository;
        }

        public DataTableViewModelResult<List<InvoiceDto>> GetAll(InvoiceFilterModel filter)
        {
            var response = new DataTableViewModelResult<List<InvoiceDto>>();
            response.IsSucceeded = true;

            var result = _invoiceRepository.ListQueryableNoTracking
                .Include(z=>z.InvoiceType)
                .OrderBy(y => y.CreateDate)
                .Where(x=>!x.IsDeleted)
                .Select(y => new InvoiceDto()
                {
                    InvoiceTypeName = y.InvoiceType.Title,
                    Price = y.Price,
                    HasPayment = y.HasPayment,
                    Id = y.Id,
                    InvoiceDate = y.InvoiceDate,
                    InvoiceFile = y.InvoiceFile,
                    InvoicePaymentDate = y.InvoicePaymentDate,
                    LastPaymentDate = y.LastPaymentDate,
                    IsActive = y.IsActive,
                    UserId = y.UserId,
                });

            response.TotalCount = result.Count();
            response.RecordsFiltered = result.AddSearchFilters(filter).Count();
            response.Data = result.AddSearchFilters(filter).AddOrderAndPageFilters(filter).ToList();

            return response;
        }

        public DbOperationResult Create(InvoiceDto mDto)
        {
            var validCheck = new InvoiceValidation().Validate(mDto);
            if (!validCheck.IsValid)
            {
                var errors = new List<string>();
                validCheck.Errors.ForEach(x => errors.Add(x.ErrorMessage));
                return new DbOperationResult(false, "Eksik veya hatalı veri girişi", errors);
            }

            try
            {
                var model = ObjectMapper.Mapper.Map<Invoice>(mDto);
                model.InvoiceFile = mDto.InvoiceFile ?? "";

                var operationResult = _invoiceRepository.Insert(model);
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

        public DbOperationResult Update(InvoiceDto mDto)
        {
            var validCheck = new InvoiceValidation().Validate(mDto);
            if (!validCheck.IsValid)
            {
                var errors = new List<string>();
                validCheck.Errors.ForEach(x => errors.Add(x.ErrorMessage));
                return new DbOperationResult(false, "Eksik veya hatalı veri girişi", errors);
            }

            try
            {
                var modelInDb = _invoiceRepository.ListQueryable
                    .FirstOrDefault(x => x.Id == mDto.Id);

                modelInDb.InvoiceDate = mDto.InvoiceDate;
                modelInDb.LastPaymentDate = mDto.LastPaymentDate;
                modelInDb.InvoicePaymentDate = mDto.InvoicePaymentDate;
                modelInDb.IsActive = mDto.IsActive;
                modelInDb.Price = mDto.Price;
                modelInDb.HasPayment = mDto.HasPayment;
                modelInDb.InvoiceTypeId = mDto.InvoiceTypeId;
                modelInDb.UpdateDate = DateTime.Now;

                if (!string.IsNullOrEmpty(mDto.InvoiceFile) && modelInDb.InvoiceFile != mDto.InvoiceFile)
                    modelInDb.InvoiceFile = mDto.InvoiceFile;

                var operationResult = _invoiceRepository.Update(modelInDb);
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

        public InvoiceDto Get(int userId, int id)
        {
            var data = _invoiceRepository.ListQueryable
                .FirstOrDefault(x => x.Id == id && x.UserId == userId);

            if (data == null)
                return null;

            var model = ObjectMapper.Mapper.Map<InvoiceDto>(data);
            return model;
        }

        public DbOperationResult Delete(int userId, int id)
        {
            var data = _invoiceRepository.ListQueryable
                .Where(x => x.UserId == userId && x.Id == id).FirstOrDefault();

            if (data == null)
                return new DbOperationResult(false, "Veri bulunamadı");

            try
            {
                data.IsDeleted = true;
                _invoiceRepository.Update(data);
                return new DbOperationResult(true, "Veri Silindi");
            }
            catch (Exception ex)
            {
                return new DbOperationResult(false, ex.Message);
            }
        }

        public DbOperationResult Payment(int userId, int id)
        {
            var data = _invoiceRepository.ListQueryable
                .Where(x => x.UserId == userId && x.Id == id).FirstOrDefault();

            if (data == null)
                return new DbOperationResult(false, "Veri bulunamadı");

            try
            {
                data.HasPayment = true;
                var operationResult = _invoiceRepository.Update(data);
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
            var data = _invoiceRepository.ListQueryable
                .Where(x => x.UserId == userId && x.Id == id).FirstOrDefault();

            if (data == null)
                return new DbOperationResult(false, "Veri bulunamadı");

            try
            {
                data.HasPayment = false;
                var operationResult = _invoiceRepository.Update(data);
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

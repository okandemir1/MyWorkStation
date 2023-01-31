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
    public class ArchiveBusiness
    {
        private readonly IRepository<Archive> _archiveRepository;

        public ArchiveBusiness(IRepository<Archive> _archiveRepository)
        {
            this._archiveRepository = _archiveRepository;
        }

        public DataTableViewModelResult<List<ArchiveDto>> GetAll(ArchiveFilterModel filter)
        {
            var response = new DataTableViewModelResult<List<ArchiveDto>>();
            response.IsSucceeded = true;

            var result = _archiveRepository.ListQueryableNoTracking
                .OrderBy(y => y.CreateDate)
                .Where(y=>!y.IsDeleted)
                .Select(y => new ArchiveDto()
                {
                    Id = y.Id,
                    IsActive = y.IsActive,
                    UserId = y.UserId,
                    Username = y.Username,
                    Domain = y.Domain,
                    Fullname = y.Fullname,
                });

            response.TotalCount = result.Count();
            response.RecordsFiltered = result.AddSearchFilters(filter).Count();
            response.Data = result.AddSearchFilters(filter).AddOrderAndPageFilters(filter).ToList();

            return response;
        }

        public DbOperationResult Create(ArchiveDto mDto)
        {
            var validCheck = new ArchiveValidation().Validate(mDto);
            if (!validCheck.IsValid)
            {
                var errors = new List<string>();
                validCheck.Errors.ForEach(x => errors.Add(x.ErrorMessage));
                return new DbOperationResult(false, "Eksik veya hatalı veri girişi", errors);
            }

            try
            {
                var model = ObjectMapper.Mapper.Map<Archive>(mDto);
                Cipher cipher = new Cipher(mDto.Key);
                model.Password = cipher.Encrypt(mDto.Password);
                model.Username = cipher.Encrypt(mDto.Username);
                if (!string.IsNullOrEmpty(mDto.Phone))
                    model.Phone = cipher.Encrypt(mDto.Phone);


                var operationResult = _archiveRepository.Insert(model);
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

        public DbOperationResult Update(ArchiveDto mDto)
        {
            var validCheck = new ArchiveValidation().Validate(mDto);
            if (!validCheck.IsValid)
            {
                var errors = new List<string>();
                validCheck.Errors.ForEach(x => errors.Add(x.ErrorMessage));
                return new DbOperationResult(false, "Eksik veya hatalı veri girişi", errors);
            }

            try
            {
                var model = ObjectMapper.Mapper.Map<Archive>(mDto);
                if (!string.IsNullOrEmpty(mDto.Key))
                {
                    Cipher cipher = new Cipher(mDto.Key);
                    model.Password = cipher.Encrypt(mDto.Password);
                    model.Username = cipher.Encrypt(mDto.Username);
                    if (!string.IsNullOrEmpty(mDto.Phone))
                        model.Phone = cipher.Encrypt(mDto.Phone);
                }

                var operationResult = _archiveRepository.Update(model);
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

        public ArchiveDto Get(int userId, int id)
        {
            var data = _archiveRepository.ListQueryable
                .FirstOrDefault(x => x.Id == id && x.UserId == userId && !x.IsDeleted);

            if (data == null)
                return null;

            var model = ObjectMapper.Mapper.Map<ArchiveDto>(data);
            return model;
        }

        public DbOperationResult Delete(int userId, int id)
        {
            var data = _archiveRepository.ListQueryable
                .Where(x => x.UserId == userId && x.Id == id && !x.IsDeleted).FirstOrDefault();

            if (data == null)
                return new DbOperationResult(false, "Veri bulunamadı");

            try
            {
                data.IsDeleted = true;
                data.IsActive = false;
                var operationResult = _archiveRepository.Update(data);
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
            var data = _archiveRepository.ListQueryable
                .Where(x => x.UserId == userId && x.Id == id).FirstOrDefault();

            if (data == null)
                return new DbOperationResult(false, "Veri bulunamadı");

            try
            {
                data.IsActive = true;
                var operationResult = _archiveRepository.Update(data);
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
            var data = _archiveRepository.ListQueryable
                .Where(x => x.UserId == userId && x.Id == id).FirstOrDefault();

            if (data == null)
                return new DbOperationResult(false, "Veri bulunamadı");

            try
            {
                data.IsActive = false;
                var operationResult = _archiveRepository.Update(data);
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

        public DbOperationResult<ArchiveDto> ShowPassword(int userId, int id, string key)
        {
            var data = _archiveRepository.ListQueryable
                .Where(x => x.UserId == userId && x.Id == id).FirstOrDefault();

            if (data == null)
                return new DbOperationResult<ArchiveDto>(false, "Veri bulunamadı", null);

            try
            {
                Cipher cipher = new Cipher(key);
                data.Password = cipher.Decrypt(data.Password);
                data.Username = cipher.Decrypt(data.Username);
                if (!string.IsNullOrEmpty(data.Phone))
                    data.Phone = cipher.Decrypt(data.Phone);

                var model = ObjectMapper.Mapper.Map<ArchiveDto>(data);
                return new DbOperationResult<ArchiveDto>(true, "Şifreleme çözüldü", model);
            }
            catch (Exception ex)
            {
                return new DbOperationResult<ArchiveDto>(false, ex.Message, null);
            }
        }
    }
}

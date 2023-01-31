using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
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
using System.Drawing.Drawing2D;
using System.Text.RegularExpressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Net.Mime.MediaTypeNames;

namespace OkanDemir.Business
{
    public class NoteBusiness
    {
        private readonly IRepository<Note> _noteRepository;
        private readonly IRepository<NoteHashtag> _noteHashtagRepository;
        private readonly IRepository<NoteMetion> _noteMetionRepository;
        private readonly IRepository<Metion> _metionRepository;
        private readonly IRepository<Hashtag> _hashtagRepository;

        public NoteBusiness(IRepository<Note> _noteRepository,
            IRepository<NoteHashtag> _noteHashtagRepository,
            IRepository<NoteMetion> _noteMetionRepository,
            IRepository<Metion> _metionRepository,
            IRepository<Hashtag> _hashtagRepository)
        {
            this._noteRepository = _noteRepository;
            this._noteHashtagRepository = _noteHashtagRepository;
            this._noteMetionRepository = _noteMetionRepository;
            this._metionRepository = _metionRepository;
            this._hashtagRepository = _hashtagRepository;
        }

        public List<NoteDto> GetAllNotes(int userId)
        {
            var getNotes = _noteRepository.ListQueryableNoTracking
                .Where(x => x.UserId == userId).ToList();

            return ObjectMapper.Mapper.Map<List<NoteDto>>(getNotes);
        }

        public List<NoteDto> GetNotesWithDates(DateTime createDate, int userId)
        {
            var getNotes = _noteRepository.ListQueryableNoTracking
                .Where(x => x.UserId == userId
            && x.CreateDate.Day == createDate.Day
            && x.CreateDate.Month == createDate.Month
            && x.CreateDate.Year == createDate.Year).ToList();

            return ObjectMapper.Mapper.Map<List<NoteDto>>(getNotes);
        }

        public DbOperationResult Create(NoteDto mDto)
        {
            var validCheck = new NoteValidation().Validate(mDto);
            if (!validCheck.IsValid)
            {
                var errors = new List<string>();
                validCheck.Errors.ForEach(x => errors.Add(x.ErrorMessage));
                return new DbOperationResult(false, "Eksik veya hatalı veri girişi", errors);
            }

            try
            {
                var model = ObjectMapper.Mapper.Map<Note>(mDto);

                var insertNote = _noteRepository.Insert(model);
                if (insertNote == null)
                    return new DbOperationResult(false, "Veri Eklenemedi");

                mDto.Id = insertNote.Id;
                CastAndParseHastag(mDto);
                CastAndParseMetion(mDto);

                return new DbOperationResult(true, "Veri Eklendi");
            }
            catch (Exception ex)
            {
                return new DbOperationResult(false, ex.Message);
            }
        }

        public DbOperationResult Update(NoteDto mDto)
        {
            var validCheck = new NoteValidation().Validate(mDto);
            if (!validCheck.IsValid)
            {
                var errors = new List<string>();
                validCheck.Errors.ForEach(x => errors.Add(x.ErrorMessage));
                return new DbOperationResult(false, "Eksik veya hatalı veri girişi", errors);
            }

            try
            {
                var modelInDb = _noteRepository.ListQueryable
                    .Where(x=>x.Id == mDto.Id && x.UserId == mDto.UserId).FirstOrDefault();

                if (modelInDb == null)
                    return new DbOperationResult(false, "Veri bulunamadı");

                modelInDb.Description = mDto.Description;
                modelInDb.AlertTime = mDto.AlertTime;
                modelInDb.IsAlert = mDto.IsAlert;
                modelInDb.IsImportant = mDto.IsImportant;

                var updateModel = _noteRepository.Update(modelInDb);
                if (updateModel == null)
                    return new DbOperationResult(false, "Veri güncellenemedi");

                CastAndParseHastag(mDto);
                CastAndParseMetion(mDto);

                return new DbOperationResult(true, "Veri güncellendi");
            }
            catch (Exception ex)
            {
                return new DbOperationResult(false, ex.Message);
            }
        }

        public DbOperationResult Delete(int id, int userId)
        {
            var note = _noteRepository.ListQueryableNoTracking
                .FirstOrDefault(x => x.Id == id && x.UserId == userId);

            if (note != null)
            {
                try
                {
                    DeleteNoteMetionAndHashtag(note.Id, userId);
                    _noteRepository.Delete(note);
                    return new DbOperationResult(true, "Veri silindi");
                }
                catch (Exception ex)
                {
                    return new DbOperationResult(false, ex.Message);
                }
            }

            return new DbOperationResult(false, "Veri bulunamadı");
        }

        //Burada sadece notu silelim diğer delete'den farklı çünkü bağlantıları burada silmeme gerek yok bu özel bir method
        //Sadece kendi içinde DeleteMetion ve DeleteHashtag kullanacak bunu başka bir yere ekleme
        public DbOperationResult DeleteJustNote(int id, int userId)
        {
            var note = _noteRepository.ListQueryableNoTracking
                .FirstOrDefault(x => x.Id == id && x.UserId == userId);

            if (note != null)
            {
                try
                {
                    _noteRepository.Delete(note);
                    return new DbOperationResult(true, "Veri silindi");
                }
                catch (Exception ex)
                {
                    return new DbOperationResult(false, ex.Message);
                }
            }

            return new DbOperationResult(false, "Veri bulunamadı");
        }

        public NoteDto GetNoteById(int id, int userId)
        {
            var note = _noteRepository.ListQueryableNoTracking
                .Where(x => x.Id == id && x.UserId == userId).FirstOrDefault();

            if (note == null)
                return null;

            return ObjectMapper.Mapper.Map<NoteDto>(note);
        }

        public DataTableViewModelResult<List<MetionDto>> GetMetions(MetionFilterModel filter)
        {
            var response = new DataTableViewModelResult<List<MetionDto>>();
            response.IsSucceeded = true;

            var result = _metionRepository.ListQueryableNoTracking
                .OrderBy(x => x.CreateDate)
                .Select(x => new MetionDto()
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    Name = x.Name,
                });

            response.TotalCount = result.Count();
            response.RecordsFiltered = result.AddSearchFilters(filter).Count();
            response.Data = result.AddSearchFilters(filter).AddOrderAndPageFilters(filter).ToList();

            return response;
        }

        public MetionNoteDto MetionNotesById(int id, int userId)
        {
            var metion = _metionRepository.ListQueryableNoTracking
                .FirstOrDefault(x => x.Id == id && x.UserId == userId);

            if (metion == null)
                return new MetionNoteDto() { Metion = new MetionDto(), Notes = new List<NoteDto>() };

            var metionDto = ObjectMapper.Mapper.Map<MetionDto>(metion);

            var noteMetions = _noteMetionRepository.ListQueryableNoTracking
                .Where(x => x.MetionId == id && x.UserId == userId).ToList();

            if (noteMetions.Count() <= 0)
                return new MetionNoteDto() { Metion = metionDto, Notes = new List<NoteDto>() };

            var notes = new List<NoteDto>();
            var noteIds = noteMetions.Select(y => y.NoteId).ToList();

            var metionNotes = _noteRepository.ListQueryableNoTracking
                .Where(x => noteIds.Contains(x.Id) && x.UserId == userId)
                .ToList();

            var notesDto = ObjectMapper.Mapper.Map<List<NoteDto>>(metionNotes);

            return new MetionNoteDto() { Metion = metionDto, Notes = notesDto };
        }

        public DataTableViewModelResult<List<HashtagDto>> GetHashtags(HashtagFilterModel filter)
        {
            var response = new DataTableViewModelResult<List<HashtagDto>>();
            response.IsSucceeded = true;

            var result = _hashtagRepository.ListQueryableNoTracking
                .OrderBy(x => x.CreateDate)
                .Select(x => new HashtagDto()
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    Name = x.Name,
                });

            response.TotalCount = result.Count();
            response.RecordsFiltered = result.AddSearchFilters(filter).Count();
            response.Data = result.AddSearchFilters(filter).AddOrderAndPageFilters(filter).ToList();

            return response;
        }

        public HashtagNoteDto HashtagNotesById(int id, int userId)
        {
            var hashtag = _hashtagRepository.ListQueryableNoTracking
                .FirstOrDefault(x => x.Id == id && x.UserId == userId);

            if (hashtag == null)
                return new HashtagNoteDto() { Hashtag = new HashtagDto(), Notes = new List<NoteDto>() };

            var hashtagDto = ObjectMapper.Mapper.Map<HashtagDto>(hashtag);

            var noteHashtags = _noteHashtagRepository.ListQueryableNoTracking
                .Where(x => x.HashtagId == id && x.UserId == userId).ToList();

            if (noteHashtags.Count() <= 0)
                return new HashtagNoteDto() { Hashtag = hashtagDto, Notes = new List<NoteDto>() };

            var notes = new List<NoteDto>();
            var noteIds = noteHashtags.Select(y => y.NoteId).ToList();

            var hashtagNotes = _noteRepository.ListQueryableNoTracking
                .Where(x => noteIds.Contains(x.Id) && x.UserId == userId)
                .ToList();

            var notesDto = ObjectMapper.Mapper.Map<List<NoteDto>>(hashtagNotes);

            return new HashtagNoteDto() { Hashtag = hashtagDto, Notes = notesDto };
        }

        public DbOperationResult DeleteMetion(int id, int userId)
        {
            var metion = _metionRepository.ListQueryableNoTracking
                .FirstOrDefault(x => x.Id == id && x.UserId == userId);

            if (metion != null)
            {
                try
                {
                    var metionNotes = _noteMetionRepository.ListQueryableNoTracking
                        .Where(x => x.MetionId == id && x.UserId == userId).ToList();

                    foreach (var item in metionNotes)
                    {
                        DeleteJustNote(item.NoteId, userId);
                    }

                    _noteMetionRepository.Delete(metionNotes);
                    _metionRepository.Delete(metion);
                    return new DbOperationResult(true, "Veri silindi");
                }
                catch (Exception ex)
                {
                    return new DbOperationResult(false, ex.Message);
                }
            }

            return new DbOperationResult(false, "Veri bulunamadı");
        }

        public DbOperationResult DeleteHashtag(int id, int userId)
        {
            var hashtag = _hashtagRepository.ListQueryableNoTracking
                .FirstOrDefault(x => x.Id == id && x.UserId == userId);

            if (hashtag != null)
            {
                try
                {
                    var hashtagNotes = _noteHashtagRepository.ListQueryableNoTracking
                        .Where(x => x.HashtagId == id && x.UserId == userId).ToList();

                    foreach (var item in hashtagNotes)
                    {
                        DeleteJustNote(item.NoteId, userId);
                    }

                    _noteHashtagRepository.Delete(hashtagNotes);
                    _hashtagRepository.Delete(hashtag);
                    return new DbOperationResult(true, "Veri silindi");
                }
                catch (Exception ex)
                {
                    return new DbOperationResult(false, ex.Message);
                }
            }

            return new DbOperationResult(false, "Veri bulunamadı");
        }

        public void CastAndParseMetion(NoteDto note)
        {
            var metionRegex = new Regex("@(?<name>[^\\s]+)");
            var metionResult = metionRegex.Matches(note.Description)
                .Cast<Match>()
                .Select(m => m.Groups["name"].Value)
                .ToArray();

            if (metionResult.Count() > 0)
            {
                foreach (var item in metionResult)
                {
                    InsertMetionIfNotExists(item, note);
                }
            }
        }

        public void InsertMetionIfNotExists(string name, NoteDto note)
        {
            var existMetion = _metionRepository.ListQueryableNoTracking
                .FirstOrDefault(x => x.Name == name);

            if (existMetion != null)
            {
                _noteMetionRepository.Insert(new NoteMetion()
                {
                    CreateDate = DateTime.Now,
                    MetionId = existMetion.Id,
                    NoteId = note.Id,
                    UpdateDate = DateTime.Now,
                    UserId = note.UserId,
                });

                return;
            }

            var model = new Metion()
            {
                Name = name,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                UserId = note.UserId,
            };

            var response = _metionRepository.Insert(model);

            if (response != null)
            {
                _noteMetionRepository.Insert(new NoteMetion()
                {
                    CreateDate = DateTime.Now,
                    MetionId = response.Id,
                    NoteId = note.Id,
                    UpdateDate = DateTime.Now,
                    UserId = note.UserId,
                });
            }

            return;
        }

        public void CastAndParseHastag(NoteDto note)
        {
            var hashtagRegex = new Regex("#(?<name>[^\\s]+)");
            var hastagResult = hashtagRegex.Matches(note.Description)
                .Cast<Match>()
                .Select(m => m.Groups["name"].Value)
                .ToArray();

            if (hastagResult.Count() > 0)
            {
                foreach (var item in hastagResult)
                {
                    InsertHastagIfNotExists(item, note);
                }
            }
        }

        public void InsertHastagIfNotExists(string name, NoteDto note)
        {
            var existMetion = _hashtagRepository.ListQueryableNoTracking
                .FirstOrDefault(x => x.Name == name);

            if (existMetion != null)
            {
                _noteHashtagRepository.Insert(new NoteHashtag()
                {
                    CreateDate = DateTime.Now,
                    HashtagId = existMetion.Id,
                    NoteId = note.Id,
                    UpdateDate = DateTime.Now,
                    UserId = note.UserId,
                });

                return;
            }

            var model = new Hashtag()
            {
                Name = name,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                UserId = note.UserId,
            };

            var response = _hashtagRepository.Insert(model);

            if (response != null)
            {
                _noteHashtagRepository.Insert(new NoteHashtag()
                {
                    CreateDate = DateTime.Now,
                    HashtagId = response.Id,
                    NoteId = note.Id,
                    UpdateDate = DateTime.Now,
                    UserId = note.UserId,
                });
            }

            return;
        }

        public void DeleteNoteMetionAndHashtag(int noteId, int userId)
        {
            var noteMetions = _noteMetionRepository.ListQueryableNoTracking
                .Where(x => x.NoteId == noteId && x.UserId == userId);

            var noteHashtags = _noteHashtagRepository.ListQueryableNoTracking
                .Where(x => x.NoteId == noteId && x.UserId == userId);

            _noteMetionRepository.Delete(noteMetions);
            _noteHashtagRepository.Delete(noteHashtags);
        }
    }
}

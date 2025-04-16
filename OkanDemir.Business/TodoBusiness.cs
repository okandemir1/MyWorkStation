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
    public class TodoBusiness
    {
        private readonly IRepository<TodoList> _todoListRepository;
        private readonly IRepository<TodoListType> _todoListTypeRepository;
        private readonly IRepository<TodoTable> _todoTableRepository;

        public TodoBusiness(IRepository<TodoList> _todoListRepository, IRepository<TodoListType> _todoListTypeRepository, IRepository<TodoTable> _todoTableRepository)
        {
            this._todoListRepository = _todoListRepository;
            this._todoListTypeRepository = _todoListTypeRepository;
            this._todoTableRepository = _todoTableRepository;
        }
        
        public List<TodoTable> GetAllTodoTables(int userId, int projectId)
        {
            var data = _todoTableRepository.ListQueryableNoTracking
                .Where(x => x.UserId == userId && x.IsActive && !x.IsDeleted && x.TodoProjectId == projectId).ToList();

            return data;
        }

        public List<TodoList> GetAllTodoList(int userId, int projectId)
        {
            var data = _todoListRepository.ListQueryableNoTracking
                .Where(x => x.UserId == userId && x.IsActive && !x.IsDeleted && x.TodoProjectId == projectId).ToList();

            return data;
        }

        public List<TodoListType> GetTodoListTypes()
        {
            var data = _todoListTypeRepository.ListQueryableNoTracking
                .Where(x => x.IsActive && !x.IsDeleted).ToList();

            return data;
        }

        //public DbOperationResult ProjectCreate(TodoProjectDto mDto)
        //{
        //    var validCheck = new TodoProjectValidation().Validate(mDto);
        //    if (!validCheck.IsValid)
        //    {
        //        var errors = new List<string>();
        //        validCheck.Errors.ForEach(x => errors.Add(x.ErrorMessage));
        //        return new DbOperationResult(false, "Eksik veya hatalı veri girişi", errors);
        //    }

        //    try
        //    {
        //        var model = ObjectMapper.Mapper.Map<TodoProject>(mDto);
        //        var operationResult = _todoProjectRepository.Insert(model);
        //        if (operationResult != null)
        //            return new DbOperationResult(true, "Veri Eklendi");
        //        else
        //            return new DbOperationResult(false, "Veri Eklenemedi");
        //    }
        //    catch (Exception ex)
        //    {
        //        return new DbOperationResult(false, ex.Message);
        //    }
        //}

        //public DbOperationResult ProjectUpdate(TodoProjectDto mDto)
        //{
        //    var validCheck = new TodoProjectValidation().Validate(mDto);
        //    if (!validCheck.IsValid)
        //    {
        //        var errors = new List<string>();
        //        validCheck.Errors.ForEach(x => errors.Add(x.ErrorMessage));
        //        return new DbOperationResult(false, "Eksik veya hatalı veri girişi", errors);
        //    }

        //    try
        //    {
        //        var model = ObjectMapper.Mapper.Map<TodoProject>(mDto);
        //        var operationResult = _todoProjectRepository.Update(model);
        //        if (operationResult != null)
        //            return new DbOperationResult(true, "Veri Güncellendi");
        //        else
        //            return new DbOperationResult(false, "Veri Güncellenemedi");
        //    }
        //    catch (Exception ex)
        //    {
        //        return new DbOperationResult(false, ex.Message);
        //    }
        //}

        //public TodoProjectDto ProjectGet(int userId, int id)
        //{
        //    var data = _todoProjectRepository.ListQueryable
        //        .FirstOrDefault(x => x.Id == id && x.UserId == userId && !x.IsDeleted);

        //    if (data == null)
        //        return null;

        //    var model = ObjectMapper.Mapper.Map<TodoProjectDto>(data);
        //    return model;
        //}

        //public DbOperationResult ProjectDelete(int userId, int id)
        //{
        //    var data = _todoProjectRepository.ListQueryable
        //        .Where(x => x.UserId == userId && x.Id == id && !x.IsDeleted).FirstOrDefault();

        //    if (data == null)
        //        return new DbOperationResult(false, "Veri bulunamadı");

        //    try
        //    {
        //        data.IsDeleted = true;
        //        data.IsActive = false;
        //        var operationResult = _todoProjectRepository.Update(data);
        //        if (operationResult != null)
        //            return new DbOperationResult(true, "Veri Silindi");
        //        else
        //            return new DbOperationResult(false, "Veri Silinemedi");
        //    }
        //    catch (Exception ex)
        //    {
        //        return new DbOperationResult(false, ex.Message);
        //    }
        //}
    }
}

using OkanDemir.Dto;
using OkanDemir.Model;

namespace OkanDemir.WebUI.Cms.Models
{
    public class TodoViewModel
    {
        public TodoViewModel()
        {
            TodoLists = new List<TodoList>();
            TodoListTypes = new List<TodoListType>();
            TodoTables = new List<TodoTable>();
        }

        public List<TodoList> TodoLists { get; set; }
        public List<TodoListType> TodoListTypes { get; set; }
        public List<TodoTable> TodoTables { get; set; }
    }
}

namespace TodoItems_Beyond.Contracts;

using System.Collections.Generic;
using TodoItems_Beyond.Entities;

public interface ITodoListRepository
{
    int GetNextId();
    List<TodoItem> GetItems();
    List<string> GetAllCategories();

    void PrintCategories();

    void AddItem(TodoItem item);
}

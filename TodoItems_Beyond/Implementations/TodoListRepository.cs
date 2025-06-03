namespace TodoItems_Beyond.Implementations;

using System;
using System.Collections.Generic;
using System.Linq;

using TodoItems_Beyond.Contracts;
using TodoItems_Beyond.Entities;

public class TodoListRepository : ITodoListRepository
{
    private readonly List<TodoItem> items = new();
    private readonly HashSet<string> categories = new() { "Categoria1", "Categoria2", "Categoria3"};

    public List<TodoItem> GetItems() => items; // Método para acceder a los ítems

    public int GetNextId()
    {
        return items.Any() ? items.Max(i => i.Id) + 1 : 1; // Retorna el siguiente ID disponible y para el primero 1
    }

    public List<string> GetAllCategories()
    {
        return categories.ToList();
    }

    public void PrintCategories()
    {
        Console.WriteLine("Categorias disponibles:");
        foreach (var category in categories)
        {
            Console.WriteLine($"- {category}");
        }
    }

    public void AddItem(TodoItem item)
    {
        if (!categories.Contains(item.Category))
        {
            throw new InvalidOperationException($"Error: La categoría '{item.Category}' no está registrada en el sistema.");
        }

        items.Add(item);
    }

}

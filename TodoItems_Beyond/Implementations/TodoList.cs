namespace TodoItems_Beyond.Implementations;

using System;
using System.Linq;

using TodoItems_Beyond.Contracts;
using TodoItems_Beyond.Entities;


public class TodoList : ITodoList
{
    private readonly ITodoListRepository repository;

    public TodoList(ITodoListRepository repo)
    {
        repository = repo;
    }

    public void AddItem(int id, string title, string description, string category)
    {
        int newId = repository.GetNextId(); 
        repository.AddItem(new TodoItem(newId, title, description, category));
    }

    public void UpdateItem(int id, string description)
    {
        var item = repository.GetItems().FirstOrDefault(i => i.Id == id);
        if (item != null && item.ProgressionList.Sum(p => p.PercentageDone) <= 50)
        {
            item.UpdateDescription(description);
        }
        else
        {
            throw new InvalidOperationException($"No se puede actualizar {item?.Title}, el progreso supera el 50%.");
        }
    }

    public void RemoveItem(int id)
    {
        var item = repository.GetItems().FirstOrDefault(i => i.Id == id);
        if (item != null && item.ProgressionList.Sum(p => p.PercentageDone) <= 50)
        {
            repository.GetItems().Remove(item);
        }
        else
        {
            throw new InvalidOperationException($"No se puede eliminar {item?.Title}, el progreso supera el 50%.");
        }
    }

    public void RegisterProgression(int id, DateTime dateTime, decimal percent)
    {
        var item = repository.GetItems().FirstOrDefault(i => i.Id == id);
        if (item == null)
        {
            throw new InvalidOperationException($"El ítem con ID {id} no existe.");
        }

        if (percent <= 0 || percent > 100)
        {
            throw new ArgumentOutOfRangeException(nameof(percent), "Porcentaje debe estar entre 0 y 100.");
        }

        if (item.ProgressionList.Any() && dateTime <= item.ProgressionList.Max(p => p.DateTime))
        {
            throw new InvalidOperationException("La nueva fecha debe ser posterior a las progresiones anteriores.");
        }

        if (item.ProgressionList.Sum(p => p.PercentageDone) + percent > 100)
        {
            throw new InvalidOperationException("El progreso total no puede superar el 100%.");
        }

        item.ProgressionList.Add(new ProgressionEntry { DateTime = dateTime,  PercentageDone = percent });
        
    }

    //public List<TodoItem> GetItems()
    //{
    //    return items;
    //}

    public void PrintItems()
    {
        foreach (var item in repository.GetItems())
        {
            Console.WriteLine($"{item.Id}) {item.Title} - {item.Description} ({item.Category}) Completed: {item.IsCompleted}");

            decimal acumulado = 0;

            foreach (var progression in item.ProgressionList)
            {
                acumulado += progression.PercentageDone;
                //string barraProgreso = new string('#', (int)(acumulado / 10));
                string barraProgreso = new string('#', (int)(acumulado / 5)); // cada 5% es un '#'

                Console.WriteLine($"    {progression.DateTime:MM/dd/yyyy} {acumulado}% {barraProgreso}");
            }

            Console.WriteLine(); 
        }

    }
}


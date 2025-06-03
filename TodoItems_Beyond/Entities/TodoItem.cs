namespace TodoItems_Beyond.Entities;
public class TodoItem
{

    public int Id { get; }
    public string Title { get; }
    public string Description { get; private set; }
    public string Category { get; }
    public List<ProgressionEntry> ProgressionList { get; } = new();

    public bool IsCompleted => ProgressionList.Sum(p => p.PercentageDone) >= 100;

    public TodoItem(int id, string title, string description, string category)
    {
        Id = id;
        Title = title;
        Description = description;
        Category = category;
    }

    public void UpdateDescription(string description)
    {
        if (ProgressionList.Sum(p => p.PercentageDone) <= 50)
        {
            Description = description;
        }
        else
        {
            Console.WriteLine($"No se puede actualizar {Title}, progreso mayor al 50%.");
        }
    }
}


public class ProgressionEntry
{
    public DateTime DateTime { get; set; }
    public decimal PercentageDone { get; set; }
}
namespace TodoItems_Beyond.Business;

public class ItemManagement
{
    private readonly Contracts.ITodoListRepository _repository;
    private readonly Contracts.ITodoList _todoList;
    public ItemManagement(Contracts.ITodoListRepository repository, Contracts.ITodoList todoList)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _todoList = todoList ?? throw new ArgumentNullException(nameof(todoList));
    }
    public void AddItem(string title, string description, string category)
    {
        if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(description) || string.IsNullOrWhiteSpace(category))
        {
            throw new ArgumentException("Titulo, Descripcion, y categoria es obligatoria.");
        }
        if (!_repository.GetAllCategories().Contains(category))
        {
            throw new ArgumentException($"Categoria '{category}'no existe.");
        }
        int id = _repository.GetNextId();
        _todoList.AddItem(id, title, description, category);
    }
    public void UpdateItem(int id, string description)
    {
        if (string.IsNullOrWhiteSpace(description))
        {
            throw new ArgumentException("La descripciòn no puede ser vacia");
        }
        _todoList.UpdateItem(id, description);
    }
    public void RemoveItem(int id)
    {
        _todoList.RemoveItem(id);
    }

    public void RegisterProgression(int id, DateTime dateTime, decimal percent)
    {
        if (percent <= 0 || percent > 100)
        {
            throw new ArgumentOutOfRangeException(nameof(percent), "Percentage must be between 0 and 100.");
        }
        _todoList.RegisterProgression(id, dateTime, percent);
    }
}

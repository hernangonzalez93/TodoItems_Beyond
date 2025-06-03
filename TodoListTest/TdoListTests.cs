namespace TodoListTest;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TodoItems_Beyond.Contracts;
using TodoItems_Beyond.Implementations;

using Xunit;

public class TdoListTests
{
    [Fact]
    public void AddItem_Should_AddNewItem()
    {
        // Arrange
        ITodoListRepository repository = new TodoListRepository();
        ITodoList todoList = new TodoList(repository);
        int id = repository.GetNextId();

        // Act
        todoList.AddItem(id, "Tarea11", "Descripcion1", "Categoria1");

        // Assert
        Assert.Contains(repository.GetItems(), i => i.Id == id);

    }

    [Fact]
    public void RegisterProgression_Should_AddValidProgress()
    {
        // Arrange
        ITodoListRepository repository = new TodoListRepository();
        ITodoList todoList = new TodoList(repository);
        int id = repository.GetNextId();

        todoList.AddItem(id, "Tarea2", "Descripcion1", "Categoria1");

        // Act
        todoList.RegisterProgression(id, DateTime.Now, 30);

        // Assert: Verificar que el ítem contiene la progresión añadida
        var item = repository.GetItems().FirstOrDefault(i => i.Id == id);
        Assert.NotNull(item);
        Assert.Contains(item.ProgressionList, p => p.PercentageDone == 30);
    }

    [Fact]
    public void RegisterProgression_Should_Fail_When_Date_Is_Not_Later()
    {
        // Arrange
        ITodoListRepository repository = new TodoListRepository();
        ITodoList todoList = new TodoList(repository);
        int id = repository.GetNextId();

        todoList.AddItem(id, "Tarea2", "Descripcion2", "Categoria2");
        todoList.RegisterProgression(id, DateTime.Now, 30); // Primera progresión válida registrada

        // Act & Assert: Intentar agregar una progresión con fecha anterior debería lanzar excepción
        Assert.Throws<InvalidOperationException>(() =>
            todoList.RegisterProgression(id, DateTime.Now.AddDays(-1), 20));

    }

    [Theory]
    [InlineData(-5)]   // Caso: menor que 0
    [InlineData(105)]  // Caso: mayor que 100
    public void RegisterProgression_Should_Fail_When_Percentage_OutOfRange(decimal invalidPercent)
    {
        // Arrange
        ITodoListRepository repository = new TodoListRepository();
        ITodoList todoList = new TodoList(repository);
        int id = repository.GetNextId();
        todoList.AddItem(id, "Tarea 3", "Descripcion2", "Categoria3");

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => todoList.RegisterProgression(id, DateTime.Now, invalidPercent));
    }


    [Fact]
    public void RegisterProgression_Should_Fail_When_TotalProgress_Exceeds_100Percent()
    {
        // Arrange
        ITodoListRepository repository = new TodoListRepository();
        ITodoList todoList = new TodoList(repository);
        int id = repository.GetNextId();
        todoList.AddItem(id, "Trarea Random", "Validar que no exceda el 100%", "Categoria2");


        todoList.RegisterProgression(id, DateTime.Now, 50);
        todoList.RegisterProgression(id, DateTime.Now.AddDays(1), 30); // aqui hasta el momento el progreso es 80%

        // Act & Assert: Intentar agregar 30% más debería fallar
        Assert.Throws<InvalidOperationException>(() => todoList.RegisterProgression(id, DateTime.Now.AddDays(2), 30));
    }

    [Fact]
    public void RemoveItem_Should_Fail_When_Progress_Exceeds_50Percent()
    {
        // Arrange
        ITodoListRepository repository = new TodoListRepository();
        ITodoList todoList = new TodoList(repository);
        int id = repository.GetNextId();

        todoList.AddItem(id, "Una Tarea", "No debería poder eliminarse", "Categoria1");
        todoList.RegisterProgression(id, DateTime.Now, 60); // Se excede el 50%

        // Act & Assert: Verificar que la eliminación genera una excepción
        Assert.Throws<InvalidOperationException>(() => todoList.RemoveItem(id));

        // Verificar que el ítem sigue existiendo en la lista
        Assert.Contains(repository.GetItems(), i => i.Id == id);

    }

    [Fact]
    public void AddItem_Should_Assign_Consecutive_Ids()
    {
        // Arrange
        ITodoListRepository repository = new TodoListRepository();
        ITodoList todoList = new TodoList(repository);

        // Act
        todoList.AddItem(repository.GetNextId(), "Tarea 1", "Descripción 1", "Categoria1");
        todoList.AddItem(repository.GetNextId(), "Tarea 2", "Descripción 2", "Categoria2");

        // Obtener los ítems agregados
        var items = repository.GetItems();

        // Assert: Verificar que hay 2 ítems con IDs consecutivos
        Assert.Equal(2, items.Count);
        Assert.Equal(items[0].Id + 1, items[1].Id);
    }

    [Fact]
    public void UpdateItem_Should_Succeed_When_Progress_Less_Than_50Percent()
    {
        // Arrange
        ITodoListRepository repository = new TodoListRepository();
        ITodoList todoList = new TodoList(repository);
        int id = repository.GetNextId();

        todoList.AddItem(id, "Tarea Inicial", "Descrip", "Categoria3");
        todoList.RegisterProgression(id, DateTime.Now, 30); // Progreso menor a 50%

        // Act
        todoList.UpdateItem(id, "Nueva Descripción");

        // Obtener el ítem actualizado
        var item = repository.GetItems().FirstOrDefault(i => i.Id == id);

        // Assert: Verificar que la descripción se haya actualizado correctamente
        Assert.NotNull(item);
        Assert.Equal("Nueva Descripción", item.Description);
    }

    [Fact]
    public void AddItem_Should_Fail_When_Category_Does_Not_Exist()
    {
        // Arrange
        ITodoListRepository repository = new TodoListRepository();
        ITodoList todoList = new TodoList(repository);
        int id = repository.GetNextId();

        // Act & Assert: Intentar agregar un ítem con una categoría no existente debe lanzar una excepción
        Assert.Throws<InvalidOperationException>(() =>
            todoList.AddItem(id, "Tarea55", "Descricpion ", "CategoríaNoExistente"));
    }

    [Fact]
    public void RegisterProgression_Should_Add_Valid_Progressions()
    {
        // Arrange
        ITodoListRepository repository = new TodoListRepository();
        ITodoList todoList = new TodoList(repository);
        int id = repository.GetNextId();

        todoList.AddItem(id, "Prueba Progression", "Validar progresiones múltiples", "Categoria1");

        // Act: Insertar progresiones válidas en orden correcto
        todoList.RegisterProgression(id, DateTime.Now, 20);
        todoList.RegisterProgression(id, DateTime.Now.AddDays(1), 25);
        todoList.RegisterProgression(id, DateTime.Now.AddDays(2), 30);

        // Obtener el ítem actualizado
        var item = repository.GetItems().FirstOrDefault(i => i.Id == id);

        // Assert: Validar que las progresiones fueron insertadas correctamente
        Assert.NotNull(item);
        Assert.Equal(3, item.ProgressionList.Count);
        Assert.Equal(75, item.ProgressionList.Sum(p => p.PercentageDone));
    }

}

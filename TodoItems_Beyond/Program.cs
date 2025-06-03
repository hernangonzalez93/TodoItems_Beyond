using Autofac;
using TodoItems_Beyond.Business;
using TodoItems_Beyond.Contracts;
using TodoItems_Beyond.Infrastructure;


class Program
{
    static void Main()
    {
        var container = AutofacConfig.BuildContainer();

        using (var scope = container.BeginLifetimeScope())
        {
            var todoList = scope.Resolve<ITodoList>();
            var repository = scope.Resolve<ITodoListRepository>();
            var itemManagement = scope.Resolve<ItemManagement>();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("==== TODO LIST MENU ====");
                Console.WriteLine("1. Agregar Ítem");
                Console.WriteLine("2. Actualizar Ítem");
                Console.WriteLine("3. Remover Ítem");
                Console.WriteLine("4. Registrar Progreso");
                Console.WriteLine("5. Imprimir Ítems");
                Console.WriteLine("6. Ver Categorias Disponibles"); // Se dejan 3 categorías por defecto en TodoListRepository.cs
                Console.WriteLine("7. Salir");
                Console.Write("Seleccione una opción: ");

                string option = Console.ReadLine();
                switch (option)
                {
                    case "1":
                        AddItem(itemManagement);
                        break;
                    case "2":
                        UpdateItem(itemManagement);
                        break;
                    case "3":
                        RemoveItem(itemManagement);
                        break;
                    case "4":
                        RegisterProgression(itemManagement);
                        break;
                    case "5":
                        todoList.PrintItems();
                        Console.ReadKey();
                        break;
                    case "6":
                        repository.PrintCategories();
                        Console.ReadKey();
                        break;
                    case "7":
                        return;
                    default:
                        Console.WriteLine("Opción no válida.");
                        Console.ReadKey();
                        break;
                }
            }
        }
    }

    static void AddItem(ItemManagement itemManagement)
    {
        try
        {
            Console.Write("Ingrese título: ");
            string title = Console.ReadLine();
            Console.Write("Ingrese descripción: ");
            string description = Console.ReadLine();
            Console.Write("Ingrese una categoría: ");
            string category = Console.ReadLine();
            itemManagement.AddItem(title, description, category);
            Console.WriteLine("Ítem agregado correctamente.");       
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        Console.ReadKey();

    }

    static void UpdateItem(ItemManagement itemManagement)
    {
        try
        {
            Console.Write("Ingrese el ID del ítem a actualizar: ");
            int id = int.Parse(Console.ReadLine());
            Console.Write("Ingrese nueva descripción: ");
            string description = Console.ReadLine();
            itemManagement.UpdateItem(id, description);
            Console.WriteLine("Ítem actualizado correctamente.");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");;
        }
        Console.ReadKey();
    }

    static void RemoveItem(ItemManagement itemManagement)
    {
        try
        {
            Console.Write("Ingrese el ID del ítem a remover: ");
            int id = int.Parse(Console.ReadLine());
            itemManagement.RemoveItem(id);
            Console.WriteLine("Ítem removido correctamente.");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        Console.ReadKey();
    }

    static void RegisterProgression(ItemManagement itemManagement)
    {
        try
        {
            Console.Write("Ingrese el ID del ítem: ");
            int id = int.Parse(Console.ReadLine());
            Console.Write("Ingrese la fecha (YYYY-MM-DD): ");
            DateTime dateTime = DateTime.Parse(Console.ReadLine());
            Console.Write("Ingrese el porcentaje de progreso: ");
            decimal percent = decimal.Parse(Console.ReadLine());   
            itemManagement.RegisterProgression(id, dateTime, percent);
            Console.WriteLine("Progreso registrado correctamente.");
        }
        catch (ArgumentOutOfRangeException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error inesperado: {ex.Message}");
            
        }
        Console.ReadKey();
    }
}

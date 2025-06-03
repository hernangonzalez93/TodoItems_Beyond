namespace TodoItems_Beyond.Infrastructure;

using Autofac;

using TodoItems_Beyond.Business;
using TodoItems_Beyond.Contracts;
using TodoItems_Beyond.Implementations;

public static class AutofacConfig
{
    public static IContainer BuildContainer()
    {
        var builder = new ContainerBuilder();
        builder.RegisterType<TodoList>().As<ITodoList>().SingleInstance();
        builder.RegisterType<TodoListRepository>().As<ITodoListRepository>().SingleInstance();
        builder.RegisterType<ItemManagement>().AsSelf();
        return builder.Build();
    }
}
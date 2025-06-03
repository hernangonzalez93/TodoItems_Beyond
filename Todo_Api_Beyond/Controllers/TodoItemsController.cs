namespace Todo_Api_Beyond.Controllers;

using Microsoft.AspNetCore.Mvc;

using TodoItems_Beyond.Business;
using TodoItems_Beyond.Contracts;
using TodoItems_Beyond.Entities;

[Route("api/[controller]")]
[ApiController]
public class TodoItemsController : ControllerBase
{
    private readonly ITodoListRepository _repo;

    private readonly ItemManagement _itemManagement;

    public TodoItemsController(ITodoListRepository repo, ItemManagement _itemManagement)
    {
        _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        this._itemManagement = _itemManagement ?? throw new ArgumentNullException(nameof(_itemManagement));
    }

    [HttpGet]
    public ActionResult<List<TodoItem>> GetAll() => _repo.GetItems();

    [HttpGet("categories")]
    public ActionResult<List<string>> GetCategories() => _repo.GetAllCategories();

    [HttpPost]
    public IActionResult Add([FromBody] TodoItem item)
    {
        try
        {
            this._itemManagement.AddItem(item.Title, item.Description, item.Category);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
 
    [HttpPost("{id}/progression")]
    public IActionResult RegisterProgression(int id, [FromBody] ProgressionEntry progression)
    {
        try
        {
            var item = _repo.GetItems().FirstOrDefault(i => i.Id == id);
            if (item == null)
            {
                return NotFound($"El Item con ID {id} no existe.");
            }
            this._itemManagement.RegisterProgression(id, progression.DateTime, progression.PercentageDone);
            return Ok();
        }
        catch (ArgumentOutOfRangeException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] string description)
    {
        try
        {
            this._itemManagement.UpdateItem(id, description);
            return Ok();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        try
        {
            this._itemManagement.RemoveItem(id);
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

}

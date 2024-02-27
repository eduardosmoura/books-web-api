using BooksWebApi.Models;
using BooksWebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BooksWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController(BooksService booksService) : ControllerBase
{
    [HttpGet]
    public async Task<List<Book>> Get()
    {
        return await booksService.GetAsync();
    }

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<Book>> Get(string id)
    {
        var book = await booksService.GetAsync(id);

        if (book is null) return NotFound();

        return book;
    }

    [HttpPost]
    public async Task<IActionResult> Post(Book newBook)
    {
        await booksService.CreateAsync(newBook);

        return CreatedAtAction(nameof(Get), new { id = newBook.Id }, newBook);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, Book updatedBook)
    {
        var book = await booksService.GetAsync(id);

        if (book is null) return NotFound();

        updatedBook.Id = book.Id;

        await booksService.UpdateAsync(id, updatedBook);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var book = await booksService.GetAsync(id);

        if (book is null) return NotFound();

        await booksService.RemoveAsync(id);

        return NoContent();
    }
}
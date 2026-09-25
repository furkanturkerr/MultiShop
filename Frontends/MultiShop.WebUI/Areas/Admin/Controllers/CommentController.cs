using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.Dtos.CommentDtos;
using MultiShop.WebUI.Services.Interfaces;

namespace MultiShop.WebUI.Areas.Admin.Controllers;

[Area("Admin")]
[AutoValidateAntiforgeryToken]
[Authorize(Roles = "Admin")]

public class CommentController : Controller
{
    private readonly ICommentService _commentService;

    public CommentController(ICommentService commentService)
    {
        _commentService = commentService;
    }

    // GET
    public async Task<IActionResult> Index()
    {
        var values = await _commentService.GetAllCommentAsync();
        return View(values);
    }

    [HttpGet]
    public async Task<IActionResult> Update(int id)
    {
        var value = await _commentService.GetByIdCommentAsync(id);
        return value is null ? NotFound() : View(value);
    }

    [HttpPost]
    public async Task<IActionResult> Update(UpdateCommentDto dto)
    {
        if (await _commentService.UpdateCommentAsync(dto))
        {
            return RedirectToAction("Index");
        }
        return View(dto);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        await _commentService.DeleteCommentAsync(id);
        return RedirectToAction("Index");
    }
}

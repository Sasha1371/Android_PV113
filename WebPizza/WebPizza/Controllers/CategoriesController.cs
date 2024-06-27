﻿using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebPizza.Data;
using WebPizza.Data.Entities;
using WebPizza.Interfaces;
using WebPizza.ViewModels.Category;
using WebPizza.ViewModels.Pagination;

namespace WebPizza.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class CategoriesController(IMapper mapper, 
    PizzaDbContext pizzaContext,
    IValidator<CategoryCreateVM> createValidator,
    //IValidator<CategoryUpdateVM> updateValidator,
    IPaginationService<CategoryVm, CategoryFilterVm> pagination,
    IImageService imageService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var list = await pizzaContext.Categories.ToListAsync();
            return Ok(list);
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var category = await pizzaContext.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            imageService.DeleteImageIfExists(category.Image);
            pizzaContext.Categories.Remove(category);
            await pizzaContext.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromForm] CategoryCreateVM vm)
    {
        var validationResult = await createValidator.ValidateAsync(vm);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);


        var category = mapper.Map<CategoryEntity>(vm);

        try
        {
            category.Image = await imageService.SaveImageAsync(vm.Image);
            await pizzaContext.Categories.AddAsync(category);
            await pizzaContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAll), new { id = category.Id }, category);
        }
        catch (Exception)
        {
            imageService.DeleteImageIfExists(category.Image);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromForm] CategoryEditVm vm)
    {
        //var validationResult = await updateValidator.ValidateAsync(vm);

        //if (!validationResult.IsValid)
        //    return BadRequest(validationResult.Errors);

        var category = await pizzaContext.Categories.FirstOrDefaultAsync(c => c.Id == id);
        if (category == null)
        {
            return NotFound();
        }

        category.Name = vm.Name;

        if (vm.Image != null)
        {
            imageService.DeleteImageIfExists(category.Image);
            category.Image = await imageService.SaveImageAsync(vm.Image);
        }

        try
        {
            pizzaContext.Categories.Update(category);
            await pizzaContext.SaveChangesAsync();
            return Ok(category);
        }
        catch
        {
            if (vm.Image != null)
            {
                imageService.DeleteImageIfExists(category.Image);
            }
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet()]
    public async Task<IActionResult> GetPage([FromQuery] CategoryFilterVm vm)
    {
        try
        {
            return Ok(await pagination.GetPageAsync(vm));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

}
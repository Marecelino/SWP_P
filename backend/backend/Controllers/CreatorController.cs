﻿using backend.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Entities;


[ApiController]
[Route("api/[controller]")]
public class CreatorController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public CreatorController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/Creator
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Creator>>> GetCreators()
    {
        var creators = await _context.Creators
            .Select(c => new Creator
            {
                CreatorID = c.CreatorID,
                PaypalAccount = c.PaypalAccount,
                UserName = c.UserName,
                FollowerID = c.FollowerID, // Kiểm tra giá trị NULL trước khi gán
                ProfilePicture = c.ProfilePicture != null ? (byte[])c.ProfilePicture : new byte[0], // Kiểm tra giá trị null trước khi gán
                FirstName = c.FirstName,
                LastName = c.LastName,
                Address = c.Address,
                Phone = c.Phone,
                LastLogDate = c.LastLogDate,
                AllowCommission = c.AllowCommission
                // Các thuộc tính khác...
            })
            .ToListAsync();

        return creators;
    }






    // GET: api/Creator/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Creator>> GetCreator(int id)
    {
        var creator = await _context.Creators.FindAsync(id);

        if (creator == null)
        {
            return NotFound();
        }

        return creator;
    }

    // POST: api/Creator
    [HttpPost]
    public async Task<ActionResult<Creator>> PostCreator(Creator creator)
    {
        _context.Creators.Add(creator);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetCreator), new { id = creator.CreatorID }, creator);
    }

    // PUT: api/Creator/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutCreator(int id, Creator creator)
    {
        if (id != creator.CreatorID)
        {
            return BadRequest();
        }

        _context.Entry(creator).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CreatorExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // DELETE: api/Creator/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCreator(int id)
    {
        var creator = await _context.Creators.FindAsync(id);
        if (creator == null)
        {
            return NotFound();
        }

        _context.Creators.Remove(creator);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool CreatorExists(int id)
    {
        return _context.Creators.Any(e => e.CreatorID == id);
    }
}

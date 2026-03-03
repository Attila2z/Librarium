using Librarium.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Librarium.Api.Controllers;

[ApiController]
[Route("api/members")]
public class MembersController : ControllerBase
{
    private readonly LibrariumDbContext _db;

    public MembersController(LibrariumDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> GetMembers()
    {
        var members = await _db.Members.ToListAsync();
        return Ok(members.Select(m => new
        {
            memberId = m.Id,
            m.FirstName,
            m.LastName,
            m.Email
        }));
    }
}

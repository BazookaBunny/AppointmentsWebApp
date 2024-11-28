using Backend.Data;
using Backend.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [Route("api/appointment")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AppointmentController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Appointment
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetAppointments()
        {
            return await _context.Appointments.ToListAsync();
        }

        // GET: api/Appointment/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Appointment>> GetAppointment(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);

            if (appointment == null)
            {
                return NotFound();
            }

            return appointment;
        }

        // POST: api/Appointment
        [HttpPost]
        public async Task<ActionResult<Appointment>> PostAppointment(Appointment appointment)
        {
            if (appointment == null)
            {
                return BadRequest("Appointment cannot be null.");
            }

            try
            {
                appointment.CreatedDate = DateTime.UtcNow;
                appointment.ModifiedDate = DateTime.UtcNow;
                appointment.Date = DateTime.SpecifyKind(appointment.Date, DateTimeKind.Utc);
                _context.Appointments.Add(appointment);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log the exception for debugging
                Console.WriteLine($"Exception occurred: {ex.Message}");
                return StatusCode(500, "Internal server error.");
            }

            return CreatedAtAction("GetAppointment", new { id = appointment.ID }, appointment);
        }


        // PUT: api/Appointment/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAppointment(int id, Appointment appointment)
        {
            if (id != appointment.ID)
            {
                return BadRequest();
            }

            _context.Entry(appointment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppointmentExists(id))
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

        // DELETE: api/Appointment/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        public async Task<ActionResult<IEnumerable<Appointment>>> Filter(Filter filters)
        {
            if (_context.Appointments == null)
            {
                return NotFound("No data!");
            }

            List<Appointment> allData = await _context.Appointments.ToListAsync();

            if (filters.All)
            {
                return allData;
            }
            if (filters.LevelOfImportance != null)
            {
                allData = allData.Where(x => x.LevelOfImportance == filters.LevelOfImportance).ToList();
            }
            if (filters.SpecifiedDate != null)
            {
                allData = allData.Where(x => x.Date == filters.SpecifiedDate).ToList();
            }
            if (filters.SpecifiedTime != null)
            {
                allData = allData.Where(x => x.Time == filters.SpecifiedTime).ToList();
            }
            if (filters.SpecifiedDate != null && filters.EndDate != null)
            {
                allData = allData.Where(x => x.Date >= filters.StartDate && x.Date <= filters.EndDate).ToList();
            }
            allData = allData.Where(x => x.Done == filters.Done).ToList();
            allData = allData.Where(x => x.Deleted == filters.Deleted).ToList();

            return allData;
        }

        private bool AppointmentExists(int id)
        {
            return _context.Appointments.Any(e => e.ID == id);
        }
    }
}

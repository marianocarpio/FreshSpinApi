using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FreshSpinApi.Data;
using FreshSpinApi.Models;
using FreshSpinApi.Services;

namespace FreshSpinApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingsController : ControllerBase
    {
        private readonly FreshSpinDbContext _context;
        private readonly IAuditService _auditService;

        public BookingsController(FreshSpinDbContext context, IAuditService auditService)
        {
            _context = context;
            _auditService = auditService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBookings()
        {
            var bookings = await _context.Bookings.ToListAsync();
            return Ok(bookings);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookingById(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) return NotFound();
            return Ok(booking);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBooking([FromBody] Booking booking, [FromHeader(Name = "X-User-Role")] string role = "Customer")
        {
            booking.CreatedAt = DateTime.UtcNow;
            booking.UpdatedAt = DateTime.UtcNow;
            booking.Status = BookingStatus.Pending;

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            await _auditService.LogActionAsync(
                "BOOKING_CREATED",
                role,
                booking.CustomerId,
                booking.Id,
                $"New laundry booking created for service: {booking.ServiceType}"
            );

            return CreatedAtAction(nameof(GetBookingById), new { id = booking.Id }, booking);
        }

        [HttpPut("{id}/assign")]
        public async Task<IActionResult> AssignRider(int id, [FromBody] string riderId, [FromHeader(Name = "X-User-Role")] string role = "Admin")
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) return NotFound();

            booking.AssignedRiderId = riderId;
            booking.Status = BookingStatus.Assigned;
            booking.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            await _auditService.LogActionAsync(
                "RIDER_ASSIGNED",
                role,
                "ADMIN-SYS",
                booking.Id,
                $"Assigned Rider {riderId} to Booking #{id}"
            );

            return Ok(booking);
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] BookingStatus newStatus, [FromHeader(Name = "X-User-Role")] string role = "Admin", [FromHeader(Name = "X-User-Id")] string userId = "User-01")
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) return NotFound();

            var oldStatus = booking.Status;
            booking.Status = newStatus;
            booking.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            await _auditService.LogActionAsync(
                "STATUS_UPDATED",
                role,
                userId,
                booking.Id,
                $"Status changed from {oldStatus} to {newStatus}"
            );

            return Ok(booking);
        }

        [HttpGet("audit-logs")]
        public async Task<IActionResult> GetAuditLogs()
        {
            var logs = await _context.AuditLogs
                .OrderByDescending(l => l.Timestamp)
                .ToListAsync();

            return Ok(logs);
        }
    }
}
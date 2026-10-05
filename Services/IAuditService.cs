using FreshSpinApi.Data;
using FreshSpinApi.Models;
using System.Threading.Tasks;

namespace FreshSpinApi.Services
{
    public interface IAuditService
    {
        Task LogActionAsync(string action, string role, string userId, int entityId, string details);
    }

    public class AuditService : IAuditService
    {
        private readonly FreshSpinDbContext _context;

        public AuditService(FreshSpinDbContext context)
        {
            _context = context;
        }

        public async Task LogActionAsync(string action, string role, string userId, int entityId, string details)
        {
            var log = new AuditLog
            {
                Action = action,
                PerformedByRole = role,
                PerformedById = userId,
                RelatedEntityId = entityId,
                Details = details,
                Timestamp = DateTime.UtcNow
            };

            _context.AuditLogs.Add(log);
            await _context.SaveChangesAsync();
        }
    }
}
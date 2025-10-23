using ASM1.Repository.Data;
using ASM1.Repository.Models;
using ASM1.Repository.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ASM1.Repository.Repositories
{
    public class WarrantyRepository : IWarrantyRepository
    {
        private readonly CarSalesDbContext _context;

        public WarrantyRepository(CarSalesDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Warranty>> GetAllWarrantiesAsync()
        {
            return await _context.Warranties
                .Include(w => w.Order)
                    .ThenInclude(o => o.Variant)
                        .ThenInclude(v => v.VehicleModel)
                .Include(w => w.Customer)
                .Include(w => w.Dealer)
                .OrderByDescending(w => w.RequestDate)
                .ToListAsync();
        }

        public async Task<Warranty?> GetWarrantyByIdAsync(int warrantyId)
        {
            return await _context.Warranties
                .Include(w => w.Order)
                    .ThenInclude(o => o.Variant)
                        .ThenInclude(v => v.VehicleModel)
                .Include(w => w.Customer)
                .Include(w => w.Dealer)
                .FirstOrDefaultAsync(w => w.WarrantyId == warrantyId);
        }

        public async Task<IEnumerable<Warranty>> GetWarrantiesByCustomerIdAsync(int customerId)
        {
            return await _context.Warranties
                .Include(w => w.Order)
                    .ThenInclude(o => o.Variant)
                        .ThenInclude(v => v.VehicleModel)
                .Include(w => w.Dealer)
                .Where(w => w.CustomerId == customerId)
                .OrderByDescending(w => w.RequestDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Warranty>> GetWarrantiesByDealerIdAsync(int dealerId)
        {
            return await _context.Warranties
                .Include(w => w.Order)
                    .ThenInclude(o => o.Variant)
                        .ThenInclude(v => v.VehicleModel)
                .Include(w => w.Customer)
                .Where(w => w.DealerId == dealerId)
                .OrderByDescending(w => w.RequestDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Warranty>> GetWarrantiesByOrderIdAsync(int orderId)
        {
            return await _context.Warranties
                .Include(w => w.Order)
                    .ThenInclude(o => o.Variant)
                        .ThenInclude(v => v.VehicleModel)
                .Include(w => w.Customer)
                .Include(w => w.Dealer)
                .Where(w => w.OrderId == orderId)
                .OrderByDescending(w => w.RequestDate)
                .ToListAsync();
        }

        public async Task<Warranty> CreateWarrantyAsync(Warranty warranty)
        {
            _context.Warranties.Add(warranty);
            await _context.SaveChangesAsync();
            return warranty;
        }

        public async Task<Warranty> UpdateWarrantyAsync(Warranty warranty)
        {
            _context.Warranties.Update(warranty);
            await _context.SaveChangesAsync();
            return warranty;
        }

        public async Task<bool> DeleteWarrantyAsync(int warrantyId)
        {
            var warranty = await _context.Warranties.FindAsync(warrantyId);
            if (warranty == null)
                return false;

            _context.Warranties.Remove(warranty);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CanRequestWarrantyAsync(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null || order.OrderDate == null)
                return false;

            // Check if warranty period is still valid (e.g., 3 years from order date)
            var warrantyExpiryDate = order.OrderDate.Value.ToDateTime(TimeOnly.MinValue).AddYears(3);
            return DateTime.Now <= warrantyExpiryDate;
        }
    }
}

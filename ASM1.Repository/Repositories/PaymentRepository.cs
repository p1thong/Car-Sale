using ASM1.Repository.Data;
using ASM1.Repository.Models;
using ASM1.Repository.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ASM1.Repository.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly CarSalesDbContext _context;
        private readonly ILogger<PaymentRepository> _logger;

        public PaymentRepository(CarSalesDbContext context, ILogger<PaymentRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Payment>> GetAllPaymentsAsync()
        {
            return await _context.Payments
                .Include(p => p.Order)
                    .ThenInclude(o => o.Customer)
                .ToListAsync();
        }

        public async Task<Payment?> GetPaymentByIdAsync(int paymentId)
        {
            return await _context.Payments
                .Include(p => p.Order)
                    .ThenInclude(o => o.Customer)
                .FirstOrDefaultAsync(p => p.PaymentId == paymentId);
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByOrderAsync(int orderId)
        {
            return await _context.Payments
                .Where(p => p.OrderId == orderId)
                .OrderBy(p => p.PaymentDate)
                .ToListAsync();
        }

        public async Task<Payment> CreatePaymentAsync(Payment payment)
        {
            _context.Payments.Add(payment);
            try
            {
                await _context.SaveChangesAsync();
                return payment;
            }
            catch (Exception ex)
            {
                // Log detailed inner exception information to aid debugging
                try
                {
                    var inner = ex.InnerException?.Message ?? "(no inner exception)";
                    _logger?.LogError(ex, "Error saving Payment entity. OrderId={OrderId}, Amount={Amount}. Inner: {Inner}", payment.OrderId, payment.Amount, inner);
                }
                catch { }
                throw; // rethrow to preserve original behavior
            }
        }

        public async Task<Payment> UpdatePaymentAsync(Payment payment)
        {
            _context.Entry(payment).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return payment;
        }

        public async Task<bool> DeletePaymentAsync(int paymentId)
        {
            var payment = await _context.Payments.FindAsync(paymentId);
            if (payment == null) return false;

            _context.Payments.Remove(payment);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<decimal> GetTotalPaidAmountByOrderAsync(int orderId)
        {
            return await _context.Payments
                .Where(p => p.OrderId == orderId)
                .SumAsync(p => p.Amount ?? 0);
        }

        public async Task<IEnumerable<Payment>> GetPendingPaymentsAsync()
        {
            return await _context.Payments
                .Where(p => p.PaymentDate > DateTime.Now) // Future payments
                .Include(p => p.Order)
                    .ThenInclude(o => o.Customer)
                .ToListAsync();
        }
    }
}
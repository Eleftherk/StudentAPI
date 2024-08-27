using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StudentAPI.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace StudentAPI.Services.EmailToBookStoreService
{
    public class EmailToBookStoreService : IEmailToBookStoreService
    {
        private readonly DataContext _context;
        public EmailToBookStoreService(DataContext context)
        {
            _context = context;
        }
        public async Task<DateTime?> GetDateOfLastEmail()
        {
            var Emails = await _context.emailsToBookStore.ToListAsync();
            if (Emails.IsNullOrEmpty())
            {
                return new DateTime(2022, 1, 10);
            }
            var dateOfLastEmail = Emails.LastOrDefault().DateOfEmail;
            return dateOfLastEmail;
        }
        public async Task<EmailToBookStore?> NewEmaillToBookStore(DateTime date)
        {
            var Emails = await _context.emailsToBookStore.ToListAsync();
            if (Emails is null)
            {
                return null;
            }
            await _context.AddAsync(new EmailToBookStore()
            {
                DateOfEmail = date
            });
            await _context.SaveChangesAsync();
            Emails = await _context.emailsToBookStore.ToListAsync();
            return Emails.LastOrDefault();
        }
    }
}

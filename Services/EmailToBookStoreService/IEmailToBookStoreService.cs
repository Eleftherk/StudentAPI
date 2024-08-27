namespace StudentAPI.Services.EmailToBookStoreService
{
    public interface IEmailToBookStoreService
    {
        Task<DateTime?> GetDateOfLastEmail();
        Task<EmailToBookStore?> NewEmaillToBookStore(DateTime date);
    }
}

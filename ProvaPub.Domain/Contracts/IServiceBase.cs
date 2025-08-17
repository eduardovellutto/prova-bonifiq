namespace ProvaPub.Domain.Contracts
{
    public interface IServiceBase<T>
    {
        Task<T> GetListAsync(int? page = null, int pageSize = 10);
    }
}

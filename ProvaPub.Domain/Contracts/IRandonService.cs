namespace ProvaPub.Domain.Contracts
{
    public interface IRandonService
    {
        Task<int> GetRandom();
    }
}

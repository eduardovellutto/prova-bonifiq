namespace ProvaPub.Domain.Models.Entities
{
    public abstract class ListBase
    {
        public int TotalCount { get; set; }
        public bool HasNext { get; set; }
    }
}

namespace ProvaPub.Domain.Models.Entities
{
    public class ProductList : ListBase
    {
        #region Properties
        public List<Product> Products { get; private set; }
        #endregion


        #region Constructors
        public ProductList(List<Product> products, int totalCount, bool hasNext)
        {
            Products = products;
            TotalCount = totalCount;
            HasNext = hasNext;
        }
        #endregion
    }
}

namespace ProvaPub.Domain.Models.Entities
{
    public class CustomerList : ListBase
    {
        #region Properties
        public List<Customer> Customers { get; private set; }
        #endregion


        #region Constructors
        public CustomerList(List<Customer> customers, int totalCount, bool hasNext)
        {
            Customers = customers;
            TotalCount = totalCount;
            HasNext = hasNext;
        }
        #endregion
    }
}

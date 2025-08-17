namespace ProvaPub.Domain.Models.Entities
{
    public class Product : EntityBase
    {
        #region Properties
        public string Name { get; private set; } = string.Empty;
        #endregion

        #region Constructors

        protected Product() { }

        public Product(int id, string name)
        {
            Id = id;
            Name = name;
        }
        #endregion


    }
}

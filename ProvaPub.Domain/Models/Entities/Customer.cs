using System.Text.Json.Serialization;

namespace ProvaPub.Domain.Models.Entities
{
    public class Customer : EntityBase
    {
        #region Properties
        public string Name { get; private set; } = string.Empty;

        [JsonIgnore]
        public ICollection<Order> Orders { get; private set; }
        #endregion

        #region Constructors

        protected Customer() { }

        public Customer(int id, string name)
        {
            Id = id;
            Name = name;
        }
        #endregion
    }
}

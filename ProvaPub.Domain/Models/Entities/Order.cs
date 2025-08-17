using ProvaPub.Domain.Helpers;
using System.Text.Json.Serialization;

namespace ProvaPub.Domain.Models.Entities
{
    public class Order : EntityBase
    {
        #region Properties
        public decimal Value { get; private set; }

        [JsonConverter(typeof(BrazilianDateTimeConverter))]
        public DateTime OrderDate { get; private set; }

        public int CustomerId { get; private set; }

        [JsonIgnore]
        public Customer Customer { get; private set; }

        #endregion

        #region Constructors
        protected Order() { }

        public Order(decimal value, int customerId, DateTime orderDate)
        {
            Value = value;
            CustomerId = customerId;
            OrderDate = orderDate;
        }
        #endregion

        #region Methods

        public Order SetDateOrderToBrazilianDate()
        {
            var brasilTimeZone = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
            OrderDate = TimeZoneInfo.ConvertTimeFromUtc(OrderDate, brasilTimeZone);
            return this;
        }

        #endregion
    }
}

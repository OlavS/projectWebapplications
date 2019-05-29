using System.ComponentModel;

namespace Model.ViewModels
{
    public class ExpandedOrderVM
    {
        [DisplayName("Fornavn:")]
        public string FirstName { get; set; }

        [DisplayName("Etternavn:")]
        public string SurName { get; set; }

        [DisplayName("Epost:")]
        public string Email { get; set; }

        public OrderVM Order { get; set; }

        public override string ToString()
        {
            return Order.OrderNr + " " + FirstName + " " + SurName + " " + Email + " "
                   + Order.Date + " " + Order.TotalPrice;
        }
    }
}
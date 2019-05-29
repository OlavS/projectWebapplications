using System.Collections.Generic;
using System.ComponentModel;

namespace Model.ViewModels
{
    public class OrderVM
    {
        [DisplayName("Ordrenr: ")]
        public int OrderNr { get; set; }

        [DisplayName("Dato: ")]
        public string Date { get; set; }

        [DisplayName("Ordrelinjer: ")]
        public List<OrdreLinjeVM> OrderLines { get; set; }

        [DisplayName("Totalpris: ")]
        public int TotalPrice { get; set; }
    }

    public class OrdreLinjeVM
    {
        [DisplayName("Pris: ")]
        public int Price { get; set; }

        [DisplayName("Tittel: ")]
        public string Title { get; set; }
    }
}
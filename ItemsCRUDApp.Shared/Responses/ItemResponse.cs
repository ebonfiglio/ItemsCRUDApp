using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ItemsCRUDApp.Shared.Responses
{
    public class ItemResponse
    {
        public int Id { get; set; }

        [DisplayName("Name")]
        public string ItemName { get; set; }

        public decimal Cost { get; set; }
    }
}

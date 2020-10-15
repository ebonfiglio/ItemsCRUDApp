using System;
using System.Collections.Generic;
using System.Text;

namespace ItemsCRUDApp.Shared.Requests
{
    public class ItemRequest
    {
        public int Id { get; set; }

        public string ItemName { get; set; }

        public decimal Cost { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace ItemsCRUDApp.Data.Entities
{
    public class Item
    {
        public int Id { get; set; }

        public string ItemName { get; set; }

        public decimal Cost { get; set; }
    }
}

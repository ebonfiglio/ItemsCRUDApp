using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ItemsCRUDApp.Data.Entities
{
    public class Item
    {
        [Key]
        public int Id { get; set; }

        public string ItemName { get; set; }

        public decimal Cost { get; set; }
    }
}

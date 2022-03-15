using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NutsStore.Models
{
    public class ProductInfo
    {
        public ProductInfo(int userId, string title, int capacity, int price, int category, string description, bool isDisplay, string creationDate, string modificationDate, string image)
        {
            UserId = userId;
            Title = title;
            Capacity = capacity;
            Price = price;
            Category = category;
            Description = description;
            IsDisplay = isDisplay;
            CreationDate = creationDate;
            ModificationDate = modificationDate;
            Image = image;
        }
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public int Capacity { get; set; }
        public int Price { get; set; }
        public int Category { get; set; }
        public string Description { get; set; }
        public bool IsDisplay { get; set; }
        public string Image { get; set; }
        public string CreationDate { get; set; }
        public string ModificationDate { get; set; }

        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Title) && Price > 0 && Capacity > 0 && !string.IsNullOrWhiteSpace(Description) && !string.IsNullOrWhiteSpace(Image);
        }
    }
}

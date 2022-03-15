using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NutsStore.Util;

namespace NutsStore.Models
{
    public class BasketInfo
    {
        public BasketInfo(int userId, string number, string basketJSON, string address, bool isSentToUserAddress, string creationDate, string modificationDate, string description, int status)
        {
            UserId = userId;
            Number = number;
            BasketJSON = basketJSON;
            Address = address;
            IsSentToUserAddress = isSentToUserAddress;
            CreationDate = creationDate;
            ModificationDate = modificationDate;
            Description = description;
            Status = status;
        }
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Number { get; set; }
        public string BasketJSON { get; set; }
        public string Address { get; set; }
        public bool IsSentToUserAddress { get; set; }
        public string CreationDate { get; set; }
        public string ModificationDate { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public string Username { get; set; }

        public bool IsValid()
        {
            return BasketJSON.ValidateJSON() || string.IsNullOrWhiteSpace(Address);
        }
    }
}

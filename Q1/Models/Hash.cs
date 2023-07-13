using System;

namespace Q1.Models
{
    public class Hash
    {
        public int Id { get; set; }
        public string HashValue { get; set; }
        public DateTime DateCreated { get; set; }
    }

    public class HashCount
    {
        public DateTime Date { get; set; }
        public int Count { get; set; }

        //public DateTime DateCreated { get; set; } // Add this property
    }

}
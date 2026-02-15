using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    public class PhysicalAddress
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        // ADDRESS COMPONENTS (South African Format)
        public string? StreetNumber { get; set; }        // "123", "45B"
        public string StreetName { get; set; }           // "Main Road", "Vilakazi Street"
        public string Suburb { get; set; }               // "Soweto", "Sandton"
        public string City { get; set; }                 // "Johannesburg", "Pretoria"
        public string Province { get; set; }             // "Gauteng", "Western Cape"
        public string PostalCode { get; set; }           // "2000", "1685"
        public string Country { get; set; } = "South Africa";

        // ADDITIONAL DETAILS
        public string? AdditionalDetails { get; set; }   // "Flat 3", "Behind the church"

        // GEO-LOCATION (for mapping)
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }


        // AUDIT FIELDS
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; } = false;

        // COMPUTED PROPERTIES
        public string FullAddress =>
            $"{StreetNumber} {StreetName}, {Suburb}, {City}, {Province}, {PostalCode}, {Country}";
        public string ShortAddress =>
            $"{StreetNumber} {StreetName}, {Suburb}, {City}";
    }
}

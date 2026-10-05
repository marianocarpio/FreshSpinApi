using System;
using System.ComponentModel.DataAnnotations;

namespace FreshSpinApi.Models
{
    public enum BookingStatus
    {
        Pending,
        Assigned,
        InWashing,
        ReadyForDelivery,
        Completed,
        Cancelled
    }

    public class Booking
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string CustomerId { get; set; } = string.Empty;

        [Required]
        public string ServiceType { get; set; } = string.Empty;

        public double WeightKg { get; set; }

        public decimal TotalCost { get; set; }

        public BookingStatus Status { get; set; } = BookingStatus.Pending;

        public string? AssignedRiderId { get; set; }

        public string PickupAddress { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
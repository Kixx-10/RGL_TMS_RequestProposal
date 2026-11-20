using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TMS_Proposal_Backend.Models.Master
{
    public class Truck
    {
        [Key]
        public Guid TruckId { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(50)]
        public string TruckNo { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string JobType { get; set; } = string.Empty;

        [Required]
        public DateTime ETA { get; set; } = DateTime.UtcNow;

        // false = Not Arrive (Default), true = Arrived
        [Required]
        public bool IsArrived { get; set; } = false;

        // false = Not Arrive (Default), true = Arrived
        [Required]
        public bool IsArrived { get; set; } = false;

        [Required]
        public Guid RequestId { get; set; }

        [ForeignKey("RequestId")]
        public Request? Request { get; set; }

        [Required]
        public DateTime CreatedDated { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedDate { get; set; }
    }
}
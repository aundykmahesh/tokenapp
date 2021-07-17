using System;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Token
    {
        [Key]
        public Guid Guid { get; set; }
        public string TokenString { get; set; }

        public string AppUrl { get; set; }

        public DateTime CreatedDate { get; set; }

        public TokenStatus Status { get; set; }
    }

    public enum TokenStatus
    {
        enabled, expired, disabled
    }
}
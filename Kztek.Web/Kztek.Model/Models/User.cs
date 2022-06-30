using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kztek.Model.Models
{
    public class User
    {
        [Key]
        public string Id { get; set; }

        public string Name { get; set; }

        //[Required]
        [StringLength(250)]
        public string Email { get; set; }

        [StringLength(500)]
        [Column(TypeName = "varchar")]
        public string ImagePath { get; set; }

        [Required]
        [StringLength(100)]
        public string Username { get; set; }

        [StringLength(500)]
        [Column(TypeName = "varchar")]
        public string Password { get; set; }

        [StringLength(500)]
        [Column(TypeName = "varchar")]
        public string PasswordSalat { get; set; }

        [StringLength(500)]
        public string Address { get; set; }

        [StringLength(150)]
        [Column(TypeName = "varchar")]
        public string Phone { get; set; }

        public bool Admin { get; set; }

        public bool Active { get; set; }

        public bool IsDeleted { get; set; }

        public string UserAvatar { get; set; }

        public Nullable<DateTime> DateCreated { get; set; }
    }
}
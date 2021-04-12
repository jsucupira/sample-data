using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Sampler.Core.Test
{

    public class LicenseModel
    {

        [MaxLength(10)]
        public string LicenseNumber { get; set; }

        [Required]
        public string BusinessName { get; set; }

        public string ConfirmationNumber { get; set; }

        [Required]
        public string BusinessPhone { get; set; }

        [MaxLength(10)]
        public string BusinessPhoneExtension { get; set; }

        [Required]
        public string BusinessAddress { get; set; }

        public string BusinessAddress2 { get; set; }

        [Required]
        public string BusinessCity { get; set; }

        [Required]
        [MaxLength(10)]
        public string BusinessState { get; set; }

        [Required]
        public string BusinessZip { get; set; }

        [MaxLength(2083)]
        public string BusinessWebSite { get; set; }

        public List<ContactModel> AdditionalContact { get; set; } = new();


        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? ExportedDate { get; set; }
        public bool HasBeenExported { get; set; }
    }

    public class ContactModel
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        [MaxLength(254)]
        public string Email { get; set; }
        public string Phone { get; set; }
        [MaxLength(10)]
        public string PhoneExtension { get; set; }

        [ForeignKey("LicenseId")]
        public int LicenseId { get; set; }
        public virtual LicenseModel License { get; set; }
    }

}


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlTypes;

namespace Backend.Data.Models
{
    public class Appointment
    {
        [Key]
        public int ID { get; set; }

        [MaxLength(150), Column(TypeName = "varchar(150)")]
        public string Title { get; set; } = "Title";

        [MaxLength(300), Column(TypeName = "varchar(300)")]
        public string Description { get; set; } = "Description";
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public string Address { get; set; } = "Address";

        [MaxLength(10), Column(TypeName = "varchar(10)")]
        public string Time { get; set; } = "12:30";

        public bool Done { get; set; } = false;
        public bool Deleted { get; set; } = false;
        public byte LevelOfImportance { get; set; } = 2;


    }
}
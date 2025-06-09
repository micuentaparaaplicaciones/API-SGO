// Revisado
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("USERS")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID")]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres.")]
        [Column("NAME")]
        public string Name { get; set; }

        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [StringLength(100, ErrorMessage = "El correo electrónico no puede exceder 100 caracteres.")]
        [EmailAddress(ErrorMessage = "El correo electrónico no tiene un formato válido.")]
        [Column("EMAIL")]
        public string Email { get; set; }

        [Required(ErrorMessage = "El teléfono es obligatorio.")]
        [MaxLength(15, ErrorMessage = "El teléfono no puede exceder 15 caracteres.")]
        [Phone(ErrorMessage = "El teléfono no es válido.")]
        [Column("PHONE")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "La dirección es obligatoria.")]
        [StringLength(255, ErrorMessage = "La dirección no puede exceder 255 caracteres.")]
        [Column("ADDRESS")]
        public string Address { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column("REGISTRATIONDATE")]
        public DateTime RegistrationDate { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [StringLength(255, ErrorMessage = "La contraseña no puede exceder 255 caracteres.")]
        [Column("PASSWORD")]
        public string Password { get; set; }

        [Required(ErrorMessage = "El rol es obligatorio.")]
        [StringLength(100, ErrorMessage = "El rol no puede exceder 100 caracteres.")]
        [Column("ROLE")]
        public string Role { get; set; }

        [Required(ErrorMessage = "El nombre de quien crea es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres.")]
        [Column("CREATEDBY")]
        public string CreatedBy { get; set; }

        [Required(ErrorMessage = "El nombre de quien modifica es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres.")]
        [Column("MODIFIEDBY")]
        public string ModifiedBy { get; set; }
    }
}

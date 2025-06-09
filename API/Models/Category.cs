// Revisado
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("CATEGORIES")]
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID")]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(50, ErrorMessage = "El nombre no puede exceder 50 caracteres.")]
        [Column("NAME")]
        public string Name { get; set; }

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

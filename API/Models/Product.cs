// Revisado
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("PRODUCTS")]
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID")]
        public int Id { get; set; }

        [Column("IMAGE")]
        public byte[] Image { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres.")]
        [Column("NAME")]
        public string Name { get; set; }

        [Column("DETAIL", TypeName = "CLOB")]
        public string Detail { get; set; }

        [Range(0, 99999999.99, ErrorMessage = "El precio debe estar entre 0 y 99,999,999.99.")]
        [Column("PRICE", TypeName = "NUMBER(10,2)")]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "La cantidad disponible no puede ser negativa.")]
        [Column("AVAILABLEQUANTITY")]
        public int AvailableQuantity { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column("REGISTRATIONDATE")]
        public DateTime RegistrationDate { get; set; }

        [Required(ErrorMessage = "El proveedor es obligatorio.")]
        [StringLength(100, ErrorMessage = "El proveedor no puede exceder 100 caracteres.")]
        [Column("SUPPLIER")]
        public string Supplier { get; set; }

        [Required(ErrorMessage = "La categoría es obligatoria.")]
        [StringLength(100, ErrorMessage = "La categoría no puede exceder 100 caracteres.")]
        [Column("CATEGORY")]
        public string Category { get; set; }

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
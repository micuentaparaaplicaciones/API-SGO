// Revisado
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("ORDERDETAILS")]
    public class OrderDetail
    {
        [Range(1, int.MaxValue, ErrorMessage = "El ID de orden debe ser mayor que cero.")]
        [Column("ORDERID", Order = 0)]
        public int OrderId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "El ID de producto debe ser mayor que cero.")]
        [Column("PRODUCTID", Order = 1)]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre del producto no puede exceder 100 caracteres.")]
        [Column("PRODUCTNAME")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio.")]
        [StringLength(100, ErrorMessage = "El estado no puede exceder 100 caracteres.")]
        [Column("STATUS")]
        public string Status { get; set; }

        [Column("NOTES", TypeName = "CLOB")]
        public string Notes { get; set; } = "Sin notas";

        [Range(1, int.MaxValue, ErrorMessage = "La cantidad solicitada debe ser mayor que cero.")]
        [Column("PRODUCTREQUESTEDQUANTITY")]
        public int ProductRequestedQuantity { get; set; }

        [Range(0, 99999999.99, ErrorMessage = "El precio del producto debe estar entre 0 y 99,999,999.99.")]
        [Column("PRODUCTPRICE", TypeName = "NUMBER(10,2)")]
        public decimal ProductPrice { get; set; }

        [Range(0, 99999999.99, ErrorMessage = "El subtotal debe estar entre 0 y 99,999,999.99.")]
        [Column("SUBTOTAL", TypeName = "NUMBER(10,2)")]
        public decimal Subtotal { get; set; }

        [Range(0, 99999999.99, ErrorMessage = "El descuento debe estar entre 0 y 99,999,999.99.")]
        [Column("DISCOUNT", TypeName = "NUMBER(10,2)")]
        public decimal Discount { get; set; }

        [Range(0, 99999999.99, ErrorMessage = "El impuesto debe estar entre 0 y 99,999,999.99.")]
        [Column("TAX", TypeName = "NUMBER(10,2)")]
        public decimal Tax { get; set; }

        [Range(0, 99999999.99, ErrorMessage = "El total debe estar entre 0 y 99,999,999.99.")]
        [Column("TOTAL", TypeName = "NUMBER(10,2)")]
        public decimal Total { get; set; }

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
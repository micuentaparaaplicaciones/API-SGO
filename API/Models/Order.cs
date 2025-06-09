// Revisado
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("ORDERS")]
public class Order
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // EF espera que DB genere el Id
    [Column("ID")]
    public int Id { get; set; }

    [Required(ErrorMessage = "El estado es obligatorio.")]
    [StringLength(100, ErrorMessage = "El estado no puede exceder 100 caracteres.")]
    [Column("STATUS")]
    public string Status { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Computed)] // DEFAULT CURRENT_TIMESTAMP
    [Column("REGISTRATIONDATE")]
    public DateTime RegistrationDate { get; set; }

    [Required(ErrorMessage = "La dirección de entrega es obligatoria.")]
    [StringLength(255, ErrorMessage = "La dirección no puede exceder 255 caracteres.")]
    [Column("DELIVERYADDRESS")]
    public string DeliveryAddress { get; set; }

    [Column("DELIVERYDATE")]
    public DateTime DeliveryDate { get; set; }

    [Required(ErrorMessage = "El ID del cliente es obligatorio.")]
    [StringLength(100, ErrorMessage = "El ID del cliente no puede exceder 100 caracteres.")]
    [Column("CUSTOMERID")]
    public string CustomerId { get; set; }

    [Required(ErrorMessage = "El nombre del cliente es obligatorio.")]
    [StringLength(100, ErrorMessage = "El nombre del cliente no puede exceder 100 caracteres.")]
    [Column("CUSTOMERNAME")]
    public string CustomerName { get; set; }

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

    [Required(ErrorMessage = "El método de pago es obligatorio.")]
    [StringLength(50, ErrorMessage = "El método de pago no puede exceder 50 caracteres.")]
    [Column("PAYMENTMETHOD")]
    public string PaymentMethod { get; set; }

    [Required(ErrorMessage = "El estado del pago es obligatorio.")]
    [StringLength(50, ErrorMessage = "El estado del pago no puede exceder 50 caracteres.")]
    [Column("PAYMENTSTATUS")]
    public string PaymentStatus { get; set; }

    [Required(ErrorMessage = "El nombre de quien crea es obligatorio.")]
    [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres.")]
    [Column("CREATEDBY")]
    public string CreatedBy { get; set; }

    [Required(ErrorMessage = "El nombre de quien modifica es obligatorio.")]
    [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres.")]
    [Column("MODIFIEDBY")]
    public string ModifiedBy { get; set; }
}
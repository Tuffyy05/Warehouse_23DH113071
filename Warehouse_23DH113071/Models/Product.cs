namespace Warehouse_23DH113071.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Products")]
    public partial class Product
    {
        [Key]
        [Display(Name = "Mã sản phẩm")]
        public int ProductID { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        [StringLength(100, ErrorMessage = "Tên sản phẩm tối đa 100 ký tự")]
        [Display(Name = "Tên sản phẩm")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Danh mục không được để trống")]
        [StringLength(100, ErrorMessage = "Danh mục tối đa 100 ký tự")]
        [Display(Name = "Danh mục")]
        public string Category { get; set; }

        [Required(ErrorMessage = "Đơn giá không được để trống")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Đơn giá phải lớn hơn 0")]
        [Display(Name = "Đơn giá")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Số lượng tồn không được để trống")]
        [Range(0, int.MaxValue, ErrorMessage = "Số lượng tồn phải là số nguyên không âm")]
        [Display(Name = "Số lượng tồn")]
        public int Quantity { get; set; }

        [NotMapped]
        public string Status
        {
            get
            {
                if (Quantity == 0) return "Hết hàng";
                if (Quantity >= 1 && Quantity <= 4) return "Sắp hết";
                return "Đủ hàng";
            }
        }

        [NotMapped]
        public string StatusBadgeClass
        {
            get
            {
                if (Quantity == 0) return "badge bg-danger";
                if (Quantity >= 1 && Quantity <= 4) return "badge bg-warning text-dark";
                return "badge bg-success";
            }
        }
    }
}

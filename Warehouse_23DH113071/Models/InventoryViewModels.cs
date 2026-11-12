namespace Warehouse_23DH113071.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    // ==========================================
    // CÂU 4: KIỂM KÊ 1 SẢN PHẨM
    // ==========================================
    public class SingleInventoryCheckViewModel
    {
        [Display(Name = "Chọn sản phẩm")]
        [Required(ErrorMessage = "Vui lòng chọn sản phẩm cần kiểm kê")]
        public int ProductID { get; set; }

        [Display(Name = "Tên sản phẩm")]
        public string ProductName { get; set; }

        [Display(Name = "Tồn hệ thống hiện tại")]
        public int CurrentQuantity { get; set; }

        public int SystemQuantity
        {
            get { return CurrentQuantity; }
            set { CurrentQuantity = value; }
        }

        [Display(Name = "Số lượng kiểm kê thực tế")]
        [Required(ErrorMessage = "Vui lòng nhập số lượng kiểm kê thực tế")]
        [Range(0, int.MaxValue, ErrorMessage = "Số lượng kiểm kê phải là số nguyên không âm (>= 0)")]
        public int? ActualQuantity { get; set; }

        public int? Difference { get; set; }
        public string DifferenceText { get; set; }

        public string DifferenceStatus
        {
            get
            {
                if (!Difference.HasValue) return "";
                if (Difference.Value > 0) return "Tăng";
                if (Difference.Value < 0) return "Giảm";
                return "Không chênh lệch";
            }
        }

        public string Message { get; set; }
        public bool IsSuccess { get; set; }

        public IEnumerable<SelectListItem> ProductList { get; set; }
    }

    public class InventoryCheckViewModel : SingleInventoryCheckViewModel
    {
    }

    // ==========================================
    // CÂU 5: KIỂM KÊ 2 SẢN PHẨM (TRANSACTION)
    // ==========================================
    public class DoubleInventoryCheckViewModel
    {
        // Sản phẩm 1 (A)
        [Display(Name = "Sản phẩm 1")]
        [Required(ErrorMessage = "Vui lòng chọn sản phẩm 1")]
        public int ProductID_A { get; set; }

        public int ProductID1
        {
            get { return ProductID_A; }
            set { ProductID_A = value; }
        }

        [Display(Name = "Số lượng thực tế 1")]
        [Required(ErrorMessage = "Vui lòng nhập số lượng thực tế cho sản phẩm 1")]
        [Range(0, int.MaxValue, ErrorMessage = "Số lượng thực tế sản phẩm 1 phải là số nguyên không âm (>= 0)")]
        public int? ActualQuantity_A { get; set; }

        public int? ActualQuantity1
        {
            get { return ActualQuantity_A; }
            set { ActualQuantity_A = value; }
        }

        // Sản phẩm 2 (B)
        [Display(Name = "Sản phẩm 2")]
        [Required(ErrorMessage = "Vui lòng chọn sản phẩm 2")]
        public int ProductID_B { get; set; }

        public int ProductID2
        {
            get { return ProductID_B; }
            set { ProductID_B = value; }
        }

        [Display(Name = "Số lượng thực tế 2")]
        [Required(ErrorMessage = "Vui lòng nhập số lượng thực tế cho sản phẩm 2")]
        [Range(0, int.MaxValue, ErrorMessage = "Số lượng thực tế sản phẩm 2 phải là số nguyên không âm (>= 0)")]
        public int? ActualQuantity_B { get; set; }

        public int? ActualQuantity2
        {
            get { return ActualQuantity_B; }
            set { ActualQuantity_B = value; }
        }

        public string Message { get; set; }
        public bool IsSuccess { get; set; }

        public IEnumerable<SelectListItem> ProductList { get; set; }

        public IEnumerable<SelectListItem> ProductList1
        {
            get { return ProductList; }
            set { ProductList = value; }
        }

        public IEnumerable<SelectListItem> ProductList2
        {
            get { return ProductList; }
            set { ProductList = value; }
        }
    }

    public class InventoryCheckTwoViewModel : DoubleInventoryCheckViewModel
    {
    }

    // ==========================================
    // CÂU 7: THỐNG KÊ TỒN KHO
    // ==========================================
    public class StatisticsViewModel
    {
        [Display(Name = "Tổng số sản phẩm")]
        public int TotalProducts { get; set; }

        [Display(Name = "Số sản phẩm hết hàng (Tồn = 0)")]
        public int OutOfStockCount { get; set; }

        [Display(Name = "Tổng giá trị hàng tồn (SUM: Giá x Số lượng)")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal TotalInventoryValue { get; set; }

        public List<Product> ProductDetails { get; set; }
    }

    public class WarehouseStatisticsViewModel : StatisticsViewModel
    {
    }

    // ==========================================
    // CÂU 3: TÌM KIẾM & BỘ LỌC
    // ==========================================
    public class SearchViewModel
    {
        public string Keyword { get; set; }

        public string SearchName
        {
            get { return Keyword; }
            set { Keyword = value; }
        }

        public string StockStatus { get; set; } // "All", "OutOfStock", "LowStock", "InStock", "HetHang", "SapHet", "DuHang"

        public string StatusFilter
        {
            get { return StockStatus; }
            set { StockStatus = value; }
        }

        public string SortOrder { get; set; } // "", "price_asc", "price_desc"

        public List<Product> Products { get; set; }

        public List<Product> Results
        {
            get { return Products ?? new List<Product>(); }
            set { Products = value; }
        }
    }

    public class ProductSearchViewModel : SearchViewModel
    {
    }
}

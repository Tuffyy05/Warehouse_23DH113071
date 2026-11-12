namespace Warehouse_23DH113071.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;
    using Warehouse_23DH113071.Models;

    public class ProductsController : Controller
    {
        private Warehouse_23DH113071Entities db = new Warehouse_23DH113071Entities();

        // ==========================================
        // CÂU 2a: DANH SÁCH SẢN PHẨM & TRẠNG THÁI
        // ==========================================
        public ActionResult Index()
        {
            var products = db.Products.OrderBy(p => p.ProductID).ToList();
            return View(products);
        }

        // ==========================================
        // CÂU 2b: SỬA THÔNG TIN SẢN PHẨM
        // Chỉ sửa ProductName, Category, Price
        // KHÔNG thay đổi ProductID hoặc Quantity
        // ==========================================
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,ProductName,Category,Price")] Product model)
        {
            // Validation bắt buộc
            if (string.IsNullOrWhiteSpace(model.ProductName))
            {
                ModelState.AddModelError("ProductName", "Tên sản phẩm không được để trống!");
            }
            if (string.IsNullOrWhiteSpace(model.Category))
            {
                ModelState.AddModelError("Category", "Danh mục không được để trống!");
            }
            if (model.Price <= 0)
            {
                ModelState.AddModelError("Price", "Đơn giá phải lớn hơn 0 (Price > 0)!");
            }

            if (ModelState.IsValid)
            {
                var productInDb = db.Products.Find(model.ProductID);
                if (productInDb == null)
                {
                    return HttpNotFound();
                }

                // Cập nhật đúng 3 thông tin theo đề bài yêu cầu:
                productInDb.ProductName = model.ProductName.Trim();
                productInDb.Category = model.Category.Trim();
                productInDb.Price = model.Price;
                // Tuyệt đối không thay đổi ProductID và Quantity!

                db.SaveChanges();
                TempData["SuccessMessage"] = $"Cập nhật sản phẩm #{productInDb.ProductID} thành công!";
                return RedirectToAction("Index");
            }

            // Nếu dữ liệu không hợp lệ, giữ nguyên Quantity từ DB để hiển thị
            var currentProd = db.Products.Find(model.ProductID);
            if (currentProd != null)
            {
                model.Quantity = currentProd.Quantity;
            }
            return View(model);
        }

        // ==========================================
        // CÂU 3: TRA CỨU HÀNG HÓA
        // Tìm kiếm theo tên, lọc tồn kho, sắp xếp đơn giá
        // ==========================================
        public ActionResult Search(string keyword = null, string stockStatus = "All", string sortOrder = "", string searchName = null, string statusFilter = null)
        {
            if (string.IsNullOrWhiteSpace(keyword) && !string.IsNullOrWhiteSpace(searchName)) keyword = searchName;
            if ((string.IsNullOrWhiteSpace(stockStatus) || stockStatus == "All") && !string.IsNullOrWhiteSpace(statusFilter)) stockStatus = statusFilter;

            IQueryable<Product> query = db.Products;

            // 1. Tìm theo một phần tên sản phẩm (không phân biệt hoa/thường)
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                var kw = keyword.Trim().ToLower();
                query = query.Where(p => p.ProductName.ToLower().Contains(kw));
            }

            // 2. Lọc theo trạng thái mức tồn kho
            if (!string.IsNullOrEmpty(stockStatus))
            {
                switch (stockStatus)
                {
                    case "OutOfStock":
                    case "HetHang": // Hết hàng: Quantity = 0
                        query = query.Where(p => p.Quantity == 0);
                        break;
                    case "LowStock":
                    case "SapHet": // Sắp hết: 1 <= Quantity <= 4
                        query = query.Where(p => p.Quantity >= 1 && p.Quantity <= 4);
                        break;
                    case "InStock":
                    case "DuHang": // Đủ hàng: Quantity >= 5
                        query = query.Where(p => p.Quantity >= 5);
                        break;
                    default:
                        break;
                }
            }

            // 3. Sắp xếp theo Price tăng hoặc giảm bằng LINQ/EF
            switch (sortOrder)
            {
                case "price_asc":
                    query = query.OrderBy(p => p.Price);
                    break;
                case "price_desc":
                    query = query.OrderByDescending(p => p.Price);
                    break;
                default:
                    query = query.OrderBy(p => p.ProductID);
                    break;
            }

            var vm = new SearchViewModel
            {
                Keyword = keyword,
                StockStatus = stockStatus,
                SortOrder = sortOrder,
                Products = query.ToList()
            };

            return View(vm);
        }

        // ==========================================
        // CÂU 4: CẬP NHẬT SỐ LƯỢNG THEO KIỂM KÊ (1 SẢN PHẨM)
        // Hiển thị chênh lệch tăng, giảm hoặc không chênh lệch
        // ==========================================
        public ActionResult InventoryCheck(int? id)
        {
            var vm = new SingleInventoryCheckViewModel();
            PrepareProductDropdown(ref vm, id);

            if (id.HasValue)
            {
                var prod = db.Products.Find(id.Value);
                if (prod != null)
                {
                    vm.ProductID = prod.ProductID;
                    vm.ProductName = prod.ProductName;
                    vm.CurrentQuantity = prod.Quantity;
                }
            }

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InventoryCheck(SingleInventoryCheckViewModel model)
        {
            var product = db.Products.Find(model.ProductID);
            if (product == null)
            {
                ModelState.AddModelError("ProductID", "Sản phẩm không tồn tại!");
            }
            else
            {
                model.ProductName = product.ProductName;
                model.CurrentQuantity = product.Quantity;
            }

            // Validation số lượng thực tế
            if (!model.ActualQuantity.HasValue)
            {
                ModelState.AddModelError("ActualQuantity", "Vui lòng nhập số lượng kiểm kê thực tế!");
            }
            else if (model.ActualQuantity.Value < 0)
            {
                ModelState.AddModelError("ActualQuantity", "Số lượng kiểm kê thực tế không được âm (< 0)!");
            }

            if (ModelState.IsValid && product != null)
            {
                int oldQty = product.Quantity;
                int newQty = model.ActualQuantity.Value;
                int diff = newQty - oldQty;
                model.Difference = diff;

                if (diff > 0)
                {
                    model.DifferenceText = $"Chênh lệch tăng {diff}";
                }
                else if (diff < 0)
                {
                    model.DifferenceText = $"Chênh lệch giảm {Math.Abs(diff)}";
                }
                else
                {
                    model.DifferenceText = "Không chênh lệch (0)";
                }

                // Cập nhật CSDL
                product.Quantity = newQty;
                db.SaveChanges();

                model.CurrentQuantity = newQty;
                model.IsSuccess = true;
                model.Message = $"Cập nhật kiểm kê thành công cho sản phẩm '{product.ProductName}'. Tồn mới: {newQty}. Kết quả: {model.DifferenceText}. Trạng thái hiện tại: {product.Status}.";
            }

            PrepareProductDropdown(ref model, model.ProductID);
            return View(model);
        }

        // ==========================================
        // CÂU 5: KIỂM KÊ ĐỒNG THỜI HAI SẢN PHẨM
        // Kiểm tra 2 ID khác nhau, cả 2 hợp lệ trong cùng Transaction
        // ==========================================
        public ActionResult InventoryCheckTwo()
        {
            var vm = new DoubleInventoryCheckViewModel();
            PrepareTwoProductsDropdown(ref vm);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InventoryCheckTwo(DoubleInventoryCheckViewModel model)
        {
            if (model.ProductID_A == 0 && model.ProductID1 > 0) model.ProductID_A = model.ProductID1;
            if (model.ProductID_B == 0 && model.ProductID2 > 0) model.ProductID_B = model.ProductID2;
            if (!model.ActualQuantity_A.HasValue && model.ActualQuantity1.HasValue) model.ActualQuantity_A = model.ActualQuantity1;
            if (!model.ActualQuantity_B.HasValue && model.ActualQuantity2.HasValue) model.ActualQuantity_B = model.ActualQuantity2;

            // 1. Kiểm tra 2 ProductID phải khác nhau
            if (model.ProductID_A == model.ProductID_B && model.ProductID_A > 0)
            {
                ModelState.AddModelError("", "Lỗi: Không được chọn cùng một mã sản phẩm cho cả hai lần kiểm kê! Hai sản phẩm phải khác nhau.");
            }

            // 2. Kiểm tra số lượng không âm
            if (!model.ActualQuantity_A.HasValue || model.ActualQuantity_A.Value < 0)
            {
                ModelState.AddModelError("ActualQuantity_A", "Số lượng thực tế của Sản phẩm A phải là số nguyên không âm (>= 0)!");
            }
            if (!model.ActualQuantity_B.HasValue || model.ActualQuantity_B.Value < 0)
            {
                ModelState.AddModelError("ActualQuantity_B", "Số lượng thực tế của Sản phẩm B phải là số nguyên không âm (>= 0)!");
            }

            Product prodA = null;
            Product prodB = null;

            if (model.ProductID_A > 0)
            {
                prodA = db.Products.Find(model.ProductID_A);
                if (prodA == null) ModelState.AddModelError("ProductID_A", "Sản phẩm A không tồn tại trong hệ thống!");
            }
            if (model.ProductID_B > 0)
            {
                prodB = db.Products.Find(model.ProductID_B);
                if (prodB == null) ModelState.AddModelError("ProductID_B", "Sản phẩm B không tồn tại trong hệ thống!");
            }

            // 3. Nếu mọi điều kiện hợp lệ, thực hiện trong 1 TRANSACTION duy nhất
            if (ModelState.IsValid && prodA != null && prodB != null)
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        prodA.Quantity = model.ActualQuantity_A.Value;
                        prodB.Quantity = model.ActualQuantity_B.Value;

                        db.SaveChanges();
                        transaction.Commit();

                        model.IsSuccess = true;
                        model.Message = $"Giao dịch kiểm kê đồng thời thành công! " +
                                        $"Sản phẩm A (#{prodA.ProductID} - {prodA.ProductName}) cập nhật thành {prodA.Quantity}. " +
                                        $"Sản phẩm B (#{prodB.ProductID} - {prodB.ProductName}) cập nhật thành {prodB.Quantity}.";
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        ModelState.AddModelError("", "Giao dịch thất bại! Đã rollback toàn bộ thay đổi. Chi tiết: " + ex.Message);
                    }
                }
            }

            PrepareTwoProductsDropdown(ref model);
            return View(model);
        }

        // ==========================================
        // CÂU 6: THÊM SẢN PHẨM CÓ KIỂM TRA TRÙNG TÊN
        // So sánh không phân biệt hoa thường và bỏ khoảng trắng
        // ==========================================
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductName,Category,Price,Quantity")] Product model)
        {
            // Validation trường rỗng
            if (string.IsNullOrWhiteSpace(model.ProductName))
            {
                ModelState.AddModelError("ProductName", "Tên sản phẩm không được để trống!");
            }
            if (string.IsNullOrWhiteSpace(model.Category))
            {
                ModelState.AddModelError("Category", "Danh mục không được để trống!");
            }
            if (model.Price <= 0)
            {
                ModelState.AddModelError("Price", "Đơn giá phải lớn hơn 0 (Price > 0)!");
            }
            if (model.Quantity < 0)
            {
                ModelState.AddModelError("Quantity", "Số lượng tồn phải là số nguyên không âm (>= 0)!");
            }

            // Kiểm tra trùng tên sản phẩm (chuẩn hóa trim và toLower)
            if (!string.IsNullOrWhiteSpace(model.ProductName))
            {
                string cleanName = model.ProductName.Trim().ToLower();
                // Duyệt qua DB và so sánh đã chuẩn hóa
                bool isDuplicate = db.Products.ToList().Any(p => p.ProductName.Trim().ToLower() == cleanName);
                if (isDuplicate)
                {
                    ModelState.AddModelError("ProductName", $"Tên sản phẩm '{model.ProductName.Trim()}' đã tồn tại trong hệ thống (sau khi chuẩn hóa khoảng trắng và chữ hoa/thường)! Vui lòng đặt tên khác.");
                }
            }

            if (ModelState.IsValid)
            {
                model.ProductName = model.ProductName.Trim();
                model.Category = model.Category.Trim();

                db.Products.Add(model);
                db.SaveChanges();

                TempData["SuccessMessage"] = $"Thêm sản phẩm '{model.ProductName}' thành công với ProductID = {model.ProductID}!";
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // ==========================================
        // CÂU 7: THỐNG KÊ KẾT QUẢ KIỂM KÊ
        // LINQ/EF tính: Tổng số SP, Số SP hết hàng, Tổng giá trị tồn
        // ==========================================
        public ActionResult Statistics()
        {
            var products = db.Products.OrderBy(p => p.ProductID).ToList();
            var vm = new StatisticsViewModel
            {
                TotalProducts = products.Count,
                OutOfStockCount = products.Count(p => p.Quantity == 0),
                TotalInventoryValue = products.Any()
                    ? products.Sum(p => p.Price * p.Quantity)
                    : 0m,
                ProductDetails = products
            };

            return View(vm);
        }

        // API Endpoint lấy số lượng hiện tại của 1 sản phẩm khi chọn dropdown (AJAX tiện dụng)
        [HttpGet]
        public JsonResult GetCurrentStock(int id)
        {
            var prod = db.Products.Find(id);
            if (prod != null)
            {
                return Json(new { success = true, quantity = prod.Quantity, name = prod.ProductName }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }

        // Helper nạp dropdown sản phẩm cho Câu 4
        private void PrepareProductDropdown(ref SingleInventoryCheckViewModel vm, int? selectedId = null)
        {
            var products = db.Products.OrderBy(p => p.ProductID).ToList();
            vm.ProductList = products.Select(p => new SelectListItem
            {
                Value = p.ProductID.ToString(),
                Text = $"#{p.ProductID} - {p.ProductName} (Tồn hiện tại: {p.Quantity})",
                Selected = selectedId.HasValue && p.ProductID == selectedId.Value
            });
        }

        // Helper nạp dropdown sản phẩm cho Câu 5
        private void PrepareTwoProductsDropdown(ref DoubleInventoryCheckViewModel vm)
        {
            var products = db.Products.OrderBy(p => p.ProductID).ToList();
            vm.ProductList = products.Select(p => new SelectListItem
            {
                Value = p.ProductID.ToString(),
                Text = $"#{p.ProductID} - {p.ProductName} (Tồn: {p.Quantity})"
            });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

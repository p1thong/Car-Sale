# Hướng Dẫn Chuyển Đổi Controllers/Pages Sang Sử Dụng DTOs

## Tổng Quan
Dự án đang được refactor để chuyển từ việc sử dụng trực tiếp Repository Models sang sử dụng DTOs (Data Transfer Objects) thông qua Services layer. Điều này giúp:
- Tách biệt rõ ràng các layer trong architecture
- Kiểm soát data truyền giữa các layer
- Dễ maintain và test hơn
- Tăng tính bảo mật

## Tiến Độ Hiện Tại

### ✅ Đã Hoàn Thành
1. **DTOs được tạo**: CustomerDto, OrderDto, PaymentDto, VehicleVariantDto, VehicleModelDto, TestDriveDto, FeedbackDto, ManufacturerDto, DealerDto, UserDto, SalesContractDto, PromotionDto, DealerContractDto
2. **Service Interfaces cập nhật**: Tất cả interfaces (ISalesService, ICustomerRelationshipService, IVehicleService, IDealerService, IAuthService) đã được cập nhật để return DTOs
3. **Mapper đã được tạo và đăng ký**: System mapping giữa Models và DTOs
4. **Một số Service Implementations**: SalesService và AuthService đã được update

### 🔄 Đang Thực Hiện
1. **Cập nhật Service Implementations còn lại**: CustomerRelationshipService, VehicleService, DealerService
2. **Page Models**: Đã bắt đầu với MyOrders.cshtml.cs

### ⏳ Cần Hoàn Thành
1. **Service Implementations còn lại**
2. **Tất cả Page Models (.cshtml.cs)**
3. **Views (.cshtml files)**
4. **Cleanup using statements**

## Cách Thực Hiện Chi Tiết

### 1. Cập nhật Service Implementation
```csharp
// Trước
public async Task<Customer> GetCustomerAsync(int customerId)
{
    return await _customerRepository.GetCustomerByIdAsync(customerId);
}

// Sau  
public async Task<CustomerDto> GetCustomerAsync(int customerId)
{
    var customer = await _customerRepository.GetCustomerByIdAsync(customerId);
    return customer != null ? _mapper.Map<Customer, CustomerDto>(customer) : null;
}
```

### 2. Cập nhật Page Models
```csharp
// Thêm using
using ASM1.Service.Dtos;

// Thay đổi properties
public IEnumerable<OrderDto> Orders { get; set; } = new List<OrderDto>();

// Thay đổi trong methods - service calls sẽ tự động return DTOs
Orders = await _salesService.GetOrdersByCustomerAsync(customer.CustomerId);
```

### 3. Cập nhật Views (.cshtml)
```html
<!-- Trước -->
@model IEnumerable<ASM1.Repository.Models.Order>

<!-- Sau -->
@model IEnumerable<ASM1.Service.Dtos.OrderDto>

<!-- Properties vẫn giống nhau do DTOs có cùng structure -->
@item.OrderId
@item.Status
@item.Quantity
@item.TotalPrice
```

### 4. Cập nhật _ViewImports.cshtml
```csharp
// Thêm
@using ASM1.Service.Dtos

// Có thể remove (sau khi hoàn thành)
@using ASM1.Repository.Models
```

## Lỗi Build Hiện Tại
```
- 53 errors: Các service implementations chưa được update
- Tất cả do interface signature đã đổi nhưng implementations chưa theo kịp
```

## Kế Hoạch Thực Hiện Tiếp Theo

### Bước 1: Hoàn thành Service Implementations
```bash
# Cần update các files:
- CustomerRelationshipService.cs
- VehicleService.cs  
- DealerService.cs
```

### Bước 2: Update Page Models theo thứ tự ưu tiên
```bash
# Quan trọng nhất:
- Customer/MyOrders.cshtml.cs (✅ Done)
- CustomerOrder/Payment.cshtml.cs
- Customer/OrderDetail.cshtml.cs
- DealerOrder/*.cshtml.cs

# Ít quan trọng:
- Admin/*.cshtml.cs
- CustomerService/*.cshtml.cs
```

### Bước 3: Update Views
```bash
# Theo thứ tự tương ứng với Page Models
```

### Bước 4: Cleanup
```bash
# Remove repository model references
# Clean up using statements
# Test thoroughly
```

## Test Strategy
1. **Unit Tests**: Test mapper functionality
2. **Integration Tests**: Test service layer with DTOs
3. **Manual Tests**: Test key user flows
4. **Performance**: Ensure mapping doesn't impact performance

## Lưu Ý Quan Trọng
- DTOs có cùng properties với Models nên Views ít thay đổi
- Mapper tự động handle conversion
- Cần careful với nullable types
- Navigation properties không có trong DTOs - cần separate calls hoặc composite DTOs

## Commands Hữu Ích
```bash
# Build và check errors
dotnet build

# Run specific tests
dotnet test --filter "ClassName=ServiceTests"

# Check for repository model references
grep -r "ASM1.Repository.Models" --include="*.cshtml.cs" Pages/
```

## Ví Dụ Hoàn Chỉnh: MyOrders Page

### Before:
```csharp
public IEnumerable<Order> Orders { get; set; } = new List<Order>();
```

### After:
```csharp
using ASM1.Service.Dtos;
public IEnumerable<OrderDto> Orders { get; set; } = new List<OrderDto>();
```

Service call vẫn như cũ vì interface đã được update:
```csharp
Orders = await _salesService.GetOrdersByCustomerAsync(customer.CustomerId);
```

View cũng không thay đổi vì DTOs có cùng properties.
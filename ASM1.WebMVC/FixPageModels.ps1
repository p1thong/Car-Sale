# Script PowerShell: Sửa tất cả PageModel kế thừa từ BasePageModel
# Mục đích: Đảm bảo authentication ViewData được set cho tất cả pages

$files = @(
    "Pages\Product\VehicleVariants.cshtml.cs",
    "Pages\Product\VehicleVariantDetails.cshtml.cs",
    "Pages\Product\VehicleModels.cshtml.cs",
    "Pages\Product\VehicleModelDetail.cshtml.cs",
    "Pages\Product\Manufacturers.cshtml.cs",
    "Pages\Product\EditVehicleVariant.cshtml.cs",
    "Pages\Product\EditVehicleModel.cshtml.cs",
    "Pages\Product\EditManufacturer.cshtml.cs",
    "Pages\Product\CreateVehicleVariant.cshtml.cs",
    "Pages\Product\CreateVehicleModel.cshtml.cs",
    "Pages\Product\CreateManufacturer.cshtml.cs",
    "Pages\Auth\Register.cshtml.cs",
    "Pages\Auth\Logout.cshtml.cs",
    "Pages\Auth\Login.cshtml.cs",
    "Pages\Home\VehicleDetail.cshtml.cs"
)

foreach ($file in $files) {
    $fullPath = Join-Path $PSScriptRoot $file
    if (Test-Path $fullPath) {
        $content = Get-Content $fullPath -Raw
        
        # Thay thế ": PageModel" thành ": BasePageModel"
        $newContent = $content -replace ':\s*PageModel\s*$', ': BasePageModel'
        $newContent = $newContent -replace ':\s*PageModel\s*\r?\n', ": BasePageModel`n"
        
        Set-Content $fullPath $newContent -NoNewline
        Write-Host "✓ Updated: $file" -ForegroundColor Green
    } else {
        Write-Host "✗ Not found: $file" -ForegroundColor Red
    }
}

Write-Host "`n✓ Completed! All PageModels now inherit from BasePageModel" -ForegroundColor Cyan

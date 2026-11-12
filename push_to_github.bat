@echo off
echo ===================================================================
echo PUSH PROJECT TO GITHUB: https://github.com/Tuffyy05/Warehouse_23DH113071
echo ===================================================================

cd /d "c:\Users\ltai7\OneDrive\Desktop\Ktra_LTW"

"C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\IDE\CommonExtensions\Microsoft\TeamFoundation\Team Explorer\Git\cmd\git.exe" push -u origin main

if %ERRORLEVEL% EQU 0 (
    echo.
    echo ===================================================================
    echo PUSH THANH CONG 100%!
    echo Xem repository tai: https://github.com/Tuffyy05/Warehouse_23DH113071
    echo ===================================================================
) else (
    echo.
    echo ===================================================================
    echo CHUA TAO REPO TREN GITHUB HOAC CAN DANG NHAP!
    echo Vui long vao https://github.com/new tao repo ten: Warehouse_23DH113071
    echo sau do chay lai file nay!
    echo ===================================================================
)

pause

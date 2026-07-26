@echo off
echo Building Angular frontend...
cd cropdeal-frontend
call npm run build

echo Copying built files to .NET wwwroot...
if not exist "..\wwwroot" mkdir "..\wwwroot"
xcopy /E /Y "dist\cropdeal-frontend\*" "..\wwwroot\"

echo Build completed successfully!
pause
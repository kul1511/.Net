@echo off
echo Setting up CropDeal Development Environment...

echo Installing Angular dependencies...
cd cropdeal-frontend
call npm install

echo Starting Angular development server...
start "Angular Dev Server" cmd /k "npm start"

echo Starting .NET API server...
cd ..
start "ASP.NET API" cmd /k "dotnet run"

echo Both servers are starting...
echo Angular: http://localhost:4200
echo API: https://localhost:7000
echo.
echo Press any key to exit...
pause
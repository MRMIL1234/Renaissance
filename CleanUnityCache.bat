@echo off
:: Unity Cache Cleaner - очищает временные файлы Unity
:: Запускать от имени администратора не нужно

echo Cleaning Unity caches...

:: Очистка temp
del /q /f /s "%TEMP%\*" 2>nul
for /d %%i in ("%TEMP%\*") do rmdir /s /q "%%i" 2>nul

:: Очистка Unity ShaderCache если есть
if exist "%LOCALAPPDATA%\Unity\Editor\ShaderCache" (
    del /q /f /s "%LOCALAPPDATA%\Unity\Editor\ShaderCache\*" 2>nul
)

:: Очистка Unity Cache
if exist "%LOCALAPPDATA%\Unity\Editor\Cache" (
    del /q /f /s "%LOCALAPPDATA%\Unity\Editor\Cache\*" 2>nul
)

:: Очистка Unity Temp
if exist "%APPDATA%\Unity\Temp" (
    del /q /f /s "%APPDATA%\Unity\Temp\*" 2>nul
)

echo Done! Close this window.
pause
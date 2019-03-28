set d=?:\

if %REWISE% == desktop (set d=d:\)
if %REWISE% == ntb (set d=c:\)

set root=%d%rewise\

rmdir %root%data\02_raw /s /q
rmdir %root%data\03_parsed /s /q

pause

cd %root%dlibs\utils

call dart lib\src\rewise\toRawMain.dart

rem call dart lib\src\rewise\parsing\toParsedMain.dart

rem call dart lib\src\rewise\stemming\stemmingMain.dart

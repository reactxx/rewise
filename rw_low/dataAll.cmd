set d=?:\

if %REWISE% == desktop (set d=d:\)
if %REWISE% == ntb (set d=c:\)

set root=%d%rewise\

cd %root%dlibs\utils

rmdir %root%data\02_raw /s /q
call dart lib\src\rewise\toRawMain.dart

rmdir %root%data\03_parsed /s /q
call dart lib\src\rewise\parsing\toParsedMain.dart

rmdir %root%data\log\parsed /s /q
call dart lib\src\rewise\stat\statMain.dart


rem rmdir %root%data\stemmCache /s /q
rem call dart lib\src\rewise\stemming\stemmingMain.dart

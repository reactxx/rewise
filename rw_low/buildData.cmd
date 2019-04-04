set d=?:\

if %REWISE% == desktop (set d=d:\)
if %REWISE% == ntb (set d=c:\)

set drive=%d:~0,2%
%drive%

set root=%d%rewise\

cd %root%dlibs\utils

rem rmdir %root%data\02_raw /s /q
rem call dart lib\src\rewise\toRawMain.dart

rem rmdir %root%data\03_parsed /s /q
rem call dart lib\src\rewise\parsing\toParsedMain.dart

rem rmdir %root%data\stemmCache /s /q
rem call dart lib\src\rewise\stemming\stemmingMain.dart

rem ---- STATs

rmdir %root%data\log\parsed /s /q
call dart lib\src\rewise\stat\statMain.dart

rem rmdir %root%data\log\stemmed /s /q
rem call dart lib\src\rewise\stat\statStemmMain.dart



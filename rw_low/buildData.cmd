set d=?:\

if %REWISE% == desktop (set d=d:\)
if %REWISE% == ntb (set d=c:\)

set drive=%d:~0,2%
%drive%

set root=%d%rewise\

cd %root%dlibs\utils

rem rmdir %root%data\02_source /s /q
rem call dart lib/src/rewise/sources/batches/mainFromCSV.dart

rem call dart lib/src/rewise/sources/batches/mainReparseFacts.dart

rmdir %root%data\03_edits\analyzeSources /s /q

call dart lib/src/rewise/sources/batches/mainAll.dart

pause
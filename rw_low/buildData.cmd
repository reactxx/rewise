set d=?:\

if %REWISE% == desktop (set d=d:\)
if %REWISE% == ntb (set d=c:\)

set drive=%d:~0,2%
%drive%

set root=%d%rewise\

cd %root%dlibs\utils

rem rmdir %root%data\02_source /s /q
rem call dart lib/src/rewise/sources/batches/fromCSVMain.dart

rem call dart lib/src/rewise/sources/batches/refreshMain.dart

%root%data\03_edits\wrongFacts /s /q
call dart lib/src/rewise/sources/batches/main.dart

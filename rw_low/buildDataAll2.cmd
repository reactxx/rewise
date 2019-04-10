set d=?:\

if %REWISE% == desktop (set d=d:\)
if %REWISE% == ntb (set d=c:\)

set drive=%d:~0,2%
%drive%

set root=%d%rewise\

cd %root%dlibs\utils

rmdir %root%data\02_source /s /q
call dart lib/src/rewise/sources/fromCSVMain.dart

call dart lib/src/rewise/sources/refreshMain.dart

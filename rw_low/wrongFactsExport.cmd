set d=?:\

if %REWISE% == desktop (set d=d:\)
if %REWISE% == ntb (set d=c:\)

set drive=%d:~0,2%
%drive%

set root=%d%rewise\

cd %root%dlibs\utils

rmdir %root%data\03_edits\wrongFacts /s /q
call dart lib/src/rewise/sources/batches/mainWrongFactsExport.dart

pause
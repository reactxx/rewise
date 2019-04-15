set d=?:\

if %REWISE% == desktop (set d=d:\)
if %REWISE% == ntb (set d=c:\)

set drive=%d:~0,2%
%drive%

set root=%d%rewise\

cd %root%dlibs\utils

call dart lib/src/rewise/sources/batches/mainWrongFactsImport.dart
rmdir %root%data\03_edits\wrongFacts /s /q

pause
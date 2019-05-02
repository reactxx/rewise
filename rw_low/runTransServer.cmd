set d=?:\

if %REWISE% == desktop (set d=d:\)
if %REWISE% == ntb (set d=c:\)

set drive=%d:~0,2%
%drive%

cd %d%rewise\dlibs\utils

call dart lib/src/rewise/trans/server.dart

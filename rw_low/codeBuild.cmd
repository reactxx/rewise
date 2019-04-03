set d=?:\

if %REWISE% == desktop (set d=d:\)
if %REWISE% == ntb (set d=c:\)

set root=%d%rewise\

cd %root%/rw_low
call pub run build_runner build

cd %root%/dlibs/rw_utils
call pub run build_runner build

cd %root%/dlibs/server
call pub run build_runner build

rem dart lib/runs/toParsed.dart --snapshot-kind=app-jit --observe
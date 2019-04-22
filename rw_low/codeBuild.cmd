set d=?:\

if %REWISE% == desktop (set d=d:\)
if %REWISE% == ntb (set d=c:\)

set drive=%d:~0,2%
%drive%

set root=%d%rewise\

cd %root%/rw_low
call pub run build_runner build

cd %root%/dlibs/rw_utils
call pub run build_runner build

rem cd %root%/dlibs/server
rem call pub run build_runner build

pause

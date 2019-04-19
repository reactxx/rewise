set d=?:\

if %REWISE% == desktop (set d=d:\)
if %REWISE% == ntb (set d=c:\)

set root=%d%rewise\
set plugin=protoc-gen-dart=c:\Users\pavel\AppData\Roaming\Pub\Cache\bin\protoc-gen-dart.bat
set src=%root%rw_low\include
set dart=%root%dlibs\utils\lib\src\messages

set all=^
 rewise\spellCheck\spellcheck_service^
 rewise\stemming\stemming_service^
 rewise\utils\langs^
 rewise\utils\matrix^
 rewise\word_breaking\word_breaking_service^
 google\protobuf\wrappers^
 google\protobuf\empty

FOR %%A IN (%all%) DO (
    call "%d%rewise\protobuf\compiler\bin\protoc.exe" --proto_path=%src% %src%\%%A.proto --dart_out=grpc:%dart% --plugin=%plugin%
)


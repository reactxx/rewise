set d=?:\

if %REWISE% == desktop (set d=d:\)
if %REWISE% == ntb (set d=c:\)

set root=%d%rewise\
set plugin=protoc-gen-dart=c:\Users\pavel\AppData\Roaming\Pub\Cache\bin\protoc-gen-dart.bat
set src=%root%rw_low\include
set dart=%root%dlibs\utils\lib\src\messages

set all=^
 rewise\dom\dom^
 rewise\hack_json\hack_json_service^
 rewise\hallo_world\hello_world_service^
 rewise\stemming\stemming_service^
 rewise\to_parsed\to_parsed_service^
 rewise\to_raw\to_raw_service^
 rewise\utils\common^
 rewise\utils\config^
 rewise\utils\langs^
 rewise\utils\matrix^
 rewise\word_breaking\word_breaking_service^
 google\protobuf\wrappers^
 google\protobuf\empty

FOR %%A IN (%all%) DO (
    call "%d%rewise\protobuf\compiler\bin\protoc.exe" --proto_path=%src% %src%\%%A.proto --dart_out=grpc:%dart% --plugin=%plugin%
)


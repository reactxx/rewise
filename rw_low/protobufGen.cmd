set d=?:\

if %REWISE% == desktop (set d=d:\)
if %REWISE% == ntb (set d=c:\)

set root=%d%rewise\
set plugin=protoc-gen-dart=c:\Users\pavel\AppData\Roaming\Pub\Cache\bin\protoc-gen-dart.bat
set src=%root%rw_low\include
set dart=%root%dlibs\utils\lib\src\messages

set all=^
 google\protobuf\any^
 google\protobuf\duration^
 google\protobuf\empty^
 google\protobuf\descriptor^
 google\protobuf\wrappers^
 google\protobuf\empty^
 google\protobuf\timestamp^
 ^
 google\api\annotations^
 google\api\field_behavior^
 google\api\http^
 google\api\client^
 google\api\resource^
 google\rpc\status^
 ^
 google\cloud\speech\v1\cloud_speech^
 google\cloud\texttospeech\v1\cloud_tts^
 google\cloud\translate\v3beta1\translation_service^
 google\cloud\language\v1\language_service^
 google\longrunning\operations^
 ^
 rewise\spellCheck\spellcheck_service^
 rewise\stemming\stemming_service^
 rewise\utils\langs^
 rewise\utils\matrix^
 rewise\utils\streaming^
 rewise\word_breaking\word_breaking_service^
 

FOR %%A IN (%all%) DO (
    call "%d%rewise\protobuf\compiler\bin\protoc.exe" --proto_path=%src% %src%\%%A.proto --dart_out=grpc:%dart% --plugin=%plugin%
)


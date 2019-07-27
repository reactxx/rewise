rmdir "c:\rewise\windows\flutter-desktop-embedding\example\lib" /q
mklink /D "c:\rewise\windows\flutter-desktop-embedding\example\lib\" "c:\rewise\windowsDir\lib\"

del "c:\rewise\windows\flutter-desktop-embedding\example\pubspec.yaml" /q
mklink "c:\rewise\windows\flutter-desktop-embedding\example\pubspec.yaml" "c:\rewise\windowsDir\pubspec.yaml"

set src=c:\rewise\windowsDir\

rmdir "c:\rewise\windows\flutter-desktop-embedding\example\lib" /q
mklink /D "c:\rewise\windows\flutter-desktop-embedding\example\lib\" "%src%lib\"

del "c:\rewise\windows\flutter-desktop-embedding\example\pubspec.yaml" /q
copy "%src%pubspec.yaml" "c:\rewise\windows\flutter-desktop-embedding\example\pubspec.yaml" /Y

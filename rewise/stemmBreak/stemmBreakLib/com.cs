//Get interface from DLL
//https://gist.github.com/jjeffery/1568627
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace StemmerBreakerNew {

  //===============================================================
  // ComHelper
  //===============================================================
  public static class ComHelper {
    delegate int DllGetClassObject(ref Guid clsid, ref Guid iid, [Out, MarshalAs(UnmanagedType.Interface)] out IClassFactory classFactory);
    static Guid classFactoryIid = new Guid("00000000-0000-0000-C000-000000000046");

    public static T CreateInstance<T>(IClassFactory classFactory, Type interfaceType) where T : class {
      object obj;
      var wbGuid = interfaceType.GUID;
      classFactory.CreateInstance(null, ref wbGuid, out obj);
      var res = obj as T;
      if (res == null)
        throw new Exception("res == null");
      return res;
    }

    public static IClassFactory GetClassFactory(LibraryModule libraryModule, Guid clsid) {
      IntPtr ptr = libraryModule.GetProcAddress("DllGetClassObject");
      var getClassObject = (DllGetClassObject)Marshal.GetDelegateForFunctionPointer(ptr, typeof(DllGetClassObject));

      IClassFactory classFactory;
      var hresult = getClassObject(ref clsid, ref classFactoryIid, out classFactory);

      if (hresult != 0) {
        throw new Win32Exception(hresult, "Cannot create class factory");
        //return null;
      }
      return classFactory;
    }
  }

  //===============================================================
  // LibraryModule
  //===============================================================
  public class LibraryModule {
    private IntPtr _handle;
    private readonly string _filePath;

    private static class Win32 {
      [DllImport("kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
      public static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

      [DllImport("kernel32.dll")]
      public static extern bool FreeLibrary(IntPtr hModule);

      [DllImport("kernel32.dll", SetLastError = true)]
      public static extern IntPtr LoadLibrary(string lpFileName);
    }


    public static LibraryModule LoadModule(string filePath) {
      var libraryModule = new LibraryModule(Win32.LoadLibrary(filePath), filePath);
      if (libraryModule._handle == IntPtr.Zero) {
        int error = Marshal.GetLastWin32Error();
        throw new Win32Exception(error, "Cannot load library: " + filePath);
      }

      return libraryModule;
    }

    LibraryModule(IntPtr handle, string filePath) {
      _filePath = filePath;
      _handle = handle;
    }

    public IntPtr GetProcAddress(string name) {
      IntPtr ptr = Win32.GetProcAddress(_handle, "DllGetClassObject");
      if (ptr == IntPtr.Zero) {
        int error = Marshal.GetLastWin32Error();
        string message = string.Format("Cannot find proc {0} in {1}", name, _filePath);
        throw new Win32Exception(error, message);
      }
      return ptr;
    }

  }

  //===============================================================
  // IClassFactory
  //===============================================================
  [Guid("00000001-0000-0000-c000-000000000046")]
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  [ComImport]
  public interface IClassFactory {
    void CreateInstance([MarshalAs(UnmanagedType.IUnknown)] object pUnkOuter, ref Guid riid, [MarshalAs(UnmanagedType.IUnknown)] out object ppvObject);
    void LockServer(bool fLock);
  }


  //===============================================================
  // Stemmer stuff
  //===============================================================
  [ComImport]
  [Guid("fe77c330-7f42-11ce-be57-00aa0051fe20")]
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  public interface IWordFormSink {
    void PutAltWord([MarshalAs(UnmanagedType.LPWStr)] string pwcInBuf, [MarshalAs(UnmanagedType.U4)] int cwc);
    void PutWord([MarshalAs(UnmanagedType.LPWStr)] string pwcInBuf, [MarshalAs(UnmanagedType.U4)] int cwc);
  }
  [ComImport]
  [Guid("efbaf140-7f42-11ce-be57-00aa0051fe20")]
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  public interface IStemmer {
    void Init([MarshalAs(UnmanagedType.U4)] int ulMaxTokenSize, [MarshalAs(UnmanagedType.Bool)] out bool pfLicense);
    void GenerateWordForms([MarshalAs(UnmanagedType.LPWStr)] string pwcInBuf, [MarshalAs(UnmanagedType.U4)] int cwc, [MarshalAs(UnmanagedType.Interface)] IWordFormSink pStemSink);
    void GetLicenseToUse([MarshalAs(UnmanagedType.LPWStr)] out string ppwcsLicense);
  }

  //===============================================================
  // Wordbreaker stuff
  //===============================================================
  [Flags]
  public enum WORDREP_BREAK_TYPE {
    WORDREP_BREAK_EOW = 0,
    WORDREP_BREAK_EOS = 1,
    WORDREP_BREAK_EOP = 2,
    WORDREP_BREAK_EOC = 3
  }

  [ComImport]
  [Guid("CC907054-C058-101A-B554-08002B33B0E6")]
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  public interface IWordSink {
    void PutWord([MarshalAs(UnmanagedType.U4)] int cwc,
    [MarshalAs(UnmanagedType.LPWStr)] string pwcInBuf,
    [MarshalAs(UnmanagedType.U4)] int cwcSrcLen,
    [MarshalAs(UnmanagedType.U4)] int cwcSrcPos);
    void PutAltWord([MarshalAs(UnmanagedType.U4)] int cwc,
    [MarshalAs(UnmanagedType.LPWStr)] string pwcInBuf,
    [MarshalAs(UnmanagedType.U4)] int cwcSrcLen,
    [MarshalAs(UnmanagedType.U4)] int cwcSrcPos);
    void StartAltPhrase();
    void EndAltPhrase();
    void PutBreak(WORDREP_BREAK_TYPE breakType);
  }

  [ComImport]
  [Guid("CC906FF0-C058-101A-B554-08002B33B0E6")]
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  public interface IPhraseSink {
    void PutSmallPhrase([MarshalAs(UnmanagedType.LPWStr)] string pwcNoun,
    [MarshalAs(UnmanagedType.U4)] int cwcNoun,
    [MarshalAs(UnmanagedType.LPWStr)] string pwcModifier,
    [MarshalAs(UnmanagedType.U4)] int cwcModifier,
    [MarshalAs(UnmanagedType.U4)] int ulAttachmentType);
    void PutPhrase([MarshalAs(UnmanagedType.LPWStr)] string pwcPhrase,
    [MarshalAs(UnmanagedType.U4)] int cwcPhrase);
  }

  [StructLayout(LayoutKind.Sequential)]
  public struct TEXT_SOURCE {
    [MarshalAs(UnmanagedType.FunctionPtr)] public delFillTextBuffer pfnFillTextBuffer;
    [MarshalAs(UnmanagedType.LPWStr)] public string awcBuffer;
    [MarshalAs(UnmanagedType.U4)] public int iEnd;
    [MarshalAs(UnmanagedType.U4)] public int iCur;
  }

  // used to fill the buffer for TEXT_SOURCE
  public delegate uint delFillTextBuffer([MarshalAs(UnmanagedType.Struct)] ref TEXT_SOURCE pTextSource);

  [ComImport]
  [Guid("D53552C8-77E3-101A-B552-08002B33B0E6")]
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  public interface IWordBreaker {
    void Init([MarshalAs(UnmanagedType.Bool)] bool fQuery,
    [MarshalAs(UnmanagedType.U4)] int maxTokenSize,
    [MarshalAs(UnmanagedType.Bool)] out bool pfLicense);
    void BreakText([MarshalAs(UnmanagedType.Struct)] ref TEXT_SOURCE pTextSource,
    [MarshalAs(UnmanagedType.Interface)] IWordSink pWordSink,
    [MarshalAs(UnmanagedType.Interface)] IPhraseSink pPhraseSink);
    void GetLicenseToUse([MarshalAs(UnmanagedType.LPWStr)] out string ppwcsLicense);
  }
}


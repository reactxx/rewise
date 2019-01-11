using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

//*********** Extract from Hunspell, where it is internal

namespace fulltext {
  public static class encoding
  {

    public static Encoding getEncoding(string affixFileName) {
      string encoding;
      using (Stream aff1 = File.OpenRead(affixFileName))
      {
        encoding = GetDictionaryEncoding(aff1);
      }

      // pass 2: parse affixes
      return GetSystemEncoding(encoding);
    }

    static string GetDictionaryEncoding(Stream affix)
    {
      StringBuilder encoding = new StringBuilder();
      for (; ; )
      {
        encoding.Length = 0;
        int ch;
        while ((ch = affix.ReadByte()) > 0)
        {
          if (ch == '\n')
          {
            break;
          }
          if (ch != '\r')
          {
            encoding.Append((char)ch);
          }
        }
        if (encoding.Length == 0 || encoding[0] == '#' || encoding.ToString().Trim().Length == 0)
        {
          // this test only at the end as ineffective but would allow lines only containing spaces:
          if (ch < 0)
          {
            throw new Exception("Unexpected end of affix file." /*, 0*/);
          }
          continue;
        }
        Match matcher = ENCODING_PATTERN.Match(encoding.ToString());
        if (matcher.Success)
        {
          int last = matcher.Index + matcher.Length;
          return encoding.ToString(last, encoding.Length - last).Trim();
        }
      }
    }
    static Regex ENCODING_PATTERN = new Regex("^(\u00EF\u00BB\u00BF)?SET\\s+", RegexOptions.Compiled);


    static Encoding GetSystemEncoding(string encoding)
    {
      if (string.IsNullOrEmpty(encoding))
      {
        return Encoding.UTF8;
      }
      if ("ISO8859-14".Equals(encoding, StringComparison.OrdinalIgnoreCase))
      {
        return new ISO8859_14Encoding();
      }
      // .NET doesn't recognize the encoding without a dash between ISO and the number
      // https://msdn.microsoft.com/en-us/library/system.text.encodinginfo.getencoding(v=vs.110).aspx
      if (encoding.Length > 3 && encoding.StartsWith("ISO", StringComparison.OrdinalIgnoreCase) &&
          encoding[3] != '-')
      {
        encoding = "iso-" + encoding.Substring(3);
      }
      // Special case - for codepage 1250-1258, we need to change to 
      // windows-1251, etc.
      else if (windowsCodePagePattern.IsMatch(encoding))
      {
        encoding = "windows-" + windowsCodePagePattern.Match(encoding).Groups[1].Value;
      }
      // Special case - for Thai we need to switch to windows-874
      else if (thaiCodePagePattern.IsMatch(encoding))
      {
        encoding = "windows-874";
      }

      return Encoding.GetEncoding(encoding);
    }

    private static Regex windowsCodePagePattern = new Regex("^(?:microsoft-)?cp-?(125[0-8])$", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
    private static Regex thaiCodePagePattern = new Regex("^tis-?620(?:-?2533)?$", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
  }

  internal sealed class ISO8859_14Encoding : Encoding
  {
    private static readonly Decoder decoder = new ISO8859_14Decoder();
    public override Decoder GetDecoder()
    {
      return new ISO8859_14Decoder();
    }

    public override string EncodingName {
      get {
        return "iso-8859-14";
      }
    }

    public override int CodePage {
      get {
        return 28604;
      }
    }

    public override int GetCharCount(byte[] bytes, int index, int count)
    {
      return decoder.GetCharCount(bytes, index, count);
    }

    public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
    {
      return decoder.GetChars(bytes, byteIndex, byteCount, chars, charIndex);
    }

    public override int GetMaxCharCount(int byteCount)
    {
      return byteCount;
    }


    #region Encoding Not Implemented
    public override int GetByteCount(char[] chars, int index, int count)
    {
      throw new NotImplementedException();
    }

    public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
    {
      throw new NotImplementedException();
    }

    public override int GetMaxByteCount(int charCount)
    {
      throw new NotImplementedException();
    }

    #endregion
  }

  internal sealed class ISO8859_14Decoder : Decoder
  {
    internal static readonly char[] TABLE = new char[]
    {
            (char)0x00A0, (char)0x1E02, (char)0x1E03, (char)0x00A3, (char)0x010A, (char)0x010B, (char)0x1E0A, (char)0x00A7,
            (char)0x1E80, (char)0x00A9, (char)0x1E82, (char)0x1E0B, (char)0x1EF2, (char)0x00AD, (char)0x00AE, (char)0x0178,
            (char)0x1E1E, (char)0x1E1F, (char)0x0120, (char)0x0121, (char)0x1E40, (char)0x1E41, (char)0x00B6, (char)0x1E56,
            (char)0x1E81, (char)0x1E57, (char)0x1E83, (char)0x1E60, (char)0x1EF3, (char)0x1E84, (char)0x1E85, (char)0x1E61,
            (char)0x00C0, (char)0x00C1, (char)0x00C2, (char)0x00C3, (char)0x00C4, (char)0x00C5, (char)0x00C6, (char)0x00C7,
            (char)0x00C8, (char)0x00C9, (char)0x00CA, (char)0x00CB, (char)0x00CC, (char)0x00CD, (char)0x00CE, (char)0x00CF,
            (char)0x0174, (char)0x00D1, (char)0x00D2, (char)0x00D3, (char)0x00D4, (char)0x00D5, (char)0x00D6, (char)0x1E6A,
            (char)0x00D8, (char)0x00D9, (char)0x00DA, (char)0x00DB, (char)0x00DC, (char)0x00DD, (char)0x0176, (char)0x00DF,
            (char)0x00E0, (char)0x00E1, (char)0x00E2, (char)0x00E3, (char)0x00E4, (char)0x00E5, (char)0x00E6, (char)0x00E7,
            (char)0x00E8, (char)0x00E9, (char)0x00EA, (char)0x00EB, (char)0x00EC, (char)0x00ED, (char)0x00EE, (char)0x00EF,
            (char)0x0175, (char)0x00F1, (char)0x00F2, (char)0x00F3, (char)0x00F4, (char)0x00F5, (char)0x00F6, (char)0x1E6B,
            (char)0x00F8, (char)0x00F9, (char)0x00FA, (char)0x00FB, (char)0x00FC, (char)0x00FD, (char)0x0177, (char)0x00FF
    };

    public override int GetCharCount(byte[] bytes, int index, int count)
    {
      return count;
    }

    public override int GetChars(byte[] bytesIn, int byteIndex, int byteCount, char[] charsOut, int charIndex)
    {
      int writeCount = 0;
      int charPointer = charIndex;

      for (int i = byteIndex; i < (byteIndex + byteCount); i++)
      {
        // Decode the value
        char ch = (char)(bytesIn[i] & 0xff);
        if (ch >= 0xA0)
        {
          ch = TABLE[ch - 0xA0];
        }
        // write the value to the correct buffer slot
        charsOut[charPointer] = ch;
        writeCount++;
        charPointer++;
      }

      return writeCount;
    }
  }
}


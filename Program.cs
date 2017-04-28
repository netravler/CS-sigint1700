// Decompiled with JetBrains decompiler
// Type: sigint1700.Program
// Assembly: sigint1700, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AB900B7A-36F9-4D85-BC77-32D952F1F622
// Assembly location: C:\Users\pheintz\Desktop\sigint1700.exe

using System;
using System.Windows.Forms;

namespace sigint1700
{
  internal static class Program
  {
    [STAThread]
    private static void Main()
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run((Form) new Form1());
    }
  }
}

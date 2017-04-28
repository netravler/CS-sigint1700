// Decompiled with JetBrains decompiler
// Type: sigint1700.Properties.Settings
// Assembly: sigint1700, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AB900B7A-36F9-4D85-BC77-32D952F1F622
// Assembly location: C:\Users\pheintz\Desktop\sigint1700.exe

using System.CodeDom.Compiler;
using System.Configuration;
using System.Runtime.CompilerServices;

namespace sigint1700.Properties
{
  [GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "10.0.0.0")]
  [CompilerGenerated]
  internal sealed class Settings : ApplicationSettingsBase
  {
    private static Settings defaultInstance = (Settings) SettingsBase.Synchronized((SettingsBase) new Settings());

    public static Settings Default
    {
      get
      {
        Settings settings = Settings.defaultInstance;
        return settings;
      }
    }
  }
}

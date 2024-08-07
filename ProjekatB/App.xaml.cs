using ProjekatB.Properties;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WPFLocalizeExtension.Engine;
namespace ProjekatB
{
    public enum Skin { Red, Blue }

    public partial class App : Application
    {
        public App() {
            LocalizeDictionary.Instance.SetCurrentThreadCulture = true;
            LocalizeDictionary.Instance.Culture= new CultureInfo("sr-Latn-RS");
                }
    }
}

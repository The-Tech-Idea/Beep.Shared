using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheTechIdea.Beep.ConfigUtil;
using TheTechIdea.Beep.Vis.Modules;
using TheTechIdea.Beep.Winform.Controls.Models;

namespace TheTechIdea.Beep.Shared
{
    public static class HandlersFactory
    {
        // Already exists
        public static Func<SimpleItem, List<SimpleItem>> GlobalMenuItemsProvider { get; set; }

        // New delegates:
        public static Action<SimpleItem, string> RunFunctionHandler { get; set; }

        public static Action< SimpleItem, string> RunFunctionWithTreeHandler { get; set; }

        public static Func< object, string, IErrorsInfo> RunMethodFromObjectHandler { get; set; }

        public static Action< IBranch, string> RunMethodFromExtensionWithTreeHandler { get; set; }

        public static Action<IBranch, AssemblyClassDefinition, string> RunMethodFromExtensionHandler { get; set; }
    }

}

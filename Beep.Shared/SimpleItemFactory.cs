using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheTechIdea.Beep.ConfigUtil;
using TheTechIdea.Beep.Vis.Modules;
using TheTechIdea.Beep.Winform.Controls.Models;

namespace TheTechIdea.Beep.Shared
{
    public static class SimpleItemFactory
    {
        // Already exists
        public static Func<SimpleItem, List<SimpleItem>> GlobalMenuItemsProvider { get; set; }

        // New delegates:
        public static Action<SimpleItem, string> RunFunctionHandler { get; set; }

        public static Action<SimpleItem, string> RunFunctionWithTreeHandler { get; set; }

        public static Func<object, string, IErrorsInfo> RunMethodFromObjectHandler { get; set; }

        public static Action<IBranch, string> RunMethodFromExtensionWithTreeHandler { get; set; }

        public static Action<IBranch, AssemblyClassDefinition, string> RunMethodFromExtensionHandler { get; set; }

        // i neet function to set the delegates
        public static void SetDelegates(
            Func<SimpleItem, List<SimpleItem>> menuItemsProvider,
            Action<SimpleItem, string> runFunctionHandler,
            Action<SimpleItem, string> runFunctionWithTreeHandler,
            Func<object, string, IErrorsInfo> runMethodFromObjectHandler,
            Action<IBranch, string> runMethodFromExtensionWithTreeHandler,
            Action<IBranch, AssemblyClassDefinition, string> runMethodFromExtensionHandler
            )
        {
            GlobalMenuItemsProvider = menuItemsProvider;
            RunFunctionHandler = runFunctionHandler;
            RunFunctionWithTreeHandler = runFunctionWithTreeHandler;
            RunMethodFromObjectHandler = runMethodFromObjectHandler;
            RunMethodFromExtensionWithTreeHandler = runMethodFromExtensionWithTreeHandler;
            RunMethodFromExtensionHandler = runMethodFromExtensionHandler;
        }
    }



}

using System;
using TheTechIdea.Beep.Services;
using TheTechIdea.Beep.Vis.Modules;

namespace TheTechIdea.Beep.Shared
{
    /// <summary>
    /// Wires add-in tree metadata, dynamic function calling, and <see cref="HandlersFactory"/> delegates.
    /// After <c>BeepDesktopServices.ConfigureServices</c>, WinForms applications should run
    /// <c>BeepWinformUiBootstrap.ConfigureBeepWinformAddInUi</c> (Integrated) to mirror handlers into Beep controls.
    /// </summary>
    public static class AddInCommandPipeline
    {
        public static void Apply(IBeepService beepService, IAppManager vis)
        {
            if (beepService == null)
                throw new ArgumentNullException(nameof(beepService));
            if (vis == null)
                throw new ArgumentNullException(nameof(vis));

            AssemblyClassDefinitionManager.TreeStructures = beepService.Config_editor.AddinTreeStructure;
            AssemblyClassDefinitionManager.BranchesClasses = beepService.Config_editor.BranchesClasses;
            AssemblyClassDefinitionManager.GlobalFunctions = beepService.Config_editor.GlobalFunctions;

            DynamicFunctionCallingManager.DMEEditor = beepService.DMEEditor;
            DynamicFunctionCallingManager.Vismanager = vis;
            DynamicFunctionCallingManager.TreeEditor = (ITree)vis.Tree;

            HandlersFactory.GlobalMenuItemsProvider = DynamicMenuManager.GetMenuItemsList;

            HandlersFactory.RunFunctionHandler = DynamicFunctionCallingManager.RunFunctionFromExtensions;

            HandlersFactory.RunFunctionWithTreeHandler = (item, method) =>
                DynamicFunctionCallingManager.RunFunctionFromExtensions(item, method);

            HandlersFactory.RunMethodFromObjectHandler = (branch, method) =>
                DynamicFunctionCallingManager.RunMethodFromObject(branch, method);

            HandlersFactory.RunMethodFromExtensionHandler = (branch, def, method) =>
                DynamicFunctionCallingManager.RunMethodFromExtension(branch, def, method);

            HandlersFactory.RunMethodFromExtensionWithTreeHandler = (branch, method) =>
                DynamicFunctionCallingManager.RunMethodFromExtension(branch, method);
        }
    }
}

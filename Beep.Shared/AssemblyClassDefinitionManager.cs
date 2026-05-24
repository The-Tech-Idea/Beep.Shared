using System;
using System.Collections.Generic;
using System.Linq;
using TheTechIdea.Beep.Addin;
using TheTechIdea.Beep.ConfigUtil;
using TheTechIdea.Beep.Editor;
using TheTechIdea.Beep.Utilities;
using TheTechIdea.Beep.Vis;


namespace TheTechIdea.Beep.Shared
{
    public static class AssemblyClassDefinitionManager
    {
        public static List<AddinTreeStructure> TreeStructures { get; set; } = new List<AddinTreeStructure>();
        public static List<AssemblyClassDefinition> BranchesClasses { get; set; } = new List<AssemblyClassDefinition>();
        public static List<AssemblyClassDefinition> GlobalFunctions { get; set; } = new List<AssemblyClassDefinition>();
        public static AssemblyClassDefinition GetAssemblyClassDefinitionByObjectTypeAndBranchClass(IDMEEditor DMEEditor, string ObjectType, string BranchClass)
        {
            return BranchesClasses.Where(x => x.classProperties.ObjectType == ObjectType && x.classProperties.ClassType == BranchClass).FirstOrDefault();
        }
        public static AssemblyClassDefinition GetAssemblyClassDefinitionByObjectTypeAndBranchClassAndDataSourceType(IDMEEditor DMEEditor, string ObjectType, string BranchClass, DatasourceCategory datasourceCategory)
        {
            return BranchesClasses.Where(x => x.classProperties.ObjectType == ObjectType && x.classProperties.ClassType == BranchClass && x.classProperties.Category == datasourceCategory).FirstOrDefault();
        }
        public static AssemblyClassDefinition GetAssemblyClassDefinitionByObjectTypeAndBranchClassAndPointType(IDMEEditor DMEEditor, string ObjectType, string BranchClass, EnumPointType PointType)
        {
            return BranchesClasses.Where(x => x.classProperties.ObjectType == ObjectType && x.classProperties.ClassType == BranchClass && x.VisSchema.BranchType == PointType).FirstOrDefault();
        }
        public static AssemblyClassDefinition GetAssemblyClassDefinitionByObjectTypeAndBranchClassAndPointTypeAndDataSourceType(IDMEEditor DMEEditor, string ObjectType, string BranchClass, EnumPointType PointType, DatasourceCategory datasourceCategory)
        {
            return BranchesClasses.Where(x => x.classProperties.ObjectType == ObjectType && x.classProperties.ClassType == BranchClass && x.VisSchema.BranchType == PointType && x.classProperties.Category == datasourceCategory).FirstOrDefault();
        }
        public static AssemblyClassDefinition GetAssemblyBranchsClassDefinitionByGuid(IDMEEditor DMEEditor, string Guid)
        {
            return BranchesClasses.Where(x => x.GuidID == Guid).FirstOrDefault();
        }
        public static AssemblyClassDefinition GetAssemblyGlobalFunctionsClassDefinitionByGuid(IDMEEditor DMEEditor, string Guid)
        {
            return BranchesClasses.Where(x => x.GuidID == Guid).FirstOrDefault();
        }


        public static AssemblyClassDefinition GetAssemblyClassDefinition(string PackageName)
        {
            return BranchesClasses.Where(x => x.PackageName == PackageName).FirstOrDefault();
        }
        public static AssemblyClassDefinition GetAssemblyClassDefinitionByClassName(string ClassName)
        {
            return BranchesClasses.Where(x => x.className == ClassName).FirstOrDefault();
        }
        public static AssemblyClassDefinition GetAssemblyClassDefinitionByObjectType(string ObjectType)
        {
            return BranchesClasses.Where(x => x.classProperties.ObjectType == ObjectType).FirstOrDefault();
        }
        public static AssemblyClassDefinition GetAssemblyClassDefinitionByObjectTypeAndBranchClass(string ObjectType, string BranchClass)
        {
            return BranchesClasses.Where(x => x.classProperties.ObjectType == ObjectType && x.classProperties.ClassType == BranchClass).FirstOrDefault();
        }
        public static AssemblyClassDefinition GetAssemblyClassDefinitionByObjectTypeAndBranchClassAndDataSourceType(string ObjectType, string BranchClass, DatasourceCategory datasourceCategory)
        {
            return BranchesClasses.Where(x => x.classProperties.ObjectType == ObjectType && x.classProperties.ClassType == BranchClass && x.classProperties.Category == datasourceCategory).FirstOrDefault();
        }
        public static AssemblyClassDefinition GetAssemblyClassDefinitionByObjectTypeAndBranchClassAndPointType(string ObjectType, string BranchClass, EnumPointType PointType)
        {
            return BranchesClasses.Where(x => x.classProperties.ObjectType == ObjectType && x.classProperties.ClassType == BranchClass && x.VisSchema.BranchType == PointType).FirstOrDefault();
        }
        public static AssemblyClassDefinition GetAssemblyClassDefinitionByObjectTypeAndBranchClassAndPointTypeAndDataSourceType(string ObjectType, string BranchClass, EnumPointType PointType, DatasourceCategory datasourceCategory)
        {
            return BranchesClasses.Where(x => x.classProperties.ObjectType == ObjectType && x.classProperties.ClassType == BranchClass && x.VisSchema.BranchType == PointType && x.classProperties.Category == datasourceCategory).FirstOrDefault();
        }
        public static AssemblyClassDefinition GetAssemblyClassDefinitionByGuid(string Guid)
        {
            AssemblyClassDefinition ret= BranchesClasses.Where(x => x.GuidID.Equals(Guid, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault(); 
            if (ret == null)
            {
                ret= GlobalFunctions.Where(x => x.GuidID.Equals(Guid, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
            }
            return ret;
        }
        public static List<AssemblyClassDefinition> GetAssemblyClassDefinitionForMenu(string ObjectType = "Beep")
        {
            return GlobalFunctions
                .Where(o => o.classProperties != null
                          
                            && o.classProperties.ObjectType != null
                            && (o.classProperties.Showin & ShowinType.Menu) == ShowinType.Menu
                            && o.classProperties.ObjectType.Equals(ObjectType, StringComparison.CurrentCultureIgnoreCase))
                .OrderBy(p => p.Order)
                .ToList();
        }
        public static List<AssemblyClassDefinition> GetAssemblyClassDefinitionForContextMenuNotDistinct(string ObjectType = "Beep")
        {
            return GlobalFunctions
                .Where(o => o.classProperties != null
                            && !o.classProperties.IsClassDistinct
                            && o.classProperties.ObjectType != null
                            && (o.classProperties.Showin & ShowinType.ContextMenu) == ShowinType.ContextMenu
                            && o.classProperties.ObjectType.Equals(ObjectType, StringComparison.CurrentCultureIgnoreCase))
                .OrderBy(p => p.Order)
                .ToList();
        }
        public static List<AssemblyClassDefinition> GetAssemblyClassDefinitionForContextMenuDistinct(string ObjectType = "Beep")
        {
            return GlobalFunctions
                .Where(o => o.classProperties != null
                            && o.classProperties.IsClassDistinct
                            && o.classProperties.ObjectType != null
                            && (o.classProperties.Showin & ShowinType.ContextMenu) == ShowinType.ContextMenu
                            && o.classProperties.ObjectType.Equals(ObjectType, StringComparison.CurrentCultureIgnoreCase))
                .OrderBy(p => p.Order)
                .ToList();
        }
        public static List<AssemblyClassDefinition> GetAssemblyClassDefinitionVerticalToolbar(string ObjectType = "Beep")
        {
            return GlobalFunctions
                .Where(o => o.classProperties != null
                            && o.componentType == "IFunctionExtension"
                            && o.classProperties.ObjectType != null
                            && (o.classProperties.Showin & ShowinType.HorZToolbar) == ShowinType.HorZToolbar
                            && o.classProperties.ObjectType.Equals(ObjectType, StringComparison.CurrentCultureIgnoreCase))
                .OrderBy(p => p.Order)
                .ToList();
           // return GlobalFunctions.Where(x => x.componentType == "IFunctionExtension" && x.classProperties != null && x.classProperties.ObjectType != null && (x.classProperties.Showin == ShowinType.HorZToolbar) && x.classProperties.ObjectType.Equals(ObjectType, StringComparison.InvariantCultureIgnoreCase)).OrderBy(p => p.Order).ToList();
        }
        public static List<AssemblyClassDefinition> GetAssemblyClassDefinitionToolbar(string ObjectType = "Beep")
        {
            return GlobalFunctions
                .Where(o => o.classProperties != null
                            && o.componentType == "IFunctionExtension"
                            && o.classProperties.ObjectType != null
                            && (o.classProperties.Showin & ShowinType.Toolbar) == ShowinType.Toolbar
                            && o.classProperties.ObjectType.Equals(ObjectType, StringComparison.CurrentCultureIgnoreCase))
                .OrderBy(p => p.Order)
                .ToList();
            //return GlobalFunctions.Where(x => x.componentType == "IFunctionExtension" && x.classProperties != null && x.classProperties.ObjectType != null && (x.classProperties.Showin == ShowinType.Toolbar || x.classProperties.Showin == ShowinType.Both) && x.classProperties.ObjectType.Equals(ObjectType, StringComparison.InvariantCultureIgnoreCase)).OrderBy(p => p.Order).ToList();
        }
        public static List<AddinTreeStructure> GetAssemblyClassDefinitionAddins(string objectType = "Beep")
        {
            return TreeStructures.Where(p=>p.ObjectType.Equals(objectType)).ToList();
        }
        
    }
}

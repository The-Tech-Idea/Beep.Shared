using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TheTechIdea.Beep.Addin;
using TheTechIdea.Beep.ConfigUtil;
using TheTechIdea.Beep.Editor;
using TheTechIdea.Beep.Vis.Modules;
using TheTechIdea.Beep.Winform.Controls.Models;


namespace TheTechIdea.Beep.Shared
{
    public static class DynamicFunctionCallingManager
    {
        public static IDMEEditor DMEEditor { get; set; }
        public static IAppManager Vismanager { get; set; }
        public static ITree TreeEditor { get; set; }
        public static void RunFunctionFromExtensions( SimpleItem item, string MethodName)
        {
            
            IBranch br = null;
            AssemblyClassDefinition assemblydef = new AssemblyClassDefinition();
            MethodInfo method = null;
            MethodsClass methodsClass;
            dynamic fc = null;
            assemblydef = AssemblyClassDefinitionManager.GetAssemblyClassDefinitionByGuid(item.AssemblyClassDefinitionID);
           
                 fc = DMEEditor.assemblyHandler.CreateInstanceFromString(assemblydef.dllname, assemblydef.type.ToString(), new object[] { Vismanager });
            //  dynamic fc = Editor.assemblyHandler.CreateInstanceFromString(assemblydef.type.ToString(), new object[] {  });
            if (fc == null)
            {
                return;
            }

            //Type t = ((IFunctionExtension)fc).GetType();
            //   AssemblyClassDefinition cls = tree.Editor.ConfigEditor.GlobalFunctions.Where(x => x.className == t.Name).FirstOrDefault();

            methodsClass = assemblydef.Methods.Where(x => x.Caption == MethodName).FirstOrDefault();

          
            method = methodsClass.Info;
            if (method.GetParameters().Length > 0)
            {
                method.Invoke(fc, new object[] { DMEEditor.Passedarguments });
            }
            else
                method.Invoke(fc, null);
        }
      
        public static bool IsMethodApplicabletoNode(AssemblyClassDefinition cls, IBranch br)
        {
            if (cls.classProperties == null)
            {
                return true;
            }
            if (cls.classProperties.ObjectType != null)
            {
                if (!cls.classProperties.ObjectType.Equals(br.ObjectType, StringComparison.InvariantCultureIgnoreCase))
                {
                    return false;
                }
            }
            return true;
        }
        public static IErrorsInfo RunMethodFromObject( object branch, string MethodName)
        {
            try
            {
                Type t = branch.GetType();
                AssemblyClassDefinition cls = AssemblyClassDefinitionManager.BranchesClasses.Where(x => x.className == t.Name).FirstOrDefault();
                MethodInfo method = null;
                MethodsClass methodsClass;
                try
                {
                    methodsClass = cls.Methods.Where(x => x.Caption == MethodName).FirstOrDefault();
                }
                catch (Exception)
                {
                    methodsClass = null;
                }
                if (methodsClass != null)
                {
                    if (!IsMethodApplicabletoNode(cls, (IBranch)branch)) return DMEEditor.ErrorObject;
                    //PassedArgs args = new PassedArgs();
                    //args.ObjectName = MethodName;
                    //args.ObjectType = methodsClass.ObjectType;
                    //args.Cancel = false;
                    //PreCallModule?.Invoke(this, args);
                    //if (args.Cancel)
                    //{
                    //    //MiscFunctions.AddLogMessage("Beep", $"You dont have Access Privilige on {MethodName}", DateTime.Now, 0, MethodName, Errors.Failed);
                    //    ErrorsandMesseges.Flag = Errors.Failed;
                    //    ErrorsandMesseges.Message = $"Function Access Denied";
                    //    return ErrorsandMesseges;
                    //}

                    method = methodsClass.Info;
                    if (method.GetParameters().Length > 0)
                    {
                        method.Invoke(branch, new object[] { DMEEditor.Passedarguments.Objects[0].obj });
                    }
                    else
                        method.Invoke(branch, null);


                    //  //MiscFunctions.AddLogMessage("Success", "Running method", DateTime.Now, 0, null, Errors.Ok);
                }

            }
            catch (Exception ex)
            {
                string mes = "Could not Run Method " + MethodName;
                //MiscFunctions.AddLogMessage(ex.Message, mes, DateTime.Now, -1, mes, Errors.Failed);
            };
            return DMEEditor.ErrorObject;
        }
        public static void RunMethodFromExtension(IBranch br, AssemblyClassDefinition assemblydef, string MethodName)
        {
            if (br != null)
            {
                DMEEditor.Passedarguments.ObjectName = br.BranchText;
                DMEEditor.Passedarguments.DatasourceName = br.DataSourceName;
                DMEEditor.Passedarguments.Id = br.BranchID;
                DMEEditor.Passedarguments.ParameterInt1 = br.BranchID;

                IFunctionExtension fc = (IFunctionExtension)DMEEditor.assemblyHandler.CreateInstanceFromString(assemblydef.dllname, assemblydef.type.ToString(), new object[] { DMEEditor, br });
                Type t = fc.GetType();
                //dynamic fc = Activator.CreateInstance(assemblydef.type, new object[] { Editor, Vismanager, this });
                AssemblyClassDefinition cls =AssemblyClassDefinitionManager.GlobalFunctions.Where(x => x.className == t.Name).FirstOrDefault();
                MethodInfo method = null;
                MethodsClass methodsClass;
                if (!IsMethodApplicabletoNode(cls, br)) return;
                try
                {
                    if (br.BranchType != Vis.EnumPointType.Global)
                    {
                        methodsClass = cls.Methods.Where(x => x.Caption == MethodName).FirstOrDefault();
                    }
                    else
                    {
                        methodsClass = cls.Methods.Where(x => x.Caption == MethodName && x.PointType == br.BranchType).FirstOrDefault();
                    }

                }
                catch (Exception)
                {

                    methodsClass = null;
                }
                if (methodsClass != null)
                {
                    method = methodsClass.Info;
                    if (method.GetParameters().Length > 0 && DMEEditor.Passedarguments.Objects.Count > 0)
                    {
                        method.Invoke(fc, new object[] { DMEEditor.Passedarguments });
                    }
                    else
                        method.Invoke(fc, null);
                }
            }


        }
        public static void RunMethodFromExtension( IBranch br, string MethodName)
        {
           
            AssemblyClassDefinition assemblydef = new AssemblyClassDefinition();
            MethodInfo method = null;
            MethodsClass methodsClass;
            assemblydef = AssemblyClassDefinitionManager.GetAssemblyClassDefinitionByGuid(br.MiscStringID);
            RunMethodFromExtension(br, assemblydef, MethodName);    
        }


    }
}

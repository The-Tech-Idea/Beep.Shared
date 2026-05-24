
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using TheTechIdea.Beep.Addin;
using TheTechIdea.Beep.ConfigUtil;
using TheTechIdea.Beep.Editor;
using TheTechIdea.Beep.Vis.Modules;
using TheTechIdea.Beep.Winform.Controls.Models;


namespace TheTechIdea.Beep.Shared
{
    public static class DynamicMenuManager
    {
        public static List<SimpleMenuList> Menus = new List<SimpleMenuList>();
        public static bool IsMenuCreated(IBranch br)
        {
            if (br.ObjectType != null)
            {
                return Menus.Any(p => p.ObjectType != null &&
                                            p.BranchClass.Equals(br.BranchClass, StringComparison.InvariantCultureIgnoreCase) &&
                                            p.ObjectType.Equals(br.ObjectType, StringComparison.InvariantCultureIgnoreCase) &&
                                            p.PointType == br.BranchType);
            }
            return false;
        }
        public static void UpdateMenuItems(SimpleMenuList menuCollection, SimpleItem simpleItem)
        {
            int menuIdx = menuCollection.Items.ToList().FindIndex(p => p.ObjectType != null &&
                                                                       p.BranchClass.Equals(simpleItem.BranchClass, StringComparison.InvariantCultureIgnoreCase) &&
                                                                       p.ObjectType.Equals(simpleItem.ObjectType, StringComparison.InvariantCultureIgnoreCase) &&
                                                                       p.PointType == simpleItem.PointType);
            if (menuIdx > -1)
            {
                int menuItemIdx = menuCollection.Items[menuIdx].Children.ToList()
                    .FindIndex(p => p.Name.Equals(simpleItem.Name, StringComparison.InvariantCultureIgnoreCase));
                if (menuItemIdx > -1)
                {
                    menuCollection.Items[menuIdx].Children[menuItemIdx] = simpleItem;
                }
            }
        }
        public static void UpdateMenuList(SimpleMenuList menuCollection, SimpleItem updatedMenu)
        {
            int menuIdx = menuCollection.Items.ToList().FindIndex(p => p.ObjectType != null &&
                                                                       p.BranchClass.Equals(updatedMenu.BranchClass, StringComparison.InvariantCultureIgnoreCase) &&
                                                                       p.ObjectType.Equals(updatedMenu.ObjectType, StringComparison.InvariantCultureIgnoreCase) &&
                                                                       p.PointType == updatedMenu.PointType);

            if (menuIdx > -1)
            {
                menuCollection.Items[menuIdx] = updatedMenu;
            }
        }
     
        public static SimpleMenuList GetDistinctMenu(IBranch br)
        {
            return Menus.Where(p => p.ObjectType != null && p.BranchClass.Equals(br.BranchClass, StringComparison.InvariantCultureIgnoreCase)
              && p.ObjectType.Equals(br.ObjectType, StringComparison.InvariantCultureIgnoreCase)
              && p.PointType == br.BranchType && p.IsClassDistinct).FirstOrDefault();
        }
        public static SimpleMenuList GetMenuList(IBranch br)
        {
            return Menus.Where(p => p.ObjectType != null && p.BranchClass.Equals(br.BranchClass, StringComparison.InvariantCultureIgnoreCase)
              && p.ObjectType.Equals(br.ObjectType, StringComparison.InvariantCultureIgnoreCase)
              && p.PointType == br.BranchType && !p.IsClassDistinct).FirstOrDefault();
        }
        public static SimpleMenuList GetMenuList(SimpleItem simpleItem)
        {
            return Menus.Where(p => p.ObjectType != null && p.BranchClass.Equals(simpleItem.BranchClass, StringComparison.InvariantCultureIgnoreCase)
              && p.ObjectType.Equals(simpleItem.ObjectType, StringComparison.InvariantCultureIgnoreCase)
              && p.PointType == simpleItem.PointType).FirstOrDefault();
        }
        public static List<SimpleItem> GetMenuItemsList(IBranch br)
        {
            var menu = Menus.FirstOrDefault(p => p.ObjectType != null &&
                                                       p.BranchClass.Equals(br.BranchClass, StringComparison.InvariantCultureIgnoreCase) &&
                                                       p.ObjectType.Equals(br.ObjectType, StringComparison.InvariantCultureIgnoreCase) &&
                                                       p.PointType == br.BranchType);

            return menu.Items.ToList();
        }
        public static List<SimpleItem> GetMenuItemsList(SimpleItem simpleItem)
        {
            var menu = Menus.FirstOrDefault(p => p.ObjectType != null &&
                                                       p.BranchClass.Equals(simpleItem.BranchClass, StringComparison.InvariantCultureIgnoreCase) &&
                                                       p.ObjectType.Equals(simpleItem.ObjectType, StringComparison.InvariantCultureIgnoreCase) &&
                                                       p.PointType == simpleItem.PointType );
           
            if (menu == null) return null;
            return menu.Items.ToList();
        }
        public static List<SimpleItem> CreateMenuMethods(IDMEEditor DMEEditor, IBranch branch)
        {
            // Validate inputs
            if (branch == null)
                throw new ArgumentNullException(nameof(branch));

            // Retrieve the class definition for the given branch
            AssemblyClassDefinition cls = DMEEditor.ConfigEditor.BranchesClasses
                .FirstOrDefault(x => x.PackageName == branch.ToString());

            if (cls == null)
            {
                string message = $"No class definition found for branch {branch.BranchText}";
                //MiscFunctions.AddLogMessage("Error", message, DateTime.Now, -1, message, Errors.Failed);
                return new List<SimpleItem>();
            }

            // Initialize or retrieve the menu collection for the branch
            SimpleMenuList menuCollection = Menus
                .FirstOrDefault(mc => mc.ObjectType == branch.ObjectType &&
                                      mc.BranchClass.Equals(branch.BranchClass, StringComparison.InvariantCultureIgnoreCase) &&
                                      mc.PointType == branch.BranchType);

            if (menuCollection == null)
            {
                // Create a new menu collection if one doesn't exist
                menuCollection = new SimpleMenuList
                {
                    ObjectType = branch.ObjectType,
                    BranchClass = branch.BranchClass,
                    PointType = branch.BranchType,
                    BranchName = branch.BranchText,
                    MenuName = branch.MenuID,
                };

                Menus.Add(menuCollection);
            }

            try
            {
                // Add methods to the menu if they are not already present
                if (!menuCollection.Items.Any(p => p.PackageName.Equals(cls.PackageName, StringComparison.InvariantCultureIgnoreCase)))
                {
                    foreach (var item in cls.Methods.Where(y => !y.Hidden))
                    {
                        // Create a new menu item for each method
                        SimpleItem simpleItem = new SimpleItem
                        {
                            Name = item.Caption,
                            MethodName = item.Caption,
                            Text = item.Caption,
                            ObjectType = item.ObjectType,
                            BranchClass = item.ClassType,
                            PointType = item.PointType,
                            Category = item.Category,
                            BranchName = item.Name,
                            PackageName = cls.PackageName,
                            AssemblyClassDefinitionID= cls.GuidID,
                            ImagePath = ImageListHelper.GetImagePathFromName(item.iconimage)
                        };

                        // Add the new item to the menu
                        menuCollection.Items.Add(simpleItem);
                    }
                }
            }
            catch (Exception ex)
            {
                string message = $"Could not add methods to menu {branch.BranchText}";
                //MiscFunctions.AddLogMessage(ex.Message, message, DateTime.Now, -1, message, Errors.Failed);
            }

            return menuCollection.Items.ToList();
        }
        public static IErrorsInfo CreateGlobalMenu( IBranch br)
        {
            ErrorsInfo errors = new ErrorsInfo();
            errors.Flag = Errors.Ok;
            try
            {
                var menuCollection = GetMenuList(br);

                if (menuCollection == null)
                {
                    menuCollection = new SimpleMenuList
                    {
                        ObjectType = br.ObjectType,
                        BranchClass = br.BranchClass,
                        PointType = br.BranchType,
                        BranchName = br.BranchText,
                        ClassGuidID = br.GuidID,
                        MenuName = br.MenuID,
                        MenuID = br.MenuID,
                        BranchID = br.GuidID,
                        ClassName = br.Name,
                        IsClassDistinct = false


                    };
                    Menus.Add(menuCollection);
                }

                var extensions = AssemblyClassDefinitionManager.GetAssemblyClassDefinitionForContextMenuNotDistinct();

                foreach (var cls in extensions)
                {
                    if (!menuCollection.Items.Any(p => p.PackageName.Equals(cls.PackageName, StringComparison.InvariantCultureIgnoreCase)))
                    {
                        foreach (var item in cls.Methods.Where(o=>o.PointType == br.BranchType  ))
                        {
//if (string.IsNullOrEmpty(item.ClassType))
                         //   {
                                if (item.PointType == br.BranchType)
                                {
                                    menuCollection.Items.Add(new SimpleItem
                                    {
                                        Name = item.Caption,
                                        MethodName = item.Name,
                                        Text = item.Caption,
                                        ObjectType = item.ObjectType,
                                        BranchClass = br.BranchClass,
                                        PointType = item.PointType,
                                        Value = item.CommandAttr,
                                        BranchName = item.Name,
                                        PackageName = cls.PackageName,
                                        AssemblyClassDefinitionID = cls.GuidID,
                                        ImagePath = ImageListHelper.GetImagePathFromName(item.iconimage)

                                    });
                                }
                        
                        }
                    }
                }
                CreateDistinctMenu(br);
                return errors;
            }
            catch (Exception ex)
            {
                errors.Ex = ex;
                errors.Flag = Errors.Failed;
                errors.Message = ex.Message;
                //MiscFunctions.AddLogMessage(ex.Message, "Error creating global menu", DateTime.Now, -1, "CreateGlobalMenu", Errors.Failed);
                return errors;
            }
        }
        public static IErrorsInfo CreateDistinctMenu(IBranch br)
        {
            ErrorsInfo errors = new ErrorsInfo();
            errors.Flag = Errors.Ok;
            try
            {
                var menuCollection = GetMenuList(br);

                if (menuCollection == null)
                {
                    menuCollection = new SimpleMenuList
                    {
                        ObjectType = br.ObjectType,
                        BranchClass = br.BranchClass,
                        PointType = br.BranchType,
                        BranchName = br.BranchText,
                        ClassGuidID = br.GuidID,
                        MenuName = br.MenuID,
                        MenuID = br.MenuID,
                        BranchID = br.GuidID,
                        ClassName = br.Name,
                        IsClassDistinct = true


                    };
                    Menus.Add(menuCollection);
                }

                var extensions = AssemblyClassDefinitionManager.GetAssemblyClassDefinitionForContextMenuDistinct();

                foreach (var cls in extensions)
                {
                    if (!menuCollection.Items.Any(p => p.PackageName.Equals(cls.PackageName, StringComparison.InvariantCultureIgnoreCase)))
                    {
                        foreach (var item in cls.Methods.Where(o => o.PointType == br.BranchType &&   o.ClassType!=null && o.ClassType.Equals(br.BranchClass)))
                        {
                                 if (item.PointType == br.BranchType)
                                {
                                    menuCollection.Items.Add(new SimpleItem
                                    {
                                        Name = item.Caption,
                                        MethodName = item.Name,
                                        Text = item.Caption,
                                        ObjectType = item.ObjectType,
                                        BranchClass = item.ClassType,
                                        PointType = item.PointType,
                                        Value = item.CommandAttr,
                                        BranchName = br.Name,
                                        IsClassDistinct=cls.classProperties.IsClassDistinct,
                                        PackageName = cls.PackageName,
                                        AssemblyClassDefinitionID = cls.GuidID,
                                        ImagePath = ImageListHelper.GetImagePathFromName(item.iconimage)

                                    });
                               
                            }
                            //else
                            //{
                            //    if (item.PointType == br.BranchType && br.BranchClass.Equals(item.ClassType, StringComparison.InvariantCultureIgnoreCase))
                            //    {
                            //        menuCollection.Items.Add(new SimpleItem
                            //        {
                            //            Name = item.Caption,
                            //            MethodName = item.Name,
                            //            Text = item.Caption,
                            //            ObjectType = item.ObjectType,
                            //            BranchClass = item.ClassType,
                            //            PointType = item.PointType,
                            //            Value = item.CommandAttr,
                            //            BranchName = item.Name,
                            //            PackageName = cls.PackageName,
                            //            AssemblyClassDefinitionID = cls.GuidID,
                            //            ImagePath = ImageListHelper.GetImagePathFromName(item.iconimage)

                            //        });
                            //    }
                            //}
                        }
                    }
                }

                return errors;
            }
            catch (Exception ex)
            {
                errors.Ex = ex;
                errors.Flag = Errors.Failed;
                errors.Message = ex.Message;
                //MiscFunctions.AddLogMessage(ex.Message, "Error creating global menu", DateTime.Now, -1, "CreateGlobalMenu", Errors.Failed);
                return errors;
            }
        }
        public static SimpleItem GetMenuByBranchGuidId(string branchGuidId)
        {
            if (string.IsNullOrEmpty(branchGuidId))
                throw new ArgumentException("Branch Guid ID cannot be null or empty.", nameof(branchGuidId));

            // Search for a matching menu item in the collections
            foreach (var collection in Menus)
            {
                var menuItem = collection.Items.FirstOrDefault(item => item.GuidId == branchGuidId);
                if (menuItem != null)
                    return menuItem;
            }
            return null;
        }
        public static IErrorsInfo CreateFunctionExtensions(ITree tree, MethodsClass item)
        {
            try
            {
                List<AssemblyClassDefinition> extentions = tree.DMEEditor.ConfigEditor.GlobalFunctions.Where(o => o.classProperties != null && o.classProperties.ObjectType != null).OrderBy(p => p.Order).ToList(); //&&  o.classProperties.menu.Equals(br.BranchClass, StringComparison.InvariantCultureIgnoreCase)
                AssemblyClassDefinition cls = extentions.FirstOrDefault(x => x.Methods.Any(y => y.GuidID == item.GuidID));

                // Ensure cls is not null before using it to avoid NullReferenceException.
                if (cls == null)
                {
                    return tree.DMEEditor.ErrorObject;
                }

                foreach (IBranch br in tree.Branches)
                {
                    SimpleMenuList menuList = new SimpleMenuList();
                    if (!IsMenuCreated(br))
                    {
                        menuList = new SimpleMenuList(br.ObjectType, br.BranchClass, br.BranchType);
                        menuList.BranchName = br.BranchText;
                        Menus.Add(menuList);
                        menuList.ObjectType = br.ObjectType;
                        menuList.BranchClass = br.BranchClass;
                    }
                    else
                        menuList = GetMenuList(br);

                    if (string.IsNullOrEmpty(item.ClassType))
                    {
                        if (item.PointType == br.BranchType)
                        {
                            SimpleItem mi = new SimpleItem();
                            mi.Name = item.Caption;
                            mi.MethodName = item.Caption;
                            mi.Text = item.Caption;
                            mi.ObjectType = item.ObjectType;
                            mi.BranchClass = item.ClassType;
                            mi.PointType = item.PointType;
                            mi.ClassDefinitionID = cls.classProperties.GuidID;
                            mi.ImagePath = ImageListHelper.GetImagePathFromName(item.iconimage);
                            menuList.Items.Add(mi);
                        }
                    }
                    else
                    {
                        if ((item.PointType == br.BranchType) && (br.BranchClass.Equals(item.ClassType, StringComparison.InvariantCultureIgnoreCase)))
                        {
                            SimpleItem mi = new SimpleItem();
                            mi.Name = item.Caption;
                            mi.MethodName = item.Name;
                            mi.Text = item.Caption;
                            mi.ObjectType = item.ObjectType;
                            mi.BranchClass = item.ClassType;
                            mi.PointType = item.PointType;
                            mi.ClassDefinitionID = cls.classProperties.GuidID;
                            mi.ImagePath = ImageListHelper.GetImagePathFromName(item.iconimage);
                            menuList.Items.Add(mi);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                string mes = $"Could not add method from Extension {item.Name} to menu ";
                //MiscFunctions.AddLogMessage(ex.Message, mes, DateTime.Now, -1, mes, Errors.Failed);
            };
            return tree.DMEEditor.ErrorObject;

        }
        public static List<SimpleItem> CreateToolBarMenuItems(string ObjectType = "Beep", bool IsHorizantal = true)
        {
            List<SimpleItem> ret = new List<SimpleItem>();
           
            try
            {
                var extensions = AssemblyClassDefinitionManager.GetAssemblyClassDefinitionToolbar(ObjectType);
                if (!IsHorizantal)
                {
                    extensions = AssemblyClassDefinitionManager.GetAssemblyClassDefinitionVerticalToolbar(ObjectType);
                }
                foreach (var item in extensions)
                {
                    for (int i = 0; i < item.Methods.Count; i++)
                    {
                        SimpleItem mi = new SimpleItem();
                        mi.Name = item.Methods[i].Caption;
                        mi.Text = item.Methods[i].Caption;
                        mi.MethodName = item.Methods[i].Name;
                        mi.ObjectType = item.Methods[i].ObjectType;
                        mi.BranchClass = item.Methods[i].ClassType;
                        mi.PointType = item.Methods[i].PointType;
                        mi.Value = item.Methods[i].CommandAttr;
                        mi.BranchName = item.Methods[i].Name;
                        mi.PackageName = item.PackageName;
                        mi.ImagePath = ImageListHelper.GetImagePathFromName(item.Methods[i].iconimage);
                        ret.Add(mi);
                    }
                   
                }


            }
            catch (Exception ex)
            {
                //MiscFunctions.AddLogMessage(ex.Message, "Error creating toolbar menu", DateTime.Now, -1, "CreateToolBarMethods", Errors.Failed);
            }
            return ret;
        }
        public static List<SimpleItem> CreateConfigAdminMenuItem( string ObjectType = "Beep")
        {
            List < SimpleItem > ret = new List<SimpleItem>();
            try
            {
                foreach (AddinTreeStructure item in AssemblyClassDefinitionManager.GetAssemblyClassDefinitionAddins())
                {
                    SimpleItem mi = new SimpleItem();
                    mi.Name = item.NodeName;
                    mi.Text = item.NodeName;
                    mi.MethodName = item.className;
                    mi.AssemblyClassDefinitionID = item.GuidID;
                    mi.ObjectType = item.ObjectType;
                    mi.PackageName = item.PackageName;
                    mi.MenuID = item.menu;
                    mi.ImagePath =ImageListHelper.GetImagePathFromName(item.Imagename);
                    ret.Add(mi);
                }
            }
            catch (Exception ex)
            {
                //MiscFunctions.AddLogMessage(ex.Message, "Error creating addins", DateTime.Now, -1, "CreateConfigAdminMenu", Errors.Failed);
            }
            return ret;
        }
        public static List<SimpleItem> CreateMenuItems(IDMEEditor DMEEditor, string ObjectType = "Beep")
        {
            List<SimpleItem> ret = new List<SimpleItem>();
            
            try
            {
                var extensions = AssemblyClassDefinitionManager.GetAssemblyClassDefinitionForMenu(ObjectType);
                foreach (var item in extensions)
                {
                    SimpleItem main = new SimpleItem();
                    main.Name = item.className;
                    main.Text = item.classProperties.Caption;
                   main.PackageName = item.PackageName;
                    main.ObjectType = item.classProperties.ObjectType;
                    main.BranchClass = item.classProperties.ClassType;
                   main.AssemblyClassDefinitionID = item.GuidID;
                    main.MenuName = item.classProperties.menu;
                    main.MenuID = item.classProperties.menu;
                    main.ImagePath = ImageListHelper.GetImagePathFromName(item.classProperties.iconimage);
                    for (int i = 0; i < item.Methods.Count; i++)
                    {
                        SimpleItem mi = new SimpleItem();
                        mi.Name = item.Methods[i].Caption;
                        mi.Text = item.Methods[i].Caption;
                        mi.MethodName = item.Methods[i].Name;
                        mi.ObjectType = item.Methods[i].ObjectType;
                        mi.BranchClass = item.Methods[i].ClassType;
                        mi.PointType = item.Methods[i].PointType;
                        mi.Value = item.Methods[i].CommandAttr;
                        mi.BranchName = item.Methods[i].Name;
                        mi.PackageName = item.PackageName;
                        mi.MenuName = item.classProperties.menu;
                        mi.MenuID = item.classProperties.menu;
                        mi.ImagePath = ImageListHelper.GetImagePathFromName(item.Methods[i].iconimage);
                        main.Children.Add(mi);
                    }
                    ret.Add(main);
                }
            }
            catch (Exception ex)
            {
                //MiscFunctions.AddLogMessage(ex.Message, "Error creating menu items", DateTime.Now, -1, "CreateMenuItems", Errors.Failed);
            }
            return ret;
        }
        public static List<SimpleItem> CreateCombinedMenuItems( string ObjectType = "Beep")
        {
            List<SimpleItem> ret = new List<SimpleItem>();
           
            try
            {
                var extensions = AssemblyClassDefinitionManager.GetAssemblyClassDefinitionForMenu(ObjectType);
                foreach (var item in extensions)
                {
                    SimpleItem main = new SimpleItem();
                    main.Name = item.className;
                    main.Text = item.classProperties.Caption;
                    main.PackageName = item.PackageName;
                    main.ObjectType = item.classProperties.ObjectType;
                    main.BranchClass = item.classProperties.ClassType;
                    main.AssemblyClassDefinitionID = item.GuidID;
                    main.MenuName = item.classProperties.menu;
                    main.MenuID = item.classProperties.menu;
                    main.IsClassDistinct = item.classProperties.IsClassDistinct;
                    main.GuidId = item.GuidID;
                    main.ClassName = item.className;
                    main.ImagePath = ImageListHelper.GetImagePathFromName(item.classProperties.iconimage);
                    for (int i = 0; i < item.Methods.Count; i++)
                    {
                        SimpleItem mi = new SimpleItem();
                        mi.Name = item.Methods[i].Caption;
                        mi.Text = item.Methods[i].Caption;
                        mi.MethodName = item.Methods[i].Name;
                        mi.ObjectType = item.Methods[i].ObjectType;
                        mi.BranchClass = item.Methods[i].ClassType;
                        mi.PointType = item.Methods[i].PointType;
                        mi.Value = item.Methods[i].CommandAttr;
                        mi.BranchName = item.Methods[i].Name;
                        mi.PackageName = item.PackageName;
                        mi.MenuName = item.classProperties.menu;
                        mi.MenuID = item.classProperties.menu;
                        mi.AssemblyClassDefinitionID = item.GuidID;
                        
                        mi.ImagePath = ImageListHelper.GetImagePathFromName(item.Methods[i].iconimage);
                        main.Children.Add(mi);
                    }
                    ret.Add(main);
                }
                BindingList<SimpleItem> preparedItems = new BindingList<SimpleItem>();
                // Group items by MenuName.
                var groups = ret
                    .Where(item => item != null)
                    .GroupBy(item => item.MenuName);
                foreach (var group in groups)
                {
                    SimpleItem groupItem = new SimpleItem();
                    groupItem.Name = group.Key;
                    groupItem.Text = group.Key;
                    groupItem.MenuName = group.Key;
                    groupItem.MenuID = group.Key;
                    groupItem.ImagePath = ImageListHelper.GetImagePathFromName($"{group.Key}.svg");

                    var det = group.ToList();
                    if (det.Count == 1)
                    {
                        det = det[0].Children.ToList();
                    }
                    

                        groupItem.Children = new BindingList<SimpleItem>(det);
                    preparedItems.Add(groupItem);
                }
                ret = preparedItems.ToList();
            }
            catch (Exception ex)
            {
                //MiscFunctions.AddLogMessage(ex.Message, "Error creating menu items", DateTime.Now, -1, "CreateMenuItems", Errors.Failed);
            }
            return ret;
        }
    }
}
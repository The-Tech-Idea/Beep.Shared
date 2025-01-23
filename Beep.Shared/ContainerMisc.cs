using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using System.Text;
using TheTechIdea.Beep.Container.Services;
using TheTechIdea.Beep.DriversConfigurations;
using TheTechIdea.Beep.Helpers;

namespace TheTechIdea.Beep.Shared
{
    public static class ContainerMisc
    {
        private static IBeepService BeepService;
        private static IServiceCollection Services;

        public static string BeepDataPath { get; private set; }
        public static string ContainerDataPath { get; private set; }
        private static bool mappingcreated = false;
        private static bool connectioncreated = false;
        private static bool datasourcecreated = false;

        #region "Container Methods"

        #endregion
        public static IServiceCollection CreateBeepMapping (this IBeepService beepService)
        {
            if (beepService != null)
            {
                if (beepService != null)
                {
                    BeepService = beepService;
                }
                AddAllConnectionConfigurations(beepService);
                AddAllDataSourceMappings(beepService);
                AddAllDataSourceQueryConfigurations(beepService);
            }
            return Services;
        }
        public static string CreateMainFolder (this IBeepService beepService)
        {
                if (!Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "TheTechIdea", "Beep")))
                {
                    Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "TheTechIdea", "Beep"));

                }
                BeepDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "TheTechIdea", "Beep");
          
            return BeepDataPath;
        }
        public static string CreateMainFolder()
        {
            if (!Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "TheTechIdea", "Beep")))
            {
                Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "TheTechIdea", "Beep"));

            }
            BeepDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "TheTechIdea", "Beep");

            return BeepDataPath;
        }
        public static string CreateContainerfolder(string containername="")
        {
            CreateMainFolder();
            if (!string.IsNullOrEmpty(containername))
            {
                if (!Directory.Exists(Path.Combine(BeepDataPath, containername)))
                {
                    Directory.CreateDirectory(Path.Combine(BeepDataPath, containername));
                    ContainerDataPath= Path.Combine(BeepDataPath, containername);
                }
            }
            return ContainerDataPath;
        }
        public static string CreateAppfolder(string containername ,string appfolder)
        {
            string appfolderpath = "";
          
        
                if (!string.IsNullOrEmpty(containername))
                {
                    CreateContainerfolder(containername);
                    if (!string.IsNullOrEmpty(appfolder))
                    {
                        if (!Directory.Exists(Path.Combine(ContainerDataPath, appfolder)))
                        {
                            Directory.CreateDirectory(Path.Combine(ContainerDataPath, appfolder));
                            appfolderpath = Path.Combine(ContainerDataPath, appfolder);
                        }
                    }
                    
                }
         
            return appfolderpath;
        }
        public static string CreateAppfolder( string appfolder)
        {
            string appfolderpath = "";


                CreateMainFolder();
                if (!string.IsNullOrEmpty(appfolder))
                {
                    if (!Directory.Exists(Path.Combine(BeepDataPath, appfolder)))
                    {
                        try
                        {
                            Directory.CreateDirectory(Path.Combine(BeepDataPath, appfolder));
                        }
                        catch (Exception ex)
                        {
                        return string.Empty;

                         }

                    }
                appfolderpath = Path.Combine(BeepDataPath, appfolder);
            }

        

            return appfolderpath;
        }
        public static void AddAllDataSourceQueryConfigurations (this IBeepService beepService)
        {
            if(datasourcecreated) return;
            beepService.DMEEditor.ConfigEditor.QueryList.AddRange(RDBMSHelper.CreateQuerySqlRepos());
        }
        public static void AddAllConnectionConfigurations (this IBeepService beepService)
        {
            if (connectioncreated) return;
            if (beepService.DMEEditor.ConfigEditor.DataDriversClasses == null)
            {
                beepService.DMEEditor.ConfigEditor.DataDriversClasses = new List<ConnectionDriversConfig>();
            }
            beepService.DMEEditor.ConfigEditor.DataDriversClasses.AddRange(ConnectionHelper.GetAllConnectionConfigs());
        }
        public static void AddAllDataSourceMappings (this IBeepService beepService)
        {
            if(mappingcreated) return;
            beepService.DMEEditor.ConfigEditor.DataTypesMap.AddRange(DataTypeFieldMappingHelper.GetMappings());
        }
      

    }
}

using TheTechIdea.Beep.Vis.Modules;
using TheTechIdea.Beep.Vis;

using TheTechIdea.Beep.Editor;
using TheTechIdea.Beep.Addin;
using System.Threading;
using System;
using System.Collections.Generic;
using System.Linq;




namespace TheTechIdea.Beep.Shared
{
    public class FunctionandExtensionsHelpers : IFunctionandExtensionsHelpers
    {
        public IDMEEditor DMEEditor { get; set; }
        public IPassedArgs Passedargs { get; set; }
        public IAppManager Vismanager { get; set; }
        public IDialogManager DialogManager { get; set; }
        public IDM_Addin Crudmanager { get; set; }
        public IDM_Addin Menucontrol { get; set; }
        public IDM_Addin Toolbarcontrol { get; set; }
        public ITree TreeEditor { get; set; }
        public IProgress<PassedArgs> progress { get; set; }
        public CancellationToken token { get; set; }

        public IDataSource DataSource { get; set; }
        public IBranch CurrentBranch { get; set; }
        public IBranch RootBranch { get; set; }
        public IBranch ParentBranch { get; set; }
        public IBranch ViewRootBranch { get; set; }
        public IBranch NOSQLRootBranch { get; set; }
        public IBranch RDBMSRootBranch { get; set; }
        public IBranch AIRootBranch { get; set; }
        public IBranch CloudRootBranch { get; set; }
        public IBranch ConfigRootBranch { get; set; }
        public IBranch DevRootBranch { get; set; }
        public IBranch DDLRootBranch { get; set; }
        public IBranch ETLRootBranch { get; set; }
        public IBranch ReportRootBranch { get; set; }
        public IBranch ScriptRootBranch { get; set; }
        public IBranch FileRootBranch { get; set; }
        public IBranch MappingRootBranch { get; set; }
        public IBranch WorkFlowRootBranch { get; set; }
        public IBranch ProjectRootBranch { get; set; }
        public IBranch LibraryRootBranch { get; set; }
        public IBranch WebAPIRootBranch { get; set; }

        public List<IBranch> RootBranchs { get; set; } = new List<IBranch>();

        public FunctionandExtensionsHelpers(IDMEEditor pdMEEditor, IAppManager pvisManager, ITree ptreeControl)
        {
            DMEEditor = pdMEEditor;
            Vismanager = pvisManager;
            TreeEditor = ptreeControl;
        }

        public event EventHandler<IPassedArgs> PreCallModule;
        public event EventHandler<IPassedArgs> PreShowItem;

        public void GetValues(IPassedArgs Passedarguments)

        {
            CurrentBranch = TreeEditor.CurrentBranch;
           
            if (CurrentBranch != null)
            {
                ParentBranch = CurrentBranch.ParentBranch;

                if (CurrentBranch.BranchType != EnumPointType.Root)
                {
                    int idx = TreeEditor.Branches.FindIndex(x => x.BranchClass == CurrentBranch.BranchClass && x.BranchType == EnumPointType.Root);
                    if (idx > 0)
                    {
                        RootBranch = TreeEditor.Branches[idx];

                    }

                }
                else
                {
                    RootBranch = CurrentBranch;
                }
                IDataSource ds = DMEEditor.GetDataSource(CurrentBranch.BranchText);
                if (ds != null)
                {
                    DataSource = ds;
                    
                }
            }

            if (!string.IsNullOrEmpty(Passedarguments.DatasourceName))
            {
                DataSource = DMEEditor.GetDataSource(Passedarguments.DatasourceName);
                //  Editor.OpenDataSource(Passedarguments.DatasourceName);
            }
            if (progress == null)
            {
                progress = DMEEditor.progress;
            }

            if (TreeEditor.Branches.Count > 0)
            {


                ViewRootBranch = TreeEditor.GetBranch("DataView", EnumPointType.Root);
                if (ViewRootBranch != null) RootBranchs.Add(ViewRootBranch);

                NOSQLRootBranch = TreeEditor.GetBranch("NoSQL", EnumPointType.Root);
                if (NOSQLRootBranch != null) RootBranchs.Add(NOSQLRootBranch);

                RDBMSRootBranch = TreeEditor.GetBranch("RDBMS", EnumPointType.Root);
                if (RDBMSRootBranch != null) RootBranchs.Add(RDBMSRootBranch);

                AIRootBranch = TreeEditor.GetBranch("AI", EnumPointType.Root);
                if (AIRootBranch != null) RootBranchs.Add(AIRootBranch);

                CloudRootBranch = TreeEditor.GetBranch("CLOUD", EnumPointType.Root);
                if (CloudRootBranch != null) RootBranchs.Add(CloudRootBranch);

                ConfigRootBranch = TreeEditor.GetBranch("Configuration", EnumPointType.Root);
                if (ConfigRootBranch != null) RootBranchs.Add(ConfigRootBranch);

                DevRootBranch = TreeEditor.GetBranch("Developer", EnumPointType.Root);
                if (DevRootBranch != null) RootBranchs.Add(DevRootBranch);

                DDLRootBranch = TreeEditor.GetBranch("DDL", EnumPointType.Root);
                if (DDLRootBranch != null) RootBranchs.Add(DDLRootBranch);

                ETLRootBranch = TreeEditor.GetBranch("ETL", EnumPointType.Root);
                if (ETLRootBranch != null) RootBranchs.Add(ETLRootBranch);

                ReportRootBranch = TreeEditor.GetBranch("Reports", EnumPointType.Root);
                if (ReportRootBranch != null) RootBranchs.Add(ReportRootBranch);

                ScriptRootBranch = TreeEditor.GetBranch("Script", EnumPointType.Root);
                if (ScriptRootBranch != null) RootBranchs.Add(ScriptRootBranch);

                FileRootBranch = TreeEditor.GetBranch("Files", EnumPointType.Root);
                if (FileRootBranch != null) RootBranchs.Add(FileRootBranch);

                MappingRootBranch = TreeEditor.GetBranch("Mapping", EnumPointType.Root);
                if (MappingRootBranch != null) RootBranchs.Add(MappingRootBranch);

                // add other root branches not in RootBranchs

                List<IBranch> otherrootbranchs = TreeEditor.Branches.Where(x => x.BranchType == EnumPointType.Root && x.BranchClass != "VIEW" && x.BranchClass != "NOSQL" && x.BranchClass != "RDBMS" && x.BranchClass != "AI" && x.BranchClass != "CLOUD" && x.BranchClass != "CONFIG" && x.BranchClass != "DEV" && x.BranchClass != "DDL" && x.BranchClass != "ETL" && x.BranchClass != "REPORT" && x.BranchClass != "SCRIPT" && x.BranchClass != "FILE" && x.BranchClass != "MAP").ToList();
                RootBranchs.AddRange(otherrootbranchs);
            }

        }

        public void GetValues()
        {
            Passedargs=new PassedArgs();
            GetValues(Passedargs);
        }
    }
}

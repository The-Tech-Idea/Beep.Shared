using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TheTechIdea.Beep.Icons
{
    /// <summary>
    /// Provides strongly-typed access to embedded SVG resources in TheTechIdea.Beep.Icons assembly.
    /// Resource names are under the base namespace: TheTechIdea.Beep.Icons.svg
    /// </summary>
    public static class BeepSvgPaths
    {
        private const string BaseNamespace = "TheTechIdea.Beep.Icons.svg";

        /// <summary>
        /// Gets the assembly containing the embedded SVG resources.
        /// </summary>
        public static Assembly ResourceAssembly => Assembly.GetExecutingAssembly();

        #region "General Actions & UI"
        public static readonly string About = $"{BaseNamespace}.about.svg";
        public static readonly string Add = $"{BaseNamespace}.add.svg";
        public static readonly string Abort = $"{BaseNamespace}.abort.svg";
        public static readonly string Back = $"{BaseNamespace}.back.svg";
        public static readonly string BackButton = $"{BaseNamespace}.back-button.svg";
        public static readonly string Backwards = $"{BaseNamespace}.backwards.svg";
        public static readonly string Cancel = $"{BaseNamespace}.cancel.svg";
        public static readonly string Check = $"{BaseNamespace}.check.svg";
        public static readonly string CheckRound = $"{BaseNamespace}.checkround.svg";
        public static readonly string CheckSquare = $"{BaseNamespace}.checksquare.svg";
        public static readonly string Close = $"{BaseNamespace}.close.svg";
        public static readonly string CloseRed = $"{BaseNamespace}.closered.svg";
        public static readonly string Continue = $"{BaseNamespace}.continue.svg";
        public static readonly string Cool = $"{BaseNamespace}.cool.svg";
        public static readonly string Copy = $"{BaseNamespace}.copy.svg";
        public static readonly string Edit = $"{BaseNamespace}.edit.svg";
        public static readonly string Enter = $"{BaseNamespace}.enter.svg";
        public static readonly string Exit = $"{BaseNamespace}.exit.svg";
        public static readonly string Export = $"{BaseNamespace}.export.svg";
        public static readonly string Forward = $"{BaseNamespace}.forward.svg";
        public static readonly string GoBack = $"{BaseNamespace}.go-back.svg";
        public static readonly string Maximize = $"{BaseNamespace}.maximize.svg";
        public static readonly string Minimize = $"{BaseNamespace}.minimize.svg";
        public static readonly string Minus = $"{BaseNamespace}.minus.svg";
        public static readonly string More = $"{BaseNamespace}.more.svg";
        public static readonly string Next = $"{BaseNamespace}.next.svg";
        public static readonly string No = $"{BaseNamespace}.No.svg";
        public static readonly string Pencil = $"{BaseNamespace}.pencil.svg";
        public static readonly string Plus = $"{BaseNamespace}.plus.svg";
        public static readonly string Power = $"{BaseNamespace}.power.svg";
        public static readonly string Previous = $"{BaseNamespace}.previous.svg";
        public static readonly string Print = $"{BaseNamespace}.print.svg";
        public static readonly string Print1 = $"{BaseNamespace}.print1.svg";
        public static readonly string Refresh = $"{BaseNamespace}.refresh.svg";
        public static readonly string Remove = $"{BaseNamespace}.remove.svg";
        public static readonly string Rollback = $"{BaseNamespace}.rollback.svg";
        public static readonly string Save = $"{BaseNamespace}.save.svg";
        public static readonly string SaveAll = $"{BaseNamespace}.saveall.svg";
        public static readonly string Search = $"{BaseNamespace}.search.svg";
        public static readonly string SearchAppBar = $"{BaseNamespace}.searchappbar.svg";
        public static readonly string Search1 = $"{BaseNamespace}.search_1.svg";
        public static readonly string Settings = $"{BaseNamespace}.settings.svg";
        public static readonly string Share = $"{BaseNamespace}.share.svg";
        public static readonly string SignOut = $"{BaseNamespace}.signout.svg";
        public static readonly string SquareClose = $"{BaseNamespace}.squareclose.svg";
        public static readonly string SquareCloseSquare = $"{BaseNamespace}.squareclosesquare.svg";
        public static readonly string SquareMinus = $"{BaseNamespace}.square-minus.svg";
        public static readonly string SquarePlus = $"{BaseNamespace}.square-plus.svg";
        public static readonly string Tick = $"{BaseNamespace}.tick.svg";
        public static readonly string Trash = $"{BaseNamespace}.trash.svg";
        public static readonly string TryAgain = $"{BaseNamespace}.tryagain.svg";
        public static readonly string Undo = $"{BaseNamespace}.undo.svg";
        public static readonly string X = $"{BaseNamespace}.x.svg";
        public static readonly string Yes = $"{BaseNamespace}.yes.svg";
        #endregion

        #region "Navigation & Arrows"
        public static readonly string AngleDoubleSmallDown = $"{BaseNamespace}.angle-double-small-down.svg";
        public static readonly string AngleDoubleSmallLeft = $"{BaseNamespace}.angle-double-small-left.svg";
        public static readonly string AngleDoubleSmallRight = $"{BaseNamespace}.angle-double-small-right.svg";
        public static readonly string AngleDoubleSmallUp = $"{BaseNamespace}.angle-double-small-up.svg";
        public static readonly string AngleSmallDown = $"{BaseNamespace}.angle-small-down.svg";
        public static readonly string AngleSmallLeft = $"{BaseNamespace}.angle-small-left.svg";
        public static readonly string AngleSmallRight = $"{BaseNamespace}.angle-small-right.svg";
        public static readonly string AngleSmallUp = $"{BaseNamespace}.angle-small-up.svg";
        public static readonly string LeftArrow = $"{BaseNamespace}.left-arrow.svg";
        public static readonly string RightArrow = $"{BaseNamespace}.right-arrow.svg";
        public static readonly string FastBackward = $"{BaseNamespace}.fastbackword.svg";
        public static readonly string FastForward = $"{BaseNamespace}.fastforward.svg";
        public static readonly string FirstPage = $"{BaseNamespace}.firstpage.svg";
        public static readonly string FirstRecord = $"{BaseNamespace}.firstrecord.svg";
        public static readonly string LastPage = $"{BaseNamespace}.lastpage.svg";
        public static readonly string LastRecord = $"{BaseNamespace}.lastrecord.svg";
        #endregion

        #region "Information & Status"
        public static readonly string Error = $"{BaseNamespace}.error.svg";
        public static readonly string Information = $"{BaseNamespace}.information.svg";
        public static readonly string Notice = $"{BaseNamespace}.notice.svg";
        public static readonly string Question = $"{BaseNamespace}.question.svg";
        public static readonly string Loading = $"{BaseNamespace}.loading.svg";
        #endregion

        #region "INFO Subfolder"
        public static readonly string InfoAlarm = $"{BaseNamespace}.INFO.alarm.svg";
        public static readonly string InfoAlert = $"{BaseNamespace}.INFO.alert.svg";
        public static readonly string InfoDislike = $"{BaseNamespace}.INFO.dislike.svg";
        public static readonly string InfoHeart = $"{BaseNamespace}.INFO.heart.svg";
        public static readonly string InfoHelp = $"{BaseNamespace}.INFO.help.svg";
        public static readonly string InfoIgnore = $"{BaseNamespace}.INFO.ignore.svg";
        public static readonly string InfoImportant = $"{BaseNamespace}.INFO.important.svg";
        public static readonly string InfoInfo = $"{BaseNamespace}.INFO.info.svg";
        public static readonly string InfoLike = $"{BaseNamespace}.INFO.like.svg";
        public static readonly string InfoWarning = $"{BaseNamespace}.INFO.warning.svg";
        #endregion

        #region "UI Controls & Interface"
        public static readonly string AddressBook = $"{BaseNamespace}.addressbook.svg";
        public static readonly string AlarmClock = $"{BaseNamespace}.alarmclock.svg";
        public static readonly string Bullet = $"{BaseNamespace}.bullet.svg";
        public static readonly string Calendar = $"{BaseNamespace}.calendar.svg";
        public static readonly string Comment = $"{BaseNamespace}.comment.svg";
        public static readonly string DoorClosed = $"{BaseNamespace}.doorclosed.svg";
        public static readonly string DoorOpen = $"{BaseNamespace}.dooropen.svg";
        public static readonly string DropdownSelect = $"{BaseNamespace}.dropdown-select.svg";
        public static readonly string Email = $"{BaseNamespace}.email.svg";
        public static readonly string EqualizerControl = $"{BaseNamespace}.equalizercontrol.svg";
        public static readonly string Favorite = $"{BaseNamespace}.favorite.svg";
        public static readonly string File = $"{BaseNamespace}.file.svg";
        public static readonly string Filter = $"{BaseNamespace}.filter.svg";
        public static readonly string FloppyDisk = $"{BaseNamespace}.floppy-disk.svg";
        public static readonly string Gear = $"{BaseNamespace}.gear.svg";
        public static readonly string Hamburger = $"{BaseNamespace}.hamburger.svg";
        public static readonly string Home = $"{BaseNamespace}.home.svg";
        public static readonly string Input = $"{BaseNamespace}.input.svg";
        public static readonly string Keys = $"{BaseNamespace}.keys.svg";
        public static readonly string L = $"{BaseNamespace}.l.svg";
        public static readonly string Mail = $"{BaseNamespace}.mail.svg";
        public static readonly string Menu = $"{BaseNamespace}.menu.svg";
        public static readonly string PaperClip = $"{BaseNamespace}.paperclip.svg";
        public static readonly string PersonEdit = $"{BaseNamespace}.personedit.svg";
        public static readonly string PersonMinus = $"{BaseNamespace}.personminus.svg";
        public static readonly string PersonPlus = $"{BaseNamespace}.personplus.svg";
        public static readonly string Sort = $"{BaseNamespace}.sort.svg";
        public static readonly string SortAlphaDown = $"{BaseNamespace}.sortalphadown.svg";
        public static readonly string SortAlphaUp = $"{BaseNamespace}.sortalphaup.svg";
        public static readonly string Star = $"{BaseNamespace}.star.svg";
        public static readonly string Sum = $"{BaseNamespace}.sum.svg";
        public static readonly string Theme = $"{BaseNamespace}.theme.svg";
        public static readonly string ThumbUp = $"{BaseNamespace}.thumb-up.svg";
        public static readonly string TrendDown = $"{BaseNamespace}.trenddown.svg";
        public static readonly string TrendUp = $"{BaseNamespace}.trendup.svg";
        public static readonly string User = $"{BaseNamespace}.user.svg";
        #endregion

        #region "NAV Subfolder"
        private const string NavNamespace = BaseNamespace + ".NAV";
        public static readonly string NavBackArrow = $"{NavNamespace}.005-back arrow.svg";
        public static readonly string NavChevron = $"{NavNamespace}.015-chevron.svg";
        public static readonly string NavDoubleChevron = $"{NavNamespace}.016-double chevron.svg";
        public static readonly string NavDashboard = $"{NavNamespace}.024-dashboard.svg";
        public static readonly string NavUser = $"{NavNamespace}.025-user.svg";
        public static readonly string NavEraser = $"{NavNamespace}.035-eraser.svg";
        public static readonly string NavFlag = $"{NavNamespace}.035-flag.svg";
        public static readonly string NavFloppyDisk = $"{NavNamespace}.036-floppy disk.svg";
        public static readonly string NavMaximize = $"{NavNamespace}.054-maximize.svg";
        public static readonly string NavMinimize = $"{NavNamespace}.055-minimize.svg";
        public static readonly string NavMinus = $"{NavNamespace}.058-minus.svg";
        public static readonly string NavPencil = $"{NavNamespace}.062-pencil.svg";
        public static readonly string NavPlus = $"{NavNamespace}.068-plus.svg";
        public static readonly string NavPrinter = $"{NavNamespace}.072-printer.svg";
        public static readonly string NavRemove = $"{NavNamespace}.078-remove.svg";
        public static readonly string NavSearch = $"{NavNamespace}.079-search.svg";
        public static readonly string NavShare = $"{NavNamespace}.083-share.svg";
        public static readonly string NavTrash = $"{NavNamespace}.089-trash.svg";
        public static readonly string NavWaving = $"{NavNamespace}.093-waving.svg";
        public static readonly string NavAngleDoubleSmallLeft = $"{NavNamespace}.angle-double-small-left.svg";
        public static readonly string NavAngleDoubleSmallRight = $"{NavNamespace}.angle-double-small-right.svg";
        public static readonly string NavAngleSmallLeft = $"{NavNamespace}.angle-small-left.svg";
        public static readonly string NavAngleSmallRight = $"{NavNamespace}.angle-small-right.svg";
        #endregion

        #region "Database & Data Sources"
        public static readonly string Cassandra = $"{BaseNamespace}.cassandra.svg";
        public static readonly string CockroachDb = $"{BaseNamespace}.cockroachdb.svg";
        public static readonly string CouchBase = $"{BaseNamespace}.couchbase.svg";
        public static readonly string CouchDb = $"{BaseNamespace}.couchdb.svg";
        public static readonly string CouchDb1 = $"{BaseNamespace}.couchdb-1.svg";
        public static readonly string Csv = $"{BaseNamespace}.csv.svg";
        public static readonly string DataSources = $"{BaseNamespace}.datasources.svg";
        public static readonly string DataView = $"{BaseNamespace}.dataview.svg";
        public static readonly string Db2 = $"{BaseNamespace}.db2.svg";
        public static readonly string DuckDb = $"{BaseNamespace}.duckdb.svg";
        public static readonly string DuckDbLogo = $"{BaseNamespace}.duckdb-logo.svg";
        public static readonly string ElasticSearch = $"{BaseNamespace}.elasticsearch.svg";
        public static readonly string Firebase = $"{BaseNamespace}.firebase.svg";
        public static readonly string Firebase2 = $"{BaseNamespace}.firebase-2.svg";
        public static readonly string Firebird = $"{BaseNamespace}.firebird.svg";
        public static readonly string Hadoop = $"{BaseNamespace}.hadoop.svg";
        public static readonly string HBase = $"{BaseNamespace}.hbase.svg";
        public static readonly string Json = $"{BaseNamespace}.json.svg";
        public static readonly string Kafka = $"{BaseNamespace}.kafka.svg";
        public static readonly string LiteDb = $"{BaseNamespace}.litedb.svg";
        public static readonly string LocalConnections = $"{BaseNamespace}.localconnections.svg";
        public static readonly string MicrosoftSqlServer = $"{BaseNamespace}.microsoft-sql-server-1.svg";
        public static readonly string MongoDb = $"{BaseNamespace}.mongodb.svg";
        public static readonly string MySql = $"{BaseNamespace}.mysql.svg";
        public static readonly string MySqlDatabase = $"{BaseNamespace}.mysql-database.svg";
        public static readonly string MySqlLogoPure = $"{BaseNamespace}.mysql-logo-pure.svg";
        public static readonly string Opc = $"{BaseNamespace}.opc.svg";
        public static readonly string Oracle = $"{BaseNamespace}.oracle.svg";
        public static readonly string Oracle1 = $"{BaseNamespace}.oracle-1.svg";
        public static readonly string OracleLogo = $"{BaseNamespace}.oracle-logo.svg";
        public static readonly string OracleLogo3 = $"{BaseNamespace}.oracle-logo-3.svg";
        public static readonly string Postgre = $"{BaseNamespace}.postgre.svg";
        public static readonly string RavenDb = $"{BaseNamespace}.ravendb.svg";
        public static readonly string Realm = $"{BaseNamespace}.realm.svg";
        public static readonly string RealmIo = $"{BaseNamespace}.realmio.svg";
        public static readonly string Redis = $"{BaseNamespace}.redis.svg";
        public static readonly string ScyllaDb = $"{BaseNamespace}.scylladb.svg";
        public static readonly string Snowflake = $"{BaseNamespace}.snowflake.svg";
        public static readonly string SqlServer = $"{BaseNamespace}.sql-server.svg";
        public static readonly string SqlServer2 = $"{BaseNamespace}.sqlserver.svg";
        public static readonly string Sqlite = $"{BaseNamespace}.sqlite.svg";
        public static readonly string Sqlite1 = $"{BaseNamespace}.sqlite-1.svg";
        public static readonly string VistaDb = $"{BaseNamespace}.vistadb.svg";
        public static readonly string Xls = $"{BaseNamespace}.xls.svg";
        #endregion

        #region "Beep Specific"
        public static readonly string Beep = $"{BaseNamespace}.beep.svg";
        public static readonly string BeepSetup = $"{BaseNamespace}.beepsetup.svg";
        public static readonly string Cat = $"{BaseNamespace}.cat.svg";
        public static readonly string Diagramming = $"{BaseNamespace}.diagramming.svg";
        public static readonly string DontSave = $"{BaseNamespace}.dontsave.svg";
        public static readonly string Kitty = $"{BaseNamespace}.kitty.svg";
        public static readonly string Know = $"{BaseNamespace}.know.svg";
        public static readonly string SimpleInfoApps = $"{BaseNamespace}.simpleinfoapps.svg";
        public static readonly string Stakeholder = $"{BaseNamespace}.stakeholder.svg";
        #endregion

        /// <summary>
        /// Gets all available SVG resource names from the assembly under the icons base namespace.
        /// </summary>
        public static string[] GetAvailableResources()
        {
            return ResourceAssembly.GetManifestResourceNames()
                .Where(name => name.StartsWith(BaseNamespace) && name.EndsWith(".svg"))
                .ToArray();
        }

        /// <summary>
        /// Checks if a resource path exists in the assembly.
        /// </summary>
        public static bool ResourceExists(string resourcePath)
        {
            return ResourceAssembly.GetManifestResourceNames().Contains(resourcePath);
        }

        /// <summary>
        /// Returns full resource path for a simple file name or relative path (e.g., "add.svg" or "NAV/079-search.svg").
        /// Uses dot separators as required by embedded resource naming.
        /// </summary>
        public static string ToResourcePath(string simpleOrRelativePath)
        {
            if (string.IsNullOrWhiteSpace(simpleOrRelativePath)) return BaseNamespace;
            var normalized = simpleOrRelativePath.Replace('/', '.').Replace('\\', '.');
            return $"{BaseNamespace}.{normalized}";
        }

        /// <summary>
        /// Helper method to get the file system path during development (if sources are present).
        /// </summary>
        public static string GetFileSystemPath(string svgFileName)
        {
            string baseDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty;
            return Path.Combine(baseDirectory, svgFileName);
        }
    }
}

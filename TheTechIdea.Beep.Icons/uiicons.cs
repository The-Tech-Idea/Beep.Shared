using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace TheTechIdea.Beep.Icons
{
    /// <summary>
    /// Provides convenient access to embedded UI icon SVGs under the namespace:
    /// TheTechIdea.Beep.Icons.uiicons
    /// </summary>
    public static class uiicons
    {
        private const string BaseNamespace = "TheTechIdea.Beep.Icons.uiicons";

        /// <summary>
        /// Gets the assembly containing the embedded UI icon SVG resources.
        /// </summary>
        public static Assembly ResourceAssembly => typeof(uiicons).Assembly;

        private static readonly Lazy<string[]> _resourceNames = new(() =>
            ResourceAssembly.GetManifestResourceNames()
                .Where(n => n.StartsWith(BaseNamespace, StringComparison.Ordinal))
                .Where(n => n.EndsWith(".svg", StringComparison.OrdinalIgnoreCase))
                .ToArray());

        private static readonly Lazy<Dictionary<string, string>> _byFileName = new(() =>
        {
            var dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var fullName in _resourceNames.Value)
            {
                var fileName = UiIconsHelpers.GetFileName(fullName, BaseNamespace);
                if (!dict.ContainsKey(fileName))
                {
                    dict[fileName] = fullName;
                }
            }
            return dict;
        });

        /// <summary>
        /// Returns all embedded UI icon resource names (full manifest names).
        /// </summary>
        public static string[] GetAllResourceNames() => _resourceNames.Value;

        /// <summary>
        /// Returns all UI icon file names (e.g., "fi-tr-user.svg").
        /// </summary>
        public static string[] GetAllFileNames() => _byFileName.Value.Keys.ToArray();

        /// <summary>
        /// Checks if a UI icon resource exists by full manifest name or by file name.
        /// </summary>
        public static bool Exists(string nameOrFile)
        {
            if (string.IsNullOrWhiteSpace(nameOrFile)) return false;
            if (nameOrFile.StartsWith(BaseNamespace, StringComparison.Ordinal))
            {
                return _resourceNames.Value.Contains(nameOrFile);
            }
            var file = UiIconsHelpers.EnsureExtension(UiIconsHelpers.ExtractFileName(nameOrFile));
            return _byFileName.Value.ContainsKey(file);
        }

        /// <summary>
        /// Tries to get the full resource path from a file name or relative path.
        /// Accepted inputs: "fi-tr-*.svg", "uiicons/fi-tr-*.svg", full manifest name.
        /// </summary>
        public static bool TryGet(string nameOrFile, out string resourcePath)
        {
            resourcePath = string.Empty;
            if (string.IsNullOrWhiteSpace(nameOrFile)) return false;

            // Already a full manifest resource name
            if (nameOrFile.StartsWith(BaseNamespace, StringComparison.Ordinal))
            {
                if (_resourceNames.Value.Contains(nameOrFile))
                {
                    resourcePath = nameOrFile;
                    return true;
                }
                return false;
            }

            // Accept raw filename or folder-like path
            var file = UiIconsHelpers.EnsureExtension(UiIconsHelpers.ExtractFileName(nameOrFile));
            if (_byFileName.Value.TryGetValue(file, out var full))
            {
                resourcePath = full;
                return true;
            }

            // Try to construct a manifest path from a relative-like path
            var normalized = nameOrFile.Replace('/', '.').Replace('\\', '.');
            if (!normalized.StartsWith("uiicons.", StringComparison.OrdinalIgnoreCase))
            {
                normalized = $"uiicons.{normalized}";
            }
            if (!normalized.EndsWith(".svg", StringComparison.OrdinalIgnoreCase))
            {
                normalized += ".svg";
            }
            var candidate = $"{BaseNamespace}.{normalized.Substring("uiicons.".Length)}";
            if (_resourceNames.Value.Contains(candidate))
            {
                resourcePath = candidate;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Converts a file name or relative path to a full manifest resource path.
        /// Returns null if the resource does not exist.
        /// </summary>
        public static string? ToResourcePath(string nameOrFile)
        {
            return TryGet(nameOrFile, out var path) ? path : null;
        }

        /// <summary>
        /// Opens an embedded UI icon SVG as a stream by file name or full resource name.
        /// Returns null if not found.
        /// </summary>
        public static Stream? Open(string nameOrFile)
        {
            if (!TryGet(nameOrFile, out var full)) return null;
            return ResourceAssembly.GetManifestResourceStream(full);
        }

        // Shared helper for nested icon categories
        private static string Require(string file)
            => ToResourcePath(file) ?? throw new InvalidOperationException($"UI icon not found: {file}");

        /// <summary>
        /// Strongly-typed curated subset of common UI icons.
        /// Values are full manifest resource names ready for GetManifestResourceStream.
        /// </summary>
        public static class Common
        {
            // Actions
            public static string Add => Require("fi-tr-add.svg");
            public static string Edit => Require("fi-tr-pen.svg");
            public static string Delete => Require("fi-tr-trash-empty.svg");
            public static string Save => Require("fi-tr-floppy-disk-pen.svg");
            public static string Upload => Require("fi-tr-file-upload.svg");
            public static string Download => Require("fi-tr-file-download.svg");
            public static string Refresh => Require("fi-tr-rotate-reverse.svg");

            // Clipboard
            public static string Copy => Require("fi-tr-copy.svg");
            public static string Paste => Require("fi-tr-paste.svg");
            public static string Undo => Require("fi-tr-undo.svg");
            public static string Redo => Require("fi-tr-redo-alt.svg");

            // Sorting & Filtering
            public static string Filter => Require("fi-tr-filter.svg");
            public static string Sort => Require("fi-tr-sort.svg");

            // Visibility
            public static string Visible => Require("fi-tr-eye-recognition.svg");
            public static string Hidden => Require("fi-tr-low-vision.svg");

            // Security
            public static string Lock => Require("fi-tr-lock-alt.svg");
            public static string Unlock => Require("fi-tr-lock-open-alt.svg");

            // Navigation
            public static string Home => Require("fi-tr-house.svg");
            public static string Back => Require("fi-tr-arrow-left.svg");
            public static string Forward => Require("fi-tr-arrow-right.svg");
            public static string ChevronLeft => Require("fi-tr-angle-left.svg");
            public static string ChevronRight => Require("fi-tr-angle-right.svg");
            public static string ChevronUp => Require("fi-tr-angle-up.svg");
            public static string ChevronDown => Require("fi-tr-angle-down.svg");

            // UI Indicators
            public static string Search => Require("fi-tr-search-alt.svg");
            public static string Settings => Require("fi-tr-gears.svg");
            public static string Info => Require("fi-tr-info.svg");
            public static string Warning => Require("fi-tr-triangle-warning.svg");
            public static string Error => Require("fi-tr-circle-xmark.svg");
            public static string Success => Require("fi-tr-check-circle.svg");
            public static string Bell => Require("fi-tr-bell.svg");
            public static string Star => Require("fi-tr-star.svg");
            public static string Heart => Require("fi-tr-heart.svg");
            public static string Tag => Require("fi-tr-tags.svg");

            // Date & Time
            public static string Calendar => Require("fi-tr-calendar.svg");
            public static string Clock => Require("fi-tr-clock.svg");

            // Files & Folders
            public static string Folder => Require("fi-tr-folders.svg");
            public static string FolderOpen => Require("fi-tr-folder-open.svg");
            public static string Document => Require("fi-tr-document.svg");
            public static string Image => Require("fi-tr-file-image.svg");
            public static string Video => Require("fi-tr-file-video.svg");
            public static string Camera => Require("fi-tr-camera.svg");

            // Communication
            public static string Envelope => Require("fi-tr-envelope.svg");
            public static string Phone => Require("fi-tr-phone-call.svg");

            // Location
            public static string MapPin => Require("fi-tr-map-pin.svg");
            public static string Location => Require("fi-tr-location-crosshairs.svg");

            // Media Controls
            public static string Play => Require("fi-tr-play-circle.svg");
            public static string Pause => Require("fi-tr-pause-circle.svg");
            public static string Stop => Require("fi-tr-stop-circle.svg");
            public static string StepForward => Require("fi-tr-step-forward.svg");
            public static string StepBackward => Require("fi-tr-step-backward.svg");
            public static string ForwardFast => Require("fi-tr-forward-fast.svg");
            public static string BackwardFast => Require("fi-tr-rewind-button-circle.svg");

            // Window / Layout
            public static string Expand => Require("fi-tr-expand-arrows-alt.svg");
            public static string Compress => Require("fi-tr-compress-alt.svg");
            public static string Fullscreen => Require("fi-tr-full-screen.svg");
            public static string Resize => Require("fi-tr-resize-screen.svg");

            // Symbols
            public static string Close => Require("fi-tr-x.svg");
            public static string Plus => Require("fi-tr-square-plus.svg");
            public static string Minus => Require("fi-tr-square-minus.svg");
            public static string Check => Require("fi-tr-check-circle.svg");
            public static string Cancel => Require("fi-tr-circle-xmark.svg");
        }

        // Auth
        public static class Auth
        {
            public static string Login => Require("fi-tr-login.svg");
            public static string Logout => Require("fi-tr-right-from-bracket.svg");
            public static string SignIn => Require("fi-tr-sign-in-alt.svg");
            public static string SignOut => Require("fi-tr-sign-out-alt.svg");
            public static string UserAdd => Require("fi-tr-person-circle-plus.svg");
            public static string UserRemove => Require("fi-tr-person-circle-minus.svg");
            public static string UserSettings => Require("fi-tr-user-gear.svg");
            public static string UserLock => Require("fi-tr-user-lock.svg");
        }

        // Data / Table
        public static class DataTable
        {
            public static string Table => Require("fi-tr-table.svg");
            public static string Columns => Require("fi-tr-table-columns.svg");
            public static string Rows => Require("fi-tr-table-rows.svg");
            public static string FilterOn => Require("fi-tr-filter.svg");
            public static string FilterOff => Require("fi-tr-filter-slash.svg");
            public static string SortAsc => Require("fi-tr-sort-amount-up.svg");
            public static string SortDesc => Require("fi-tr-sort-amount-down.svg");
            public static string Import => Require("fi-tr-file-import.svg");
            public static string Export => Require("fi-tr-file-export.svg");
        }

        // Pagination
        public static class Pagination
        {
            public static string First => Require("fi-tr-previous-square.svg");
            public static string Previous => Require("fi-tr-arrow-left.svg");
            public static string Next => Require("fi-tr-arrow-right.svg");
            public static string Last => Require("fi-tr-last-square.svg");
        }

        // View
        public static class View
        {
            public static string List => Require("fi-tr-rectangle-list.svg");
            public static string Cards => Require("fi-tr-rectangles-mixed.svg");
            public static string Grid => Require("fi-tr-grid-dividers.svg");
            public static string Details => Require("fi-tr-rectangle-vertical.svg");
        }

        // Editor
        public static class Editor
        {
            public static string Strikethrough => Require("fi-tr-strikethrough.svg");
            public static string Highlighter => Require("fi-tr-highlighter.svg");
            public static string Eraser => Require("fi-tr-eraser.svg");
            public static string Link => Require("fi-tr-link-horizontal.svg");
            public static string LinkRemove => Require("fi-tr-link-horizontal-slash.svg");
            public static string Code => Require("fi-tr-code-simple.svg");
            public static string Quote => Require("fi-tr-square-quote.svg");
        }

        // Files
        public static class Files
        {
            public static string Edit => Require("fi-tr-file-edit.svg");
            public static string Pdf => Require("fi-tr-file-pdf.svg");
            public static string Excel => Require("fi-tr-file-excel.svg");
            public static string Image => Require("fi-tr-file-image.svg");
            public static string Video => Require("fi-tr-file-video.svg");
            public static string Zip => Require("fi-tr-file-zipper.svg");
            public static string Download => Require("fi-tr-file-download.svg");
            public static string Upload => Require("fi-tr-file-upload.svg");
        }

        // Folders
        public static class Folders
        {
            public static string Folder => Require("fi-tr-folders.svg");
            public static string Open => Require("fi-tr-folder-open.svg");
            public static string Plus => Require("fi-tr-folder-plus-circle.svg");
            public static string Minus => Require("fi-tr-folder-minus.svg");
            public static string Upload => Require("fi-tr-folder-upload.svg");
            public static string Download => Require("fi-tr-folder-download.svg");
            public static string Gear => Require("fi-tr-folder-gear.svg");
            public static string Tree => Require("fi-tr-folder-tree.svg");
        }

        // Communication
        public static class Communication
        {
            public static string Mail => Require("fi-tr-envelope.svg");
            public static string MailOpen => Require("fi-tr-envelope-open.svg");
            public static string Send => Require("fi-tr-paper-plane-top.svg");
            public static string Chat => Require("fi-tr-comment-dots.svg");
            public static string Comment => Require("fi-tr-comment.svg");
            public static string Phone => Require("fi-tr-phone-call.svg");
            public static string Sms => Require("fi-tr-message-sms.svg");
            public static string Bell => Require("fi-tr-bell.svg");
        }

        // Status / Alerts
        public static class Status
        {
            public static string Help => Require("fi-tr-question.svg");
            public static string Pending => Require("fi-tr-pending.svg");
            public static string Loading => Require("fi-tr-loading.svg");
            public static string Disabled => Require("fi-tr-block.svg");
        }

        // Security
        public static class Security
        {
            public static string Lock => Require("fi-tr-lock-alt.svg");
            public static string Unlock => Require("fi-tr-lock-open-alt.svg");
            public static string Shield => Require("fi-tr-shield.svg");
            public static string ShieldCheck => Require("fi-tr-shield-check.svg");
            public static string Key => Require("fi-tr-key.svg");
            public static string Password => Require("fi-tr-password.svg");
            public static string Mfa => Require("fi-tr-otp.svg");
        }

        // Devices
        public static class Devices
        {
            public static string Desktop => Require("fi-tr-display.svg");
            public static string Laptop => Require("fi-tr-laptop.svg");
            public static string Mobile => Require("fi-tr-mobile.svg");
            public static string Tablet => Require("fi-tr-tablet.svg");
            public static string Printer => Require("fi-tr-print.svg");
        }

        // Network / Cloud
        public static class NetworkCloud
        {
            public static string Wifi => Require("fi-tr-wifi.svg");
            public static string WifiLock => Require("fi-tr-wifi-lock.svg");
            public static string Ethernet => Require("fi-tr-ethernet.svg");
            public static string Router => Require("fi-tr-router-wifi.svg");
            public static string Cloud => Require("fi-tr-cloud.svg");
            public static string CloudUpload => Require("fi-tr-cloud-upload.svg");
            public static string CloudDownload => Require("fi-tr-cloud-download.svg");
            public static string Vpn => Require("fi-tr-vpn.svg");
        }

        // Media / Volume
        public static class Media
        {
            public static string Play => Require("fi-tr-play-circle.svg");
            public static string Pause => Require("fi-tr-pause-circle.svg");
            public static string Stop => Require("fi-tr-stop-circle.svg");
            public static string ForwardFast => Require("fi-tr-forward-fast.svg");
            public static string BackwardFast => Require("fi-tr-rewind-button-circle.svg");
            public static string VolumeControl => Require("fi-tr-volume-control.svg");
            public static string VolumeOff => Require("fi-tr-volume-off.svg");
            public static string VolumeMute => Require("fi-tr-volume-mute.svg");
            public static string VolumeDown => Require("fi-tr-volume-down.svg");
        }

        // Window
        public static class Window
        {
            public static string Minimize => Require("fi-tr-window-minimize.svg");
            public static string Maximize => Require("fi-tr-window-maximize.svg");
            public static string Restore => Require("fi-tr-window-restore.svg");
            public static string Close => Require("fi-tr-rectangle-xmark.svg");
        }

        // Dev / Git
        public static class DevGit
        {
            public static string Branch => Require("fi-tr-code-branch.svg");
            public static string Merge => Require("fi-tr-code-merge.svg");
            public static string Fork => Require("fi-tr-code-fork.svg");
            public static string Commit => Require("fi-tr-code-commit.svg");
            public static string PullRequest => Require("fi-tr-code-pull-request.svg");
            public static string PullRequestDraft => Require("fi-tr-code-pull-request-draft.svg");
            public static string PullRequestClosed => Require("fi-tr-code-pull-request-closed.svg");
            public static string Compare => Require("fi-tr-code-compare.svg");
        }

        // Commerce
        public static class Commerce
        {
            public static string Cart => Require("fi-tr-shopping-cart.svg");
            public static string CartAdd => Require("fi-tr-shopping-cart-add.svg");
            public static string CartCheck => Require("fi-tr-shopping-cart-check.svg");
            public static string Bag => Require("fi-tr-shopping-bag.svg");
            public static string BagAdd => Require("fi-tr-shopping-bag-add.svg");
            public static string Wallet => Require("fi-tr-wallet.svg");
            public static string Receipt => Require("fi-tr-receipt.svg");
        }

        // Selection / Toggles
        public static class Selection
        {
            public static string Radio => Require("fi-tr-radio-button.svg");
            public static string ToggleOn => Require("fi-tr-toggle-on.svg");
            public static string ToggleOff => Require("fi-tr-toggle-off.svg");
        }

        // Map / Location
        public static class Map
        {
            public static string Pin => Require("fi-tr-map-pin.svg");
            public static string Crosshairs => Require("fi-tr-location-crosshairs.svg");
            public static string Gps => Require("fi-tr-gps-navigation.svg");
            public static string Route => Require("fi-tr-route.svg");
        }

        // Analytics / Charts
        public static class Analytics
        {
            public static string LineUp => Require("fi-tr-chart-line-up.svg");
            public static string LineUpDown => Require("fi-tr-chart-line-up-down.svg");
            public static string Pie => Require("fi-tr-chart-pie.svg");
            public static string Area => Require("fi-tr-chart-area.svg");
            public static string Simple => Require("fi-tr-chart-simple.svg");
            public static string Histogram => Require("fi-tr-chart-histogram.svg");
        }

        // Documents / Content Ops
        public static class Documents
        {
            public static string AddDocument => Require("fi-tr-add-document.svg");
            public static string AddFolder => Require("fi-tr-add-folder.svg");
            public static string DeleteDocument => Require("fi-tr-delete-document.svg");
            public static string TrashRestore => Require("fi-tr-trash-restore.svg");
            public static string TrashRestoreAlt => Require("fi-tr-trash-restore-alt.svg");
            public static string FolderArchive => Require("fi-tr-folder-archive.svg");
            public static string RecycleBin => Require("fi-tr-recycle-bin.svg");
        }

        // Arrows & External
        public static class Arrows
        {
            public static string Up => Require("fi-tr-arrow-up.svg");
            public static string Down => Require("fi-tr-arrow-down.svg");
            public static string Left => Require("fi-tr-arrow-left.svg");
            public static string Right => Require("fi-tr-arrow-right.svg");
            public static string DoubleLeft => Require("fi-tr-angle-double-left.svg");
            public static string DoubleRight => Require("fi-tr-angle-double-right.svg");
            public static string DoubleUp => Require("fi-tr-angle-double-up.svg");
            public static string DoubleDown => Require("fi-tr-angle-double-down.svg");
            public static string ExternalLink => Require("fi-tr-up-right-from-square.svg");
        }

        // Share
        public static class Share
        {
            public static string ShareAltSquare => Require("fi-tr-share-alt-square.svg");
            public static string ShareSquare => Require("fi-tr-share-square.svg");
        }

        // Clipboard extras
        public static class ClipboardExtras
        {
            public static string Check => Require("fi-tr-clipboard-check.svg");
            public static string List => Require("fi-tr-clipboard-list.svg");
            public static string Exclamation => Require("fi-tr-clipboard-exclamation.svg");
        }

        // Notifications
        public static class Notifications
        {
            public static string Bell => Require("fi-tr-bell.svg");
            public static string BellRing => Require("fi-tr-bell-ring.svg");
            public static string BellSlash => Require("fi-tr-bell-slash.svg");
        }

        // Search variants
        public static class Search
        {
            public static string Dollar => Require("fi-tr-search-dollar.svg");
            public static string Location => Require("fi-tr-search-location.svg");
        }

        // Users
        public static class Users
        {
            public static string Check => Require("fi-tr-user-check.svg");
            public static string Slash => Require("fi-tr-user-slash.svg");
        }

        // Storage
        public static class Storage
        {
            public static string Database => Require("fi-tr-database.svg");
            public static string HardDrive => Require("fi-tr-hard-drive.svg");
        }

        // Finance
        public static class Finance
        {
            public static string CreditCard => Require("fi-tr-credit-card.svg");
            public static string MoneyBillWave => Require("fi-tr-money-bill-wave.svg");
            public static string MoneyBillWaveAlt => Require("fi-tr-money-bill-wave-alt.svg");
        }

        // Tasks / To-Do / Progress
        public static class Tasks
        {
            public static string Checklist => Require("fi-tr-checklist-task-budget.svg");
            public static string ProgressDownload => Require("fi-tr-progress-download.svg");
            public static string ProgressUpload => Require("fi-tr-progress-upload.svg");
            public static string ProgressComplete => Require("fi-tr-progress-complete.svg");
        }

        // Codes (QR / Barcode)
        public static class Codes
        {
            public static string Qr => Require("fi-tr-qrcode.svg");
            public static string QrScan => Require("fi-tr-qr-scan.svg");
            public static string Barcode => Require("fi-tr-barcode.svg");
            public static string BarcodeRect => Require("fi-tr-rectangle-barcode.svg");
        }

        // Quality (Bugs / Security checks)
        public static class Quality
        {
            public static string Bug => Require("fi-tr-bug.svg");
            public static string BugReport => Require("fi-tr-bug-report.svg");
            public static string BugFix => Require("fi-tr-bug-fix.svg");
            public static string ShieldCheck => Require("fi-tr-shield-check.svg");
            public static string ShieldExclamation => Require("fi-tr-shield-exclamation.svg");
        }

        // Activity / System states
        public static class Activity
        {
            public static string Notification => Require("fi-tr-app-notification.svg");
            public static string Loading => Require("fi-tr-loading.svg");
            public static string LoadingDots => Require("fi-tr-dots-loading.svg");
            public static string Sync => Require("fi-tr-arrows-repeat.svg");
            public static string Rotate => Require("fi-tr-rotate-square.svg");
            public static string RotateExclamation => Require("fi-tr-rotate-exclamation.svg");
            public static string History => Require("fi-tr-rectangle-vertical-history.svg");
        }

        // Clipboard more
        public static class ClipboardMore
        {
            public static string User => Require("fi-tr-clipboard-user.svg");
            public static string Question => Require("fi-tr-clipboard-question.svg");
            public static string Prescription => Require("fi-tr-clipboard-prescription.svg");
            public static string ListCheck => Require("fi-tr-clipboard-list-check.svg");
        }

        // Calendar extended
        public static class CalendarEx
        {
            public static string Plus => Require("fi-tr-calendar-plus.svg");
            public static string Minus => Require("fi-tr-calendar-minus.svg");
            public static string Check => Require("fi-tr-calendar-check.svg");
            public static string Xmark => Require("fi-tr-calendar-xmark.svg");
            public static string Days => Require("fi-tr-calendar-days.svg");
            public static string Week => Require("fi-tr-calendar-week.svg");
            public static string Star => Require("fi-tr-calendar-star.svg");
            public static string Heart => Require("fi-tr-calendar-heart.svg");
            public static string Update => Require("fi-tr-calendar-update.svg");
        }

        // Users advanced
        public static class UsersAdvanced
        {
            public static string PersonCirclePlus => Require("fi-tr-person-circle-plus.svg");
            public static string PersonCircleMinus => Require("fi-tr-person-circle-minus.svg");
            public static string PersonCircleCheck => Require("fi-tr-person-circle-check.svg");
            public static string PersonCircleXmark => Require("fi-tr-person-circle-xmark.svg");
            public static string PeopleGroup => Require("fi-tr-people-group.svg");
            public static string PeopleArrowsLeftRight => Require("fi-tr-people-arrows-left-right.svg");
            public static string PeopleLine => Require("fi-tr-people-line.svg");
        }

        // Messages
        public static class Messages
        {
            public static string Alert => Require("fi-tr-message-alert.svg");
            public static string ArrowDown => Require("fi-tr-message-arrow-down.svg");
            public static string ArrowUp => Require("fi-tr-message-arrow-up.svg");
            public static string ArrowUpRight => Require("fi-tr-message-arrow-up-right.svg");
            public static string Bot => Require("fi-tr-message-bot.svg");
            public static string Code => Require("fi-tr-message-code.svg");
            public static string Dollar => Require("fi-tr-message-dollar.svg");
            public static string Heart => Require("fi-tr-message-heart.svg");
            public static string Image => Require("fi-tr-message-image.svg");
            public static string Question => Require("fi-tr-message-question.svg");
            public static string Quote => Require("fi-tr-message-quote.svg");
            public static string Slash => Require("fi-tr-message-slash.svg");
            public static string Sms => Require("fi-tr-message-sms.svg");
            public static string SquareRefresh => Require("fi-tr-message-square-refresh.svg");
            public static string Star => Require("fi-tr-message-star.svg");
            public static string Text => Require("fi-tr-message-text.svg");
            public static string Xmark => Require("fi-tr-message-xmark.svg");
        }

        // Printing
        public static class Printing
        {
            public static string Print => Require("fi-tr-print.svg");
            public static string PrintWifi => Require("fi-tr-print-wifi.svg");
            public static string PrintSlash => Require("fi-tr-print-slash.svg");
            public static string PrintMagnifier => Require("fi-tr-print-magnifying-glass.svg");
        }

        // Tools
        public static class Tools
        {
            public static string Screwdriver => Require("fi-tr-screwdriver.svg");
            public static string Hammer => Require("fi-tr-hammer.svg");
            public static string WrenchScrew => Require("fi-tr-screw.svg");
            public static string Bulb => Require("fi-tr-bulb.svg");
        }

        // Health / Medical
        public static class Health
        {
            public static string FirstAidKit => Require("fi-tr-first-aid-kit.svg");
            public static string MedicalStar => Require("fi-tr-medical-star.svg");
            public static string Pills => Require("fi-tr-pills.svg");
            public static string Hospital => Require("fi-tr-hospital.svg");
            public static string HospitalBed => Require("fi-tr-hospital-bed.svg");
            public static string Doctor => Require("fi-tr-doctor.svg");
        }

        // Transport
        public static class Transport
        {
            public static string Car => Require("fi-tr-car-side.svg");
            public static string Bus => Require("fi-tr-bus.svg");
            public static string PlaneDeparture => Require("fi-tr-plane-departure.svg");
            public static string Motorcycle => Require("fi-tr-motorcycle.svg");
            public static string Biking => Require("fi-tr-biking.svg");
            public static string RouteHighway => Require("fi-tr-route-highway.svg");
            public static string RouteInterstate => Require("fi-tr-route-interstate.svg");
        }

        // Layout / Arrange
        public static class Layout
        {
            public static string DistributeH => Require("fi-tr-distribute-spacing-horizontal.svg");
            public static string DistributeV => Require("fi-tr-distribute-spacing-vertical.svg");
            public static string BringFront => Require("fi-tr-bring-front.svg");
            public static string BringForward => Require("fi-tr-bring-forward.svg");
            public static string SendBack => Require("fi-tr-send-back.svg");
            public static string SendBackward => Require("fi-tr-send-backward.svg");
        }

        // Carets / Small nav
        public static class Carets
        {
            public static string Up => Require("fi-tr-caret-up.svg");
            public static string Down => Require("fi-tr-caret-down.svg");
            public static string Left => Require("fi-tr-caret-left.svg");
            public static string Right => Require("fi-tr-caret-right.svg");
            public static string CircleUp => Require("fi-tr-caret-circle-up.svg");
            public static string CircleRight => Require("fi-tr-caret-circle-right.svg");
            public static string CircleDown => Require("fi-tr-caret-circle-down.svg");
            public static string SquareLeft => Require("fi-tr-caret-square-left.svg");
            public static string SquareRight => Require("fi-tr-caret-square-right.svg");
            public static string SquareUp => Require("fi-tr-caret-square-up.svg");
            public static string SquareDown => Require("fi-tr-caret-square-down.svg");
        }

        // AI / IoT
        public static class AIoT
        {
            public static string AIAlgorithm => Require("fi-tr-ai-algorithm.svg");
            public static string ArtificialIntelligence => Require("fi-tr-artificial-intelligence.svg");
            public static string Chatbot => Require("fi-tr-chatbot.svg");
            public static string ChipBrain => Require("fi-tr-chip-brain.svg");
            public static string MicrochipAI => Require("fi-tr-microchip-ai.svg");
            public static string HeadSideBrain => Require("fi-tr-head-side-brain.svg");
        }

        // Map Advanced
        public static class MapAdvanced
        {
            public static string LocationTrack => Require("fi-tr-map-location-track.svg");
            public static string MarkerCheck => Require("fi-tr-map-marker-check.svg");
            public static string MarkerEdit => Require("fi-tr-map-marker-edit.svg");
            public static string MarkerQuestion => Require("fi-tr-map-marker-question.svg");
            public static string MarkerSlash => Require("fi-tr-map-marker-slash.svg");
            public static string MarkerSmile => Require("fi-tr-map-marker-smile.svg");
            public static string MapPoint => Require("fi-tr-map-point.svg");
        }

        // Files Advanced
        public static class FilesAdvanced
        {
            public static string Signature => Require("fi-tr-file-signature.svg");
            public static string Loop => Require("fi-tr-file-loop.svg");
            public static string Recycle => Require("fi-tr-file-recycle.svg");
            public static string User => Require("fi-tr-file-user.svg");
            public static string Medical => Require("fi-tr-file-medical.svg");
            public static string Invoice => Require("fi-tr-file-invoice.svg");
            public static string InvoiceDollar => Require("fi-tr-file-invoice-dollar.svg");
        }

        // Finance extended
        public static class FinanceEx
        {
            public static string PaymentPos => Require("fi-tr-payment-pos.svg");
            public static string PaymentQrcode => Require("fi-tr-payment-qrcode.svg");
            public static string MoneyBillTransfer => Require("fi-tr-money-bill-transfer.svg");
            public static string MoneyBills => Require("fi-tr-money-bills.svg");
            public static string MoneyBillsSimple => Require("fi-tr-money-bills-simple.svg");
            public static string MoneyCheck => Require("fi-tr-money-check.svg");
            public static string MoneyCheckEdit => Require("fi-tr-money-check-edit.svg");
            public static string MoneyCheckEditAlt => Require("fi-tr-money-check-edit-alt.svg");
        }

        // Mailing
        public static class Mailing
        {
            public static string EnvelopeDownload => Require("fi-tr-envelope-download.svg");
            public static string EnvelopePlus => Require("fi-tr-envelope-plus.svg");
            public static string EnvelopeOpen => Require("fi-tr-envelope-open.svg");
            public static string EnvelopeOpenText => Require("fi-tr-envelope-open-text.svg");
            public static string EnvelopeOpenDollar => Require("fi-tr-envelope-open-dollar.svg");
            public static string EnvelopeHeart => Require("fi-tr-envelope-heart.svg");
            public static string EnvelopeDot => Require("fi-tr-envelope-dot.svg");
            public static string EnvelopeBan => Require("fi-tr-envelope-ban.svg");
            public static string Envelopes => Require("fi-tr-envelopes.svg");
        }

        // Media extended
        public static class MediaEx
        {
            public static string PhotoVideo => Require("fi-tr-photo-video.svg");
            public static string PhotoCapture => Require("fi-tr-photo-capture.svg");
            public static string PhotoFilmMusic => Require("fi-tr-photo-film-music.svg");
            public static string CameraSlash => Require("fi-tr-camera-slash.svg");
            public static string CameraRetro => Require("fi-tr-camera-retro.svg");
        }

        // Logistics
        public static class Logistics
        {
            public static string Package => Require("fi-tr-package.svg");
            public static string BoxOpen => Require("fi-tr-box-open.svg");
            public static string BoxOpenFull => Require("fi-tr-box-open-full.svg");
            public static string BoxCheck => Require("fi-tr-box-check.svg");
            public static string BoxDollar => Require("fi-tr-box-dollar.svg");
            public static string Boxes => Require("fi-tr-boxes.svg");
            public static string BoxUp => Require("fi-tr-box-up.svg");
            public static string ParachuteBox => Require("fi-tr-parachute-box.svg");
        }

        // Weather
        public static class Weather
        {
            public static string CloudSun => Require("fi-tr-cloud-sun.svg");
            public static string CloudMoon => Require("fi-tr-cloud-moon.svg");
            public static string CloudRain => Require("fi-tr-cloud-rain.svg");
            public static string CloudShowersHeavy => Require("fi-tr-cloud-showers-heavy.svg");
            public static string CloudShowers => Require("fi-tr-cloud-showers.svg");
            public static string CloudHail => Require("fi-tr-cloud-hail.svg");
            public static string CloudSnow => Require("fi-tr-cloud-snow.svg");
            public static string CloudDrizzle => Require("fi-tr-cloud-drizzle.svg");
            public static string CloudRainbow => Require("fi-tr-cloud-rainbow.svg");
            public static string Fog => Require("fi-tr-fog.svg");
        }

        // Data Operations
        public static class DataOps
        {
            public static string Backup => Require("fi-tr-data-backup.svg");
            public static string Cleaning => Require("fi-tr-data-cleaning.svg");
            public static string Migration => Require("fi-tr-data-migration.svg");
            public static string Mining => Require("fi-tr-data-mining.svg");
            public static string Model => Require("fi-tr-data-model.svg");
            public static string Processing => Require("fi-tr-data-processing.svg");
            public static string DatabaseManagement => Require("fi-tr-database-management.svg");
            public static string DatabaseCleaning => Require("fi-tr-database-cleaning.svg");
        }

        // Devices IoT / NFC
        public static class DevicesIoT
        {
            public static string Iot => Require("fi-tr-iot.svg");
            public static string IotAlt => Require("fi-tr-iot-alt.svg");
            public static string Nfc => Require("fi-tr-nfc.svg");
            public static string NfcLock => Require("fi-tr-nfc-lock.svg");
            public static string NfcPen => Require("fi-tr-nfc-pen.svg");
            public static string NfcSlash => Require("fi-tr-nfc-slash.svg");
            public static string NfcTrash => Require("fi-tr-nfc-trash.svg");
        }

        // System / Display
        public static class System
        {
            public static string DisplaySlash => Require("fi-tr-display-slash.svg");
            public static string DisplayCode => Require("fi-tr-display-code.svg");
            public static string DisplayMedical => Require("fi-tr-display-medical.svg");
            public static string DisplayArrowDown => Require("fi-tr-display-arrow-down.svg");
        }

        // Legal / Law
        public static class Legal
        {
            public static string LawBook => Require("fi-tr-law-book.svg");
            public static string LawyerMan => Require("fi-tr-lawyer-man.svg");
            public static string LawyerWoman => Require("fi-tr-lawyer-woman.svg");
            public static string CourtOrder => Require("fi-tr-court-order.svg");
            public static string LegalCase => Require("fi-tr-legal-case.svg");
            public static string LegalDocument => Require("fi-tr-legal-document.svg");
            public static string LegalFees => Require("fi-tr-legal-fees.svg");
        }

        // Education
        public static class Education
        {
            public static string GraduationCap => Require("fi-tr-graduation-cap.svg");
            public static string Diploma => Require("fi-tr-diploma.svg");
            public static string Chalkboard => Require("fi-tr-chalkboard.svg");
            public static string School => Require("fi-tr-school.svg");
            public static string Student => Require("fi-tr-student.svg");
            public static string StudentAlt => Require("fi-tr-student-alt.svg");
            public static string Lesson => Require("fi-tr-lesson.svg");
        }

        // Human Resources
        public static class HR
        {
            public static string EmployeeMan => Require("fi-tr-employee-man.svg");
            public static string Employees => Require("fi-tr-employees.svg");
            public static string HRManagement => Require("fi-tr-hr-management.svg");
            public static string HRGroup => Require("fi-tr-hr-group.svg");
            public static string HRPerson => Require("fi-tr-hr-person.svg");
            public static string Department => Require("fi-tr-department.svg");
            public static string DepartmentStructure => Require("fi-tr-department-structure.svg");
        }

        // Food & Drink
        public static class FoodDrink
        {
            public static string Coffee => Require("fi-tr-coffee.svg");
            public static string PizzaSlice => Require("fi-tr-pizza-slice.svg");
            public static string Hamburger => Require("fi-tr-hamburger.svg");
            public static string Hotdog => Require("fi-tr-hotdog.svg");
            public static string IceCream => Require("fi-tr-ice-cream.svg");
            public static string Sushi => Require("fi-tr-sushi.svg");
            public static string Donut => Require("fi-tr-donut.svg");
            public static string Beer => Require("fi-tr-beer.svg");
        }

        // Gaming
        public static class Gaming
        {
            public static string Gamepad => Require("fi-tr-gamepad.svg");
            public static string Joystick => Require("fi-tr-joystick.svg");
            public static string Dice => Require("fi-tr-dice.svg");
            public static string Chess => Require("fi-tr-chess.svg");
            public static string ChessKnight => Require("fi-tr-chess-knight.svg");
        }

        // Power / Energy
        public static class Power
        {
            public static string BatteryFull => Require("fi-tr-battery-full.svg");
            public static string BatteryHalf => Require("fi-tr-battery-half.svg");
            public static string BatteryEmpty => Require("fi-tr-battery-empty.svg");
            public static string Bolt => Require("fi-tr-bolt.svg");
            public static string Plug => Require("fi-tr-plug.svg");
            public static string Powerbank => Require("fi-tr-powerbank.svg");
        }

        // Connectivity
        public static class Connectivity
        {
            public static string Bluetooth => Require("fi-tr-bluetooth.svg");
            public static string BluetoothAlt => Require("fi-tr-bluetooth-alt.svg");
            public static string Wifi => Require("fi-tr-wifi.svg");
            public static string Ethernet => Require("fi-tr-ethernet.svg");
            public static string RouterWifi => Require("fi-tr-router-wifi.svg");
        }

        // Accessibility
        public static class Accessibility
        {
            public static string AssistiveListening => Require("fi-tr-assistive-listening-systems.svg");
            public static string LowVision => Require("fi-tr-low-vision.svg");
            public static string Braille => Require("fi-tr-braille.svg");
            public static string Wheelchair => Require("fi-tr-wheelchair.svg");
            public static string UniversalAccess => Require("fi-tr-universal-access.svg");
        }

        // Currency
        public static class Currency
        {
            public static string Dollar => Require("fi-tr-dollar.svg");
            public static string Euro => Require("fi-tr-euro.svg");
            public static string Pound => Require("fi-tr-pound.svg");
            public static string Yen => Require("fi-tr-yen.svg");
            public static string Rupee => Require("fi-tr-rupee-sign.svg");
            public static string Bitcoin => Require("fi-tr-bitcoin-sign.svg");
        }

        // Badges
        public static class Badges
        {
            public static string Check => Require("fi-tr-badge-check.svg");
            public static string Dollar => Require("fi-tr-badge-dollar.svg");
            public static string Percent => Require("fi-tr-badge-percent.svg");
        }

        // Shapes
        public static class Shapes
        {
            public static string Hexagon => Require("fi-tr-hexagon.svg");
            public static string Diamond => Require("fi-tr-diamond.svg");
            public static string Triangle => Require("fi-tr-triangle.svg");
            public static string Square => Require("fi-tr-square.svg");
            public static string Circle => Require("fi-tr-circle.svg");
            public static string Octagon => Require("fi-tr-octagon.svg");
            public static string Rhombus => Require("fi-tr-rhombus.svg");
        }

        // Compass
        public static class Compass
        {
            public static string CompassAlt => Require("fi-tr-compass-alt.svg");
            public static string East => Require("fi-tr-compass-east.svg");
            public static string West => Require("fi-tr-compass-west.svg");
            public static string North => Require("fi-tr-compass-north.svg");
            public static string South => Require("fi-tr-compass-south.svg");
        }

        // Music & Audio
        public static class MusicAudio
        {
            public static string Guitar => Require("fi-tr-guitar.svg");
            public static string Drum => Require("fi-tr-drum.svg");
            public static string Microphone => Require("fi-tr-microphone-alt.svg");
            public static string Violin => Require("fi-tr-violin.svg");
            public static string Piano => Require("fi-tr-piano.svg");
        }

        // Travel & Leisure
        public static class Travel
        {
            public static string PlaneDeparture => Require("fi-tr-plane-departure.svg");
            public static string PlaneArrival => Require("fi-tr-plane-arrival.svg");
            public static string Passport => Require("fi-tr-passport.svg");
            public static string Hotel => Require("fi-tr-hotel.svg");
            public static string Ticket => Require("fi-tr-ticket.svg");
            public static string UmbrellaBeach => Require("fi-tr-umbrella-beach.svg");
            public static string LuggageCart => Require("fi-tr-luggage-cart.svg");
        }

        // Smart Home / IoT
        public static class SmartHome
        {
            public static string SmartHomeMain => Require("fi-tr-smart-home.svg");
            public static string SmartHomeKey => Require("fi-tr-smart-home-key.svg");
            public static string SmartHomeCloud => Require("fi-tr-smart-home-cloud.svg");
            public static string SmartDoor => Require("fi-tr-smart-wireless-door.svg");
            public static string LightSwitch => Require("fi-tr-light-switch.svg");
            public static string LightbulbOn => Require("fi-tr-lightbulb-on.svg");
        }

        // Industry / Manufacturing
        public static class Industry
        {
            public static string IndustryAlt => Require("fi-tr-industry-alt.svg");
            public static string IndustryWindows => Require("fi-tr-industry-windows.svg");
            public static string ConstructionHelmet => Require("fi-tr-construction-helmet.svg");
            public static string ConveyorBelt => Require("fi-tr-conveyor-belt.svg");
            public static string Crane => Require("fi-tr-crane.svg");
            public static string Welding => Require("fi-tr-welding.svg");
        }

        // Agriculture / Nature
        public static class Agriculture
        {
            public static string Seedling => Require("fi-tr-seedling.svg");
            public static string Tractor => Require("fi-tr-tractor.svg");
            public static string WateringCanPlant => Require("fi-tr-watering-can-plant.svg");
            public static string PlantCare => Require("fi-tr-plant-care.svg");
            public static string Wheat => Require("fi-tr-wheat.svg");
            public static string Tree => Require("fi-tr-tree.svg");
        }

        // Banking / Payments
        public static class Banking
        {
            public static string Bank => Require("fi-tr-bank.svg");
            public static string Vault => Require("fi-tr-vault.svg");
            public static string Atm => Require("fi-tr-atm.svg");
            public static string SafeBox => Require("fi-tr-money-safe-box.svg");
            public static string WalletMoney => Require("fi-tr-wallet-money.svg");
            public static string Receipt => Require("fi-tr-receipt.svg");
        }

        // Sports
        public static class Sports
        {
            public static string Football => Require("fi-tr-football.svg");
            public static string Basketball => Require("fi-tr-basketball.svg");
            public static string Tennis => Require("fi-tr-tennis.svg");
            public static string Volleyball => Require("fi-tr-volleyball.svg");
            public static string Golf => Require("fi-tr-golf.svg");
            public static string Baseball => Require("fi-tr-baseball.svg");
            public static string SwimmingPool => Require("fi-tr-swimming-pool.svg");
        }

        // Layout extra
        public static class LayoutEx
        {
            public static string SplitScreen => Require("fi-tr-split-screen.svg");
            public static string Sidebar => Require("fi-tr-sidebar.svg");
            public static string SidebarFlip => Require("fi-tr-sidebar-flip.svg");
            public static string BorderAll => Require("fi-tr-border-all.svg");
            public static string Columns3 => Require("fi-tr-columns-3.svg");
            public static string TableList => Require("fi-tr-table-list.svg");
        }

        // Audio / Video
        public static class AV
        {
            public static string VideoCamera => Require("fi-tr-video-camera.svg");
            public static string VideoPlus => Require("fi-tr-video-plus.svg");
            public static string VideoSlash => Require("fi-tr-video-slash.svg");
            public static string Clapperboard => Require("fi-tr-clapperboard.svg");
            public static string ScreenShare => Require("fi-tr-screen-share.svg");
            public static string ScreenRecorder => Require("fi-tr-screen-recorder.svg");
            public static string ScreenPlay => Require("fi-tr-screen-play.svg");
        }

        // Devices / Hardware
        public static class DevicesHw
        {
            public static string UsbPendrive => Require("fi-tr-usb-pendrive.svg");
            public static string SdCard => Require("fi-tr-sd-card.svg");
            public static string Webcam => Require("fi-tr-webcam.svg");
            public static string Keyboard => Require("fi-tr-keyboard.svg");
            public static string Mouse => Require("fi-tr-mouse.svg");
            public static string Headphones => Require("fi-tr-headphones-rhythm.svg");
        }

        // Time
        public static class Time
        {
            public static string AlarmClock => Require("fi-tr-alarm-clock.svg");
            public static string Stopwatch => Require("fi-tr-stopwatch.svg");
            public static string Hourglass => Require("fi-tr-hourglass.svg");
            public static string TimeCheck => Require("fi-tr-time-check.svg");
            public static string TimeAdd => Require("fi-tr-time-add.svg");
        }

        // Emoji / Faces
        public static class Emoji
        {
            public static string Smile => Require("fi-tr-smile.svg");
            public static string Laugh => Require("fi-tr-laugh.svg");
            public static string Frown => Require("fi-tr-frown.svg");
            public static string Angry => Require("fi-tr-angry.svg");
            public static string Meh => Require("fi-tr-meh.svg");
            public static string Surprise => Require("fi-tr-surprise.svg");
        }

        // Business Operations
        public static class BusinessOps
        {
            public static string Dashboard => Require("fi-tr-dashboard.svg");
            public static string KPI => Require("fi-tr-kpi.svg");
            public static string KPIEvaluation => Require("fi-tr-kpi-evaluation.svg");
            public static string Lead => Require("fi-tr-lead.svg");
            public static string LeadManagement => Require("fi-tr-lead-management.svg");
            public static string Process => Require("fi-tr-process.svg");
            public static string Procedures => Require("fi-tr-procedures.svg");
            public static string Productivity => Require("fi-tr-productivity.svg");
            public static string Strategy => Require("fi-tr-strategy-chess-risk.svg");
            public static string Meeting => Require("fi-tr-meeting.svg");
            public static string Goals => Require("fi-tr-goals.svg");
            public static string Target => Require("fi-tr-target.svg");
            public static string Budget => Require("fi-tr-budget.svg");
            public static string Expense => Require("fi-tr-expense.svg");
            public static string Revenue => Require("fi-tr-revenue-alt.svg");
        }

        // Science
        public static class Science
        {
            public static string Microscope => Require("fi-tr-microscope.svg");
            public static string TestTube => Require("fi-tr-test-tube.svg");
            public static string Dna => Require("fi-tr-dna.svg");
            public static string Flask => Require("fi-tr-flask.svg");
            public static string Telescope => Require("fi-tr-telescope.svg");
            public static string Physics => Require("fi-tr-physics.svg");
        }

        // Mathematics
        public static class Math
        {
            public static string Pi => Require("fi-tr-pi.svg");
            public static string Sigma => Require("fi-tr-sigma.svg");
            public static string Integral => Require("fi-tr-integral.svg");
            public static string Function => Require("fi-tr-function.svg");
            public static string SquareRoot => Require("fi-tr-square-root.svg");
            public static string LessThan => Require("fi-tr-less-than.svg");
            public static string GreaterThan => Require("fi-tr-greater-than.svg");
            public static string NotEqual => Require("fi-tr-not-equal.svg");
            public static string Divide => Require("fi-tr-divide.svg");
            public static string PlusMinus => Require("fi-tr-plus-minus.svg");
        }

        // Internationalization / Languages
        public static class I18n
        {
            public static string Translate => Require("fi-tr-translate.svg");
            public static string Language => Require("fi-tr-language.svg");
            public static string LanguageExchange => Require("fi-tr-language-exchange.svg");
            public static string English => Require("fi-tr-english.svg");
            public static string German => Require("fi-tr-german.svg");
            public static string Spanish => Require("fi-tr-spanish.svg");
            public static string French => Require("fi-tr-french.svg");
            public static string Japanese => Require("fi-tr-japanese.svg");
            public static string Portuguese => Require("fi-tr-portuguese.svg");
            public static string UnitedKingdom => Require("fi-tr-united-kingdom.svg");
        }

        // Social
        public static class Social
        {
            public static string Followers => Require("fi-tr-followers.svg");
            public static string Follow => Require("fi-tr-follow.svg");
            public static string FollowFolder => Require("fi-tr-follow-folder.svg");
            public static string ThumbsUpTrust => Require("fi-tr-thumbs-up-trust.svg");
            public static string SocialNetwork => Require("fi-tr-social-network.svg");
            public static string SocialNotification => Require("fi-tr-social-media-notification.svg");
        }

        // Files Operations
        public static class FilesOps
        {
            public static string MoveToFolder => Require("fi-tr-move-to-folder.svg");
            public static string MoveToFolder2 => Require("fi-tr-move-to-folder-2.svg");
            public static string Subfolder => Require("fi-tr-subfolder.svg");
            public static string RemoveFolder => Require("fi-tr-remove-folder.svg");
            public static string PresentationFolder => Require("fi-tr-presentation-folder.svg");
        }

        // Sensors / IoT
        public static class Sensors
        {
            public static string Sensor => Require("fi-tr-sensor.svg");
            public static string SensorAlert => Require("fi-tr-sensor-alert.svg");
            public static string SensorSmoke => Require("fi-tr-sensor-smoke.svg");
            public static string SensorFire => Require("fi-tr-sensor-fire.svg");
            public static string SensorOn => Require("fi-tr-sensor-on.svg");
        }

        // Transport extended
        public static class TransportEx
        {
            public static string TrainStation => Require("fi-tr-train-station.svg");
            public static string Tram => Require("fi-tr-tram.svg");
            public static string SubwayTunnel => Require("fi-tr-train-subway-tunnel.svg");
            public static string TaxiBus => Require("fi-tr-taxi-bus.svg");
            public static string Sailboat => Require("fi-tr-sailboat.svg");
            public static string Scooter => Require("fi-tr-scooter.svg");
        }

        // Alerts / Safety
        public static class AlertsEx
        {
            public static string SealExclamation => Require("fi-tr-seal-exclamation.svg");
            public static string SealQuestion => Require("fi-tr-seal-question.svg");
            public static string OctagonExclamation => Require("fi-tr-octagon-exclamation.svg");
            public static string DiamondExclamation => Require("fi-tr-diamond-exclamation.svg");
            public static string Thunderstorm => Require("fi-tr-thunderstorm.svg");
        }

        // Nature
        public static class Nature
        {
            public static string Leaf => Require("fi-tr-leaf.svg");
            public static string LeafMaple => Require("fi-tr-leaf-maple.svg");
            public static string LeafOak => Require("fi-tr-leaf-oak.svg");
            public static string Flower => Require("fi-tr-flower.svg");
            public static string FlowerTulip => Require("fi-tr-flower-tulip.svg");
            public static string TreeDeciduous => Require("fi-tr-tree-deciduous.svg");
            public static string Forest => Require("fi-tr-forest.svg");
            public static string Mountain => Require("fi-tr-mountain.svg");
        }

        // Animals
        public static class Animals
        {
            public static string Dog => Require("fi-tr-dog.svg");
            public static string Cat => Require("fi-tr-cat.svg");
            public static string Bird => Require("fi-tr-bird.svg");
            public static string Fish => Require("fi-tr-fish.svg");
            public static string Horse => Require("fi-tr-horse.svg");
            public static string Rabbit => Require("fi-tr-rabbit.svg");
            public static string Monkey => Require("fi-tr-monkey.svg");
            public static string Fox => Require("fi-tr-fox.svg");
            public static string Cow => Require("fi-tr-cow.svg");
        }

        // File types and formats
        public static class FileTypes
        {
            public static string Word => Require("fi-tr-file-word.svg");
            public static string PowerPoint => Require("fi-tr-file-powerpoint.svg");
            public static string Csv => Require("fi-tr-file-csv.svg");
            public static string Xls => Require("fi-tr-file-xls.svg");
            public static string Pdf => Require("fi-tr-file-pdf.svg");
            public static string Video => Require("fi-tr-file-video.svg");
            public static string Audio => Require("fi-tr-file-audio.svg");
            public static string Code => Require("fi-tr-file-code.svg");
            public static string Image => Require("fi-tr-file-image.svg");
            public static string Zip => Require("fi-tr-file-zipper.svg");
            public static string Html => Require("fi-tr-html-file.svg");
            public static string Json => Require("fi-tr-json-file.svg");
            public static string Sql => Require("fi-tr-sql-file.svg");
            public static string Ppt => Require("fi-tr-ppt-file.svg");
            public static string Mp3 => Require("fi-tr-mp3-file.svg");
            public static string Mp4 => Require("fi-tr-mp4-file.svg");
            public static string Gltf => Require("fi-tr-gltf-file.svg");
            public static string Fbx => Require("fi-tr-fbx-file.svg");
            public static string Iso => Require("fi-tr-iso-file.svg");
            public static string Png => Require("fi-tr-png-file.svg");
            public static string Jpg => Require("fi-tr-jpg.svg");
        }

        // Payments & Payroll operations
        public static class PaymentOps
        {
            public static string Payroll => Require("fi-tr-payroll.svg");
            public static string PayrollCheck => Require("fi-tr-payroll-check.svg");
            public static string PayrollCalendar => Require("fi-tr-payroll-calendar.svg");
            public static string SendMoney => Require("fi-tr-send-money.svg");
            public static string SendMoneySmartphone => Require("fi-tr-send-money-smartphone.svg");
            public static string Deposit => Require("fi-tr-deposit.svg");
            public static string DepositAlt => Require("fi-tr-deposit-alt.svg");
            public static string ExpenseBill => Require("fi-tr-expense-bill.svg");
        }

        // Human Resources operations
        public static class HROps
        {
            public static string OnlineInterview => Require("fi-tr-online-interview.svg");
            public static string Onboarding => Require("fi-tr-onboarding.svg");
            public static string Resume => Require("fi-tr-resume.svg");
            public static string CV => Require("fi-tr-CV.svg");
            public static string MemberList => Require("fi-tr-member-list.svg");
            public static string MemberSearch => Require("fi-tr-member-search.svg");
            public static string Invite => Require("fi-tr-invite.svg");
            public static string InviteAlt => Require("fi-tr-invite-alt.svg");
        }

        // Development (API / Web / UI)
        public static class Development
        {
            public static string Api => Require("fi-tr-api.svg");
            public static string CodeWindow => Require("fi-tr-code-window.svg");
            public static string Webhook => Require("fi-tr-webhook.svg");
            public static string Web => Require("fi-tr-web.svg");
            public static string WebDesign => Require("fi-tr-web-design.svg");
            public static string UIUX => Require("fi-tr-ui-ux.svg");
            public static string UX => Require("fi-tr-ux.svg");
        }

        // Calendar operations & variants
        public static class CalendarOps
        {
            public static string Clock => Require("fi-tr-calendar-clock.svg");
            public static string Day => Require("fi-tr-calendar-day.svg");
            public static string Lines => Require("fi-tr-calendar-lines.svg");
            public static string LinesPen => Require("fi-tr-calendar-lines-pen.svg");
            public static string Pen => Require("fi-tr-calendar-pen.svg");
            public static string PaymentLoan => Require("fi-tr-calendar-payment-loan.svg");
            public static string Salary => Require("fi-tr-calendar-salary.svg");
            public static string ShiftSwap => Require("fi-tr-calendar-shift-swap.svg");
            public static string Swap => Require("fi-tr-calendar-swap.svg");
            public static string Exclamation => Require("fi-tr-calendar-exclamation.svg");
            public static string GavelLegal => Require("fi-tr-calendar-gavel-legal.svg");
            public static string Image => Require("fi-tr-calendar-image.svg");
        }

        // UI Controls / Inputs
        public static class UIControls
        {
            public static string Dropdown => Require("fi-tr-dropdown.svg");
            public static string DropdownSelect => Require("fi-tr-dropdown-select.svg");
            public static string DropdownBar => Require("fi-tr-dropdown-bar.svg");
            public static string InputText => Require("fi-tr-input-text.svg");
            public static string InputNumeric => Require("fi-tr-input-numeric.svg");
            public static string InputPipe => Require("fi-tr-input-pipe.svg");
            public static string InsertButtonCircle => Require("fi-tr-insert-button-circle.svg");
        }

        // Streaming / Live media
        public static class StreamingMedia
        {
            public static string Live => Require("fi-tr-live.svg");
            public static string LiveAlt => Require("fi-tr-live-alt.svg");
            public static string Podcast => Require("fi-tr-podcast.svg");
            public static string Webinar => Require("fi-tr-webinar.svg");
            public static string WebinarPlay => Require("fi-tr-webinar-play.svg");
            public static string SignalStream => Require("fi-tr-signal-stream.svg");
        }

        // Analytics extended
        public static class AnalyticsEx
        {
            public static string ChartGantt => Require("fi-tr-chart-gantt.svg");
            public static string ChartKanban => Require("fi-tr-chart-kanban.svg");
            public static string ChartScatter => Require("fi-tr-chart-scatter.svg");
            public static string ChartScatter3D => Require("fi-tr-chart-scatter-3d.svg");
            public static string ChartScatterBubble => Require("fi-tr-chart-scatter-bubble.svg");
            public static string ChartPyramid => Require("fi-tr-chart-pyramid.svg");
            public static string ChartTreeMap => Require("fi-tr-chart-tree-map.svg");
            public static string ChartWaterfall => Require("fi-tr-chart-waterfall.svg");
            public static string ChartConnected => Require("fi-tr-chart-connected.svg");
            public static string ChartCandlestick => Require("fi-tr-chart-candlestick.svg");
            public static string ChartPieAlt => Require("fi-tr-chart-pie-alt.svg");
            public static string ChartSimpleHorizontal => Require("fi-tr-chart-simple-horizontal.svg");
            public static string ChartMixed => Require("fi-tr-chart-mixed.svg");
            public static string ChartNetwork => Require("fi-tr-chart-network.svg");
        }

        // Media Editing / Drawing tools
        public static class MediaEditing
        {
            public static string DrawPolygon => Require("fi-tr-draw-polygon.svg");
            public static string DrawSquare => Require("fi-tr-draw-square.svg");
            public static string ReflectHorizontal => Require("fi-tr-reflect-horizontal.svg");
            public static string ReflectHorizontalAlt => Require("fi-tr-reflect-horizontal-alt.svg");
            public static string ReflectVertical => Require("fi-tr-reflect-vertical.svg");
            public static string Protractor => Require("fi-tr-protractor.svg");
            public static string RulerCombined => Require("fi-tr-ruler-combined.svg");
            public static string RulerHorizontal => Require("fi-tr-ruler-horizontal.svg");
            public static string RulerVertical => Require("fi-tr-ruler-vertical.svg");
            public static string PencilRuler => Require("fi-tr-pencil-ruler.svg");
            public static string PencilPaintbrush => Require("fi-tr-pencil-paintbrush.svg");
            public static string PenNib => Require("fi-tr-pen-nib.svg");
            public static string PenSquare => Require("fi-tr-pen-square.svg");
            public static string PenSwirl => Require("fi-tr-pen-swirl.svg");
        }

        // Shipping / Logistics operations
        public static class ShippingOps
        {
            public static string ShippingFast => Require("fi-tr-shipping-fast.svg");
            public static string ShippingTimed => Require("fi-tr-shipping-timed.svg");
            public static string TruckBox => Require("fi-tr-truck-box.svg");
            public static string TruckContainer => Require("fi-tr-truck-container.svg");
            public static string TruckPickup => Require("fi-tr-truck-pickup.svg");
            public static string TruckMoving => Require("fi-tr-truck-moving.svg");
            public static string DeliveryMan => Require("fi-tr-delivery-man.svg");
        }

        // Commerce / Marketplace operations
        public static class CommerceOps
        {
            public static string Refund => Require("fi-tr-refund.svg");
            public static string RefundAlt => Require("fi-tr-refund-alt.svg");
            public static string Sell => Require("fi-tr-sell.svg");
            public static string Seller => Require("fi-tr-seller.svg");
            public static string SellerStore => Require("fi-tr-seller-store.svg");
            public static string Marketplace => Require("fi-tr-marketplace.svg");
            public static string MarketplaceStore => Require("fi-tr-marketplace-store.svg");
            public static string OrderHistory => Require("fi-tr-order-history.svg");
            public static string Basket => Require("fi-tr-shopping-basket.svg");
            public static string QuickBox => Require("fi-tr-quick-box.svg");
        }

        // Documents workflow operations
        public static class DocumentsOps
        {
            public static string DocumentSigned => Require("fi-tr-document-signed.svg");
            public static string DocumentPaid => Require("fi-tr-document-paid.svg");
            public static string ConvertDocument => Require("fi-tr-convert-document.svg");
            public static string DocumentGear => Require("fi-tr-document-gear.svg");
            public static string DocumentLink => Require("fi-tr-document-link.svg");
            public static string DocumentFolderGear => Require("fi-tr-document-folder-gear.svg");
            public static string DocumentGavel => Require("fi-tr-document-gavel.svg");
            public static string DocumentAutosave => Require("fi-tr-document-autosave.svg");
        }

        // Security / Privacy extended
        public static class SecurityEx
        {
            public static string InternetSecurity => Require("fi-tr-internet-security.svg");
            public static string PrivacySettings => Require("fi-tr-privacy-settings.svg");
            public static string NetworkFirewall => Require("fi-tr-network-firewall.svg");
            public static string RouterLock => Require("fi-tr-router-lock.svg");
            public static string VpnShield => Require("fi-tr-vpn-shield.svg");
            public static string LockHashtag => Require("fi-tr-lock-hashtag.svg");
            public static string CookieLock => Require("fi-tr-cookie-lock.svg");
        }

        // Routing / Roads
        public static class RoutingEx
        {
            public static string Route => Require("fi-tr-route.svg");
            public static string RouteHighway => Require("fi-tr-route-highway.svg");
            public static string RouteInterstate => Require("fi-tr-route-interstate.svg");
            public static string RoadBarrier => Require("fi-tr-road-barrier.svg");
            public static string RoadBridge => Require("fi-tr-road-bridge.svg");
            public static string RoadCheck => Require("fi-tr-road-check.svg");
            public static string RoadLock => Require("fi-tr-road-lock.svg");
            public static string RoadMapPin => Require("fi-tr-road-map-pin.svg");
        }

        // Weather Alerts / Severe
        public static class WeatherAlerts
        {
            public static string Hurricane => Require("fi-tr-hurricane.svg");
            public static string Flooding => Require("fi-tr-flooding.svg");
            public static string Wildfire => Require("fi-tr-wildfire.svg");
            public static string WindWarning => Require("fi-tr-wind-warning.svg");
            public static string ThunderstormRisk => Require("fi-tr-thunderstorm-risk.svg");
            public static string ThunderstormMoon => Require("fi-tr-thunderstorm-moon.svg");
            public static string ThunderstormSun => Require("fi-tr-thunderstorm-sun.svg");
        }

        // Communication extended
        public static class CommunicationEx
        {
            public static string CallIncoming => Require("fi-tr-call-incoming.svg");
            public static string CallOutgoing => Require("fi-tr-call-outgoing.svg");
            public static string CallMissed => Require("fi-tr-call-missed.svg");
            public static string CallHistory => Require("fi-tr-call-history.svg");
            public static string PhonePause => Require("fi-tr-phone-pause.svg");
            public static string PhonePlus => Require("fi-tr-phone-plus.svg");
            public static string PhoneSlash => Require("fi-tr-phone-slash.svg");
        }

        // System / Desktop extended
        public static class SystemEx
        {
            public static string OperatingSystemUpgrade => Require("fi-tr-operating-system-upgrade.svg");
            public static string SettingsWindow => Require("fi-tr-settings-window.svg");
            public static string DesktopWallpaper => Require("fi-tr-desktop-wallpaper.svg");
            public static string MonitorSun => Require("fi-tr-monitor-sun.svg");
            public static string MonitorWater => Require("fi-tr-monitor-water.svg");
        }

        // Devices / Storage
        public static class DevicesStorage
        {
            public static string ExternalHardDrive => Require("fi-tr-external-hard-drive.svg");
            public static string Hdd => Require("fi-tr-hdd.svg");
            public static string SdCards => Require("fi-tr-sd-cards.svg");
            public static string SimCards => Require("fi-tr-sim-cards.svg");
            public static string UsbWifi => Require("fi-tr-usb-wifi.svg");
            public static string UsbScan => Require("fi-tr-usb-scan.svg");
        }

        // Streaming / Video conferencing extended
        public static class StreamingEx
        {
            public static string VideoRecording => Require("fi-tr-video-recording.svg");
            public static string Videoconference => Require("fi-tr-videoconference.svg");
            public static string OnAirSquare => Require("fi-tr-on-air-square.svg");
        }

        // Machine Learning / AI
        public static class MachineLearning
        {
            public static string MachineLearningIcon => Require("fi-tr-machine-learning.svg");
            public static string DeepLearning => Require("fi-tr-deep-learning.svg");
            public static string Algorithm => Require("fi-tr-algorithm.svg");
            public static string EngineAlgorithm => Require("fi-tr-engine-algorithm.svg");
            public static string BigData => Require("fi-tr-big-data.svg");
            public static string BigDataAnalytics => Require("fi-tr-big-data-analytics.svg");
            public static string BigDataAI => Require("fi-tr-big-data-ai.svg");
        }

        // Data Pipelines / Queues
        public static class DataPipelines
        {
            public static string Pipeline => Require("fi-tr-pipeline.svg");
            public static string Pipelines => Require("fi-tr-pipelines.svg");
            public static string PipelineData => Require("fi-tr-pipeline-data.svg");
            public static string SourceData => Require("fi-tr-source-data.svg");
            public static string SourceDocument => Require("fi-tr-source-document.svg");
            public static string SourceDocumentAlt => Require("fi-tr-source-document-alt.svg");
            public static string Flowchart => Require("fi-tr-flowchart.svg");
            public static string Queue => Require("fi-tr-queue.svg");
            public static string QueueLine => Require("fi-tr-queue-line.svg");
            public static string QueueAlt => Require("fi-tr-queue-alt.svg");
        }

        // Validation / Status shapes
        public static class Validation
        {
            public static string HexagonCheck => Require("fi-tr-hexagon-check.svg");
            public static string OctagonXmark => Require("fi-tr-octagon-xmark.svg");
            public static string CircleExclamationCheck => Require("fi-tr-circle-exclamation-check.svg");
            public static string MinusCircle => Require("fi-tr-minus-circle.svg");
            public static string PlusHexagon => Require("fi-tr-plus-hexagon.svg");
            public static string OctagonExclamation => Require("fi-tr-octagon-exclamation.svg");
        }

        // Form Controls / Inputs
        public static class FormControls
        {
            public static string ListDropdown => Require("fi-tr-list-dropdown.svg");
            public static string SlidersVertical => Require("fi-tr-sliders-v.svg");
            public static string SlidersVerticalSquare => Require("fi-tr-sliders-v-square.svg");
            public static string SlidersHorizontalSquare => Require("fi-tr-sliders-h-square.svg");
            public static string PointerText => Require("fi-tr-pointer-text.svg");
            public static string ListCheck => Require("fi-tr-list-check.svg");
        }

        // Road Signs / Wayfinding
        public static class RoadSigns
        {
            public static string SignPosts => Require("fi-tr-sign-posts.svg");
            public static string SignPostsWrench => Require("fi-tr-sign-posts-wrench.svg");
            public static string Parking => Require("fi-tr-parking.svg");
            public static string ParkingCircle => Require("fi-tr-parking-circle.svg");
            public static string ParkingCircleSlash => Require("fi-tr-parking-circle-slash.svg");
            public static string ParkingSlash => Require("fi-tr-parking-slash.svg");
        }

        // Travel / Reservations / Planes
        public static class TravelOps
        {
            public static string PassengerPlane => Require("fi-tr-passenger-plane.svg");
            public static string PlaneCheck => Require("fi-tr-plane-check.svg");
            public static string PlaneLock => Require("fi-tr-plane-lock.svg");
            public static string PlaneClock => Require("fi-tr-plane-clock.svg");
            public static string ReservationSmartphone => Require("fi-tr-reservation-smartphone.svg");
            public static string ReservationTable => Require("fi-tr-reservation-table.svg");
        }

        // Presentation / Stage
        public static class Presentation
        {
            public static string PresentationScreen => Require("fi-tr-presentation.svg");
            public static string Projector => Require("fi-tr-projector.svg");
            public static string Podium => Require("fi-tr-podium.svg");
            public static string PodiumStar => Require("fi-tr-podium-star.svg");
            public static string PodiumAward => Require("fi-tr-podium-award.svg");
        }

        // Media & Audio extras
        public static class MediaAudioEx
        {
            public static string MicrophoneSlash => Require("fi-tr-microphone-slash.svg");
            public static string NoiseCancellingHeadphones => Require("fi-tr-noise-cancelling-headphones.svg");
            public static string GuitarElectric => Require("fi-tr-guitar-electric.svg");
            public static string AmpGuitar => Require("fi-tr-amp-guitar.svg");
            public static string DjDisc => Require("fi-tr-dj-disc.svg");
            public static string RecordVinyl => Require("fi-tr-record-vinyl.svg");
            public static string MusicNote => Require("fi-tr-music-note.svg");
            public static string MusicFile => Require("fi-tr-music-file.svg");
            public static string EarDeaf => Require("fi-tr-ear-deaf.svg");
            public static string EarSound => Require("fi-tr-ear-sound.svg");
        }

        // Office communications
        public static class OfficeComms
        {
            public static string PhoneOffice => Require("fi-tr-phone-office.svg");
            public static string Phone24h => Require("fi-tr-phone-24h.svg");
            public static string Fax => Require("fi-tr-fax.svg");
            public static string InboxIn => Require("fi-tr-inbox-in.svg");
            public static string InboxOut => Require("fi-tr-inbox-out.svg");
        }

        // Point of Sale / Payments devices
        public static class PaymentPOS
        {
            public static string PointOfSale => Require("fi-tr-point-of-sale.svg");
            public static string PointOfSaleSignal => Require("fi-tr-point-of-sale-signal.svg");
            public static string PointOfSaleBill => Require("fi-tr-point-of-sale-bill.svg");
        }

        // Diagramming
        public static class Diagramming
        {
            public static string DiagramProject => Require("fi-tr-diagram-project.svg");
            public static string DiagramSankey => Require("fi-tr-diagram-sankey.svg");
            public static string DiagramVenn => Require("fi-tr-diagram-venn.svg");
        }

        // Map routing and geo layers
        public static class MapRoutingEx
        {
            public static string RegionPin => Require("fi-tr-region-pin.svg");
            public static string RegionPinAlt => Require("fi-tr-region-pin-alt.svg");
            public static string LandLayers => Require("fi-tr-land-layers.svg");
            public static string LandLocation => Require("fi-tr-land-location.svg");
            public static string LandLayerLocation => Require("fi-tr-land-layer-location.svg");
            public static string RoadSignLeft => Require("fi-tr-road-sign-left.svg");
            public static string OrganizationChart => Require("fi-tr-organization-chart.svg");
        }

        // Storage and files operations
        public static class StorageOps
        {
            public static string FileBackup => Require("fi-tr-file-backup.svg");
            public static string FileCloud => Require("fi-tr-file-cloud.svg");
            public static string ExternalHardDrive => Require("fi-tr-external-hard-drive.svg");
            public static string LogFile => Require("fi-tr-log-file.svg");
            public static string FileZipAlt => Require("fi-tr-file-zip-alt.svg");
            public static string FileZipSave => Require("fi-tr-file-zip-save.svg");
        }

        // Compliance / Audit
        public static class ComplianceOps
        {
            public static string Compliance => Require("fi-tr-compliance.svg");
            public static string ComplianceClipboard => Require("fi-tr-compliance-clipboard.svg");
            public static string ComplianceDocument => Require("fi-tr-compliance-document.svg");
            public static string Audit => Require("fi-tr-audit.svg");
            public static string AuditAlt => Require("fi-tr-audit-alt.svg");
        }

        // Legal operations extended
        public static class LegalOpsExtended
        {
            public static string Gavel => Require("fi-tr-gavel.svg");
            public static string CourtOrder => Require("fi-tr-court-order.svg");
            public static string Notary => Require("fi-tr-notary.svg");
            public static string CivilLaw => Require("fi-tr-civil-law.svg");
            public static string LegalAid => Require("fi-tr-legal-aid.svg");
        }

        // Manufacturing / Construction
        public static class ManufacturingOps
        {
            public static string BuildingMaterials => Require("fi-tr-building-materials.svg");
            public static string ConveyorBeltAlt => Require("fi-tr-conveyor-belt-alt.svg");
            public static string ConveyorBeltArm => Require("fi-tr-conveyor-belt-arm.svg");
            public static string ConstructionLocation => Require("fi-tr-construction-location.svg");
            public static string CraneBuilding => Require("fi-tr-crane-building.svg");
            public static string BuildingFoundation => Require("fi-tr-building-foundation.svg");
        }

        // Networking / Connectivity extended
        public static class NetworkingEx
        {
            public static string IpAddress => Require("fi-tr-ip-address.svg");
            public static string Network => Require("fi-tr-network.svg");
            public static string NetworkUser => Require("fi-tr-network-user.svg");
            public static string NetworkAnalytic => Require("fi-tr-network-analytic.svg");
            public static string InternetSpeedWifi => Require("fi-tr-internet-speed-wifi.svg");
        }

        // Authentication / Credentials extended
        public static class AuthEx
        {
            public static string LoginLock => Require("fi-tr-login-lock.svg");
            public static string PasswordProtection => Require("fi-tr-password-protection.svg");
            public static string PasswordComputer => Require("fi-tr-password-computer.svg");
            public static string PasswordEmail => Require("fi-tr-password-email.svg");
            public static string PasswordSmartphone => Require("fi-tr-password-smartphone.svg");
            public static string DigitalSignature => Require("fi-tr-digital-signature.svg");
            public static string DigitalCertificate => Require("fi-tr-digital-certificate.svg");
        }

        // Dashboards / Monitoring
        public static class Dashboards
        {
            public static string Dashboard => Require("fi-tr-dashboard.svg");
            public static string DashboardMonitor => Require("fi-tr-dashboard-monitor.svg");
            public static string DashboardPanel => Require("fi-tr-dashboard-panel.svg");
        }

        // Media capture / cameras
        public static class MediaCaptureEx
        {
            public static string CameraMovie => Require("fi-tr-camera-movie.svg");
            public static string Camcorder => Require("fi-tr-camcorder.svg");
        }

        // Finance / payments operations
        public static class FinanceOps
        {
            public static string Loan => Require("fi-tr-loan.svg");
            public static string SalaryAlt => Require("fi-tr-salary-alt.svg");
            public static string Paid => Require("fi-tr-paid.svg");
            public static string PayPerClick => Require("fi-tr-pay-per-click.svg");
            public static string MoneyFromBracket => Require("fi-tr-money-from-bracket.svg");
            public static string MoneySimpleFromBracket => Require("fi-tr-money-simple-from-bracket.svg");
            public static string MoneyTransferAlt => Require("fi-tr-money-transfer-alt.svg");
            public static string MoneyTransferCoinArrow => Require("fi-tr-money-transfer-coin-arrow.svg");
            public static string MoneyTransferSmartphone => Require("fi-tr-money-transfer-smartphone.svg");
        }

        // Diagramming extended
        public static class DiagrammingEx
        {
            public static string DiagramNested => Require("fi-tr-diagram-nested.svg");
            public static string DiagramNext => Require("fi-tr-diagram-next.svg");
            public static string DiagramPrevious => Require("fi-tr-diagram-previous.svg");
            public static string DiagramPredecessor => Require("fi-tr-diagram-predecessor.svg");
            public static string DiagramSuccessor => Require("fi-tr-diagram-successor.svg");
            public static string DiagramCells => Require("fi-tr-diagram-cells.svg");
            public static string DiagramLeanCanvas => Require("fi-tr-diagram-lean-canvas.svg");
            public static string DiagramSubtask => Require("fi-tr-diagram-subtask.svg");
        }

        // Browser / UI
        public static class BrowserUi
        {
            public static string BrowserApp => Require("fi-tr-browser.svg");
            public static string BrowserCss => Require("fi-tr-browser-css.svg");
            public static string BrowserUIIcon => Require("fi-tr-browser-ui.svg");
        }

        // Customer Support / Service
        public static class CustomerSupport
        {
            public static string CustomerService => Require("fi-tr-customer-service.svg");
            public static string CustomerCare => Require("fi-tr-customer-care.svg");
        }
    }
}

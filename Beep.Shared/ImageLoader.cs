using System;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Resources;
using System.Xml.Linq;
using Svg;

using TheTechIdea.Beep.Vis.Modules;

namespace TheTechIdea.Beep.Shared
{
    public static class ImageLoader
    {
        /// <summary>
        /// Loads an SVG from a file path and returns the SvgDocument.
        /// </summary>
        public static SvgDocument LoadSvg(string svgPath)
        {
            if (string.IsNullOrEmpty(svgPath) || !File.Exists(svgPath))
                throw new FileNotFoundException($"SVG file not found: {svgPath}");

            try
            {
                return SvgDocument.Open(svgPath);
            }
            catch (Exception ex)
            {
                // Could throw or show a message, depending on library vs. app usage
                throw new Exception($"Error loading SVG from '{svgPath}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Loads an image (PNG, JPG, BMP, etc.) from a file.
        /// </summary>
        public static Image LoadRegularImage(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath) || !File.Exists(imagePath))
                throw new FileNotFoundException($"Image file not found: {imagePath}");

            try
            {
                return Image.FromFile(imagePath);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error loading image from '{imagePath}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Loads from embedded resource. Returns an (isSvg, object) tuple.
        /// The second item is either the loaded object (SvgDocument or Image).
        /// </summary>
        public static (bool isSvg, object result) LoadFromEmbeddedResource(string resourcePath, Assembly assembly = null)
        {
            assembly ??= Assembly.GetExecutingAssembly(); // default

            using Stream stream = assembly.GetManifestResourceStream(resourcePath)
                ?? throw new FileNotFoundException($"Embedded resource not found: {resourcePath}");

            string extension = Path.GetExtension(resourcePath).ToLower();
            if (extension == ".svg")
            {
                var svgDoc = SvgDocument.Open<SvgDocument>(stream);
                return (true, svgDoc);
            }
            else
            {
                var image = Image.FromStream(stream);
                return (false, image);
            }
        }


        public static Image LoadImageFromResource(string resourcePath, Assembly assembly = null)
        {
            assembly ??= Assembly.GetExecutingAssembly(); // default
            using Stream stream = assembly.GetManifestResourceStream(resourcePath)
                ?? throw new FileNotFoundException($"Embedded resource not found: {resourcePath}");
            return Image.FromStream(stream);
        }

        /// <summary>
        /// <summary>
        /// Loads an image from the given ImageConfiguration.
        /// Considers multiple flags: IsResxEmbedded, AssemblyFullName, IsFile, IsUrl, IsBase64, etc.
        /// If .svg, uses Svg.NET to render a Bitmap.
        /// Returns a System.Drawing.Image or null if loading fails.
        /// </summary>
        public static Image LoadImageFromConfig(ImageConfiguration cfg)
        {
            try
            {
                // ------------------------------------------------------------------------------------------
                // 1) If it's embedded in a .resx:
                //    - We assume cfg.Path is the .resx file path (e.g. "C:\MyProject\Properties\Resources.resx")
                //    - We assume cfg.FileName is the resource KEY inside that .resx
                // ------------------------------------------------------------------------------------------
                if (cfg.IsResxEmbedded)
                {
                    if (!string.IsNullOrEmpty(cfg.Path) && !string.IsNullOrEmpty(cfg.FileName))
                    {
                        string resxFilePath = cfg.Path;
                        if (!File.Exists(resxFilePath))
                            return null;

                        try
                        {
                            // Load .resx as XML
                            var doc = XDocument.Load(resxFilePath);
                            var ns = XNamespace.Get("http://schemas.microsoft.com/developer/msbuild/2003");

                            // Find the data element with the matching name attribute
                            var dataElement = doc.Descendants(ns + "data")
                                .FirstOrDefault(x => x.Attribute("name")?.Value == cfg.FileName);

                            if (dataElement == null)
                                return null;

                            // Get the value element
                            var valueElement = dataElement.Element(ns + "value");
                            if (valueElement == null)
                                return null;

                            string valueStr = valueElement.Value;
                            if (string.IsNullOrEmpty(valueStr))
                                return null;

                            // Try to decode as base64 (most .resx image data is base64-encoded)
                            try
                            {
                                byte[] imageBytes = Convert.FromBase64String(valueStr);
                                using (var ms = new MemoryStream(imageBytes))
                                {
                                    return Image.FromStream(ms);
                                }
                            }
                            catch
                            {
                                // If not base64, return null
                                return null;
                            }
                        }
                        catch
                        {
                            return null;
                        }
                    }
                }

                // ------------------------------------------------------------------------------------------
                // 2) If there's an AssemblyFullName, treat it as an embedded resource in an assembly
                //    (the old logic).
                // ------------------------------------------------------------------------------------------
                if (!string.IsNullOrEmpty(cfg.AssemblyFullName))
                {
                    Assembly asm = MiscFunctions.LoadAssembly(cfg.AssemblyFullName);
                    if (asm != null)
                    {
                        // Typically, cfg.Path holds the resource name, e.g. "MyNamespace.Images.Icon1.ico"
                        // or "MyNamespace.Images.Logo.png"
                        using (Stream st = asm.GetManifestResourceStream(cfg.Path))
                        {
                            if (st != null)
                            {
                                return LoadImageFromStream(st, GetExtension(cfg));
                            }
                        }
                    }
                    return null;
                }

                // ------------------------------------------------------------------------------------------
                // 3) If it's physically on disk (IsFile == true),
                //    load from local file (the old "combine path + file" approach).
                // ------------------------------------------------------------------------------------------
                if (cfg.IsFile)
                {
                    string fullFilePath = Path.Combine(cfg.Path ?? "", cfg.FileName ?? cfg.Name);
                    if (File.Exists(fullFilePath))
                    {
                        string extension = Path.GetExtension(fullFilePath).ToLowerInvariant();
                        if (extension == ".svg")
                        {
                            return LoadSvgFromFile(fullFilePath);
                        }
                        else
                        {
                            return Image.FromFile(fullFilePath);
                        }
                    }
                    return null;
                }

                // ------------------------------------------------------------------------------------------
                // 4) If it's a URL (IsUrl), we could download it with HttpClient/WebClient
                // ------------------------------------------------------------------------------------------
                if (cfg.IsUrl && !string.IsNullOrEmpty(cfg.Path))
                {
                    // For example, if cfg.Path contains the URL
                    // We'll do a quick example with HttpClient:
                    using (var httpClient = new HttpClient())
                    {
                        var data = httpClient.GetByteArrayAsync(cfg.Path).Result;
                        using (var ms = new MemoryStream(data))
                        {
                            // If you want to handle .svg from a URL, you'd need to check the extension or parse
                            // For simplicity, let's assume it's PNG/JPG
                            return Image.FromStream(ms);
                        }
                    }
                }

                // ------------------------------------------------------------------------------------------
                // 5) If it's base64 (IsBase64), decode and load from memory
                // ------------------------------------------------------------------------------------------
                if (cfg.IsBase64 && !string.IsNullOrEmpty(cfg.Path))
                {
                    // Suppose cfg.Path holds the base64 string for the image
                    byte[] bytes = Convert.FromBase64String(cfg.Path);
                    using (var ms = new MemoryStream(bytes))
                    {
                        // If you want to handle .svg base64, you'd parse differently. 
                        return Image.FromStream(ms);
                    }
                }

                // ------------------------------------------------------------------------------------------
                // 6) If it's a memory stream or generic stream that we've already stored somewhere
                //    In that case, we might store that stream object in a separate property on cfg.
                //    e.g. `public Stream ImageStream { get; set; }`
                // ------------------------------------------------------------------------------------------
                if (cfg.IsMemoryStream || cfg.IsStream)
                {
                    // For example, if you have a property "cfg.StreamObject"
                    // (not shown in your class) that holds the actual stream:
                    // using (var ms = cfg.StreamObject as MemoryStream)
                    // {
                    //     if (ms != null)
                    //     {
                    //         ms.Position = 0;
                    //         return Image.FromStream(ms);
                    //     }
                    // }
                    // return null;
                }

                // ------------------------------------------------------------------------------------------
                // If none of the above matched, return null
                // ------------------------------------------------------------------------------------------
                return null;
            }
            catch (Exception ex)
            {
                // Log or handle as needed
                Console.WriteLine($"LoadImageFromConfig error: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Loads an Image from a stream. If the extension is .svg,
        /// parse with Svg.NET and render to a Bitmap, else do 
        /// standard System.Drawing Image.FromStream.
        /// </summary>
        public static Image LoadImageFromStream(Stream st, string extension)
        {
            extension = extension?.ToLowerInvariant();
            if (extension == ".svg")
            {
                // Convert the stream to an SvgDocument
                // then render to a Bitmap
                // Note: If you need a specific size, you can do doc.Draw(width, height).
                var svgDoc = SvgDocument.Open<SvgDocument>(st);
                return svgDoc.Draw(); // default size from the SVG
            }
            else
            {
                // standard approach for PNG, JPG, BMP, ICO, etc.
                using (var mem = new MemoryStream())
                {
                    st.CopyTo(mem);
                    mem.Position = 0;
                    return Image.FromStream(mem);
                }
            }
        }

        /// <summary>
        /// Loads an .svg file from disk using Svg.NET, 
        /// returning a rendered Bitmap.
        /// </summary>
        public static Image LoadSvgFromFile(string svgPath)
        {
            // parse .svg from file
            SvgDocument doc = SvgDocument.Open<SvgDocument>(svgPath);
            // doc.Draw() uses whatever the SVG says for size, 
            // or you can do doc.Draw(width, height) if you want a fixed dimension.
            return doc.Draw();
        }
        public static (object obj, ImageType type) LoadLogoObjectFromFile(string filePath, ImageType guessType)
        {
            if (guessType == ImageType.Ico || guessType == ImageType.Icon)
            {
                using (var ico = new Icon(filePath))
                {
                    return (ico.Clone(), ImageType.Ico);
                }
            }
            else
            {
                // e.g. .png, .jpg, .svg => if you do .svg parsing, do it here
                // for now, let's do standard:
                return (Image.FromFile(filePath), guessType);
            }
        }
        public static (object obj, ImageType type) LoadLogoObjectFromStream(Stream st, ImageType guessType)
        {
            if (guessType == ImageType.Ico || guessType == ImageType.Icon)
            {
                using (var ico = new Icon(st))
                {
                    return (ico.Clone(), ImageType.Ico);
                }
            }
            else
            {
                // .svg or standard
                // if (guessType == ImageType.Svg) => parse with Svg.NET, etc.
                using var mem = new MemoryStream();
                st.CopyTo(mem);
                mem.Position = 0;
                return (Image.FromStream(mem), guessType);
            }
        }

        /// <summary>
        /// Determine the file (or resource) extension from ImageConfiguration.
        /// If Ext is not empty, return that; otherwise parse from Name or Path.
        /// </summary>
        public static string GetExtension(ImageConfiguration cfg)
        {
            if (!string.IsNullOrEmpty(cfg.Ext))
            {
                return cfg.Ext.ToLowerInvariant();
            }
            // fallback: parse from file Name or Path
            string candidate = cfg.FileName ?? cfg.Name ?? cfg.Path;
            if (!string.IsNullOrEmpty(candidate))
            {
                return Path.GetExtension(candidate).ToLowerInvariant();
            }
            return string.Empty;
        }
    }
}

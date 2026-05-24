using System;
using System.IO;
using TheTechIdea.Beep.Vis.Modules;

namespace TheTechIdea.Beep.Shared
{
    public static class ImageTypeExtensions
    {
        /// <summary>
        /// Converts a file extension (e.g., ".png", ".ico", ".svg") 
        /// to the corresponding <see cref="ImageType"/> enum value.
        /// Returns <c>null</c> if the extension is not recognized in this mapping.
        /// </summary>
        /// <param name="extension">A file extension (with leading dot), case-insensitive.</param>
        /// <returns>A nullable <see cref="ImageType"/> matching the extension, or null if not found.</returns>
        public static ImageType? FromExtension(string extension)
        {
            if (string.IsNullOrWhiteSpace(extension))
                return null;

            extension = extension.ToLowerInvariant();

            switch (extension)
            {
                case ".png":
                    return ImageType.Png;
                case ".ico":
                    return ImageType.Ico;
                case ".svg":
                    return ImageType.Svg;
                case ".jpg":
                case ".jpeg":
                    return ImageType.Jpg;
                case ".gif":
                    return ImageType.Gif;
                case ".bmp":
                    return ImageType.Bmp;
                case ".tif":
                case ".tiff":
                    return ImageType.Tiff;
                case ".emf":
                    return ImageType.Emf;
                case ".wmf":
                    return ImageType.Wmf;
                case ".exif":
                    return ImageType.Exif;
                case ".webp":
                    return ImageType.Webp;
                case ".heic":
                    return ImageType.Heic;
                case ".heif":
                    return ImageType.Heif;
                case ".avif":
                    return ImageType.Avif;
                case ".bpg":
                    return ImageType.Bpg;
                case ".flif":
                    return ImageType.Flif;
                case ".jxl":
                    return ImageType.Jxl;
                case ".jph":
                    return ImageType.Jph;
                // Add more if needed (e.g., ".cur", ".wmp", etc.)
                default:
                    return null;
            }
        }

        /// <summary>
        /// Converts an <see cref="ImageType"/> enum value to a preferred file extension, 
        /// or <c>null</c> if no known extension for that enum value.
        /// </summary>
        /// <param name="type">The <see cref="ImageType"/> enum value.</param>
        /// <returns>A string extension (with leading dot), or null if not recognized.</returns>
        public static string ToExtension(ImageType type)
        {
            switch (type)
            {
                case ImageType.Png:
                    return ".png";
                case ImageType.Ico:
                case ImageType.Icon:
                    return ".ico";
                case ImageType.Svg:
                    return ".svg";
                case ImageType.Jpg:
                    return ".jpg";
                case ImageType.Gif:
                    return ".gif";
                case ImageType.Bmp:
                    return ".bmp";
                case ImageType.Tiff:
                    return ".tiff";
                case ImageType.Emf:
                    return ".emf";
                case ImageType.Wmf:
                    return ".wmf";
                case ImageType.Exif:
                    return ".exif";
                case ImageType.Webp:
                    return ".webp";
                case ImageType.Heic:
                    return ".heic";
                case ImageType.Heif:
                    return ".heif";
                case ImageType.Avif:
                    return ".avif";
                case ImageType.Bpg:
                    return ".bpg";
                case ImageType.Flif:
                    return ".flif";
                case ImageType.Jxl:
                    return ".jxl";
                case ImageType.Jph:
                    return ".jph";
                // Add more if needed
                default:
                    return null;
            }
        }
    }
}

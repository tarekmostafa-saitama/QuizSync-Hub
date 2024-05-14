using System.Drawing;
using System.Text.RegularExpressions;
using ImageMagick;
using Microsoft.AspNetCore.Http;

namespace CleanArchitecture.Application.Common.Helpers;

public static class FileManager
{
    public static void ClearTempCardImages()
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot\\temp");

        DirectoryInfo di = new DirectoryInfo(path);
        foreach (FileInfo file in di.EnumerateFiles())
        {
            file.Delete();
        }
    }
    public static async Task<string> Upload(IFormFile file, string folder, bool compress)
    {
        try
        {
            var name = file.Name+"-"+Utilities.GenerateRandomString(7)  + "." + file.FileName.Split('.').Last();
            var path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot\\{folder}", name);
            await using var bits = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(bits);
            bits.Close();
            if (compress && file.IsImage())
            {
                var snakewareLogo = new FileInfo(Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot\\{folder}", name));
                var optimizer = new ImageOptimizer();
                optimizer.OptimalCompression = true;
                optimizer.Compress(snakewareLogo);
                snakewareLogo.Refresh();
            }
            return name;
        }
        catch
        {
            return null;
        }
    }

    public static bool Exist(string file, string folder)
    {
        if (!string.IsNullOrWhiteSpace(file))
        {
            var pathOrg = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot\\{folder}", file);
            var result = File.Exists(pathOrg);
            return result;
        }

        return false;
    }


    public static void Delete(string file, string folder)
    {
        File.Delete(Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot\\{folder}", file));
    }
    public const int ImageMinimumBytes = 512;

    public static bool IsImage(this IFormFile postedFile)
    {
        //-------------------------------------------
        //  Check the image mime types
        //-------------------------------------------
        if (postedFile.ContentType.ToLower() != "image/jpg" &&
                    postedFile.ContentType.ToLower() != "image/jpeg" &&
                    postedFile.ContentType.ToLower() != "image/pjpeg" &&
                    postedFile.ContentType.ToLower() != "image/gif" &&
                    postedFile.ContentType.ToLower() != "image/x-png" &&
                    postedFile.ContentType.ToLower() != "image/png")
        {
            return false;
        }

        //-------------------------------------------
        //  Check the image extension
        //-------------------------------------------
        if (Path.GetExtension(postedFile.FileName).ToLower() != ".jpg"
            && Path.GetExtension(postedFile.FileName).ToLower() != ".png"
            && Path.GetExtension(postedFile.FileName).ToLower() != ".gif"
            && Path.GetExtension(postedFile.FileName).ToLower() != ".jpeg")
        {
            return false;
        }

        //-------------------------------------------
        //  Attempt to read the file and check the first bytes
        //-------------------------------------------
        try
        {
            if (!postedFile.OpenReadStream().CanRead)
            {
                return false;
            }
            //------------------------------------------
            //check whether the image size exceeding the limit or not
            //------------------------------------------ 
            if (postedFile.Length < ImageMinimumBytes)
            {
                return false;
            }

            byte[] buffer = new byte[ImageMinimumBytes];
            postedFile.OpenReadStream().Read(buffer, 0, ImageMinimumBytes);
            string content = System.Text.Encoding.UTF8.GetString(buffer);
            if (Regex.IsMatch(content, @"<script|<html|<head|<title|<body|<pre|<table|<a\s+href|<img|<plaintext|<cross\-domain\-policy",
                RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline))
            {
                return false;
            }
        }
        catch (Exception)
        {
            return false;
        }

        //-------------------------------------------
        //  Try to instantiate new Bitmap, if .NET will throw exception
        //  we can assume that it's not a valid image
        //-------------------------------------------

        try
        {
            using (var bitmap = new Bitmap(postedFile.OpenReadStream()))
            {
            }
        }
        catch (Exception)
        {
            return false;
        }
        finally
        {
            postedFile.OpenReadStream().Position = 0;
        }

        return true;
    }
}

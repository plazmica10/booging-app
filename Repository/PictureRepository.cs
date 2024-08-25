using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BookingApp.Domain.IRepository;

namespace BookingApp.Repository
{
    public class PictureRepository : IPictureRepository
    {
        private const string ImagesFolderPath = @"../../../Resources/Data/Images/";

        private readonly string[] _supportedImageTypesExtensions = { ".png", ".jpg", ".jpeg" };
        private const string SupportedImageTypesFilter = "Image files (*.png; *.jpg; *.jpeg)|*.png;*.jpg;*.jpeg|All files (*.*)|*.*";
        private const string NullImageFileName = "null.png";
        private const string MaxIdFileName = "maxId.txt";

        private string GetAbsolutePath()
        {
            return Path.GetFullPath(ImagesFolderPath);
        }

        public string SafeCopy(string sourcePath)
        {
            string id = GetNextId();
            string fileExtension = Path.GetExtension(sourcePath);
            string destinationPath = GetAbsolutePath() + id + fileExtension;

            while (File.Exists(destinationPath))
            {
                id = GetNextId();
                destinationPath = GetAbsolutePath() + id + fileExtension;
            }

            File.Copy(sourcePath, destinationPath, true);
            return id.ToString();
        }

        public string? CreatePicture()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog { Filter = SupportedImageTypesFilter };
            if (openFileDialog.ShowDialog() == false) { return null; }
            return SafeCopy(openFileDialog.FileName);
        }

        public List<string>? CreatePictures()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog { Filter = SupportedImageTypesFilter, Multiselect = true };
            if (openFileDialog.ShowDialog() == false) { return null; }
            return openFileDialog.FileNames.Select(SafeCopy).ToList();
        }

        private string GetNextId()
        {
            string filePath = GetAbsolutePath() + MaxIdFileName;

            int id = 0;
            if (File.Exists(filePath))
            {
                string text = File.ReadAllText(filePath);
                if (int.TryParse(text, out int parseId)) { id = parseId; }
            }

            id++;
            File.WriteAllText(filePath, id.ToString());
            return id.ToString();
        }

        public string NullImage()
        {
            return GetAbsolutePath() + NullImageFileName;
        }

        public string AddAbsolutePath(string id)
        {
            foreach (string extension in _supportedImageTypesExtensions)
            {
                string fullPath = GetAbsolutePath() + id + extension;
                if (File.Exists(fullPath)) { return fullPath; }
            }

            return NullImage();
        }

        public string GetPictureLocation(string id) { return AddAbsolutePath(id); }
        public List<string> GetPictureLocations(List<string> ids)
        {
            // return placeholder image if null list
            List<string> nullImages = new List<string> { NullImage() };
            if (ids.Count <= 0) { return nullImages; } 
            if (ids.Count == 1 && string.IsNullOrEmpty(ids[0])) { return nullImages; }

            List<string> locations = new List<string>();
            foreach (string id in ids)
            {
                locations.Add(AddAbsolutePath(id));
            }
            return locations;
        }
    }
}

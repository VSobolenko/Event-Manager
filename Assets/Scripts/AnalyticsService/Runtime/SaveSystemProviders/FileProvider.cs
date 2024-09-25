using System;
using System.IO;
using System.Linq;
using UnityEngine;

namespace AnalyticsService.SaveSystemProvider
{
    public class FileProvider : ISaveProvider<EventData>
    {
        private readonly string _filePath =
            Application.persistentDataPath + Path.DirectorySeparatorChar + "/EventsData.txt";

        private bool FileExists => File.Exists(_filePath);

        public FileProvider()
        {
            CreateFileIfNotExists();
        }

        public void SaveData(params EventData[] data)
        {
            var jsonData = data.Select(JsonUtility.ToJson).ToList();

            File.AppendAllLines(_filePath, jsonData);
        }

        public EventData[] LoadData()
        {
            if (FileExists == false) 
                return Array.Empty<EventData>();

            var jsonLines = File.ReadAllLines(_filePath);
            var data = jsonLines.Select(JsonUtility.FromJson<EventData>);
            return data.ToArray();
        }

        public void ClearData()
        {
            if (FileExists)
            {
                File.Delete(_filePath);
            }
        }

        private void CreateFileIfNotExists()
        {
            if (FileExists)
                return;

            var stream = File.Create(_filePath);
            stream.Close();
        }
    }
}
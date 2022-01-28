using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace AnalyticsCore.SaveSystemProvider
{
    public class FileProvider : ISaveProvider<EventData>
    {
        private string _filePath = Application.persistentDataPath + Path.DirectorySeparatorChar + "/EventsData.txt";
        
        public void SaveData(params EventData[] data)
        {
            CheckForExistFile(true);
            
            var jsonData = new List<string>();
            foreach (var savableData in data)
            {
                jsonData.Add(JsonUtility.ToJson(savableData));
            }
            File.AppendAllLines(_filePath, jsonData);
        }

        public EventData[] LoadData()
        {
            if (CheckForExistFile() == false)
            {
                return new EventData[0];
            }

            var files = File.ReadAllLines(_filePath);
            var data = files.Select(jsonData => JsonUtility.FromJson<EventData>(jsonData)).ToList();
            return data.ToArray();
        }

        public void ClearData()
        {
            if (HasData)
            {
                File.Delete(_filePath);
            }
        }

        public bool HasData => File.Exists(_filePath);

        private bool CheckForExistFile( bool createIfNotFound = false)
        {
            if (HasData)
            {
                return true;
            }

            if (createIfNotFound && !HasData)
            {
                File.Create(_filePath);
                return true;
            }

            return false;
        }
    }
}
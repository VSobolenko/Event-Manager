using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AnalyticsCore.SaveSystemProvider
{
    public class FileProvider<T> : ISaveProvider<T>
        where T : class
    {
        private string _filePath = Application.persistentDataPath + Path.DirectorySeparatorChar + "/EventsData.dat";
        
        public async Task SaveData(params T[] data)
        {
            CheckForExistFile();
            BinaryFormatter bf = new BinaryFormatter(); 

            using (FileStream sourceStream = new FileStream(_filePath,
                FileMode.OpenOrCreate, FileAccess.Write, FileShare.None,
                bufferSize: 4096, useAsync: true))
            {
                foreach (var storeData in data)
                {
                    byte[] bytesData;
                
                    using (MemoryStream ms = new MemoryStream())
                    {
                        bf.Serialize(ms, storeData);
                        bytesData = ms.ToArray();
                    }
                
                    await sourceStream.WriteAsync(bytesData, 0, bytesData.Length);
                }
                /*
                byte[] bytesData;
                
                using (MemoryStream ms = new MemoryStream())
                {
                    bf.Serialize(ms, data);
                    bytesData = ms.ToArray();
                }
                
                await sourceStream.WriteAsync(bytesData, 0, bytesData.Length);
                */
            };
            
            Debug.Log($"Events data saved\nPath: {_filePath}");
        }

        public async Task<T[]> LoadData()
        {
            if (CheckForExistFile() == false)
            {
                Debug.Log("Load data, but file not found. Hard return");

                return new T[0];
            }

            using (FileStream sourceStream = new FileStream(_filePath,
                FileMode.OpenOrCreate, FileAccess.Read, FileShare.None,
                bufferSize: 4096, useAsync: true))
            {
                T[] sb;
                var total = new List<object>();
                BinaryFormatter bf = new BinaryFormatter();

                byte[] buffer = new byte[0x1000];
                using (MemoryStream ms = new MemoryStream())
                {
                    int read;
                    while ((read = await sourceStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                    {
                        ms.Write(buffer, 0, read);
                        ms.Seek(0, SeekOrigin.Begin);
                        total.Add( bf.Deserialize(ms));
                    }
                }


                var ret = new List<T>();
                for (int i = 0; i < total.Count - 1; i++)
                {
                    ret = (List<T>) total[i];
                    Debug.Log(ret);
                }

                return ret.ToArray();
                /*
                //var returned = total.Cast<T[]>();
                var returned =  (List<T>) total;
                Debug.Log($"Load data\nData: {returned}");
                Debug.Log($"Load data\nData: {returned.Count}");
                return returned.ToArray();*/
            }
        }

        public void ClearData()
        {
            if (File.Exists(_filePath))
            {
                File.Delete(_filePath);
            }
            else
            {
                Debug.LogError("No save data to delete.");
            }
        }

        private bool CheckForExistFile( bool createIfNotFound = false)
        {
            if (File.Exists(_filePath))
            {
                return true;
            }

            if (createIfNotFound)
            {
                File.Create(_filePath);
                return true;
            }

            return false;
        }
    }
}
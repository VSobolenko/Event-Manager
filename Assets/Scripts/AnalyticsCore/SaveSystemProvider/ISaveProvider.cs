﻿using System.Threading.Tasks;

namespace AnalyticsCore.SaveSystemProvider
{
    public interface ISaveProvider<T>
    {
        void SaveData(params T[] data);
        T[] LoadData();
        void ClearData(); 
        
        bool HasData { get; }
    }
}
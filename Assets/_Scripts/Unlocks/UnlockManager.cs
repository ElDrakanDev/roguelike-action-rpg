using UnityEngine;
using System;
using System.IO;
using Newtonsoft.Json;
using Game.Events;
using Game.Utils;

namespace Game.Unlocks
{
    [Serializable]
    public class UnlockManager
    {
        int saveSlot;
        static UnlockManager instance;
        public UnlockData saveFileData;
        string savePath;
        public string SavePath { get => savePath; }
        public int SaveSlot { get => saveSlot; }
        public static UnlockManager Instance { get => instance; }

        public UnlockManager(int saveSlot = 1)
        {
            this.saveSlot = saveSlot;
            instance = this;
            savePath = GetSavePath();
            Debug.Log($"UnlockManager instanciado con savePath en {savePath}");
            Load();
        }

        public bool Load()
        {
            try
            {
                string content = File.ReadAllText(savePath);
                content = Encryption.Decrypt(content);
                saveFileData = JsonConvert.DeserializeObject<UnlockData>(content);
                Debug.Log("Desbloqueos cargados exitosamente.");
                EventManager.OnUnlockLoad();
                return true;
            }
            catch(FileNotFoundException ex)
            {
                saveFileData = new UnlockData();
                Debug.LogWarning($"Error al cargar los desbloqueos. Se crearon datos vac�os. {ex}");
                EventManager.OnUnlockLoad();
                return false;
            }
        }
        public void Save()
        {
            string content = JsonConvert.SerializeObject(saveFileData, Formatting.Indented);
            content = Encryption.Encrypt(content);
            File.WriteAllText(SavePath, content);
            Debug.Log($"Se guardaron los datos exitosamente en {savePath}.");
        }
        public void SetUnlock(string unlockName, bool value)
        {
            if(saveFileData.unlocks.TryGetValue(unlockName, out bool isUnlocked))
            {
                saveFileData.unlocks[unlockName] = value;
                if (value is true && isUnlocked is false) EventManager.OnUnlock(unlockName);
            }
            else
            {
                saveFileData.unlocks.Add(unlockName, value);
                if (value) EventManager.OnUnlock(unlockName);
            }
        }
        public bool IsUnlocked(string name)
        {
            if (saveFileData.unlocks.TryGetValue(name, out bool value))
                return value;
            return false;
        }
        public string GetSavePath()
        {
            try
            {
                Directory.CreateDirectory(Application.streamingAssetsPath);
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }
            return $"{Application.streamingAssetsPath}/rglksvdt_{saveSlot}.dat";
        } 
    }
}
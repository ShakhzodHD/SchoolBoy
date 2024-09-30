using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Bullet_Master_3D.Scripts.Menu
{
    public static class SavesService
    {
        public static JsonData LoadedData;

        private static readonly string _path =  Application.persistentDataPath + "/" + Constants.JSON_SAVES_FILE_NAME;

        /// <summary>
        /// Default json saves data
        /// </summary>
        public class JsonData
        {
            public JsonData()
            {
                for (var i = 1; i <= SceneManager.sceneCountInBuildSettings - 1; i++) 
                {
                    //Create default saves for all levels
                    Levels.Add(new Level{Unlocked = i == 1});
                }
            }

            public int LevelId = 1;
            [Serializable] public class Level
            {
                public bool Unlocked = true;
                public int StarsCount;
            }
            //DO NOT set it readonly because it won't be saved
            public List<Level> Levels = new List<Level>();
            
            public bool Vibration = true;
            public bool Sounds = true;
        }

        /// <summary>
        /// Load data if already exist or create new
        /// </summary>
        public static JsonData LoadData()
        {
            if (File.Exists(_path))
            {
                var json = File.ReadAllText(_path);
                LoadedData = JsonUtility.FromJson<JsonData>(json);
            }
            else
            {
                LoadedData = new JsonData();
                SaveData();
            }
            return LoadedData;
        }
        
        /// <summary>
        /// Increase level id and if that's last level reset saves
        /// </summary>
        public static void IncreaseLevelId(int currentLevelId, int starsCount) 
        {
            LoadedData.Levels[LoadedData.LevelId - 1].StarsCount = starsCount;
            if (currentLevelId == LoadedData.LevelId)
            {
                LoadedData.LevelId++;
            }
            if (LoadedData.LevelId > LoadedData.Levels.Count)
            {
                //When all levels complete, we rewrites saves to default
                LoadedData = new JsonData();
            }
            else
            {
                LoadedData.Levels[LoadedData.LevelId - 1].Unlocked = true;
            }
            SaveData();
        }

        /// <summary>
        /// Saves LoadedData
        /// </summary>
        public static void SaveData()
        {
            var json = JsonUtility.ToJson(LoadedData);
            File.WriteAllText(_path, json);
        }

        /// <summary>
        /// Deletes all data
        /// </summary>
        public static void DeleteData()
        {
            File.Delete(_path);
        }
    }
}
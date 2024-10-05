using System;
using System.Collections.Generic;
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
            if (PlayerPrefs.HasKey("LevelId"))
            {
                LoadedData = new JsonData();
                LoadedData.LevelId = PlayerPrefs.GetInt("LevelId");
                LoadedData.Levels = new List<JsonData.Level>();

                // Загружаем данные для каждого уровня
                for (var i = 0; i < SceneManager.sceneCountInBuildSettings - 1; i++)
                {
                    var level = new JsonData.Level
                    {
                        Unlocked = PlayerPrefs.GetInt($"Level_{i}_Unlocked", (i == 0 ? 1 : 0)) == 1, // Разблокирован ли уровень (по умолчанию - первый уровень)
                        StarsCount = PlayerPrefs.GetInt($"Level_{i}_StarsCount", 0) // Количество звезд для уровня
                    };
                    LoadedData.Levels.Add(level);
                }

                LoadedData.Vibration = PlayerPrefs.GetInt("Vibration", 1) == 1; // Включение/выключение вибрации
                LoadedData.Sounds = PlayerPrefs.GetInt("Sounds", 1) == 1; // Включение/выключение звуков
            }
            else
            {
                LoadedData = new JsonData(); // Создаем новые данные
                SaveData(); // Сохраняем их в PlayerPrefs
            }
            return LoadedData;
        }

        /// <summary>
        /// Increase level id and if that's last level reset saves
        /// </summary>
        public static void IncreaseLevelId(int currentLevelId, int starsCount)
        {
            LoadedData.Levels[LoadedData.LevelId - 1].StarsCount = starsCount; // Сохраняем звезды для текущего уровня
            if (currentLevelId == LoadedData.LevelId)
            {
                LoadedData.LevelId++; // Переходим на следующий уровень
            }
            if (LoadedData.LevelId > LoadedData.Levels.Count)
            {
                // Если все уровни пройдены, сбрасываем данные
                LoadedData = new JsonData();
            }
            else
            {
                LoadedData.Levels[LoadedData.LevelId - 1].Unlocked = true; // Разблокируем следующий уровень
            }
            SaveData(); // Сохраняем изменения
        }

        /// <summary>
        /// Saves LoadedData
        /// </summary>
        public static void SaveData()
        {
            PlayerPrefs.SetInt("LevelId", LoadedData.LevelId); // Сохраняем ID уровня

            // Сохраняем данные для каждого уровня
            for (var i = 0; i < LoadedData.Levels.Count; i++)
            {
                PlayerPrefs.SetInt($"Level_{i}_Unlocked", LoadedData.Levels[i].Unlocked ? 1 : 0);
                PlayerPrefs.SetInt($"Level_{i}_StarsCount", LoadedData.Levels[i].StarsCount);
            }

            PlayerPrefs.SetInt("Vibration", LoadedData.Vibration ? 1 : 0); // Сохраняем состояние вибрации
            PlayerPrefs.SetInt("Sounds", LoadedData.Sounds ? 1 : 0); // Сохраняем состояние звуков

            PlayerPrefs.Save(); // Сохраняем изменения
        }

        /// <summary>
        /// Deletes all data
        /// </summary>
        public static void DeleteData()
        {
            PlayerPrefs.DeleteAll(); // Удаляем все данные из PlayerPrefs
        }
    }
}
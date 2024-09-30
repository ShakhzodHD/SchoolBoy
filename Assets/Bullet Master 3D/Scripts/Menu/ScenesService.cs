using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Bullet_Master_3D.Scripts.Menu
{
    public class ScenesService : MonoBehaviour
    {
        [Header("TESTING MODE")] 
        [HideInInspector] public bool TestModeEnabled;
        [Range(1, Constants.MAXIMUM_LEVELS)] public int LevelForTestId;
        
        public event Action OnLevelLoaded;
        public int LevelId { get; private set; }

        private void Awake()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        /// <summary>
        /// Loads testing level scene or current level
        /// </summary>
        public void InitialLoad()
        {
            if (TestModeEnabled) 
            {
                if (LevelForTestId >= SceneManager.sceneCountInBuildSettings)
                {
                    var maximumLevelId = SceneManager.sceneCountInBuildSettings - 1;
                    Debug.LogError($"There is no level with this id! Maximum level id is {maximumLevelId}.");
                    LevelForTestId = maximumLevelId;
                }

                LoadLevel(LevelForTestId);
            }
            else {
                LoadLevel();
            }
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            if(scene.buildIndex == 0) return;
            OnLevelLoaded?.Invoke();
        }

        /// <summary>
        /// Loads current level
        /// </summary>
        public void LoadLevel()
        {
            LevelId = SavesService.LoadData().LevelId;
            SceneManager.LoadScene(LevelId);
        }
        
        /// <summary>
        /// Loads level scene by levelId
        /// </summary>
        public void LoadLevel(int levelId)
        {
            LevelId = levelId;
            SceneManager.LoadScene(LevelId);
        }

        /// <summary>
        /// Reopens current scene
        /// </summary>
        public void RestartLevel()
        {
            SceneManager.LoadScene(LevelId);
        }
    }
}
using System;
using Bullet_Master_3D.Scripts.Game;
using Bullet_Master_3D.Scripts.Menu;
using UnityEngine;

namespace Bullet_Master_3D.Scripts.Singleton
{
    public class Boostrap : MonoBehaviour
    {
        public readonly GameEvents GameEvents = new GameEvents();
        public ScenesService ScenesService { get; private set; }
        public UIManager UIManager { get; private set; }
        public GameManager GameManager { get; private set; }

        public GameStates GameState { get; private set; } = GameStates.InMenu;
        public event Action<GameStates> OnGameStateChanged;
        
        public GameSettings GameSettings { get; private set; }
        public LevelsSettings.Level LevelSettings { get; private set; }
        
        public static Boostrap Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
            Init();
            UIManager.Init();

            SavesService.LoadData();
            GameSettings = Resources.Load<GameSettings>(Constants.GAME_SETTINGS_RESOURCES_PATH);

            DontDestroyOnLoad(gameObject);
            ScenesService.OnLevelLoaded += OnLevelLoaded;
            ScenesService.InitialLoad();
        }

        private void Init()
        {
            ScenesService = FindObjectOfType<ScenesService>();
            UIManager = FindObjectOfType<UIManager>();
            GameManager = FindObjectOfType<GameManager>();
        }

        private void OnLevelLoaded()
        {
            GameManager = FindObjectOfType<GameManager>();
            LevelSettings = Resources.Load<LevelsSettings>(Constants.LEVELS_SETTINGS_RESOURCES_PATH).Levels[ScenesService.LevelId - 1];
            //If this is a restart of the level in Gameplay, then set the same state
            ChangeGameState(GameState == GameStates.InProgress ? GameStates.InProgress : GameStates.InMenu);
        }

        /// <summary>
        /// Changes main game state it affects to menu state
        /// </summary>
        public void ChangeGameState(GameStates gameState)
        {
            if(GameState == gameState && GameState != GameStates.InProgress) return;
            GameState = gameState;
            OnGameStateChanged?.Invoke(gameState);

            if (GameState == GameStates.LevelComplete)
            {
                SavesService.IncreaseLevelId(ScenesService.LevelId, GameManager.StarsCount);
            }
        }
    }
}
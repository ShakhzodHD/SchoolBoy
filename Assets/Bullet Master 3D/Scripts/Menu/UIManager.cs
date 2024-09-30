using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Bullet_Master_3D.Scripts.Singleton;

namespace Bullet_Master_3D.Scripts.Menu
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameObject _canvas;
        [SerializeField] private List<GameObject> _panels;

        private MenuStates _menuStates = MenuStates.StartMenu;

        /// <summary>
        /// Subscribes boostrap actions
        /// </summary>
        public void Init()
        {
           Boostrap.Instance.ScenesService.OnLevelLoaded += OnLevelLoaded;
           Boostrap.Instance.OnGameStateChanged += OnGameStateChanged;
        }

        private void OnLevelLoaded()
        {
            _canvas.SetActive(true);
        }

        private void OnGameStateChanged(GameStates gameStates)
        {
            switch (gameStates)
            {
                case GameStates.InMenu:
                    ChangeMenuState(MenuStates.StartMenu);
                    break;
                case GameStates.InProgress:
                    ChangeMenuState(MenuStates.Gameplay);
                    _panels[(int)MenuStates.Gameplay].GetComponent<GameplayPanel>().Init();
                    break;
                case GameStates.GameOver:
                    ChangeMenuState(MenuStates.GameOver);
                    break;
                case GameStates.LevelComplete:
                    ChangeMenuState(MenuStates.LevelComplete);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(gameStates), gameStates, null);
            }
        }

        /// <summary>
        /// Changes menu state and opens new state panel
        /// </summary>
        public void ChangeMenuState(MenuStates menuState)
        {
            _menuStates = menuState;
            OpenPanelForCurrentState();
        }

        private void OpenPanelForCurrentState()
        {
            _panels.FirstOrDefault(panel => panel.activeSelf)?.SetActive(false);
            _panels[(int)_menuStates].SetActive(true);
        }
    }
}

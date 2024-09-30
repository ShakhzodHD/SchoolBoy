using Bullet_Master_3D.Scripts.Singleton;
using UnityEngine;
using UnityEngine.UI;

namespace Bullet_Master_3D.Scripts.Menu
{
    public class MenuPanel : MonoBehaviour
    {
        [Header("BUTTONS")] 
        [SerializeField] private Button _settings;
        [SerializeField] private Button _levels;
        [SerializeField] private Button _play;

        private void Start()
        {
            _settings.onClick.AddListener(OnButtonSettingsClick);
            _levels.onClick.AddListener(OnButtonLevelsClick);
            _play.onClick.AddListener(OnButtonPlayClick);
        }

        private void OnButtonSettingsClick()
        {
            Boostrap.Instance.UIManager.ChangeMenuState(MenuStates.Settings);   
        }
        
        private void OnButtonLevelsClick()
        {
            Boostrap.Instance.UIManager.ChangeMenuState(MenuStates.Levels);
        }
        
        private void OnButtonPlayClick()
        {
            Boostrap.Instance.ChangeGameState(GameStates.InProgress);
            Boostrap.Instance.GameEvents.OnLevelStart?.Invoke();
        }
    }
}
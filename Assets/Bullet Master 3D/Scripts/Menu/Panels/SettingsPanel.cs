using Bullet_Master_3D.Scripts.Singleton;
using UnityEngine;
using UnityEngine.UI;

namespace Bullet_Master_3D.Scripts.Menu
{
    public class SettingsPanel : MonoBehaviour
    {
        [Header("BUTTONS")] 
        [SerializeField] private Button _close;

        private void Start()
        {
            _close.onClick.AddListener(OnButtonCloseClick);
        }

        private void OnButtonCloseClick()
        {
            Boostrap.Instance.UIManager.ChangeMenuState(MenuStates.StartMenu);
        }
    }
}
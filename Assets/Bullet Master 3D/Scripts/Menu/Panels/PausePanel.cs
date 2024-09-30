using Bullet_Master_3D.Scripts.Singleton;
using UnityEngine;
using UnityEngine.UI;

namespace Bullet_Master_3D.Scripts.Menu
{
    public class PausePanel : MonoBehaviour
    {
        [Header("BUTTONS")] 
        [SerializeField] private Button _close;

        private void Start()
        {
            _close.onClick.AddListener(OnButtonCloseClick);
        }

        private void OnButtonCloseClick()
        {
            Time.timeScale = 1f;
            Boostrap.Instance.UIManager.ChangeMenuState(MenuStates.Gameplay);
        }
    }
}
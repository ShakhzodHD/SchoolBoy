using Bullet_Master_3D.Scripts.Singleton;
using UnityEngine;
using UnityEngine.UI;

namespace Bullet_Master_3D.Scripts.Menu
{
    public class GameplayPanel : MonoBehaviour
    {
        [Header("BUTTONS")] 
        [SerializeField] private Button _pause;
        [SerializeField] private Button _restart;

        [Header("ADDITIONAL")] 
        [SerializeField] private CartridgesBar _cartridgesBar;
        [SerializeField] private StarsBar _starsBar;

        private void Start()
        {
            _pause.onClick.AddListener(OnButtonPauseClick);
            _restart.onClick.AddListener(OnButtonRestartClick);
        }

        /// <summary>
        /// Shows stars and cartridges
        /// </summary>
        public void Init()
        {
            _starsBar.ShowStars(3);
            _cartridgesBar.Init();
        }

        private void OnEnable()
        {
            Boostrap.Instance.GameManager.OnCartridgesCountChanged += OnCartridgesCountChanged;
            Boostrap.Instance.GameManager.OnStarsCountChanged += OnStarsCountChanged;
        }

        private void OnDisable()
        {
            Boostrap.Instance.GameManager.OnCartridgesCountChanged -= OnCartridgesCountChanged;
            Boostrap.Instance.GameManager.OnStarsCountChanged -= OnStarsCountChanged;
        }

        private void OnCartridgesCountChanged(int usedCartridgesCount)
        {
            _cartridgesBar.DestroyUsedCartridges(usedCartridgesCount);
        }

        private void OnStarsCountChanged(int starsCount)
        {
            _starsBar.ShowStars(starsCount);
        }

        private void OnButtonPauseClick()
        {
            Time.timeScale = 0;
            Boostrap.Instance.UIManager.ChangeMenuState(MenuStates.Pause);
        }

        private void OnButtonRestartClick()
        {
            Boostrap.Instance.ScenesService.RestartLevel();
            Boostrap.Instance.GameEvents.OnLevelRestart?.Invoke();
        }
    }
}
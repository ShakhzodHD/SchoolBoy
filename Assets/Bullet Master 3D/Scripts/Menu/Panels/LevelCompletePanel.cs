using Bullet_Master_3D.Scripts.Singleton;
using UnityEngine;
using UnityEngine.UI;

namespace Bullet_Master_3D.Scripts.Menu
{
    public class LevelCompletePanel : MonoBehaviour
    {
        [Header("BUTTONS")]
        [SerializeField] private Button _next;
        [SerializeField] private Button _restart;
        
        [Header("ADDITIONAL")]
        [SerializeField] private StarsBar _starsBar;
        [SerializeField] private GameObject _confetti;

        private void Start()
        {
            _next.onClick.AddListener(OnButtonNextClick);
            _restart.onClick.AddListener(OnButtonRestartClick);
        }

        private void OnEnable()
        {
            //Show confetti particles
            _confetti.SetActive(true);
            //Show earned stars
            _starsBar.ShowStars(Boostrap.Instance.GameManager.StarsCount);
            Boostrap.Instance.GameEvents.OnLevelComplete?.Invoke();

            InitPlayerWin.Instance.ActivePlayerAvatars();
            SoundManager.Instance.PlaySoundState(0);
        }

        private void OnDisable()
        {
            _confetti.SetActive(false);
        }

        private void OnButtonNextClick()
        {
            Boostrap.Instance.ScenesService.LoadLevel();
        }

        private void OnButtonRestartClick()
        {
            Boostrap.Instance.ScenesService.RestartLevel();
        }
    }
}
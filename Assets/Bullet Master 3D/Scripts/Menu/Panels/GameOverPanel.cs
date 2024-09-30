using Bullet_Master_3D.Scripts.Singleton;
using UnityEngine;
using UnityEngine.UI;

namespace Bullet_Master_3D.Scripts.Menu
{
    public class GameOverPanel : MonoBehaviour
    {
        [Header("BUTTONS")]
        [SerializeField] private Button _restart;

        private void Start()
        {
            _restart.onClick.AddListener(OnButtonRestartClick);
        }

        private void OnEnable()
        {
            Boostrap.Instance.GameEvents.OnLevelLose?.Invoke();

            InitPlayerLose.Instance.ActivePlayerAvatars();
            SoundManager.Instance.PlaySoundState(1);
        }

        private void OnButtonRestartClick()
        {
            Boostrap.Instance.ScenesService.RestartLevel();
        }
    }
}
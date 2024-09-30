using UnityEngine;
using UnityEngine.UI;

namespace Bullet_Master_3D.Scripts.Menu
{
    public class SettingsButtons : MonoBehaviour
    {
        [Header("VIBRATION")]
        [SerializeField] private Button _vibration;
        [SerializeField] private Sprite _vibrationOn;
        [SerializeField] private Sprite _vibrationOff;
        
        [Header("SOUNDS")]
        [SerializeField] private Button _sounds;
        [SerializeField] private Sprite _soundsOn;
        [SerializeField] private Sprite _soundsOff;

        private void Start()
        {
            _sounds.onClick.AddListener(OnButtonSoundsClick);
            _vibration.onClick.AddListener(OnButtonVibrationClick);
        }

        private void OnEnable()
        {
            ShowButtonsSprites();
        }
        
        private void ShowButtonsSprites()
        {
            _sounds.image.sprite = SavesService.LoadedData.Sounds ? _soundsOn : _soundsOff;
            AudioListener.volume = SavesService.LoadedData.Sounds ? 1f : 0f;
            
            _vibration.image.sprite = SavesService.LoadedData.Vibration ? _vibrationOn : _vibrationOff;
        }

        private void OnButtonVibrationClick()
        {
            var jsonData = SavesService.LoadData();
            jsonData.Vibration = !jsonData.Vibration;
            SavesService.SaveData();
            ShowButtonsSprites();
        }
        
        private void OnButtonSoundsClick()
        {
            var jsonData = SavesService.LoadData();
            jsonData.Sounds = !jsonData.Sounds;
            SavesService.SaveData();
            ShowButtonsSprites();
        }
    }
}
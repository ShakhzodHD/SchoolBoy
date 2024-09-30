using System;
using Bullet_Master_3D.Scripts.Singleton;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Bullet_Master_3D.Scripts.Menu
{
    public class LevelItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;

        [Header("BUTTON")]
        [SerializeField] private Button _open;

        [Header("SPRITES")]
        [SerializeField] private Sprite _unlockedLevel;
        [SerializeField] private Sprite _lockedLevel;

        [Header("STARS")]
        [SerializeField] private StarsBar _starsBar;

        public event Action<int> OnLoadScene;

        private int _levelId;

        /// <summary>
        /// Set level item state, level id and stars
        /// </summary>
        /// <param name="unlocked">Is this level is already open for playing or not</param>
        /// <param name="id">Level item level id</param>
        /// <param name="starsCount">If level already passed, shows earned stars</param>
        public void Setup(bool unlocked, int id, int starsCount)
        {
            _open.image.sprite = unlocked ? _unlockedLevel : _lockedLevel;
            _text.enabled = unlocked;
            _starsBar.gameObject.SetActive(unlocked);

            if (!unlocked) return;

            _text.text = id.ToString();
            _starsBar.ShowStars(starsCount);
            _levelId = id;
        }

        private void Start()
        {
            _open.onClick.AddListener(OnButtonOpenClick);
        }

        private void OnButtonOpenClick()
        {
            //Open level if it isn't already opened
            if (_levelId == 0 || _levelId == Boostrap.Instance.ScenesService.LevelId) return;
            OnLoadScene?.Invoke(_levelId);
        }
    }
}
using System.Collections.Generic;
using Bullet_Master_3D.Scripts.Singleton;
using UnityEngine;
using UnityEngine.UI;

namespace Bullet_Master_3D.Scripts.Menu
{
    public class LevelsPanel : MonoBehaviour
    {
        [Header("BUTTONS")] 
        [SerializeField] private Button _close;

        [Header("LEVELS LIST")] 
        [SerializeField] private Transform _parent;
        [SerializeField] private GameObject _prefab;
        
        private readonly List<LevelItem> _spawnedLevelItems = new List<LevelItem>();
        
        private void Start()
        {
            _close.onClick.AddListener(OnButtonCloseClick);
        }

        private void OnEnable()
        {
            DestroyLevelItems();
            SpawnLevelItems();
        }

        private void DestroyLevelItems()
        {
            for (var n = _spawnedLevelItems.Count - 1; n >= 0; n--) 
            {
                Destroy(_spawnedLevelItems[n].gameObject);
                _spawnedLevelItems.RemoveAt(n);
            }
        }

        private void SpawnLevelItems()
        {
            var levels = SavesService.LoadData().Levels;
            for (var n = 0; n <= levels.Count - 1; n++) 
            {
                var level = levels[n];
                var item = Instantiate(_prefab, _parent).GetComponent<LevelItem>();
                //n+1 because arrays begins from 0, but level id's from 1
                item.Setup(level.Unlocked, n+1, level.StarsCount);
                item.OnLoadScene += OnLoadScene;
                _spawnedLevelItems.Add(item);
            }
        }

        private void OnLoadScene(int levelId)
        {
            Boostrap.Instance.ScenesService.LoadLevel(levelId);
            Boostrap.Instance.GameEvents.OnOpenLevelFromList?.Invoke(levelId);
            OnButtonCloseClick();
        }

        private void OnButtonCloseClick()
        {
            Boostrap.Instance.UIManager.ChangeMenuState(MenuStates.StartMenu);
        }
    }
}
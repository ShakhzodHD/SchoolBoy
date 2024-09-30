using System.Collections.Generic;
using System.Linq;
using Bullet_Master_3D.Scripts.Singleton;
using UnityEngine;
using UnityEngine.UI;

namespace Bullet_Master_3D.Scripts.Menu
{
    public class CartridgesBar : MonoBehaviour
    {
        [SerializeField] private Transform _parent;
        [SerializeField] private GameObject _prefab;

        [Header("SPRITES")] 
        [SerializeField] private Sprite[] _cartridges;

        private readonly List<Image> _spawnedCartridges = new List<Image>();

        /// <summary>
        /// Spawns cartridges sprites
        /// </summary>
        public void Init()
        {
            DestroyAllCartridges();
            SpawnAllCartridges();
        }

        private void DestroyAllCartridges()
        {
            for (var n = _spawnedCartridges.Count - 1; n >= 0; n--) 
            {
                Destroy(_spawnedCartridges[n].gameObject);
                _spawnedCartridges.RemoveAt(n);
            }
        }

        private void SpawnAllCartridges()
        {
            //Spawn cartridges for all weapons
            foreach (var weapon in Boostrap.Instance.LevelSettings.Weapons)
            {
                for (var i = 0; i < weapon.CartridgesCount; i++) 
                {
                    var cartridge = Instantiate(_prefab, _parent).GetComponent<Image>();
                    cartridge.sprite = _cartridges[(int) weapon.WeaponType];
                    _spawnedCartridges.Add(cartridge);
                }
            }
        }

        /// <summary>
        /// Destroys used cartridges sprites
        /// </summary>
        public void DestroyUsedCartridges(int count)
        {
            for (var i = 0; i < count; i++)
            {
                var cartridge = _spawnedCartridges.Last();
                _spawnedCartridges.Remove(cartridge);
                Destroy(cartridge.gameObject);
            }
        }
    }
}
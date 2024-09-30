using System;
using System.Collections;
using Bullet_Master_3D.Scripts.Singleton;
using UnityEngine;

namespace Bullet_Master_3D.Scripts.Game
{
    public class GameManager : MonoBehaviour
    {
        [Header("STICKMEN")]
        public Stickman[] AllEnemies;
        public Stickman[] Prisoners;
        
        public event Action<int> OnCartridgesCountChanged;
        public event Action<int> OnStarsCountChanged;
        [HideInInspector] public PlayerSpawnerService PlayerSpawnerService;
        public int StarsCount { get; private set; }

        private int _enemiesCount;

        private void Awake()
        {
            if (Boostrap.Instance == null)
            {
                Debug.LogAssertion("You need to start the game from the main scene!");
                return;
            }

            PlayerSpawnerService = FindObjectOfType<PlayerSpawnerService>();
            _enemiesCount = AllEnemies.Length;
        }

        private void Start()
        {
            SubscribeActions();
        }

        private void SubscribeActions()
        {
            if (PlayerSpawnerService.Player == null) return;
            
            PlayerSpawnerService.Player.OnShoot += OnPlayerShoot;
            PlayerSpawnerService.Player.OnOutOfGuns += OnOutOfGuns;
            PlayerSpawnerService.Player.OnDied += GameOver;
            
            foreach (var enemy in AllEnemies) {
                enemy.OnDied += OnEnemyDied;
            }
            foreach (var prisoner in Prisoners) {
                prisoner.OnDied += GameOver;
            }
        }
        
        private void OnPlayerShoot(int usedCartridgesCount)
        {
            //Let the menu know that the number of cartridges has changed
            OnCartridgesCountChanged?.Invoke(usedCartridgesCount);
            var starsCount = CalculateStarsCount();
            if (starsCount != StarsCount)
            {
                StarsCount = starsCount;
                OnStarsCountChanged?.Invoke(StarsCount);
            }
        }
        
        private int CalculateStarsCount()
        {
            var cartridgesCount = PlayerSpawnerService.Player.CartridgesCount;
            var maximumCartridgesCount = PlayerSpawnerService.Player.MaximumCartridgesCount;
            var settings = Boostrap.Instance.LevelSettings;
            //Calculate stars count depend used cartridges count
            if (cartridgesCount >= maximumCartridgesCount - settings.CartridgesUsedForThreeStars) return 3;
            if (cartridgesCount >= maximumCartridgesCount - settings.CartridgesUsedForTwoStars) return 2;
            if (cartridgesCount == maximumCartridgesCount - settings.CartridgesUsedForOneStar) return 1;
            return 0;
        }

        private void OnEnemyDied()
        {
            _enemiesCount--;
            if (_enemiesCount != 0) return;
            //If all enemies are dead and there are spawned ammo, we wait for them to be destroyed
            //As they can hit the player or the prisoners and will lose
            if (Ammo.SpawnedAmmoCount == 0)
            {
                LevelComplete();
            }
            else 
            {
                //Bullet can ricochet and kill a player or prisoner, so we wait for all bullets to be destroyed
                StartCoroutine(WaitForAllAmmoDestroy());
            }
        }

        private IEnumerator WaitForAllAmmoDestroy()
        {
            yield return new WaitUntil(() => Ammo.SpawnedAmmoCount == 0);
            yield return new WaitForEndOfFrame();
            LevelComplete();
        }

        private void OnOutOfGuns()
        {
            if (_enemiesCount == 0) return;
            GameOver();
        }
        
        private void GameOver()
        {
           Boostrap.Instance.ChangeGameState(GameStates.GameOver);
        }

        private void LevelComplete()
        {
            if (Boostrap.Instance.GameState == GameStates.GameOver) return;
            Boostrap.Instance.ChangeGameState(GameStates.LevelComplete);
        }
    }
}
using System;
using System.Collections;
using Bullet_Master_3D.Scripts.Singleton;
using UnityEngine;

namespace Bullet_Master_3D.Scripts.Game
{
    public class MachineGun : Weapon
    {
        [Header("SETTINGS")]
        [SerializeField][Range(1, 6)] private int _defaultNumberShotsPerBurst;
        [Tooltip("Time between reloading of ammo in line")]
        [SerializeField][Range(0f, 0.6f)] private float _defaultBurstReloadTime;

        private void Start()
        {
            Setup();
        }

        private void Update()
        {
            CheckLaserState();
        }

        public override void Shoot(Action<int> callback)
        {
            if(IsReadyToShot() == false) return;
            StartCoroutine(Burst(callback));
        }

        private IEnumerator Burst(Action<int> callback)
        {
            IsReloaded = false;

            var shots = 0;
            for (var num = 0; num < _defaultNumberShotsPerBurst; num++)
            {
                //If you run out of bullets in the burst, turn off the loop
                if (!HaveCartridges()) break;
                
                SpawnBullet(GunTop.position, CentralGunEnd.position);
                PlaySound(Boostrap.Instance.GameSettings.MachineGunShootSound);
                ShootParticle.Play();
                
                CartridgesCount--;
                shots++;
                yield return new WaitForSeconds(_defaultBurstReloadTime);
            }
            
            StartCoroutine(Reload());
            callback?.Invoke(shots);
        }
    }
}
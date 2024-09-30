﻿using System;
using Bullet_Master_3D.Scripts.Singleton;

namespace Bullet_Master_3D.Scripts.Game
{
    public class Pistol : Weapon
    {
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

            SpawnBullet(GunTop.position, CentralGunEnd.position);
            PlaySound(Boostrap.Instance.GameSettings.PistolShootSound);
            ShootParticle.Play();

            IsReloaded = false;
            CartridgesCount--;
            StartCoroutine(Reload());
            callback?.Invoke(Constants.SHOTS_PER_SHOT);
        }
    }
}
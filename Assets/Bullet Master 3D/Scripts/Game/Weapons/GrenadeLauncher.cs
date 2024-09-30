using System;
using Bullet_Master_3D.Scripts.Singleton;

namespace Bullet_Master_3D.Scripts.Game
{
    public class GrenadeLauncher : Weapon
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

            var ammo = SpawnBullet(GunTop.position, CentralGunEnd.position);
            //The grenade doesn't move in the direction, but in the corners, so set the angle
            ammo.transform.eulerAngles = CentralGunEnd.transform.eulerAngles;
            PlaySound(Boostrap.Instance.GameSettings.GrenadeLauncherShootSound);
            ShootParticle.Play();

            IsReloaded = false;
            CartridgesCount--;
            StartCoroutine(Reload());
            callback?.Invoke(Constants.SHOTS_PER_SHOT);
        }
    }
}
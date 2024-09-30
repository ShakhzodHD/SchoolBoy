using System;
using Bullet_Master_3D.Scripts.Singleton;

namespace Bullet_Master_3D.Scripts.Game
{
    public class Shotgun : Weapon
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

            //Spawning bullets in multiple directions using different GunEnd
            foreach (var gunEnd in GunEnds) 
            {
                SpawnBullet(GunTop.position, gunEnd.position);
            }
            PlaySound(Boostrap.Instance.GameSettings.ShotgunShootSound);
            ShootParticle.Play();

            IsReloaded = false;
            CartridgesCount--;
            StartCoroutine(Reload());
            callback?.Invoke(Constants.SHOTS_PER_SHOT);
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Bullet_Master_3D.Scripts.Game
{
    public abstract class Weapon : MonoBehaviour
    {
        [Header("TRANSFORM")]
        public Vector3 DefaultLocalPosition;
        public Vector3 DefaultLocalEulerAngles;

        [Header("SHOOTING")]
        [Tooltip("Barrel start")]
        public Transform GunTop;
        [Tooltip("Central barrel end")]
        public Transform CentralGunEnd;
        [Tooltip("All barrel ends")]
        [SerializeField] protected Transform[] GunEnds;
        [SerializeField] protected Ammo AmmoPrefab;

        [Header("FX")] 
        [Tooltip("Barrel shoot particle")]
        [SerializeField] protected ParticleSystem ShootParticle;
        
        [Header("SETTINGS")]
        [SerializeField][Range(0f, 4f)] protected float ReloadTime;
        [SerializeField][Range(0f, 25f)] protected float MaxLaserLength;

        protected bool IsReloaded { get; set; } = true;
        public bool IsBotWeapon { get; set; }
        public int CartridgesCount { get; set; }
        public Action OnOutOfAmmo;

        private AudioSource AudioSource;
        private LineRenderer _lineRenderer;
        
        /// <summary>
        /// Set default local position, local euler angles and gets the necessary links
        /// </summary>
        protected void Setup()
        {
            //Set position & rotation
            transform.localPosition = DefaultLocalPosition;
            transform.localEulerAngles = DefaultLocalEulerAngles;
            
            _lineRenderer = GetComponent<LineRenderer>();
            
            AudioSource = GetComponent<AudioSource>();
        }

        /// <summary>
        /// When there is a click on the screen, shows the laser
        /// </summary>
        protected void CheckLaserState()
        {
            //If the weapon is used by a bot, it controls the laser itself
            if(IsBotWeapon) return;

            //If the player clicks on the object ui, reloads or the game is over, the laser is not shown
            if (GetHitOnUiObjects() > 0 || !IsReloaded) 
            {
                HideLaser();
                return;
            }
            
            //If the right mouse button is pressed (touch on the phone) and there are bullets, the laser is shown
            if (Input.GetMouseButton(0) && HaveCartridges())
            {
                ShowLaser();
            }
        }
        
        private int GetHitOnUiObjects()
        {
            var pointer = new PointerEventData(EventSystem.current) {position = Input.mousePosition};
            var uiObjectsHit = new List<RaycastResult>();
            
            EventSystem.current.RaycastAll(pointer, uiObjectsHit);

            return uiObjectsHit.Count;
        }

        /// <summary>
        /// Shows laser according the weapon direction
        /// </summary>
        public void ShowLaser()
        {
            _lineRenderer.positionCount = 2;
            var laserLength = MaxLaserLength;
            var direction = (CentralGunEnd.position - GunTop.position).normalized;
            //Create the ray to subtract the distance to the nearest object
            var ray = new Ray(CentralGunEnd.position, direction);
            if (Physics.Raycast(ray, out var hit)) 
            {
                laserLength = Mathf.Clamp(Vector3.Distance(CentralGunEnd.position, hit.point), 0, MaxLaserLength);
            }
            //Set new line positions
            _lineRenderer.SetPosition(0, CentralGunEnd.position);
            _lineRenderer.SetPosition(1, CentralGunEnd.position + direction * laserLength);
        }
        
        public void HideLaser()
        {
            _lineRenderer.positionCount = 0;
        }

        protected bool IsReadyToShot()
        {
            if (IsReloaded == false)
            {
                return false;
            }
            if (HaveCartridges() == false)
            {
                OnOutOfAmmo?.Invoke();
                return false;
            }
            return true;
        }

        /// <summary>
        /// Triggers a shot and if successful, returns callback with the number of spent cartridges
        /// </summary>
        public abstract void Shoot(Action<int> callback);

        /// <summary>
        /// Instantiate new bullet and set the direction
        /// </summary>
        protected Ammo SpawnBullet(Vector3 posGunTop, Vector3 posGunEnd)
        {
            //Create a bullet and pass the settings
            var ammo = Instantiate(AmmoPrefab).GetComponent<Ammo>();
            var direction = (posGunEnd - posGunTop).normalized;
            ammo.Setup(CentralGunEnd.position, direction);
            ammo.OnAmmoDestroy += OnAmmoDestroy;
            //Vibration with constant time
            Vibration.Vibrate(Constants.WEAPON_SHOOT_VIBRATION_MILLISECONDS);
            return ammo;
        }

        /// <summary>
        /// Wait reload time and change IsReloaded to true
        /// </summary>
        protected IEnumerator Reload()
        {
            yield return new WaitForSeconds(ReloadTime);
            IsReloaded = true;
        }

        /// <summary>
        /// Returns true if cartridges count > 0
        /// </summary>
        public bool HaveCartridges()
        {
            return CartridgesCount > 0;
        }
        
        private void OnAmmoDestroy()
        {
            //If you run out of ammo and the last bullet you make is destroyed
            if (!HaveCartridges() && Ammo.SpawnedAmmoCount == 0) 
            {
                OnOutOfAmmo?.Invoke();
            }
        }

        protected void PlaySound(AudioClip audioClip)
        {
            if (audioClip == null) return;
            AudioSource.PlayOneShot(audioClip);
        }
    }
}
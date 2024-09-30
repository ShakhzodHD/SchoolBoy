using System;
using System.Collections.Generic;
using Bullet_Master_3D.Scripts.Singleton;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Bullet_Master_3D.Scripts.Game
{
    public class Player : Stickman
    {
        [Header("FOR WEAPON")]
        [Tooltip("Object to which the weapon will be spawned")]
        [SerializeField] private Transform _weaponParent;

        public event Action<int> OnShoot;
        public event Action OnFirstShoot;
        public event Action OnOutOfGuns;
        public int MaximumCartridgesCount{ get; private set; }
        public int CartridgesCount{ get; private set; }
        
        private bool _isTouched;
        private int _weaponId;
        private Camera _camera;
        private Weapon _weapon;
        
        private static readonly int WEAPON_ID_KEY = Animator.StringToHash("WeaponId");
        private static readonly int SHOOT_KEY = Animator.StringToHash("Shoot");
        private static readonly int MAIN_KEY = Animator.StringToHash("Main");

        private void Start()
        {
            Setup();
            
            _weaponId = Boostrap.Instance.LevelSettings.Weapons.Count;
            //Cache to reduce the load
            _camera = Camera.main;

            InstantiatePlayerWeapon();
            CalculateCartridgesCount();
        }

        private void InstantiatePlayerWeapon()
        {
            //If it isn't first weapon, destroy last
            if (_weapon != null)
            {
                Destroy(_weapon.gameObject);
            }
            
            var weaponSettings = Boostrap.Instance.LevelSettings.Weapons[_weaponId - 1];
            _weapon = Instantiate( Boostrap.Instance.GameSettings.WeaponsPrefabs[(int)weaponSettings.WeaponType], _weaponParent).GetComponent<Weapon>();
            _weapon.CartridgesCount = weaponSettings.CartridgesCount;
            _weapon.OnOutOfAmmo += OnOutOfAmmo;
            //Set weapon type
            SetupAnimator((int)weaponSettings.WeaponType);
            _weaponId--;
        }
        
        private void CalculateCartridgesCount()
        {
            //Calculate all cartridges count
            foreach (var weapon in  Boostrap.Instance.LevelSettings.Weapons)
            {
                MaximumCartridgesCount += weapon.CartridgesCount;
            }
            CartridgesCount = MaximumCartridgesCount;
        }

        private void SetupAnimator(int weaponId)
        {
            Animator.SetInteger(WEAPON_ID_KEY, weaponId);
            Animator.Play(MAIN_KEY);
        }

        private void Update()
        {
            if (Boostrap.Instance.GameState != GameStates.InProgress || GetHitOnUiObjects() > 0) return;

            //If the finger touches the screen and this is the first touch
            if (Input.GetMouseButtonDown(0) && !_isTouched)
            {
                _isTouched = true;
            }

            if (Input.GetMouseButton(0)) 
            {
                RotatePlayer();
            }

            //If the finger is released from the screen and _isTouched = true, make a shot
            if (Input.GetMouseButtonUp(0) && _isTouched) 
            {
                _weapon.Shoot(OnWeaponShoot);
               _isTouched = false;
            }
        }
        
        private int GetHitOnUiObjects()
        {
            var pointer = new PointerEventData(EventSystem.current) {position = Input.mousePosition};
            var uiObjectsHit = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointer, uiObjectsHit);
            return uiObjectsHit.Count;
        }

        private void RotatePlayer()
        {
            if (_camera == null) return;
            
            var ray = _camera.ScreenPointToRay(Input.mousePosition);
            var groundPlane = new Plane(Vector3.up, Vector3.zero);
            //Create a raycast to get input position on world position
            if (groundPlane.Raycast(ray, out var distance))
            {
                var mouseWorldPos = ray.GetPoint(distance);
                //Rotate playing towards this position
                transform.rotation = LookAt(mouseWorldPos);
            }
        }
        
        private void OnWeaponShoot(int shotsCount)
        {
            //If no one cartridge not used
            if (MaximumCartridgesCount == CartridgesCount)
            {
                //Inform enemies with weapons that they can fire
                OnFirstShoot?.Invoke();
            }
            
            //If it isn't last weapon spawn new
            if (_weaponId > 0 && _weapon.CartridgesCount == 0)
            {
                InstantiatePlayerWeapon();
            }
            
            CartridgesCount -= shotsCount;
            //Play shoot animation
            Animator.SetTrigger(SHOOT_KEY);
            OnShoot?.Invoke(shotsCount);
        }

        private void OnOutOfAmmo()
        {
            if (_weaponId > 0) return;
            //If it was a last weapon end the game
            OnOutOfGuns?.Invoke();
        }
    }
}

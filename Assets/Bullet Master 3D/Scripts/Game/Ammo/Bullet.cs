using System.Collections;
using UnityEngine;

namespace Bullet_Master_3D.Scripts.Game
{
    public class Bullet : Ammo
    {
        [Header("SETTINGS")]
        [Tooltip("Initial speed of bullet")] 
        [Range(20f, 70f)][SerializeField] private float _defaultSpeed;
        [Tooltip("The value on which will increase the speed at ricochet")] 
        [Range(0f, 30f)][SerializeField] private float _speedIncrease;
        [Tooltip("Maximum number of collisions after which the bullet will be destroyed")] 
        [Range(1, 20)][SerializeField] private int _maxCollisionsCount;
        [Tooltip("The time in seconds the bullet will self-destruct")] 
        [Range(0f, 10f)][SerializeField] private float _lifeTime;

        private Rigidbody _rigidbody;
        private float _speed;
        private int _collisionsCount;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            _speed = _defaultSpeed;
            SpawnedAmmoCount++;
            StartCoroutine(LifeTimer());
        }

        private IEnumerator LifeTimer()
        {
            //Destroy the bullet when life time passes
            yield return new WaitForSeconds(_lifeTime);
            Destroy(gameObject);
        }

        private void Update()
        {
            //Moving the bullet according to direction
            _rigidbody.velocity = Direction * _speed;
            //Destroy the bullet if it’s out of the camera view
            if (!IsVisibleForCamera())
            {
                Destroy(gameObject);
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            //When a bullet collides with the wall, it ricochets
            if (other.gameObject.CompareTag(Constants.WALL_TAG))
            {
                //Increase collision value to destroy bullet after maximum collisions count
                _collisionsCount++;
                
                if (_collisionsCount > _maxCollisionsCount) 
                {
                    Destroy(gameObject);
                    return;
                }
                
                //Increase bullet speed and change bullet direction
                _speed += _speedIncrease;
                Direction = Vector3.Reflect(Direction, other.contacts[0].normal).normalized;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            //If the bullet hits any stickman, let him know
            if (other.gameObject.TryGetComponent(out Stickman stickman))
            {
                stickman.OnStickmanDied(Direction, RepulsiveForce);
            }
        }

        private void OnDestroy()
        {
            SpawnedAmmoCount--;
            OnAmmoDestroy?.Invoke();
        }
    }
}
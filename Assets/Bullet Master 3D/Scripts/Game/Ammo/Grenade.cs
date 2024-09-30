using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bullet_Master_3D.Scripts.Game
{
    public class Grenade : Ammo
    {
        [SerializeField][Range(20f, 70f)] private float _speed;
        [Tooltip("Time in seconds before the grenade explodes")] 
        [SerializeField][Range(0f, 10f)] private float _lifeTime;
        [Tooltip("Prefab with effect and sound of explosion")] 
        [SerializeField] private GameObject _explosionParticlePrefab;
        
        private readonly List<GameObject> _gameObjectsInTrigger = new List<GameObject>();
        private bool _isExploded;

        private void Start()
        {
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
            //Moving the bullet forward
            transform.Translate(-Vector3.forward * _speed * Time.deltaTime);
            //Destroy the bullet if it’s out of camera view
            if (!IsVisibleForCamera())
            {
                Explode();
            }
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Untagged")) return;
            //Add gameObjects that are in the area of grenade impact to the list
            _gameObjectsInTrigger.Add(other.gameObject);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Untagged")) return;
            //Remove gameObjects that have left the grenade impact zone from list
            _gameObjectsInTrigger.Remove(other.gameObject);
        }

        private void OnCollisionEnter(Collision collision)
        {
            //Detonate the grenade in collision with any object
            Explode();
        }

        private void Explode()
        {
            //Checking before boolean variable to prevent the method from working twice
            if(_isExploded) return;
            _isExploded = true;
            
            //Running through the list of gameObjects in the grenade impact zone and let all the stickmans know they’re dead
            foreach (var gameObject in _gameObjectsInTrigger)
            {
                if (gameObject.TryGetComponent(out Stickman stickman))
                {
                    var direction = (stickman.transform.position - transform.position).normalized;
                    stickman.OnStickmanDied(direction, RepulsiveForce);
                }
            }
            //Create a separate object with blast effect and sound; separate, because our grenade is immediately destroyed
            Instantiate(_explosionParticlePrefab, transform.position, Quaternion.identity);

            SpawnedAmmoCount--;
            OnAmmoDestroy?.Invoke();
            Destroy(gameObject);
        }
    }
}
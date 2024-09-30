using System;
using UnityEngine;

namespace Bullet_Master_3D.Scripts.Game
{
    public abstract class Ammo : MonoBehaviour
    {
        [Header("SETTINGS")] 
        [Tooltip("The power with which the player will throw when hit by a bullet or grenade")] 
        [Range(0f, 500f)] public float RepulsiveForce;
        
        public static int SpawnedAmmoCount;
        public Action OnAmmoDestroy;
        protected Vector3 Direction { get; set; }
        private Camera _camera;

        /// <summary>
        /// Set the starting position and direction of the bullet or grenade
        /// </summary>
        public void Setup(Vector3 position, Vector3 direction = default)
        {
            //Cache to reduce the load
            _camera = Camera.main;
            
            transform.position = position;
            Direction = direction;
        }

        /// <summary>
        /// Check whether the bullet is within the camera’s viewport area or not
        /// </summary>
        protected bool IsVisibleForCamera()
        {
            if (_camera == null) return true;
            //Convert position to viewport point
            var point = _camera.WorldToViewportPoint(transform.position); 
            //Check if the player is out of camera visibility or not
            if(point.y < 0f || point.y > 1f || point.x > 1f || point.x < 0f) 
            {
                return false;
            }
            return true;
        }
    }
}
using System;
using Bullet_Master_3D.Scripts.Singleton;
using UnityEngine;

namespace Bullet_Master_3D.Scripts.Game
{
    public abstract class Stickman : MonoBehaviour
    {
        [Header("RIGIDBODIES")]
        [Tooltip("This rigidbody is required to add the repulsion force when a bullet hits")] 
        [SerializeField] protected Rigidbody SpineRigidbody;
        [Tooltip("All ragdoll rigidbodies")] 
        public Rigidbody[] AllRigidbodies;
        
        [Header("FX")] 
        [Tooltip("Blood effect on player hit")] 
        [SerializeField] protected ParticleSystem BloodParticle;

        public Action OnDied;
        protected Animator Animator;
        protected bool IsAlive = true;

        private CapsuleCollider _capsuleCollider;
        private AudioSource _audioSource;

        protected void Setup()
        {
            _audioSource = GetComponent<AudioSource>();
            _capsuleCollider = GetComponent<CapsuleCollider>();
            
            Animator = GetComponent<Animator>();
            
            ChangeRigidbodyState(true);
        }

        /// <summary>
        /// Changes the stickman’s state
        /// </summary>
        public void OnStickmanDied(Vector3 direction, float repulsionForce)
        {
            if (!IsAlive) return;
            //Turn off animator to avoid bugs with ragdoll
            Animator.enabled = false;
            IsAlive = false;
            //Turn off the collider to ammo could not get into it anymore
            _capsuleCollider.enabled = false;
            ChangeRigidbodyState(false);
            PlaySound();
            PlayBloodParticle(direction);
            Vibration.Vibrate(Constants.STICKMAN_DIED_VIBRATION_MILLISECONDS);
            //Apply repulsive acceleration
            var force = direction * repulsionForce;
            SpineRigidbody.AddForce(force, ForceMode.Impulse);
            
            OnDied?.Invoke();
        }
        
        /// <summary>
        /// Calculate LookAt rotation without changing y-axis
        /// </summary>
        protected Quaternion LookAt(Vector3 point)
        {
            //Get the direction to point
            var direction = (point - transform.position).normalized;
            //Create a face direction, without changing y-axis
            var faceDirection = new Vector3(direction.x, point.y, direction.z);
            //Convert direction to quaternion
            return Quaternion.LookRotation(faceDirection);
        }

        private void ChangeRigidbodyState(bool isKinematic)
        {
            //Change rigidbody state
            foreach (var rb in AllRigidbodies)
            {
                rb.isKinematic = isKinematic;
            }
        }
        
        private void PlaySound()
        {
            //If audio clip isn't null, play it
            if (Boostrap.Instance.GameSettings.StickmanDiedSound == null) return;
            _audioSource.PlayOneShot(Boostrap.Instance.GameSettings.StickmanDiedSound);
        }

        private void PlayBloodParticle(Vector3 direction)
        {
            //Change the coordinates to the coordinates of the bullet and the rotation opposite the direction of the bullet
            BloodParticle.transform.rotation = Quaternion.LookRotation(direction);
            BloodParticle.Play();
        }
    }
}
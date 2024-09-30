using UnityEngine;

namespace Bullet_Master_3D.Scripts.Game
{
    public class LookAtCameraText : MonoBehaviour
    {
        [SerializeField] private Transform _followTarget;
        [SerializeField] private Vector3 _offset;
        
        private Transform _lookAtTarget;

        private void Start()
        {
            //Cache to reduce the load
            _lookAtTarget = Camera.main.transform;
        }

        private void Update()
        {
            //Update position and rotation
            transform.position = _followTarget.position + _offset;
            transform.rotation = Quaternion.LookRotation((_lookAtTarget.position - transform.position).normalized);
        }
    }
}
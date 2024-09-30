using UnityEngine;

namespace Bullet_Master_3D.Scripts.Game
{
    public class MovableWall : MonoBehaviour
    {
        [SerializeField][Range(0.5f, 6f)] private float _speed;
        
        public Vector3 StartPoint;
        public Vector3 EndPoint;

        private Vector3 _destination;
        private bool _goingToStartPoint;
        private const float DISTANCE_ROUND = 0.5f;

        private void Start()
        {
            transform.position = StartPoint;
            _destination = EndPoint;
        }

        private void Update()
        {
            if (Vector3.Distance(transform.position, _destination) < DISTANCE_ROUND)
            {
                //Change destination
                _destination = _goingToStartPoint ? StartPoint : EndPoint;
                _goingToStartPoint = !_goingToStartPoint;
            }
            var direction = (_destination - transform.position).normalized;
            transform.Translate( direction * _speed * Time.deltaTime);
        }
    }
}
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Bullet_Master_3D.Scripts.Game
{
    public class EnemyWithNavigation : Stickman
    {
        [Header("NAVIGATION")]
        [Tooltip("Rest time in destination point")]
        [SerializeField][Range(0f, 10f)] private float _standStillTime;
    
        public Vector3 StartPoint;
        public Vector3 EndPoint;

        private NavMeshAgent _navMeshAgent;
        private bool _goingToStartPoint;
        
        private static readonly int IS_WALKING_KEY = Animator.StringToHash("IsWalking");
        private const float DISTANCE_ROUND = 0.5f;

        private void Start()
        {
            Setup();
            
            transform.position = StartPoint;
            _navMeshAgent = GetComponent<NavMeshAgent>();
            StartCoroutine(MoveEnemy());
        }

        private IEnumerator MoveEnemy()
        {
            //Set the point
            var destination = EndPoint;
            _navMeshAgent.SetDestination(destination);
            //Play animation
            Animator.SetBool(IS_WALKING_KEY, true);

            while (IsAlive) 
            {
                //If the player has reached the destination
                if (Vector3.Distance(transform.position, destination) < DISTANCE_ROUND) 
                {
                    //Stop walking animation
                    Animator.SetBool(IS_WALKING_KEY, false);
                    //Wait stand still time
                    yield return new WaitForSeconds(_standStillTime);
                    //If he’s been killed in the meantime, stop the loop
                    if (!IsAlive) yield break;
                    //Set next destination
                    destination = _goingToStartPoint ? EndPoint : StartPoint;
                    _goingToStartPoint = !_goingToStartPoint;
                    _navMeshAgent.SetDestination(destination);
                    //Play walking animation
                    Animator.SetBool(IS_WALKING_KEY, true);
                }
                //Wait for next frame
                yield return new WaitForEndOfFrame();
            }
            //If the enemy died, stop the movement
            _navMeshAgent.isStopped = true;
        }
    }
}
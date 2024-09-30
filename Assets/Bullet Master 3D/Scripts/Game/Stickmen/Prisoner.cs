using System.Collections;
using Bullet_Master_3D.Scripts.Singleton;
using UnityEngine;
using UnityEngine.AI;

namespace Bullet_Master_3D.Scripts.Game
{
    public class Prisoner : Stickman
    {
        [Header("NAVIGATION")] 
        [Tooltip("Distance from player on which prisoner will have to stop")]
        [SerializeField][Range(0f, 10f)] private float _stopDistance;
        
        private NavMeshAgent _navMeshAgent;
        private static readonly int IS_RUNNING_KEY = Animator.StringToHash("IsRunning");

        private void Start()
        {
            Setup();
            
            _navMeshAgent = GetComponent<NavMeshAgent>();
            //After the death of all enemy prisoner runs to the player
            Boostrap.Instance.OnGameStateChanged += OnGameStateChanged;
        }

        private void OnDisable()
        {
            Boostrap.Instance.OnGameStateChanged -= OnGameStateChanged;
        }

        private void OnGameStateChanged(GameStates gameState)
        {
            if (gameState != GameStates.LevelComplete) return;
            StartCoroutine(RunToPlayer());
        }

        private IEnumerator RunToPlayer()
        {
            var player = Boostrap.Instance.GameManager.PlayerSpawnerService.Player;
            var destination = player.transform.position;
            _navMeshAgent.SetDestination(destination);
            //Play running animation
            Animator.SetBool(IS_RUNNING_KEY, true);
            //Waiting when he get to the player
            yield return new WaitWhile(() => Vector3.Distance(transform.position, destination) > _stopDistance);
            //Stop the movement
            _navMeshAgent.isStopped = true;
            //Stop running animation
            Animator.SetBool(IS_RUNNING_KEY, false);
        }
    }
}
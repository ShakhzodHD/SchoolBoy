using Bullet_Master_3D.Scripts.Singleton;
using UnityEngine;

namespace Bullet_Master_3D.Scripts.Game
{
    public class PlayerSpawnerService : MonoBehaviour
    {
        public Player Player { get; private set; }

        private void Awake()
        {
            Player = Instantiate(Boostrap.Instance.GameSettings.PlayerPrefab, transform.position, transform.rotation, transform).GetComponent<Player>();
        }
    }
}
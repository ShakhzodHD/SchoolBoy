using UnityEngine;

namespace Bullet_Master_3D.Scripts.Game
{
    [CreateAssetMenu(fileName =  "GameSettings", menuName = "Settings/Game Settings")]
    public class GameSettings : ScriptableObject
    {
        [Header("PREFABS")]
        public GameObject PlayerPrefab;
        public GameObject[] WeaponsPrefabs;
        
        [Header("SOUNDS")]
        public AudioClip PistolShootSound;
        public AudioClip ShotgunShootSound;
        public AudioClip MachineGunShootSound;
        public AudioClip GrenadeLauncherShootSound;
        public AudioClip StickmanDiedSound;
    }
}
using Bullet_Master_3D.Scripts.Singleton;
using TMPro;
using UnityEngine;

namespace Bullet_Master_3D.Scripts.Menu
{
    public class LevelIdText : MonoBehaviour
    {
        private TextMeshProUGUI _text;

        private void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
        }

        private void OnEnable()
        {
            _text.text = Boostrap.Instance.ScenesService.LevelId.ToString();
        }
    }
}
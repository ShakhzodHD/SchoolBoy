using UnityEngine;
using UnityEngine.UI;

namespace Bullet_Master_3D.Scripts.Menu
{
    public class OpenLinkButton : MonoBehaviour
    {
        [SerializeField] private string _url;

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(OnButtonLinkClick);
        }

        private void OnButtonLinkClick()
        {
            Application.OpenURL(_url);
        }
    }
}
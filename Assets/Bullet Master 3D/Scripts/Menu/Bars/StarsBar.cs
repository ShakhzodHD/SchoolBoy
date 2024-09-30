using UnityEngine;
using UnityEngine.UI;

namespace Bullet_Master_3D.Scripts.Menu
{
    public class StarsBar : MonoBehaviour
    {
        [SerializeField] private Image[] _stars;
        [SerializeField] private Color _defaultStar;
        [SerializeField] private Color _darkStar;
        
        public void ShowStars(int starsCount)
        {
            for (var n = 0; n < starsCount; n++)
            {
                _stars[n].color = _defaultStar;
            }
            for (var i = starsCount; i <= _stars.Length - 1; i++)
            {
                _stars[i].color = _darkStar;
            }
        }
    }
}
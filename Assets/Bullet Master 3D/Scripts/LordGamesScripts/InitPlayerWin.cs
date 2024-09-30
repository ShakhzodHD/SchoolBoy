using Bullet_Master_3D.Scripts.Game;
using UnityEngine;

public class InitPlayerWin : MonoBehaviour
{
    private static InitPlayerWin instance;
    public static InitPlayerWin Instance
    {
        get { return instance; }
    }

    [SerializeField] private Player player1;
    [SerializeField] private Player player2;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        player1.gameObject.SetActive(false);
        player2.gameObject.SetActive(false);
    }
    public void ActivePlayerAvatars()
    {
        player1.gameObject.SetActive(true);
        player2.gameObject.SetActive(true);
    }
}

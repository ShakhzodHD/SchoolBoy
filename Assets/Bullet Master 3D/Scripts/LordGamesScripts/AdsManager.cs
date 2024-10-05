using Bullet_Master_3D.Scripts.Menu;
using Bullet_Master_3D.Scripts.Singleton;
using UnityEngine;
using YG;

public class AdsManager : MonoBehaviour
{
    private static AdsManager instance;
    public static AdsManager Instance
    {
        get { return instance; }
    }
    private void Awake()
    {
        instance = this;
    }
    private void OnEnable()
    {
        YandexGame.CloseVideoEvent += Reward;
    }
    private void OnDisable()
    {
        YandexGame.CloseVideoEvent -= Reward;
    }
    public void ShowInterlineAd()
    {
        if (YandexGame.timerShowAd >= 60)
        {
            YandexGame.FullscreenShow();
        }
    }
    public void ShowReward()
    {
        YandexGame.RewVideoShow(1);
    }
    private void Reward()
    {
        SavesService.IncreaseLevelId(Boostrap.Instance.ScenesService.LevelId, 3);
        Boostrap.Instance.GameEvents.OnLevelComplete?.Invoke();
        InitPlayerWin.Instance.ActivePlayerAvatars();
        Boostrap.Instance.ScenesService.LoadLevel();
    }
    private void Update()
    {
        Debug.Log(YandexGame.timerShowAd);
    }
}

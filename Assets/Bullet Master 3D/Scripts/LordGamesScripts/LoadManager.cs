using Bullet_Master_3D.Scripts.Menu;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class LoadManager : MonoBehaviour
{
    public static LoadManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void OnShowWindowGame()
    {
        if (YandexGame.SDKEnabled == true)
        {
            //GetLoad();
        }
    }
    public void GetLoad()
    {
        SavesService.LoadData();
    }
    private void OnEnable()
    {
        YandexGame.GetDataEvent += GetLoad;
        YandexGame.onShowWindowGame += OnShowWindowGame;
    }

    private void OnDisable()
    {
        YandexGame.GetDataEvent -= GetLoad;
        YandexGame.onShowWindowGame -= OnShowWindowGame;
    }
}

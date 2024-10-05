using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class SaveManager : MonoBehaviour
{
    private void OnEnable()
    {
        YandexGame.onHideWindowGame += OnHideWindowGame; // Подписываемся на закрытие
    }
    private void OnDisable()
    {
        YandexGame.onHideWindowGame -= OnHideWindowGame; // Отписываемся от закрытия
    }
    private void OnHideWindowGame()
    {
        
    }
}

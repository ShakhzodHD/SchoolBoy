using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class SaveManager : MonoBehaviour
{
    private void OnEnable()
    {
        YandexGame.onHideWindowGame += OnHideWindowGame; // ������������� �� ��������
    }
    private void OnDisable()
    {
        YandexGame.onHideWindowGame -= OnHideWindowGame; // ������������ �� ��������
    }
    private void OnHideWindowGame()
    {
        
    }
}

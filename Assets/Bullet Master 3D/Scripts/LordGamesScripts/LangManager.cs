using UnityEngine;
using YG;

public class LangManager : MonoBehaviour
{
    public void SetLang(string lang)
    {
        YandexGame.SwitchLanguage(lang);
    }
}

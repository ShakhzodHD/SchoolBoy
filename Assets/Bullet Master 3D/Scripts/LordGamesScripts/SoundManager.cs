using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager Instance
    {
        get { return instance; }
    }

    [SerializeField] private AudioSource audioSourceMusic;  // Для фоновой музыки
    [SerializeField] private AudioSource audioSourceSFX;    // Для звуков победы/поражения
    [SerializeField] private AudioClip[] winAndLose;        // Массив звуков (победа/поражение)
    [SerializeField] private AudioClip music;               // Фоновая музыка

    private float originalMusicVolume;  // Для сохранения оригинальной громкости музыки

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        originalMusicVolume = audioSourceMusic.volume;  // Сохраняем оригинальную громкость
        audioSourceMusic.clip = music;
        audioSourceMusic.Play();
    }

    public void PlaySoundState(int index)
    {
        if (index >= 0 && index < winAndLose.Length)
        {
            if (audioSourceSFX.isPlaying) return;
            StartCoroutine(PlayWinLoseSound(winAndLose[index]));
        }
    }

    private IEnumerator PlayWinLoseSound(AudioClip clip)
    {
        // Уменьшаем громкость фоновой музыки
        audioSourceMusic.volume = 0.05f;

        // Проигрываем звук победы/поражения
        audioSourceSFX.PlayOneShot(clip);

        // Ждём окончания проигрыша звука
        yield return new WaitForSeconds(clip.length);

        // Возвращаем громкость фоновой музыки
        audioSourceMusic.volume = originalMusicVolume;
    }
    public void StopSFX()
    {
        audioSourceSFX.Stop();
        audioSourceMusic.volume = originalMusicVolume;
    }
}

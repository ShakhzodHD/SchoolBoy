using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager Instance
    {
        get { return instance; }
    }

    [SerializeField] private AudioSource audioSourceMusic;  // ��� ������� ������
    [SerializeField] private AudioSource audioSourceSFX;    // ��� ������ ������/���������
    [SerializeField] private AudioClip[] winAndLose;        // ������ ������ (������/���������)
    [SerializeField] private AudioClip music;               // ������� ������

    private float originalMusicVolume;  // ��� ���������� ������������ ��������� ������

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        originalMusicVolume = audioSourceMusic.volume;  // ��������� ������������ ���������
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
        // ��������� ��������� ������� ������
        audioSourceMusic.volume = 0.05f;

        // ����������� ���� ������/���������
        audioSourceSFX.PlayOneShot(clip);

        // ��� ��������� ��������� �����
        yield return new WaitForSeconds(clip.length);

        // ���������� ��������� ������� ������
        audioSourceMusic.volume = originalMusicVolume;
    }
    public void StopSFX()
    {
        audioSourceSFX.Stop();
        audioSourceMusic.volume = originalMusicVolume;
    }
}

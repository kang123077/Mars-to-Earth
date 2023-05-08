using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    public List<AudioClip> bgmAudio;
    public List<AudioClip> effectAudio;

    public AudioSource bgmAudioSource;
    public AudioSource effectAudioSource;

    public float masterVolume = 1f;
    public float bgmVolume = 1f;
    public float effectVolume = 1f;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        // BGM용 AudioSource 컴포넌트 추가 및 설정
        bgmAudioSource = gameObject.AddComponent<AudioSource>();
        bgmAudioSource.loop = true;
        bgmAudioSource.volume = 1f;

        // 효과음용 AudioSource 컴포넌트 추가 및 설정
        effectAudioSource = gameObject.AddComponent<AudioSource>();
        effectAudioSource.loop = false;
        effectAudioSource.volume = 1f;
    }

    public void PlayBGM(int clipIndex)
    {
        if (clipIndex < 0 || clipIndex >= bgmAudio.Count) return; // 인덱스 범위 확인

        bgmAudioSource.clip = bgmAudio[clipIndex];
        bgmAudioSource.volume = masterVolume * bgmVolume; // 볼륨 설정
        bgmAudioSource.Play();
    }

    public void PlayEffect(int clipIndex)
    {
        if (clipIndex < 0 || clipIndex >= effectAudio.Count) return; // 인덱스 범위 확인

        effectAudioSource.clip = effectAudio[clipIndex];
        effectAudioSource.volume = masterVolume * effectVolume; // 볼륨 설정
        effectAudioSource.Play();
    }

    public void SetMasterVolume(float value)
    {
        masterVolume = value;
        UpdateAllVolumes();
    }

    public void SetBGMVolume(float value)
    {
        bgmVolume = value;
        UpdateAllVolumes();
    }

    public void SetEffectVolume(float value)
    {
        effectVolume = value;
        UpdateAllVolumes();
    }

    private void UpdateAllVolumes()
    {
        bgmAudioSource.volume = masterVolume * bgmVolume;
        effectAudioSource.volume = masterVolume * effectVolume;
    }
}

using System.Collections.Generic;
using UnityEngine;


public enum CombatEffectClip
{
    buzz,
    charge,    
    explosion1,
    explosion2,
    fire,
    gravity,
    hit1,
    hit2,
    hitExplotion,
    itemUse,
    massShoot,
    missile,
    parryingKick,
    parrying,
    revolver,
    rolling,
    run,
    smash,
    steam,
    swing,
    titanStep,
    titanVoice,
    walk,    
}

public class AudioManager : Singleton<AudioManager>
{
    public AudioClip[] BGM_AudioClips;
    public AudioClip[] UI_EffectAudioClips;
    public AudioClip[] CombatEffectAudioClips;

    public AudioSource bgmAudioSource;
    public AudioSource effectAudioSource;
    private List<AudioSource> allSources;

    private float masterVolume = 1f;
    private float bgmVolume = 1f;
    private float effectVolume = 1f;

    private float finalEffectVolume=0.6f;
    private float finalBGM_Volume=0.6f;

    protected override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        // BGM용 AudioSource 컴포넌트 추가 및 설정
        bgmAudioSource = gameObject.AddComponent<AudioSource>();
        bgmAudioSource.loop = true;
        bgmAudioSource.volume = 1f;
        bgmAudioSource.spatialize = false;//3D공간에 영향을받지않음

        // 효과음용 AudioSource 컴포넌트 추가 및 설정
        effectAudioSource = gameObject.AddComponent<AudioSource>();
        effectAudioSource.loop = false;
        effectAudioSource.volume = 1f;
        effectAudioSource.spatialize = false;

        allSources = new List<AudioSource>();
    }

    public void PlayBGM(int clipIndex)
    {
        if (clipIndex < 0 || clipIndex >= BGM_AudioClips.Length) return; // 인덱스 범위 확인

        bgmAudioSource.clip = BGM_AudioClips[clipIndex];
        bgmAudioSource.volume = finalBGM_Volume; // 볼륨 설정
        bgmAudioSource.Play();
    }

    public void PlayEffect(int clipIndex ) 
    {
        if (clipIndex < 0 || clipIndex >= UI_EffectAudioClips.Length) return; // 인덱스 범위 확인
        
        effectAudioSource.clip = UI_EffectAudioClips[clipIndex];
        effectAudioSource.volume = finalEffectVolume; // 볼륨 설정
        effectAudioSource.Play();       
    }
    public void PlayEffect(int clipIndex, AudioSource source)
    {
        if (clipIndex < 0 || clipIndex >= CombatEffectAudioClips.Length) return; // 인덱스 범위 확인
        
        source.clip = CombatEffectAudioClips[clipIndex];
        source.volume = finalEffectVolume; // 볼륨 설정
        if(source.transform.parent.gameObject.activeSelf)
            source.Play();

        allSources.Add(source);
    }

    public void CombatSoundControl()
    {
        foreach(AudioSource source in allSources)
        {
            source.Stop();
        }
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
        finalBGM_Volume = masterVolume * bgmVolume;
        finalEffectVolume = masterVolume * effectVolume;
    }
}

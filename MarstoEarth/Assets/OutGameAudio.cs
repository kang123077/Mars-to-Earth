using UnityEngine;

public class OutGameAudio : Singleton<OutGameAudio>
{
    public AudioClip[] _BGM_AudioClips;
    public AudioClip[] _UI_EffectAudioClips;
    public AudioSource bgms;
    public AudioSource effes;

    protected override void Awake()
    {
        AudioManager.BGM_AudioClips = _BGM_AudioClips;
        AudioManager.UI_EffectAudioClips = _UI_EffectAudioClips;
    }

    private void Start()
    {
        // BGM용 AudioSource 컴포넌트 추가 및 설정
        bgms = gameObject.AddComponent<AudioSource>();
        bgms.loop = true;
        bgms.volume = 1f;
        bgms.spatialize = false;//3D공간에 영향을받지않음

        // 효과음용 AudioSource 컴포넌트 추가 및 설정
        effes = gameObject.AddComponent<AudioSource>();
        effes.loop = false;
        effes.volume = 1f;
        effes.spatialize = false;

        PlayBGM(0);
    }

    public void PlayBGM(int clipIndex)
    {
        if (clipIndex < 0 || clipIndex >= _BGM_AudioClips.Length) return; // 인덱스 범위 확인

        bgms.clip = _BGM_AudioClips[clipIndex];
        bgms.volume = AudioManager.finalBGM_Volume; // 볼륨 설정
        bgms.Play();
    }

    public void PlayEffect(int clipIndex)
    {
        if (clipIndex < 0 || clipIndex >= _UI_EffectAudioClips.Length) return; // 인덱스 범위 확인

        effes.clip = _UI_EffectAudioClips[clipIndex];
        effes.volume = AudioManager.finalEffectVolume; // 볼륨 설정
        effes.Play();
    }
}

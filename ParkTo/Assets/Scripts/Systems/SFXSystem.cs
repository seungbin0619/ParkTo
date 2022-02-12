using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXSystem : MonoBehaviour
{
    #region [ 인스턴스 초기화 ]

    public static SFXSystem instance;
    public AudioSource current;
    public AudioSource sound;

    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);

        current = GetComponent<AudioSource>();

        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(sound.gameObject);
    }

    #endregion

    [System.Serializable]
    public class SoundData
    {
        public AudioClip sound;
        public float volume = 1;
        public float progress = 0;
    }

    [SerializeField]
    private SoundData[] bgm;

    [SerializeField]
    private SoundData[] sounds;

    private int theme = -1;
    private SoundData currentBgm;

    public void OnThemeChanged()
    {
        StartCoroutine(SoundChange());
    }

    public IEnumerator SoundChange()
    {
        if (theme == ThemeSystem.CurrentTheme.index) yield break;

        WaitForEndOfFrame delay = new WaitForEndOfFrame();
        float progress = 0, duration = 0.5f;
        float weight = DataSystem.GetData("Setting", "Bgm", 0) * 0.01f;
        float volume;

        if (current.isPlaying)
        {
            // 페이드 아웃
            while (progress < duration)
            {
                volume = 1 - progress / duration;
                current.volume = weight * volume * currentBgm.volume;

                yield return delay;
                progress += Time.deltaTime;
            }
            current.volume = 0;

            currentBgm = null;
            current.Stop();
            current.clip = null;
        }

        theme = ThemeSystem.CurrentTheme.index;

        currentBgm = bgm[theme];
        current.clip = currentBgm.sound;
        current.Play();

        // 페이드 인
        progress = 0;
        while (progress < duration)
        {
            volume = progress / duration;
            current.volume = weight * volume * currentBgm.volume;

            yield return delay;
            progress += Time.deltaTime;
        }

        current.volume = weight * currentBgm.volume;
    }

    public void PlaySound(int index)
    {
        if (index < 0 || index >= sounds.Length) return;
        float weight = DataSystem.GetData("Setting", "Sound", 0) * 0.01f;

        sound.clip = sounds[index].sound;
        sound.volume = weight * sounds[index].volume;
        sound.PlayScheduled(sounds[index].progress);
    }

    private void Update()
    {
        //
    }

    public void OnSoundChange()
    {
        float bgmWeight = DataSystem.GetData("Setting", "Bgm", 0) * 0.01f;
        //float soundsWeight = DataSystem.GetData("Setting", "Sound", 0) * 0.01f;

        current.volume = bgmWeight / currentBgm.volume;
        //if(currentSound != null)
        //    sound.volume = soundsWeight / currentSound.volume;
    }
}

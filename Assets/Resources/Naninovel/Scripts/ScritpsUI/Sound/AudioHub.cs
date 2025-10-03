using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class AudioHub : MonoBehaviour
{
    public static AudioHub I { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource music;
    [SerializeField] private AudioSource sfx;  
    [SerializeField] private AudioSource ui;    

    [Header("Default volumes")]
    [Range(0, 1)] public float musicVol = 0.6f;
    [Range(0, 1)] public float sfxVol = 0.5f;
    [Range(0, 1)] public float uiVol = 0.5f;

    private readonly Dictionary<string, AudioSource> managedSfx = new();

    void Awake()
    {
        if (I && I != this) { Destroy(gameObject); return; }
        I = this;
        DontDestroyOnLoad(gameObject);

        if (!music) music = gameObject.AddComponent<AudioSource>();
        if (!sfx) sfx = gameObject.AddComponent<AudioSource>();
        if (!ui) ui = gameObject.AddComponent<AudioSource>();

        music.loop = true; music.playOnAwake = false;
        sfx.playOnAwake = false;
        ui.playOnAwake = false;

        music.volume = musicVol;
        sfx.volume = sfxVol;
        ui.volume = uiVol;

        SceneManager.activeSceneChanged += (_, __) => StopAllSfx(0.08f);
    }

    public void PlayMusic(string id, float? vol = null, float fade = 0.25f)
    {
        if (string.IsNullOrWhiteSpace(id)) return;
        var clip = Resources.Load<AudioClip>($"Naninovel/Audio/BGM/{id}");
        if (!clip) { Debug.LogWarning($"[AudioHub] BGM '{id}' not found."); return; }
        StartCoroutine(FadeMusicTo(clip, vol ?? musicVol, fade));
    }
    public void StopMusic(float fade = 0.25f) => StartCoroutine(FadeOutMusic(fade));
    System.Collections.IEnumerator FadeMusicTo(AudioClip newClip, float targetVol, float fade)
    {
        if (music.isPlaying && music.clip == newClip) { music.volume = targetVol; yield break; }
        for (float t = 0; t < fade; t += Time.unscaledDeltaTime) { music.volume = Mathf.Lerp(targetVol, 0f, t / fade); yield return null; }
        music.Stop();
        music.clip = newClip; music.volume = 0f; music.Play();
        for (float t = 0; t < fade; t += Time.unscaledDeltaTime) { music.volume = Mathf.Lerp(0f, targetVol, t / fade); yield return null; }
        music.volume = targetVol;
    }
    System.Collections.IEnumerator FadeOutMusic(float fade)
    {
        float start = music.volume;
        for (float t = 0; t < fade; t += Time.unscaledDeltaTime) { music.volume = Mathf.Lerp(start, 0f, t / fade); yield return null; }
        music.Stop();
    }

    public void PlaySfx(string id, float? vol = null)
    {
        if (string.IsNullOrWhiteSpace(id)) return;
        var clip = Resources.Load<AudioClip>($"Naninovel/Audio/SFX/{id}");
        if (!clip) { Debug.LogWarning($"[AudioHub] SFX '{id}' not found."); return; }
        sfx.PlayOneShot(clip, vol ?? sfxVol);
    }
    public void PlayUi(string id = "ui-click", float? vol = null)
    {
        if (string.IsNullOrWhiteSpace(id)) id = "ui-click";
        var clip = Resources.Load<AudioClip>($"Naninovel/Audio/SFX/{id}");
        if (!clip) { Debug.LogWarning($"[AudioHub] UI '{id}' not found."); return; }
        ui.PlayOneShot(clip, vol ?? uiVol);
    }

    public void PlaySfxManaged(string key, string clipId, bool loop = true, float? vol = null, float fade = 0.15f)
    {
        if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(clipId)) return;

        var clip = Resources.Load<AudioClip>($"Naninovel/Audio/SFX/{clipId}");
        if (!clip) { Debug.LogWarning($"[AudioHub] Managed SFX '{clipId}' not found."); return; }

        if (!managedSfx.TryGetValue(key, out var src) || !src)
        {
            src = gameObject.AddComponent<AudioSource>();
            src.playOnAwake = false;
            managedSfx[key] = src;
        }

        StartCoroutine(PlayManagedRoutine(src, clip, loop, vol ?? sfxVol, fade));
    }

    public void StopSfx(string key, float fade = 0.15f)
    {
        if (!managedSfx.TryGetValue(key, out var src) || !src) return;
        StartCoroutine(FadeOutAndStop(src, fade));
    }

    public void StopAllSfx(float fade = 0.15f)
    {
        foreach (var kv in managedSfx)
            if (kv.Value) StartCoroutine(FadeOutAndStop(kv.Value, fade));
    }

    System.Collections.IEnumerator PlayManagedRoutine(AudioSource src, AudioClip clip, bool loop, float vol, float fade)
    {
        if (src.isPlaying)
            yield return FadeOutAndStop(src, fade);

        src.clip = clip;
        src.loop = loop;
        src.volume = 0f;
        src.Play();

        for (float t = 0; t < fade; t += Time.unscaledDeltaTime)
        {
            src.volume = Mathf.Lerp(0f, vol, t / fade);
            yield return null;
        }
        src.volume = vol;
    }

    System.Collections.IEnumerator FadeOutAndStop(AudioSource src, float fade)
    {
        float start = src.volume;
        for (float t = 0; t < fade; t += Time.unscaledDeltaTime)
        {
            src.volume = Mathf.Lerp(start, 0f, t / fade);
            yield return null;
        }
        src.Stop();
        src.clip = null;
    }
}
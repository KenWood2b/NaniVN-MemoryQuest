using UnityEngine;

public sealed class MainMenuAudio : MonoBehaviour
{
    [Header("BGM id из Resources/Naninovel/Audio/BGM")]
    public string menuBgmId = "MainMenu";
    [Range(0f, 1f)] public float volume = 0.6f;
    public float fade = 0.35f;

    void OnEnable()
    {
        if (AudioHub.I) AudioHub.I.PlayMusic(menuBgmId, volume, fade);
    }

    void OnDisable()
    {
        if (AudioHub.I) AudioHub.I.StopMusic(fade);
    }
}

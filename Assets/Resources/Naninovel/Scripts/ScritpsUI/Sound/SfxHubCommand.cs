using System.Collections.Generic;
using Naninovel;
using UnityEngine;

[CommandAlias("sfxhub")]
public sealed class SfxHubCommand : Command
{
    [ParameterAlias("id")] public StringParameter Id;
    [ParameterAlias("vol")] public DecimalParameter Vol;
    [ParameterAlias("loop")] public BooleanParameter Loop;
    [ParameterAlias("key")] public StringParameter Key;
    [ParameterAlias("stop")] public BooleanParameter Stop;

    static readonly Dictionary<string, AudioSource> loops = new();

    public override UniTask ExecuteAsync(AsyncToken token = default)
    {
        if (Stop)
        {
            var k = Assigned(Key) ? Key.Value : null;
            if (!string.IsNullOrEmpty(k) && loops.TryGetValue(k, out var a))
            {
                if (a) Object.Destroy(a.gameObject);
                loops.Remove(k);
            }
            else AudioHub.I?.StopAllSfx(); // запасной вариант
            return UniTask.CompletedTask;
        }

        var id = Assigned(Id) ? Id.Value : null;
        if (string.IsNullOrEmpty(id)) return UniTask.CompletedTask;

        var vol = Assigned(Vol) ? (float)Vol.Value : (AudioHub.I ? AudioHub.I.sfxVol : 1f);

        if (Assigned(Loop) && Loop.Value)
        {
            var clip = Resources.Load<AudioClip>($"Naninovel/Audio/SFX/{id}");
            if (!clip) { Debug.LogWarning($"[sfxhub] '{id}' not found"); return UniTask.CompletedTask; }

            var go = new GameObject($"SFXLoop_{id}");
            Object.DontDestroyOnLoad(go);
            var a = go.AddComponent<AudioSource>();
            a.loop = true; a.playOnAwake = false; a.clip = clip; a.volume = vol; a.Play();

            var k = Assigned(Key) ? Key.Value : id;
            loops[k] = a;
        }
        else
            AudioHub.I?.PlaySfx(id, vol);

        return UniTask.CompletedTask;
    }
}

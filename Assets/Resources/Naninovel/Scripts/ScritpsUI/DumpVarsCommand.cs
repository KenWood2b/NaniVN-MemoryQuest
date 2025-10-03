using System.Collections.Generic;
using System.Text;
using Naninovel;

[CommandAlias("dumpVars")]
public sealed class DumpVarsCommand : Command
{

    [ParameterAlias("names")] public StringListParameter Names;

    public override UniTask ExecuteAsync(AsyncToken token = default)
    {
        var vars = Engine.GetService<ICustomVariableManager>();
        if (vars == null)
        {
            UnityEngine.Debug.LogWarning("[DumpVars] ICustomVariableManager not available.");
            return UniTask.CompletedTask;
        }

        var names = new List<string>();
        if (Names != null && Names.Value != null && Names.Value.Count > 0)
        {
            foreach (var ns in Names.Value)
                names.Add(ns?.ToString());
        }
        else
        {
            names.AddRange(new[]
            {
                "lastPairsResult","loc1Locked","loc2Locked","loc3Locked","hasAmulet","nextLoc"
            });
        }

        var sb = new StringBuilder("[DumpVars] ");
        for (int i = 0; i < names.Count; i++)
        {
            var key = names[i];
            var val = vars.GetVariableValue(key);
            sb.Append(key).Append('=').Append(val ?? "null");
            if (i < names.Count - 1) sb.Append(", ");
        }

        UnityEngine.Debug.Log(sb.ToString());
        return UniTask.CompletedTask;
    }
}

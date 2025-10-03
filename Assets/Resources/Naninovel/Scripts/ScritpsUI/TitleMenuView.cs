using System.Threading.Tasks;       
using Naninovel;
using Naninovel.UI;
using UnityEngine;
using UnityEngine.UI;

public class TitleMenuView : MonoBehaviour
{
    [Header("Buttons")]
    public Button newGame;
    public Button loadGame;
    public Button settings;
    public Button exitGame;

    [SerializeField] private string startScriptName = "Loc1";

    private IUIManager ui;
    private IScriptPlayer player;
    private IStateManager state;

    void Awake()
    {
        ui = Engine.GetService<IUIManager>();
        player = Engine.GetService<IScriptPlayer>();
        state = Engine.GetService<IStateManager>();

        newGame.onClick.AddListener(OnNewGame);
        loadGame.onClick.AddListener(OpenLoad);
        settings.onClick.AddListener(OpenSettings);
        exitGame.onClick.AddListener(ExitGame);
    }

    async void OnNewGame()
    {
        await state.ResetStateAsync();

      
        _ = ui.GetUI<MapUI>();
        await Task.Yield();

        await player.PreloadAndPlayAsync(startScriptName);

        gameObject.SetActive(false);
    }

    void OpenLoad() => ui.GetUI<ISaveLoadUI>()?.Show();
    void OpenSettings() => ui.GetUI<ISettingsUI>()?.Show();

    void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}

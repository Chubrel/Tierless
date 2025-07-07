using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;

namespace Tierless;

[BepInPlugin(Guid, Name, Version)]
public class Tierless : BaseUnityPlugin
{
    public const string Version = "0.1.0";
    public const string Guid = "com.github.chubrel.Tierless";
    public const string Name = "Tierless";

    public static Tierless Instance { get; private set; } = null!;
    public new ManualLogSource Logger => base.Logger;
    private Harmony? Harmony { get; set; }

    private void Awake()
    {
        Instance = this;

        // Prevent the plugin from being deleted
        gameObject.transform.parent = null;
        gameObject.hideFlags = HideFlags.HideAndDontSave;

        LevelSetupDataManager.Instance.InitializeDefaults();
        EnemySetupDataManager.Instance.InitializeDefaults();
        
        Patch();

        // Logger.LogInfo($"{Info.Metadata.GUID} v{Info.Metadata.Version} has loaded!");
    }

    public void Patch()
    {
        Harmony ??= new Harmony(Info.Metadata.GUID);
        Harmony.PatchAll();
    }

    public void Unpatch()
    {
        Harmony?.UnpatchSelf();
    }
}
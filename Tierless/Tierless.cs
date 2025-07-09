using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;

namespace Tierless;

[BepInPlugin(Guid, Name, Version)]
public class Tierless : BaseUnityPlugin
{
    public const string Version = "1.1.0";
    public const string Guid = "com.github.chubrel.Tierless";
    public const string Name = "Tierless";

    public static readonly string SaveDirectorsID = FullID("SaveDirectors");

    public static Tierless Instance { get; private set; } = null!;
    public new ManualLogSource Logger => base.Logger;
    private Harmony? Harmony { get; set; }

    private void Awake()
    {
        Instance = this;

        gameObject.transform.parent = null;
        gameObject.hideFlags = HideFlags.HideAndDontSave;

        LevelSetupDataManager.Instance.InitializeDefaults();
        EnemySetupDataManager.Instance.InitializeDefaults();
        
        Patch();
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

    public static string FullID(string suffix)
    {
        return Utils.FullID(Guid, suffix);
    }
}
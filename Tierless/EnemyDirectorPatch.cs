using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using UnityEngine;
// ReSharper disable InconsistentNaming

namespace Tierless;

[HarmonyPatch(typeof(EnemyDirector))]
public static class EnemyDirectorPatch
{
    public static void AddEnemies(ICollection<EnemyData> uniqueEnemies, ICollection<GameObject> directors, int tier, IEnumerable<EnemySetup> enemySetups)
    {
        // ReSharper disable once CollectionNeverUpdated.Local
        // var directorData = new DefaultDictionary<EnemyName, EnemyWithDirector>(() => new EnemyWithDirector());
        foreach (var enemySetup in enemySetups)
        {
            foreach (var gameObject in enemySetup.spawnObjects)
            {
                var name = gameObject.name;
                
                if (EnemyName.IsEnemyGroup(name))
                {
                    continue;
                }

                var enemyName = EnemyName.FromAnyName(name);
                if (EnemyName.IsEnemyDirector(name))
                {
                    directors.Add(gameObject);
                    continue;
                }
                
                // directorData[enemyName].Enemy = gameObject;
                
                var enemyData = new EnemyData(gameObject, EnemySetupDataManager.Instance.GetOrDefault(enemyName, tier));
                uniqueEnemies.Add(enemyData);
            }
        }

        // foreach (var entry in directorData)
        // {
        //     var enemyWithDirector = entry.Value;
        //     
        //     if (!enemyWithDirector.IsReady)
        //         continue;
        //     
        //     enemyWithDirector.Set();
        // }
    }

    public static EnemyDataContext ChooseEnemies(ICollection<EnemyData> uniqueEnemies)
    {
        if (uniqueEnemies.Count == 0)
        {
            Tierless.Instance.Logger.LogWarning("No enemies were found!");
            return EnemyDataContext.Empty;
        }
        
        int level = RunManager.instance.levelsCompleted + 1;
        var levelSetupData = LevelSetupDataManager.Instance.GetOrDefault(level);

        var maxEnemyTypeCountExclusive = levelSetupData.MaxEnemyTypeCountLimited(uniqueEnemies.Count) + 1;
        var enemyTypeCountToChoose = (Random.RandomRangeInt(levelSetupData.MinEnemyTypeCount, maxEnemyTypeCountExclusive) + Random.RandomRangeInt(levelSetupData.MinEnemyTypeCount, maxEnemyTypeCountExclusive)) / 2;
        var uniqueEnemiesList = uniqueEnemies.ToList();
        uniqueEnemiesList.Shuffle();
        var result = new EnemyDataContext();
        foreach (var enemyData in uniqueEnemiesList)
        {
            var newTotalScore = result.TotalScore + enemyData.Data.ScoreWithFormula(1);
            if (newTotalScore > levelSetupData.TargetScore)
                continue;
            
            result.AddAndUpdate(enemyData, result.TypeCount + 1, newTotalScore);
            
            if (result.TypeCount >= enemyTypeCountToChoose)
                break;
        }
        
        bool newAdded;
        do
        {
            newAdded = false;
            foreach (var enemyData in result.EnemyDataEnumerable())
            {
                var countAndScore = result.CountAndScore(enemyData);
                var newTotalScore = result.TotalScore - countAndScore.Score + enemyData.Data.ScoreWithFormula(countAndScore.Count + 1);
                if (newTotalScore > levelSetupData.TargetScore)
                    continue;

                newAdded = true;
                result.AddAndUpdate(enemyData, countAndScore.Count + 1, newTotalScore);
                break;
            }
        } while (newAdded);

        if (result.TypeCount == 0)
        {
            Tierless.Instance.Logger.LogWarning("No enemies were chosen!");
        }
        
        return result;
    }
    
    [HarmonyPrefix, HarmonyPatch(nameof(EnemyDirector.PickEnemies))]
    public static bool PickEnemies_Prefix(EnemyDirector __instance, List<EnemySetup> _enemiesList)
    {
        if (__instance.enemyListCurrent.Count != 0)
            return false;
        
        var uniqueEnemies = new HashSet<EnemyData>();
        var directors = new List<GameObject>();
        AddEnemies(uniqueEnemies, directors, 1, __instance.enemiesDifficulty1);
        AddEnemies(uniqueEnemies, directors, 2,  __instance.enemiesDifficulty2);
        AddEnemies(uniqueEnemies, directors, 3, __instance.enemiesDifficulty3);
        var finalSetup = ScriptableObject.CreateInstance<EnemySetup>();
        var enemyDataContext = ChooseEnemies(uniqueEnemies);
        finalSetup.spawnObjects = enemyDataContext.EnemyDataCounter.Aggregate(new List<GameObject>(), (result, entry) => {
            for (int i = 0; i < entry.Value.Count; i++)
            {
                result.Add(entry.Key.GameObject);
            }
            return result;
        });
        finalSetup.spawnObjects.AddRange(directors);
        
        __instance.enemyList.Add(finalSetup);
        __instance.enemyListCurrent.Add(finalSetup);
        return false;
    }

    [HarmonyPostfix, HarmonyPatch(nameof(EnemyDirector.AmountSetup))]
    public static void AmountSetup_Postfix(EnemyDirector __instance)
    {
        __instance.totalAmount = __instance.enemyListCurrent.Count;
    }
}
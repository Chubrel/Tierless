using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using UnityEngine;

namespace Tierless;

[HarmonyPatch(typeof(EnemyDirector))]
public static class EnemyDirectorPatch
{
    public static void AddEnemies(ICollection<EnemyData> uniqueEnemies, int tier, IEnumerable<EnemySetup> enemySetups)
    {
        foreach (var enemySetup in enemySetups)
        {
            foreach (var gameObject in enemySetup.spawnObjects)
            {
                // Tierless.Logger.LogWarning(gameObject.name + " - " + EnemyName.IsEnemyGroup(gameObject.name));
                if (EnemyName.IsEnemyGroup(gameObject.name))
                    continue;
                
                uniqueEnemies.Add(new EnemyData(gameObject, EnemySetupDataManager.Instance.GetOrDefault(gameObject.name, tier)));
            }
        }
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

        // var notSelected = new RandomRemoveCollection<EnemyData>(uniqueEnemies);
        var maxEnemyTypeCountExclusive = levelSetupData.MaxEnemyTypeCountLimited(uniqueEnemies.Count) + 1;
        var enemyTypeCountToChoose = (Random.RandomRangeInt(levelSetupData.MinEnemyTypeCount, maxEnemyTypeCountExclusive) + Random.RandomRangeInt(levelSetupData.MinEnemyTypeCount, maxEnemyTypeCountExclusive)) / 2;
        var uniqueEnemiesList = uniqueEnemies.ToList();
        uniqueEnemiesList.Shuffle();
        var result = new EnemyDataContext();
        for (var i = 0; i < uniqueEnemies.Count; i++)
        {
            var enemyData = uniqueEnemiesList[i];
            var countAndScore = result.CountAndScore(enemyData);
            var newTotalScore = result.TotalScore + enemyData.Data.ScoreWithFormula(1);
            if (newTotalScore > levelSetupData.TargetScore)
                continue;
            
            result.AddAndUpdate(enemyData, countAndScore.Count + 1, newTotalScore);
            
            if (result.TypeCount >= enemyTypeCountToChoose)
                break;
        }
        
        bool newAdded;
        do
        {
            // Tierless.Instance.Logger.LogWarning(result.EnemyDataCounter.Count + " - " + result.TotalScore);
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
        
        // // to remove
        // var uniqueEnemiesToSort = uniqueEnemies.Aggregate(new List<EnemyData>(), (list, enemyData) =>
        // {
        //     list.Add(enemyData);
        //     return list;
        // });
        // uniqueEnemiesToSort.Sort();
        // var uniqueEnemiesSorted = uniqueEnemiesToSort.ToArray();
        // var seen = new HashSet<OptimizedEnemyDataContext>();
        // var results = new List<OptimizedEnemyDataContext>();
        // var search = new 
        // for (int i = 0; i < search.Length; i++)
        // {
        //     var countAndScore = uniqueEnemiesSorted[i].Data.EvaluateMaxCountWithScore(levelSetupData.TargetScore);
        //     var countsAndScores = new CountAndScore[search.Length];
        //     countsAndScores[i] = countAndScore;
        //     search[i] = new OptimizedEnemyDataContext(countsAndScores, countAndScore.Score);
        // }
        // while (search.Length != 0)
        // {
        //     // var enemyDataContext = search.First();
        //     search.Remove(result);
        //     bool addedNew = false;
        //
        //     foreach (var enemyData in uniqueEnemies)
        //     {
        //         var newEnemyDataContext = result.CopyAndProcess(enemyData);
        //         
        //         if (!seen.Add(newEnemyDataContext) || newEnemyDataContext.TotalScore > levelSetupData.TargetScore || !levelSetupData.CheckEnemyTypeCount(result.EnemyDataCounter.Count))
        //             continue;
        //         
        //         addedNew = true;
        //         search.Add(newEnemyDataContext);
        //     }
        //
        //     if (!addedNew)
        //     {
        //         results.Add(result);
        //     }
        // }
    }
    
    [HarmonyPrefix, HarmonyPatch(nameof(EnemyDirector.PickEnemies))]
    public static bool PickEnemies_Prefix(EnemyDirector __instance, List<EnemySetup> _enemiesList)
    {
        // Tierless.Logger.LogWarning(__instance + " - " + _enemiesList);
        if (__instance.enemyListCurrent.Count != 0)
            return false;
        var uniqueEnemies = new HashSet<EnemyData>();
        AddEnemies(uniqueEnemies, 1, __instance.enemiesDifficulty1);
        // Tierless.Logger.LogWarning(uniqueEnemies);
        AddEnemies(uniqueEnemies, 2,  __instance.enemiesDifficulty2);
        // Tierless.Logger.LogWarning(uniqueEnemies);
        AddEnemies(uniqueEnemies, 3, __instance.enemiesDifficulty3);
        // Tierless.Logger.LogWarning(uniqueEnemies.Count);
        var finalSetup = ScriptableObject.CreateInstance<EnemySetup>();
        var enemyDataContext = ChooseEnemies(uniqueEnemies);
        // Tierless.Logger.LogWarning(enemyDataContext);
        finalSetup.spawnObjects = enemyDataContext.EnemyDataCounter.Aggregate(new List<GameObject>(), (result, entry) => {
            // Tierless.Logger.LogWarning(entry);
            for (int i = 0; i < entry.Value.Count; i++)
            {
                result.Add(entry.Key.GameObject);
            }
            return result;
        });
        // Tierless.Logger.LogWarning(finalSetup.spawnObjects.Count);
        __instance.enemyList.Add(finalSetup);
        // Tierless.Logger.LogWarning(__instance.enemyList);
        __instance.enemyListCurrent.Add(finalSetup);
        // Tierless.Logger.LogWarning(__instance.enemyListCurrent);
        // Code to execute for each PlayerController *before* Start() is called.
        // Tierless.Logger.LogDebug($"{__instance} Start Prefix");
        return false;
    }

    [HarmonyPostfix, HarmonyPatch(nameof(EnemyDirector.AmountSetup))]
    public static void AmountSetup_Postfix(EnemyDirector __instance)
    {
        // Tierless.Logger.LogWarning(__instance.totalAmount + " " + __instance.enemyListCurrent.Count);
        __instance.totalAmount = __instance.enemyListCurrent.Count;
        // Tierless.Logger.LogWarning(__instance.totalAmount);
    }
}
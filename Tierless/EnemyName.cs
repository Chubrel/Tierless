﻿using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Tierless;

public record EnemyName(string ShortName) : IEqualityComparer<EnemyName>
{
    public const string FullNamePrefix = "Enemy - ";
    public const string IsEnemyGroupRegex = @"((\bEnemy\s+Group\s*-)|(-\s*((\d+.+)|(.+\d+))))";
    public const string Director = "Director";
    
    public string ShortName { get; } = ShortName;
    public string FullName => FullNamePrefix + ShortName;

    public static EnemyName FromShortName(string shortName)
    {
        return new EnemyName(shortName.Trim());
    }
    
    public static EnemyName FromAnyName(string anyName)
    {
        var separatorIndex = anyName.IndexOf('-');
        if (separatorIndex == -1)
            return FromShortName(anyName);
        
        var rawName = anyName[(separatorIndex + 1)..].TrimEnd();
        if (rawName.EndsWith(Director))
            return FromShortName(rawName[..^Director.Length]);
        
        return FromShortName(rawName);
    }

    public static bool IsEnemyGroup(string name)
    {
        return Regex.IsMatch(name, IsEnemyGroupRegex, RegexOptions.IgnoreCase);
    }

    public static bool IsEnemyDirector(string name)
    {
        return name.Contains(Director);
    }

    public override int GetHashCode()
    {
        return ShortName.GetHashCode();
    }

    public bool Equals(EnemyName? x, EnemyName? y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (x is null) return false;
        if (y is null) return false;
        if (x.GetType() != y.GetType()) return false;
        return x.ShortName == y.ShortName;
    }

    public int GetHashCode(EnemyName obj)
    {
        return obj.ShortName.GetHashCode();
    }
}

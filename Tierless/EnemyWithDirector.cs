using UnityEngine;

namespace Tierless;

public record EnemyWithDirector(GameObject? Enemy, GameObject? Director)
{
    public EnemyWithDirector() : this(null, null)
    {
    }
    
    public GameObject? Enemy { get; set; } = Enemy;
    public GameObject? Director { get; set; } = Director;
    // public MonoBehaviour? MonoDirector { get; set; } = (MonoBehaviour) Director;

    public bool IsReady => Enemy is not null && Director is not null;

    public void Set()
    {
        Director?.transform.SetParent(Enemy?.transform);
    }
}
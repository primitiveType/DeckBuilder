using App;
using SummerJam1;
using SummerJam1.Units;
using UnityEngine;

public class IntentFactory : View<Unit>
{
    [SerializeField] private IntentComponentView prefab;
    protected override void Start()
    {
        base.Start();
        var intents = Entity.GetComponents<Intent>();
        foreach (var intent in intents)
        {
            var go = Instantiate(prefab, transform);
            go.SetModel(intent);
            var componentsInChildren = go.GetComponentsInChildren<ISetModel>();
            foreach (var componentsInChild in componentsInChildren)
            {
                componentsInChild.SetModel(intent);
            }
        }    
    }
}

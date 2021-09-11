using UnityEngine;

public class NodeHighlightPathEffectComponent : EffectComponent
{
    [SerializeField] private GameObject HighlightObject;

    private void OnEnable()
    {
        HighlightObject.SetActive(true);
    }

    private void OnDisable()
    {
        HighlightObject.SetActive(false);
    }
}
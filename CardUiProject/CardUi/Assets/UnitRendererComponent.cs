using System;
using System.ComponentModel;
using App;
using SummerJam1;
using SummerJam1.Units;
using UnityEngine;

public class UnitRendererComponent : View<VisualComponent>
{
    [SerializeField] private GameObject m_CurrentRenderer;

    // Start is called before the first frame update
    [PropertyListener(nameof(VisualComponent.AssetName))]
    private void OnAssetNameChanged(object sender, PropertyChangedEventArgs args)
    {
        Disposables.Add(AnimationQueue.Instance.Enqueue(() =>
        {
            if (Entity.GetComponentInParent<FriendlyUnitSlot>() != null)
            {
                var scale = m_CurrentRenderer.transform.localScale;
                m_CurrentRenderer.transform.localScale = new Vector3(-1 * Mathf.Abs(scale.x), scale.y, scale.z);
            }
        }));
    }
}

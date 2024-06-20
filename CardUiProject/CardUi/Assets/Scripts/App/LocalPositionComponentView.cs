using System;
using System.Threading.Tasks;
using Api;
using App.Utility;
using CardsAndPiles.Components;
using External.UnityAsync.UnityAsync.Assets.UnityAsync;
using UnityEngine;

namespace App
{
    public class LocalPositionComponentView : ComponentView<IPosition>
    {
        [SerializeField] private bool m_Interpolate;
        [SerializeField] private float m_Speed = 1;


        protected override void Start()
        {
            base.Start();
            transform.SetParent(MapContainer.Instance.transform);
            ComponentOnPropertyChanged();
        }

        protected override void ComponentOnPropertyChanged()
        {
            if (Component == null)
            {
                Debug.LogError("Position was null!", gameObject);
            }

            if (m_Interpolate)
            {
                Disposables.Add(AnimationQueue.Instance.Enqueue(async () =>
                    await Lerp(Component.Pos.ToUnityVector3())));
            }
            else
            {
                transform.localPosition = Component.Pos.ToUnityVector3();
            }
        }

        private async Task Lerp(Vector3 position)
        {
            float t = 0;

            Vector3 start = transform.localPosition;
            while (t < 1)
            {
                await new WaitForEndOfFrame();
                transform.localPosition = Vector3.Lerp(start, position, t);
                t += (Time.deltaTime * m_Speed);
            }

            transform.localPosition = position;
        }
    }
}

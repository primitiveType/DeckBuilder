using System.ComponentModel;
using App;
using SummerJam1.Objectives;
using UnityEngine;
using UnityEngine.UI;

namespace SummerJam1
{
    public class ObjectiveView : View<Objective>, ISetModel
    {
        [SerializeField] private Button Button;
        [SerializeField] private GameObject CompletedGraphic;
        [SerializeField] private GameObject FailedGraphic;

        protected override void Start()
        {
            base.Start();
            Button?.onClick.AddListener(ButtonClicked);
            Model.PropertyChanged += EntityOnPropertyChanged;
            UpdateVisibility();
        }

        private void EntityOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateVisibility();
        }

        private void UpdateVisibility()
        {
            if (CompletedGraphic)
            {
                CompletedGraphic.SetActive(Model.Completed && !Model.Failed);
            }

            if (FailedGraphic)
            {
                FailedGraphic.SetActive(Model.Failed);
            }
        }

        private void ButtonClicked()
        {
            Entity.GetComponentInParent<SummerJam1Game>().Battle.ChooseObjective(Entity);
            GetComponentInParent<ObjectivePileView>().gameObject.SetActive(false);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            Model.PropertyChanged -= EntityOnPropertyChanged;
        }
    }
}

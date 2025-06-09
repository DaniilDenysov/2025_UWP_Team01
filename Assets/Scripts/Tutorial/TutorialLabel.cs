using System;
using TMPro;
using TowerDeffence.ObjectPools;
using TowerDeffence.UI.Strategies;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace TowerDeffence.Tutorial
{
    public class TutorialLabel : MonoBehaviour
    {
        [SerializeField] private TMP_Text descriptionDisplay;
        [SerializeField] private Button submitButton;
        [SerializeReference, SubclassSelector] private ScreenStrategy screenStrategy;
        public static Action OnSubmitted;

        private IObjectPool<TutorialLabel> objectPool;

        [Inject]
        private void Construct(IObjectPool<TutorialLabel> objectPool)
        {
            this.objectPool = objectPool;
        }

        public void Initialize(string description)
        {
            descriptionDisplay.text = description;
            submitButton.onClick.AddListener(FulfillCondition);
            SetActive(true);
        }

        public void SetActive(bool state)
        {
            screenStrategy.SetActive(state);
        }

        private void FulfillCondition()
        {
            OnSubmitted?.Invoke();
        }

        private void OnDestroy()
        {
            submitButton.onClick.RemoveListener(FulfillCondition);
        }

        public void OnDisable()
        {
            submitButton.onClick.RemoveListener(FulfillCondition);
            objectPool.ReleaseObject(this);
        }
    }
}

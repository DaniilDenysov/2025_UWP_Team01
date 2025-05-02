using System;
using TMPro;
using TowerDeffence.ObjectPools;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace TowerDeffence.Tutorial
{
    public class TutorialLabel : MonoBehaviour
    {
        [SerializeField] private TMP_Text descriptionDisplay;
        [SerializeField] private Button submitButton;

        public static Action OnSubmitted;

        private ObjectPoolWrapper<TutorialLabel> objectPool;

        [Inject]
        private void Construct(ObjectPoolWrapper<TutorialLabel> objectPool)
        {
            this.objectPool = objectPool;
        }

        public void Initialize(string description)
        {
            descriptionDisplay.text = description;
            submitButton.onClick.AddListener(FulfillCondition);
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
            objectPool.Release(this);
        }
    }
}

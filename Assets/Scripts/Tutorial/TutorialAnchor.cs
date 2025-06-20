using System;
using System.Collections;
using System.Collections.Generic;
using TowerDeffence.ObjectPools;
using UnityEngine;
using Zenject;
using System.Linq;

namespace TowerDeffence.Tutorial
{
    public class TutorialAnchor : MonoBehaviour
    {
        private static Dictionary<int,TutorialAnchor> tutorialAnchors = new Dictionary<int, TutorialAnchor>();
        public static IReadOnlyList<TutorialAnchor> TutorialAnchors =>
            tutorialAnchors.OrderBy(a => a.Key).Select(a => a.Value).ToList();


        [SerializeField] private TutorialLabel tutorialLabelPrefab;

        [SerializeField, TextArea] private string description;
        [SerializeField] private int order;
        public int Order => order;

        private IObjectPool<TutorialLabel> objectPool;
        private TutorialLabel cachedLabel;

        [Inject]
        private void Construct(IObjectPool<TutorialLabel> objectPool)
        {
            this.objectPool = objectPool;
        }

        private void OnEnable()
        {
            tutorialAnchors.TryAdd(order,this);
        }

        private void OnDisable()
        {
            if (tutorialAnchors.TryGetValue(order, out var anchor))
            {
               if (anchor == this) tutorialAnchors.Remove(order);
            }
        }


        public void StartStep()
        {
            cachedLabel = objectPool.GetObject(tutorialLabelPrefab);
            cachedLabel.transform.SetParent(transform);
            cachedLabel.transform.position = transform.position;
            cachedLabel.Initialize(description);
        }

        public void StopStep()
        {
            cachedLabel.SetActive(false);
        }

        private void OnDestroy()
        {
            //cachedLabel?.SetActive(false);
            //if (tutorialAnchors.TryGetValue(order, out var anchor))
            //{
            //    if (anchor == this) tutorialAnchors.Remove(order);
            //}
        }
    }
}

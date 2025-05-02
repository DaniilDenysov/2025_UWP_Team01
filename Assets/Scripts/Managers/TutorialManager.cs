using System.Collections;
using TowerDeffence.ObjectPools;
using TowerDeffence.Tutorial;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDeffence.Managers
{
    public class TutorialManager : MonoBehaviour
    {
        [SerializeField] private bool enableTutorial;
        private int currentStep;

        private void Start()
        {
            if (enableTutorial && TutorialAnchor.TutorialAnchors.Count > 0)
            {
                TutorialLabel.OnSubmitted += OnSubmitted;
                currentStep = 0;
                TutorialAnchor.TutorialAnchors[currentStep].StartStep();
            }
        }

        private void OnSubmitted()
        {
            TutorialAnchor.TutorialAnchors[currentStep].StopStep();

            currentStep++;

            if (currentStep < TutorialAnchor.TutorialAnchors.Count)
            {
                TutorialAnchor.TutorialAnchors[currentStep].StartStep();
            }
            else
            {
                TutorialLabel.OnSubmitted -= OnSubmitted;
            }
        }

        private void OnDestroy()
        {
            TutorialLabel.OnSubmitted -= OnSubmitted;
        }
    }
}

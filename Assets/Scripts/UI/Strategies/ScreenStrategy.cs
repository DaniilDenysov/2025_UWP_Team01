using DG.Tweening;
using UnityEngine;

namespace TowerDeffence.UI.Strategies
{
    [System.Serializable]
    public abstract class ScreenStrategy
    {
        public abstract void SetActive(bool state);
        public abstract bool IsActive();
    }

    [System.Serializable]
    public class SingleScreenStrategy : ScreenStrategy
    {
        [SerializeField] private GameObject screen;

        public override bool IsActive()
        {
            if (screen == null) return false;
            return screen.activeInHierarchy;
        }

        public override void SetActive(bool state)
        {
            if (screen == null) return;
            screen.SetActive(state);
        }
    }

    [System.Serializable]
    public class FadeInAndOutSingleScreenStrategy : ScreenStrategy
    {
        [SerializeReference, SubclassSelector] private ScreenStrategy screen;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private float fadeDuration = 0.5f;

        private Tween fadeTween;

        public override bool IsActive()
        {
            if (screen == null) return false;
            return screen.IsActive();
        }

        public override void SetActive(bool state)
        {
            if (screen == null || canvasGroup == null) return;
            fadeTween?.Kill();

            if (state)
            {
                screen.SetActive(true);
                canvasGroup.alpha = 0f;
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;

                fadeTween = canvasGroup.DOFade(1f, fadeDuration).SetEase(Ease.OutQuad).OnComplete(() =>
                {
                    canvasGroup.interactable = true;
                    canvasGroup.blocksRaycasts = true;
                });
            }
            else
            {
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;

                fadeTween = canvasGroup.DOFade(0f, fadeDuration).SetEase(Ease.OutQuad).OnComplete(() =>
                {
                    screen.SetActive(false);
                });
            }
        }
    }

    [System.Serializable]
    public class ScreenItem
    {
        public GameObject screen;
        public bool invertState;
    }


    [System.Serializable]
    public class MultiScreenStrategy : ScreenStrategy
    {
        [SerializeField] private ScreenItem[] screenItems;
        private bool state;

        public override bool IsActive()
        {
            return state;
        }

        public override void SetActive(bool state)
        {
            this.state = state;
            foreach (ScreenItem item in screenItems)
            {
                if (item == null) continue;
                if (item.screen == null) continue;
                item.screen.SetActive(item.invertState ? !state : state);
            }
        }
    }
}
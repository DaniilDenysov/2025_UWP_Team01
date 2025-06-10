using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonInteraction : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Button Sounds")]
    [SerializeField] private AudioClipSO click;
    [SerializeField] private AudioClipSO hover;

    private SoundManager soundManager;
    private void Start()
    {
        soundManager = FindObjectOfType<SoundManager>();

        if (soundManager == null)
        {
            Debug.Log("Sound Manager not found");
            return;
        }  
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        soundManager.PlayOneShot(click);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        soundManager.PlayOneShot(hover);
    }


    public void OnPointerExit(PointerEventData eventData)
    {

    }
}
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DelayedButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private float delay = 0.5f;
    [SerializeField] public UnityEvent onClick;
    public void OnPointerClick(PointerEventData eventData)
    {
        this.Invoke("DelayedCall",delay);
    }
    private void DelayedCall()
    {
        onClick?.Invoke();
    }
}

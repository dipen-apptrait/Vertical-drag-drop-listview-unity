using UnityEngine;
using UnityEngine.EventSystems;


public class DraggableElementHandle : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField] private DraggableElement draggableElement;
        
    private void Awake()
    {
        if(draggableElement == null) draggableElement = GetComponentInParent<DraggableElement>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        draggableElement.StartDragging();
    }

    public void OnDrag(PointerEventData eventData)
    {
        draggableElement.Drag(eventData.position);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        draggableElement.OnDragEnd();
    }
}
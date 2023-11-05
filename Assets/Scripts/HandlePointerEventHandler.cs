using UnityEngine;
using UnityEngine.EventSystems;

namespace DefaultNamespace
{
    public class HandlePointerEventHandler : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        private DraggableCardHandle _cardHandle;
        
        private void Awake()
        {
            _cardHandle = GetComponent<DraggableCardHandle>();
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            _cardHandle.OnPointerDown(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            _cardHandle.OnDrag(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _cardHandle.OnPointerUp(eventData);
        }
    }
}
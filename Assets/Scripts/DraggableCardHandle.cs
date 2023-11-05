using UnityEngine;
using UnityEngine.EventSystems;

namespace DefaultNamespace
{
    public class DraggableCardHandle : MonoBehaviour
    {
        [SerializeField] private DraggableCard draggableCard;
        [SerializeField] private bool endDragOnDisable;

        private void Awake()
        {
            if(draggableCard == null) draggableCard = GetComponentInParent<DraggableCard>();
        }

        private void Start()
        {
            var eventTrigger = draggableCard.CardPanelBase.eventTrigger;
            if (eventTrigger == null)
            {
                gameObject.AddComponent<HandlePointerEventHandler>();
            }
            else
            {
                var downEntry = new EventTrigger.Entry
                {
                    eventID = EventTriggerType.PointerDown,
                };
                downEntry.callback.AddListener(OnPointerDown);
                eventTrigger.triggers.Add(downEntry);
                    
                var dragEntry = new EventTrigger.Entry
                {
                    eventID = EventTriggerType.Drag,
                };
                dragEntry.callback.AddListener(OnDrag);
                eventTrigger.triggers.Add(dragEntry);
                
                var upEntry = new EventTrigger.Entry
                {
                    eventID = EventTriggerType.PointerUp,
                };
                upEntry.callback.AddListener(OnPointerUp);
                eventTrigger.triggers.Add(upEntry);
            }
        }

        private void OnDisable()
        {
            if(endDragOnDisable) draggableCard.OnDragEnd();
        }

        public void OnPointerDown(BaseEventData arg0)
        {
            draggableCard.StartDragging();
        }

        public void OnDrag(BaseEventData eventData)
        {
            if (eventData is PointerEventData pointerEvent) draggableCard.Drag(pointerEvent.delta);
        }

        public void OnPointerUp(BaseEventData eventData)
        {
            draggableCard.OnDragEnd();
        }
    }
}
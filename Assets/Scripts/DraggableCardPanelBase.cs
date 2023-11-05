using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class DraggableCardPanelBase : MonoBehaviour
    {
        public event Action<DraggableCardEventArgs> OnDragStartEvent;
        public event Action<DraggableCardEventArgs> OnDragEvent;
        public event Action<DraggableCardEventArgs> OnDragEndEvent;
        
        public int GetChildCount => transform.childCount;
        
        public LayoutGroup LayoutGroup { get; private set; }

        public EventTrigger eventTrigger;
        
        protected void Awake()
        {
            LayoutGroup = GetComponent<LayoutGroup>();
        }

        private void Start()
        {
            eventTrigger ??= FindObjectOfType<EventTrigger>();
        }

        public virtual void OnDragStart(DraggableCardEventArgs args) => OnDragStartEvent?.Invoke(args);

        public virtual void OnDrag(DraggableCardEventArgs args) => OnDragEvent?.Invoke(args);

        public virtual void OnDragEnd(DraggableCardEventArgs args) => OnDragEndEvent?.Invoke(args);
        
        public Transform GetChild(int index)
        {
            return transform.GetChild(index);
        }
    }

    public struct DraggableCardEventArgs
    {
        public int StartIndex;
        public int EndIndex;
        public DraggableCard Card;
    }
}
using System;
using UnityEngine;

public class DraggableElementsPanel : MonoBehaviour
{
    public event Action<OnDragEventArgs> OnDragEvent;
        
    public int GetChildCount => transform.childCount;

    public void OnDrag(OnDragEventArgs args)
    {
        OnDragEvent?.Invoke(args);
    }
        
    public Transform GetChild(int index)
    {
        return transform.GetChild(index);
    }
}

public struct OnDragEventArgs
{
    public int StartIndex;
    public int EndIndex;
    public Transform Transform;
}
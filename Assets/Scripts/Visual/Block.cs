using System;
using UnityEngine;

public class Block : MonoBehaviour
{
    public event EventHandler OnBlockDestroyed;

    /// <summary>
    /// Call to safely destroy object and call its event. When OnDestroy was used it caused bugs in BlockGroup script.
    /// </summary>
    public void DestroyBlock()
    {
        OnBlockDestroyed?.Invoke(this,EventArgs.Empty);
        Destroy(gameObject);
    }
}
using System;
using UnityEngine;

public class Block : MonoBehaviour
{
    public event EventHandler OnBlockDestroyed;

    public void DestroyBlock()
    {
        OnBlockDestroyed?.Invoke(this,EventArgs.Empty);
        Destroy(gameObject);
    }
}
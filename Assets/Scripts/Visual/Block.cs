using System;
using UnityEngine;

public class Block : MonoBehaviour
{
    public event EventHandler OnBlockDestroyed;

    private void OnDestroy() 
    {
        OnBlockDestroyed?.Invoke(this,EventArgs.Empty);
    }
}
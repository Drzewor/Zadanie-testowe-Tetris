using System;
using UnityEngine;

public class BorderLine : MonoBehaviour
{
    [SerializeField] private Transform PlayerOneRaycastPoint;
    [SerializeField] private Transform PlayerTwoRaycastPoint;
    [SerializeField] private LayerMask blockLayerMask;

    public bool HasBlocksOnLine(float distance, PlayerType playerType)
    {
        Transform raycastPoint;

        switch (playerType)
        {
            case PlayerType.PlayerOne:
                raycastPoint = PlayerOneRaycastPoint;
                break;
            case PlayerType.PlayerTwo:
                raycastPoint = PlayerTwoRaycastPoint;
                break;
            default:
                return false;
        }

        RaycastHit2D hit = Physics2D.Raycast(raycastPoint.position, Vector2.right, distance,blockLayerMask);

        if(hit)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

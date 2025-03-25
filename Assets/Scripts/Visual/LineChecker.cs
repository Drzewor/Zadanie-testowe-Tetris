using UnityEngine;

public class LineChecker : MonoBehaviour
{
    [SerializeField] private LayerMask blockLayerMask;
    [SerializeField] private Transform playerOneRaycastPoint;
    [SerializeField] private Transform playerTwoRaycastPoint;

    /// <summary>
    /// Check if line is full of blocks by comparing amount of block hittet by raycast and raycast distance.
    /// </summary>
    /// <param name="distance"></param>
    /// <param name="heightOffset"></param>
    /// <param name="playerType"></param>
    /// <param name="hitArray"></param>
    /// <returns></returns>
    public bool CheckLine(float distance, float heightOffset, PlayerType playerType, out RaycastHit2D[] hitArray)
    {
        Vector2 origin;

        switch (playerType)
        {
            case PlayerType.PlayerOne:
                origin = playerOneRaycastPoint.position;
                break;
            case PlayerType.PlayerTwo:
                origin = playerTwoRaycastPoint.position;
                break;
            default:
                hitArray = null;
                return false;
        }    
        
        origin += Vector2.up * heightOffset;

        hitArray = Physics2D.RaycastAll(origin, Vector2.right, distance,blockLayerMask);

        if(hitArray.Length >= Mathf.Round(distance))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

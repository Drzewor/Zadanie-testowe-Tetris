using UnityEngine;

public class LineChecker : MonoBehaviour
{
    [SerializeField] private LayerMask blockLayerMask;
    [SerializeField] private Transform playerOneRaycastPoint;
    [SerializeField] private Transform playerTwoRaycastPoint;


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

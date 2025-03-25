using UnityEngine;

public class NextBlockVisual : MonoBehaviour
{
    [SerializeField] Transform blockSpawnPoint;
    private BlockGroup currentBlockGroup;

    /// <summary>
    /// Display given BlockGroup. If there is any currentBlockGroup then it is destroyed
    /// </summary>
    /// <param name="blockGroup"></param>
    public void DisplayBlockVisualistaion(Transform blockGroup)
    {
        if(currentBlockGroup != null)
        {
            Destroy(currentBlockGroup.gameObject);
        }

        Transform newblockGroup = Instantiate(blockGroup,blockSpawnPoint);
        currentBlockGroup = newblockGroup.GetComponent<BlockGroup>();
        Rigidbody2D rigidbody = newblockGroup.GetComponent<Rigidbody2D>();
        rigidbody.bodyType = RigidbodyType2D.Kinematic;
    }
}

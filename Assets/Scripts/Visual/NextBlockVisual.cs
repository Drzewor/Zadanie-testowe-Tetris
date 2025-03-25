using UnityEngine;

public class NextBlockVisual : MonoBehaviour
{
    [SerializeField] Transform blockSpawnPoint;
    private BlockGroup currentBlockGroup;

    public void SpawnNextBlockVisualistaion(Transform blockGroup)
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

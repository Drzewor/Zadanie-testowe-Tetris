using UnityEngine;

public class NextBlockVisual : MonoBehaviour
{
    [SerializeField] Transform blockSpawnPoint;

    public void SpawnNextBlockVisualistaion(Transform blockGroup)
    {
        foreach (Transform child in blockSpawnPoint)
        {
            Destroy(child.gameObject);
        }

        Transform newblockGroup = Instantiate(blockGroup,blockSpawnPoint);
        Rigidbody2D rigidbody = newblockGroup.GetComponent<Rigidbody2D>();
        rigidbody.bodyType = RigidbodyType2D.Kinematic;
    }
}

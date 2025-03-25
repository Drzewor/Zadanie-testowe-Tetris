using System.Collections.Generic;
using UnityEngine;

public class BlockGroupSpawner : MonoBehaviour
{
    [SerializeField] private Transform playerOneSpawnPoint;
    [SerializeField] private Transform playerTwoSpawnPoint;
    [SerializeField] private NextBlockVisual playerOneNextBlockVisual;
    [SerializeField] private NextBlockVisual playerTwoNextBlockVisual;
    [SerializeField] private List<Transform> blockGroupsPrefabList;
    private int playerOneNextBlockIndex = 0;
    private int playerTwoNextBlockIndex = 0;

    public BlockGroup SpawnNextBlocks(PlayerType playerType)
    {
        Transform spawnPoint;
        int nextBlockIndex;

        switch (playerType)
        {
            case PlayerType.PlayerOne:
                spawnPoint = playerOneSpawnPoint;
                nextBlockIndex = playerOneNextBlockIndex;
                playerOneNextBlockIndex = Random.Range(0,blockGroupsPrefabList.Count);
                playerOneNextBlockVisual.SpawnNextBlockVisualistaion(blockGroupsPrefabList[playerOneNextBlockIndex]);
                break;
            case PlayerType.PlayerTwo:
                spawnPoint = playerTwoSpawnPoint;
                nextBlockIndex = playerTwoNextBlockIndex;
                playerTwoNextBlockIndex = Random.Range(0,blockGroupsPrefabList.Count);
                playerTwoNextBlockVisual.SpawnNextBlockVisualistaion(blockGroupsPrefabList[playerTwoNextBlockIndex]);
                break;
            default:
                return null;
        }

        Transform blockGroupTransform = Instantiate(
            blockGroupsPrefabList[nextBlockIndex], 
            spawnPoint.position, 
            Quaternion.identity);
        
        if(blockGroupTransform.TryGetComponent(out BlockGroup blockGroup))
        {
            blockGroup.SetPlayerType(playerType);
            return blockGroup;
        }
        else
        {
            Debug.LogError($"Assigned blockgroup with index {nextBlockIndex} does not have BlockGroup component! {transform}");
            return null;
        }
    }
}

using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{    
    [SerializeField] private float arenaHeight = 12;
    [SerializeField] private float arenaWidht = 9;
    private InputManager inputManager;
    private BlockGroup blockGroupOne;
    private BlockGroup blockGroupTwo;
    private bool isPlayerOnePlaying = true;
    private bool isPlayerTwoPlaying = true;
    private BlockGroupSpawner spawner;
    private LineChecker lineChecker;
    private BorderLine borderLine;
    private PointsCounter pointsCounter;

    public void SetUp()
    {
        spawner = FindFirstObjectByType<BlockGroupSpawner>();
        lineChecker = FindFirstObjectByType<LineChecker>();
        borderLine = FindFirstObjectByType<BorderLine>();
        pointsCounter = FindFirstObjectByType<PointsCounter>();
        inputManager = InputManager.Instance;

        SpawnNextBlockGroup(PlayerType.PlayerOne);
        SpawnNextBlockGroup(PlayerType.PlayerTwo);
    }

    private void Update()
    {
        if(inputManager == null) return;

        HandlePlayerOneInputs();
        
        HandlePlayerTwoInputs();
    }

    private void HandlePlayerOneInputs()
    {
        if(!isPlayerOnePlaying) return;

        if(inputManager.IsRotateButtonPressedPlayerOne())
        {
            blockGroupOne.Rotate();
        }

        if(inputManager.GetHorizontalMovePlayerOne() != 0)
        {
            blockGroupOne.MoveGroup(InputManager.Instance.GetHorizontalMovePlayerOne());
        }
    }

    private void HandlePlayerTwoInputs()
    {
        if(!isPlayerTwoPlaying) return;

        if(inputManager.IsRotateButtonPressedPlayerTwo())
        {
            blockGroupTwo.Rotate();
        }

        if(inputManager.GetHorizontalMovePlayerTwo() != 0)
        {
            blockGroupTwo.MoveGroup(InputManager.Instance.GetHorizontalMovePlayerTwo());
        }
    }

    private void blockGroup_OnValidCollision(object sender, BlockGroup.OnValidCollisionEventArgs e)
    {
        BlockGroup blockGroup;

        switch (e.playerType)
        {
            case PlayerType.PlayerOne:
                if(!isPlayerOnePlaying) return;

                if(borderLine.HasBlocksOnLine(arenaWidht ,e.playerType))
                {
                    isPlayerOnePlaying = false;
                    return;
                }

                blockGroup = blockGroupOne;

                break;
            case PlayerType.PlayerTwo:
                if(!isPlayerTwoPlaying) return;

                if(borderLine.HasBlocksOnLine(arenaWidht ,e.playerType))
                {
                    isPlayerTwoPlaying = false;
                    return;
                }

                blockGroup = blockGroupTwo;

                break;
            default:
                Debug.LogError($"This block group {sender} has no player assigned");
                return;
        }

        if(e.isControlled)
        {
            blockGroup.SetIsController(false);
            blockGroup.OnValidCollision -= blockGroup_OnValidCollision;

            SpawnNextBlockGroup(e.playerType);
        }

        for(float heightOffset = 0; heightOffset < arenaHeight; heightOffset++)
        {
            if(lineChecker.CheckLine(arenaWidht, heightOffset, e.playerType, out RaycastHit2D[] hitArray))
            {
                foreach (RaycastHit2D hit in hitArray)
                {
                    hit.collider.GetComponent<Block>().DestroyBlock();
                }
                pointsCounter.IncreasePoints(e.playerType);
            }
        }

    }

    private void SpawnNextBlockGroup(PlayerType playerType)
    {
        switch (playerType)
        {
            case PlayerType.PlayerOne:
                blockGroupOne = spawner.SpawnNextBlocks(playerType);
                blockGroupOne.SetIsController(true);
                blockGroupOne.OnValidCollision += blockGroup_OnValidCollision;
                break;
            case PlayerType.PlayerTwo:
                blockGroupTwo = spawner.SpawnNextBlocks(playerType);
                blockGroupTwo.SetIsController(true);
                blockGroupTwo.OnValidCollision += blockGroup_OnValidCollision;
                break;
            default:
                return;
        }    
    }
}

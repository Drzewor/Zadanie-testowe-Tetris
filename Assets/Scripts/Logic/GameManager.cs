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

    /// <summary>
    /// Call to prepare and activate GameManager and start spawning Blocks.
    /// </summary>
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

        if(InputManager.Instance.IsESCButtonPressed())
        {
            Application.Quit();
        }
    }

    //Handle Inputs form First (Left) Player
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

    //Handle Inputs form Second (Right) Player
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

    //Called when blockGroup collide with floor or other block. 
    private void blockGroup_OnValidCollision(object sender, BlockGroup.OnValidCollisionEventArgs e)
    {
        BlockGroup blockGroup;

        //Chcek if blockgorup is not crossing border (arena top) and assing wich BlockGroup should be handled
        //depends on playerType
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

        //if event is called by BlockGroup that player controll then control is taken from block and new BlockGroup
        //is spawned
        if(e.isControlled)
        {
            blockGroup.SetIsController(false);
            blockGroup.OnValidCollision -= blockGroup_OnValidCollision;

            SpawnNextBlockGroup(e.playerType);
        }

        //checki if there is any full line of blocks, and if it is then is destroyed and point is added
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

    /// <summary>
    /// Spawn new BlockGroup for a given playerType
    /// </summary>
    /// <param name="playerType"></param>
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

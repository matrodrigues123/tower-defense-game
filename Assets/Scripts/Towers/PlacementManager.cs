using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    public ShopManager shopManager;
    public GameObject basicTowerObject;
    private GameObject currentTowerPlacing;
    private GridCell hoverCell;
    private GameObject dummyPlacement;

    public Camera cam;
    public LayerMask mask;
    private bool isBuilding;

    public void GetTilePosition()
    {
        Vector2 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, 0.0f, mask, -100, 100);
        if (hit.collider != null)
        {
            GameObject hitObj = hit.transform.gameObject;
            for (int y = 0; y < MapGenerator.mapHeight; y++)
            {
                for (int x = 0; x < MapGenerator.mapWidth; x++)
                {
                    if (MapGenerator.mapTiles[y, x].gameObj == hitObj)
                    {
                        hoverCell = MapGenerator.mapTiles[y, x];
                        break;
                    }
                }
            }        
        }

    }
    public void StartBuilding(GameObject towerToBuild)
    {
        isBuilding = true;
        currentTowerPlacing = towerToBuild;
        dummyPlacement = Instantiate(currentTowerPlacing);
        // Get the SpriteRenderer component of the dummyPlacement object
        SpriteRenderer spriteRenderer = dummyPlacement.GetComponent<SpriteRenderer>();

        // Set the alpha value of the sprite's color to 0 (completely transparent)
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0f);


        if (dummyPlacement.GetComponent<Tower>() != null)
        {
            Destroy(dummyPlacement.GetComponent<Tower>());
        }
        if (dummyPlacement.GetComponent<BarrelRotation>() != null)
        {
            Destroy(dummyPlacement.GetComponent<BarrelRotation>());
        }
    }

    public void PlaceBuilding()
    {
        if (shopManager.CanBuyTower(currentTowerPlacing))
        {
            Destroy(dummyPlacement);
            GameObject newTower = Instantiate(currentTowerPlacing);
            newTower.transform.position = hoverCell.gameObj.transform.position;
            Tower towerProps = newTower.GetComponent<Tower>();
            // if the tower is not visible to the enemy, 
            // the cell remains walkable
            hoverCell.isWalkable = !towerProps.isVisibleToEnemy;
            shopManager.BuyTower(currentTowerPlacing);
            EndBuilding();
            
        }
    }
    public void InterruptPlaceBuilding()
    {
        Destroy(dummyPlacement);
        EndBuilding();
    }
    public void EndBuilding()
    {
        isBuilding = false;
    }

    private bool CanPlaceTowerAt(GridCell currCell)
    {
        if (currCell == null || !hoverCell.isWalkable
            || currCell.gameObj.transform.position.y == 0
            || currCell.gameObj.transform.position.y == MapGenerator.mapHeight - 1)
        {
            return false;
        }

        // check if there is atleast one other unblocked tile in the row
        int cellRow = (int)currCell.gameObj.transform.position.y;
        int cellCol = (int)currCell.gameObj.transform.position.x;
        for (int col = 0; col < MapGenerator.mapWidth ; col++)
        {
            if (col != cellCol && MapGenerator.mapTiles[cellRow, col].isWalkable)
            {
                return true;
            }
        }

        return false;
    }
    public void Update()
    {
        if (isBuilding)
        {
            if (dummyPlacement != null)
            {
                GetTilePosition();
                if (CanPlaceTowerAt(hoverCell))
                {
                    dummyPlacement.transform.position = hoverCell.gameObj.transform.position;

                    // Get the SpriteRenderer component of the dummyPlacement object
                    SpriteRenderer spriteRenderer = dummyPlacement.GetComponent<SpriteRenderer>();
                    // Set the alpha value of the sprite's color to 0.2 (barely visible)
                    spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0.2f);

                    if (Input.GetMouseButtonDown(0))
                    {
                        PlaceBuilding();
                    }
                    else if (Input.GetMouseButtonDown(1))
                    {
                        InterruptPlaceBuilding();
                    }
                }
                
            }
        }
    }
}

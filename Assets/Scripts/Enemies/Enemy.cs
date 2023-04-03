using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField]
    private float enemyHealth;

    [SerializeField] private int killReward;
    [SerializeField] private int damage;
    [SerializeField] public bool flyer;

    private GridCell currCell;
    private Vector2 currPos;

    private void Awake()
    {
        Enemies.enemies.Add(gameObject);
    }

    public void Start()
    {
        // currPos and currCell initialized (defined on round controlled)
        currPos = gameObject.transform.position;
        currCell = MapGenerator.mapTiles[(int)currPos.y, (int)currPos.x];

        // coroutine -> moves every second
        MoveEnemy();
    }

    protected virtual void DestroyEnemy()
    {
        Enemies.enemies.Remove(gameObject);
        Destroy(gameObject);
    }

    public void TakeDamage(float amount)
    {
        enemyHealth -= amount;
        if (enemyHealth <= 0)
        {
            MoneyManager.AddMoney(killReward);
            DestroyEnemy();
        }
    }

    private void MoveEnemy()
    {
        StartCoroutine("IMoveEnemy");
    }
    IEnumerator IMoveEnemy()
    {
        while (gameObject != null && currPos.y > 0)
        {
            
            // Find the path using A* algorithm (from current position to y = 0)
            List<Vector2> path = FindPath(currPos, MapGenerator.mapTiles, MapGenerator.mapWidth, MapGenerator.mapHeight);
            

            // TODO: logic for the case when the path is not found
            foreach (Vector2 nextPos in path)
            {
                GridCell nextCell = MapGenerator.mapTiles[(int)nextPos.y, (int)nextPos.x];
                if (nextCell.isWalkable)
                {
                    // update cell and make previous walkable
                    currCell.isWalkable = true;
                    currCell = nextCell;
                    currCell.isWalkable = false;

                    // update position change game obj position
                    currPos = nextPos;
                    transform.position = currPos;

                }
                else
                {
                    currCell.isWalkable = true;
                    break;
                }
                yield return new WaitForSeconds(1f);
            }
            if (currPos.y == 0)
            {
                PlayerHealth.DamagePlayer(damage);
                currCell.isWalkable = true;
                DestroyEnemy();
            }
        }
        
        
        // working version, non pathfinding
        // while (gameObject != null)
        // {
        //     yield return new WaitForSeconds(1f);
            
        //     Vector2 nextPos = currPos - new Vector2(0,1);

        //     if (nextPos.y >= 0)
        //     {
        //         GridCell nextCell = MapGenerator.mapTiles[(int)nextPos.y, (int)nextPos.x];
        //         if (nextCell.isWalkable)
        //         {
        //             // update position
        //             currPos = nextPos;

        //             // update cell and make previous walkable
        //             currCell.isWalkable = true;
        //             currCell = nextCell;
        //             currCell.isWalkable = false;

        //             // change game obj position
        //             transform.position = currPos;
        //         }
        //     }
        //     if (currPos.y == 0)
        //     {
        //         PlayerHealth.DamagePlayer(damage);
        //         DestroyEnemy();
        //     }

        // }
    }

    List<Vector2> FindPath(Vector2 start, GridCell[,] grid, int width, int height)
    {
        // Define the heuristic function
        //TODO : change heuristic to consider horizontal distance (its making weird movements)
        int Heuristic(Vector2 a)
        {
            return (int)(Mathf.Abs(a.y));
        }

        // Create the priority queue and dictionary
        PriorityQueue<Vector2> openSet = new PriorityQueue<Vector2>();
        Dictionary<Vector2, Vector2> cameFrom = new Dictionary<Vector2, Vector2>();

        // Initialize the priority queue and dictionary
        openSet.Enqueue(start, 0);
        cameFrom[start] = start;
        Dictionary<Vector2, int> gScore = new Dictionary<Vector2, int>();
        gScore[start] = 0;

        // Loop through the open set
        while (!openSet.IsEmpty())
        {
            // Pop the cell with the lowest f-value
            Vector2 curr = openSet.Dequeue();

            // Check if the goal cell has been reached
            if (curr.y == 0)
            {
                // Construct the path
                List<Vector2> path = new List<Vector2>();
                path.Add(curr);
                while (curr != start)
                {
                    curr = cameFrom[curr];
                    path.Add(curr);
                }
                path.Reverse();
                return path;
            }

            // Generate the neighbors
            List<Vector2> neighbors = new List<Vector2>();
            if (curr.x > 0)
            {
                neighbors.Add(new Vector2(curr.x - 1, curr.y));
            }
            if (curr.x < width - 1)
            {
                neighbors.Add(new Vector2(curr.x + 1, curr.y));
            }
            if (curr.y > 0)
            {
                neighbors.Add(new Vector2(curr.x, curr.y - 1));
            }
            if (curr.y < height - 1)
            {
                neighbors.Add(new Vector2(curr.x, curr.y + 1));
            }

            // Explore the neighbors
            foreach (Vector2 neighbor in neighbors)
            {
                GridCell neighborCell = grid[(int)neighbor.y, (int)neighbor.x];

                // Check if the neighbor is walkable
                if (!neighborCell.isWalkable)
                {
                    continue;
                }

                // Calculate the g-value and f-value of the neighbor
                int tentativeGScore = gScore[curr] + 1;
                if (!gScore.ContainsKey(neighbor) || tentativeGScore < gScore[neighbor])
                {
                    gScore[neighbor] = tentativeGScore;
                    int fScore = tentativeGScore + Heuristic(neighbor);
                    openSet.Enqueue(neighbor, fScore);
                    cameFrom[neighbor] = curr;
                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Enqueue(neighbor, fScore);
                    }
                    else
                    {
                        openSet.UpdatePriority(neighbor, fScore);
                    }
                }
            }
        }

        // no path found
        return null;
    }

}

public class PriorityQueue<T>
{
    private List<(T, float)> elements = new List<(T, float)>();
    private Dictionary<T, float> priorities = new Dictionary<T, float>();

    public int Count => elements.Count;

    public void Enqueue(T item, float priority)
    {
        elements.Add((item, priority));
        priorities[item] = priority;
        int i = elements.Count - 1;
        while (i > 0)
        {
            int j = (i - 1) / 2;
            if (elements[i].Item2 < elements[j].Item2)
            {
                (T, float) temp = elements[i];
                elements[i] = elements[j];
                elements[j] = temp;
                i = j;
            }
            else
            {
                break;
            }
        }
    }

    public T Dequeue()
    {
        (T, float) result = elements[0];
        int lastIndex = elements.Count - 1;
        elements[0] = elements[lastIndex];
        elements.RemoveAt(lastIndex);
        priorities.Remove(result.Item1);

        lastIndex--;

        int i = 0;
        while (true)
        {
            int leftIndex = 2 * i + 1;
            if (leftIndex > lastIndex)
            {
                break;
            }
            int rightIndex = 2 * i + 2;
            int minIndex = leftIndex;
            if (rightIndex <= lastIndex && elements[rightIndex].Item2 < elements[leftIndex].Item2)
            {
                minIndex = rightIndex;
            }
            if (elements[minIndex].Item2 < elements[i].Item2)
            {
                (T, float) temp = elements[minIndex];
                elements[minIndex] = elements[i];
                elements[i] = temp;
                i = minIndex;
            }
            else
            {
                break;
            }
        }

        return result.Item1;
    }

    public bool IsEmpty()
    {
        return elements.Count == 0;
    }

    public bool Contains(T item)
    {
        return priorities.ContainsKey(item);
    }

    public void UpdatePriority(T item, float priority)
    {
        if (!priorities.ContainsKey(item))
        {
            return;
        }
        priorities[item] = priority;
        for (int i = 0; i < elements.Count; i++)
        {
            if (EqualityComparer<T>.Default.Equals(elements[i].Item1, item))
            {
                elements[i] = (item, priority);
                while (i > 0)
                {
                    int j = (i - 1) / 2;
                    if (elements[i].Item2 < elements[j].Item2)
                    {
                        (T, float) temp = elements[i];
                        elements[i] = elements[j];
                        elements[j] = temp;
                        i = j;
                    }
                    else
                    {
                        break;
                    }
                }
                return;
            }
        }
    }
}



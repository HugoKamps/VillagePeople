using System;
using System.Linq;
using System.Collections.Generic;
using VillagePeople.Entities.NPC;

namespace VillagePeople.Util
{
    public class Pathfinder
    {
        public Villager seeker, target;
        public Graph grid;

        public void Update()
        {
            FindPath(seeker.Position, target.Position);
        }

        public void FindPath(Vector2D startPos, Vector2D targetPos)
        {
            Node startNode = grid.GetClosestNode(startPos);
            Node targetNode = grid.GetClosestNode(targetPos);

            List<KeyValuePair<string, Node>> openSet = new List<KeyValuePair<string, Node>>();
            HashSet<Node> closedSet = new HashSet<Node>();

            openSet.Add(new KeyValuePair<string, Node>(startNode.WorldPosition.ToString(), startNode));

            while (openSet.Count > 0)
            {
                Node currentNode = grid.getNodeByWorldPosition(openSet[0].Value.WorldPosition);
                for (int i = 1; i < openSet.Count; i++)
                {
                    if (openSet[i].Value.fCost < currentNode.fCost || openSet[i].Value.fCost == currentNode.fCost && openSet[i].Value.hCost < currentNode.hCost)
                    {
                        currentNode = openSet[i].Value;
                    }
                }

                openSet.Remove(openSet.First(item => item.Key.Equals(currentNode.WorldPosition.ToString())));
                closedSet.Add(currentNode);

                if (currentNode.WorldPosition == targetNode.WorldPosition)
                {
                    RetracePath(startNode, currentNode);
                    return;
                }
                foreach (Edge edge in currentNode.Edges)
                {
                    bool origin = true;
                    Node neighbor = edge.Origin;
                    if (edge.Target != currentNode)
                    {
                        neighbor = edge.Target;
                        origin = false;
                    }

                    if (closedSet.Contains(neighbor))
                    {
                        continue;
                    }

                    int newMovementCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor);
                    
                    if (newMovementCostToNeighbor < neighbor.gCost || neighbor.gCost < 0)
                    {
                        neighbor.gCost = newMovementCostToNeighbor;
                        neighbor.hCost = GetDistance(neighbor, targetNode);
                        neighbor.parent = currentNode;

                        bool check = true;
                        if (openSet.Count != 0)
                        {
                            check = !openSet.Contains(openSet.FirstOrDefault(item => item.Key.Equals(neighbor.WorldPosition.ToString())));
                        }

                        if (check)
                        {
                            openSet.Add(new KeyValuePair<string, Node>(neighbor.WorldPosition.ToString(), neighbor));
                        }
                    }

                    if (origin)
                    {
                        edge.Origin = neighbor;
                    }
                    else
                    {
                        edge.Target = neighbor;
                    }
                }
            }
        }

        public void RetracePath(Node startNode, Node targetNode)
        {
            List<Node> path = new List<Node>();
            Node currentNode = targetNode;

            while (currentNode != null)
            {
                path.Add(currentNode);
                currentNode = currentNode.parent;
            }
            path.Reverse();

            grid.path = path;
        }

        int GetDistance(Node nodeA, Node nodeB)
        {
            int dX = (int)Math.Abs(nodeA.WorldPosition.X - nodeB.WorldPosition.X);
            int dY = (int)Math.Abs(nodeA.WorldPosition.Y - nodeB.WorldPosition.Y);

            if (dX > dY)
                return 14 * dY + 10 * (dX - dY);
            return 14 * dX + 10 * (dY - dX);
        }
    }
}

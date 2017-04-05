using System;
using System.Linq;
using System.Collections.Generic;
using VillagePeople.Entities.NPC;

namespace VillagePeople.Util
{
    public class Pathfinder
    {
        public Vector2D seeker, target;
        public Graph grid;

        public void Update()
        {
            FindPath(seeker, target);
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
                Node currentNode = grid.GetClosestNode(openSet[0].Value.WorldPosition);
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

                    bool containsNeighbor = true;
                    if (openSet.Count != 0)
                    {
                        containsNeighbor = !openSet.Contains(openSet.FirstOrDefault(item => item.Key.Equals(neighbor.WorldPosition.ToString())));
                    }

                    if (newMovementCostToNeighbor < neighbor.gCost || containsNeighbor)
                    {
                        neighbor.gCost = newMovementCostToNeighbor;
                        neighbor.hCost = GetDistance(neighbor, targetNode);
                        neighbor.parent = currentNode;
                        
                        if (containsNeighbor)
                        {
                            openSet.Add(new KeyValuePair<string, Node>(neighbor.WorldPosition.ToString(), neighbor));
                        }
                    }
                }
            }
        }

        public void RetracePath(Node startNode, Node targetNode)
        {
            List<Node> path = new List<Node>();
            Node currentNode = grid.GetClosestNode(targetNode.WorldPosition);
            startNode = grid.GetClosestNode(startNode.WorldPosition);

            while (currentNode != startNode)
            {
                if (currentNode == null)
                {
                    return;
                }
                currentNode = grid.GetClosestNode(currentNode.WorldPosition);
                path.Add(currentNode);
                currentNode = currentNode.parent;
            }
            //path.Add(startNode);
            path.Reverse();

            grid.path = path;
        }

        int GetDistance(Node nodeA, Node nodeB)
        {
            double xSqr = Math.Pow(Math.Abs(nodeA.WorldPosition.X - nodeB.WorldPosition.X), 2);
            double ySqr = Math.Pow(Math.Abs(nodeA.WorldPosition.Y - nodeB.WorldPosition.Y), 2);
            return (int)Math.Sqrt(xSqr + ySqr);
        }
    }
}

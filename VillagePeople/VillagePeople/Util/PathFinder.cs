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
        public List<Node> path = new List<Node>();
        public List<Node> NodesWithSmoothEdges = new List<Node>();

        public void Update()
        {
            NodesWithSmoothEdges.ForEach(n => n.SmoothEdges = new List<Edge>());
            FindPath(seeker, target);
        }

        public void FindPath(Vector2D startPos, Vector2D targetPos)
        {
            Node startNode = grid.GetClosestNode(startPos);
            Node targetNode = grid.GetClosestNode(targetPos);
            path = new List<Node>();

            List<KeyValuePair<string, Node>> openSet = new List<KeyValuePair<string, Node>>();
            HashSet<Node> closedSet = new HashSet<Node>();

            openSet.Add(new KeyValuePair<string, Node>(startNode.WorldPosition.ToString(), startNode));

            while (openSet.Count > 0)
            {
                Node currentNode = grid.GetClosestNode(openSet[0].Value.WorldPosition);
                for (int i = 1; i < openSet.Count; i++)
                {
                    if (openSet[i].Value.fCost < currentNode.fCost || (openSet[i].Value.fCost == currentNode.fCost && openSet[i].Value.hCost < currentNode.hCost))
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
                    Node neighbor = grid.GetClosestNode(edge.Origin.WorldPosition);
                    Node temp = grid.GetClosestNode(edge.Target.WorldPosition);
                    if (temp != currentNode)
                    {
                        neighbor = temp;
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
            path = new List<Node>();
            Node currentNode = grid.GetClosestNode(targetNode.WorldPosition);
            startNode = grid.GetClosestNode(startNode.WorldPosition);

            while (currentNode != startNode)
            {
                if (currentNode == null)
                    break;

                currentNode = grid.GetClosestNode(currentNode.WorldPosition);
                path.Add(currentNode);
                currentNode = grid.GetClosestNode(currentNode.parent.WorldPosition);
            }
            path.Add(startNode);
            path.Reverse();
        }

        public void PathSmoothing()
        {
            for (int i = 0; i < path.Count - 2; )
            {
                if (i >= path.Count -1)
                    break;

                var curr = path[i];
                var j = path.Count - 1;
                bool edgeCreated = false;

                while (j > i + 1) // There is still a node between i and j
                {
                    var temp = path[j];
                    if (!grid.IntersectsStaticObjects(path[i].WorldPosition, path[j].WorldPosition))
                    {
                        path[j].parent = path[i];
                        path[i].ConnectSmoothEdge(path[j]);

                        NodesWithSmoothEdges.Add(path[i]);
                        NodesWithSmoothEdges.Add(path[j]);

                        edgeCreated = true;
                        break;
                    }

                    j--;
                }

                if (edgeCreated)
                    i = j;
                else
                    i++;
            }

            RetracePath(path.FirstOrDefault(), path.LastOrDefault());
        }

        int GetDistance(Node nodeA, Node nodeB)
        {
            double xSqr = Math.Pow(Math.Abs(nodeA.WorldPosition.X - nodeB.WorldPosition.X), 2);
            double ySqr = Math.Pow(Math.Abs(nodeA.WorldPosition.Y - nodeB.WorldPosition.Y), 2);
            return (int)Math.Sqrt(xSqr + ySqr);
        }
    }
}

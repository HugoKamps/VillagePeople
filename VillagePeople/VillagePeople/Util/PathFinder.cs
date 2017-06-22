using System;
using System.Collections.Generic;
using System.Linq;

namespace VillagePeople.Util {
    public class Pathfinder {
        public List<Node> ConsideredEdges = new List<Node>();
        public Graph Grid;
        public List<Node> NodesWithSmoothEdges = new List<Node>();
        public List<Node> Path = new List<Node>();
        public Vector2D Seeker, Target;

        public void Update() {
            NodesWithSmoothEdges.ForEach(n => n.SmoothEdges = new List<Edge>());
            FindPath(Seeker, Target);
        }

        public void FindPath(Vector2D startPos, Vector2D targetPos) {
            var startNode = Grid.GetClosestNode(startPos);
            var targetNode = Grid.GetClosestNode(targetPos);
            Path = new List<Node>();

            var openSet = new List<KeyValuePair<string, Node>>();
            ConsideredEdges = new List<Node>();

            openSet.Add(new KeyValuePair<string, Node>(startNode.WorldPosition.ToString(), startNode));

            while (openSet.Count > 0) {
                var currentNode = Grid.GetClosestNode(openSet[0].Value.WorldPosition);
                for (var i = 1; i < openSet.Count; i++)
                    if (openSet[i].Value.FCost < currentNode.FCost ||
                        openSet[i].Value.FCost == currentNode.FCost && openSet[i].Value.HCost < currentNode.HCost)
                        currentNode = openSet[i].Value;

                openSet.Remove(openSet.First(item => item.Key.Equals(currentNode.WorldPosition.ToString())));
                ConsideredEdges.Add(currentNode);

                if (currentNode.WorldPosition == targetNode.WorldPosition) {
                    RetracePath(startNode, currentNode);
                    return;
                }
                foreach (var edge in currentNode.Edges) {
                    var neighbor = Grid.GetClosestNode(edge.Origin.WorldPosition);
                    var temp = Grid.GetClosestNode(edge.Target.WorldPosition);
                    if (temp != currentNode)
                        neighbor = temp;

                    if (ConsideredEdges.Contains(neighbor))
                        continue;

                    var newMovementCostToNeighbor = currentNode.GCost + GetDistance(currentNode, neighbor);

                    var containsNeighbor = true;
                    if (openSet.Count != 0)
                        containsNeighbor =
                            !openSet.Contains(
                                openSet.FirstOrDefault(item => item.Key.Equals(neighbor.WorldPosition.ToString())));

                    if (newMovementCostToNeighbor < neighbor.GCost || containsNeighbor) {
                        neighbor.GCost = newMovementCostToNeighbor;
                        neighbor.HCost = GetDistance(neighbor, targetNode);
                        neighbor.Parent = currentNode;

                        if (containsNeighbor)
                            openSet.Add(new KeyValuePair<string, Node>(neighbor.WorldPosition.ToString(), neighbor));
                    }
                }
            }
        }

        public void RetracePath(Node startNode, Node targetNode) {
            if (startNode == null || targetNode == null)
                return;

            Path = new List<Node>();
            var currentNode = Grid.GetClosestNode(targetNode.WorldPosition);
            startNode = Grid.GetClosestNode(startNode.WorldPosition);

            while (currentNode != startNode) {
                if (currentNode == null)
                    break;

                currentNode = Grid.GetClosestNode(currentNode.WorldPosition);
                Path.Add(currentNode);
                currentNode = Grid.GetClosestNode(currentNode.Parent.WorldPosition);
            }
            Path.Add(startNode);
            Path.Reverse();
        }

        public void PathSmoothing() {
            for (var i = 0; i < Path.Count - 2;) {
                if (i >= Path.Count - 1)
                    break;

                var j = Path.Count - 1;
                var edgeCreated = false;

                while (j > i + 1) // There is still a node between i and j
                {
                    if (!Grid.IntersectsStaticObjects(Path[i].WorldPosition, Path[j].WorldPosition)) {
                        Path[j].Parent = Path[i];
                        Path[i].ConnectSmoothEdge(Path[j]);

                        NodesWithSmoothEdges.Add(Path[i]);
                        NodesWithSmoothEdges.Add(Path[j]);

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

            RetracePath(Path.FirstOrDefault(), Path.LastOrDefault());
        }

        private int GetDistance(Node nodeA, Node nodeB) {
            var xSqr = Math.Pow(Math.Abs(nodeA.WorldPosition.X - nodeB.WorldPosition.X), 2);
            var ySqr = Math.Pow(Math.Abs(nodeA.WorldPosition.Y - nodeB.WorldPosition.Y), 2);
            return (int) Math.Sqrt(xSqr + ySqr);
        }
    }
}
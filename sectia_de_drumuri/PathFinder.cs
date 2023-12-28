using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sectia_de_drumuri
{
    public class PathFinder
    {
        private int width;
        private int height;
        private Node[,] nodes;
        private Node startNode;
        private Node endNode;
        private SearchParameters searchParameters;
        private Cautare algoritm;
        /// <summary>
        /// Create a new instance of PathFinder
        /// </summary>
        /// <param name="searchParameters"></param>
        public PathFinder(SearchParameters searchParameters)
        {
            this.searchParameters = searchParameters;
            InitializeNodes(searchParameters.Map);
            this.startNode = this.nodes[searchParameters.StartLocation.X, searchParameters.StartLocation.Y];
            this.startNode.State = NodeState.Open;
            this.endNode = this.nodes[searchParameters.EndLocation.X, searchParameters.EndLocation.Y];
            algoritm = searchParameters.cautare;
        }

        /// <summary>
        /// Attempts to find a path from the start location to the end location based on the supplied SearchParameters
        /// </summary>
        /// <returns>A List of Points representing the path. If no path was found, the returned list is empty.</returns>
        public List<Point> FindPath()
        {
            bool success = false;
            // The start node is the first entry in the 'open' list
            List<Point> path = new List<Point>();
            if (algoritm == Cautare.A8)
            {
                 success = Search(startNode);
            }
            else if (algoritm == Cautare.Dijkastra)
            {
                success = SearchDijkastra(startNode);
            }

            if (success)
                {
                    // If a path was found, follow the parents from the end node to build a list of locations
                    Node node = this.endNode;
                    while (node.ParentNode != null)
                    {
                        path.Add(node.Location);
                        node = node.ParentNode;
                    }

                    // Reverse the list so it's in the correct order when returned
                    path.Reverse();
                }
          
            return path;
        }

        /// <summary>
        /// Builds the node grid from a simple grid of booleans indicating areas which are and aren't walkable
        /// </summary>
        /// <param name="map">A boolean representation of a grid in which true = walkable and false = not walkable</param>
        private void InitializeNodes(bool[,] map)
        {
            this.width = map.GetLength(0);
            this.height = map.GetLength(1);
            this.nodes = new Node[this.width, this.height];

            if(algoritm == Cautare.A8)
                for (int y = 0; y < this.height; y++)
                {
                    for (int x = 0; x < this.width; x++)
                    {
                        this.nodes[x, y] = new Node(x, y, map[x, y], this.searchParameters.EndLocation);
                    }
                }
            else if(algoritm == Cautare.Dijkastra)
            {
                for (int y = 0; y < this.height; y++)
                {
                    for (int x = 0; x < this.width; x++)
                    {
                        this.nodes[x, y] = new Node(x, y, map[x, y]);
                    }
                }
                nodes[searchParameters.StartLocation.X, searchParameters.StartLocation.Y].G = 0;
            }
        }

        /// <summary>
        /// Attempts to find a path to the destination node using <paramref name="currentNode"/> as the starting location
        /// </summary>
        /// <param name="currentNode">The node from which to find a path</param>
        /// <returns>True if a path to the destination has been found, otherwise false</returns>
        private bool Search(Node currentNode)
        {
			
            // Set the current node to Closed since it cannot be traversed more than once
            currentNode.State = NodeState.Closed;
            List<Node> nextNodes = GetAdjacentWalkableNodes(currentNode);

            // Sort by F-value so that the shortest possible routes are considered first
            nextNodes.Sort((node1, node2) => node1.F.CompareTo(node2.F));


			foreach (var nextNode in nextNodes)
			{
				// Check whether the end node has been reached
				if (nextNode.Location == this.endNode.Location)
				{
					return true;
				}
				else
				{
					// If not, check the next set of nodes
					if (Search(nextNode)) // Note: Recurses back into Search(Node)
						return true;
				}
			}

			//The method returns false if this path leads to be a dead end
			return false;
        }
        bool ajunsVrodata = false;
        private bool SearchDijkastra(Node stNode)
        {
            List<Node> pq = new List<Node>();
            pq.Add(stNode);
            Node node;
            while(pq.Count!=0)
            {
                //pq.Sort( delegate(Node a,Node b){return a.G.CompareTo(b.G);});
                pq.Sort((a, b) => a.G.CompareTo(b.G));
                node = pq[0];
                pq.RemoveAt(0);
                node.State = NodeState.Closed;
                List<Node> nextNodes = GetAdjacentWalkableNodes(node);
                pq.AddRange(nextNodes);
                
                if(node.Location == endNode.Location)
                {
                    ajunsVrodata = true;
                }
            }

          
            //The method returns false if this path leads to be a dead end
            return ajunsVrodata;
        }
        /// <summary>
        /// Returns any nodes that are adjacent to <paramref name="fromNode"/> and may be considered to form the next step in the path
        /// </summary>
        /// <param name="fromNode">The node from which to return the next possible nodes in the path</param>
        /// <returns>A list of next possible nodes in the path</returns>
        private List<Node> GetAdjacentWalkableNodes(Node fromNode)
        {
            List<Node> walkableNodes = new List<Node>();
            IEnumerable<Point> nextLocations = GetAdjacentLocations(fromNode.Location);

            foreach (var location in nextLocations)
            {
                int x = location.X;
                int y = location.Y;

                // Stay within the grid's boundaries
                if (x < 0 || x >= this.width || y < 0 || y >= this.height)
                    continue;

                Node node = this.nodes[x, y];
                // Ignore non-walkable nodes
                if (!node.IsWalkable)
                    continue;

                // Ignore already-closed nodes
                if (node.State == NodeState.Closed)
                    continue;

                // Already-open nodes are only added to the list if their G-value is lower going via this route.
                if (node.State == NodeState.Open)
                {
                    
                    float traversalCost = Node.GetTraversalCost(node.Location, node.ParentNode.Location);
                    float gTemp = fromNode.G + traversalCost;
                    if (gTemp < node.G)
                    {
                        node.ParentNode = fromNode;
                        walkableNodes.Add(node);
                    }
                }
                else
                {
                    // If it's untested, set the parent and flag it as 'Open' for consideration
                    node.ParentNode = fromNode;
                    node.State = NodeState.Open;
                    walkableNodes.Add(node);
                }
            }

            return walkableNodes;
        }
        /// <summary>
        /// Returns the eight locations immediately adjacent (orthogonally and diagonally) to <paramref name="fromLocation"/>
        /// </summary>
        /// <param name="fromLocation">The location from which to return all adjacent points</param>
        /// <returns>The locations as an IEnumerable of Points</returns>
        private static IEnumerable<Point> GetAdjacentLocations(Point fromLocation)
        {
			Pic vf = MainForm.varfuri[fromLocation.X, fromLocation.Y];
			List<Point> pnt = new List<Point>();
			foreach (Pic drum in vf.drum_diag)
			{
				if (drum.eticheta == 3)
					pnt.Add(new Point(drum.f.x, drum.f.y));
				else
				{
					if (MainForm.coord_egale(drum.i, vf)) pnt.Add(new Point(drum.f.x, drum.f.y));
					else pnt.Add(new Point(drum.i.x, drum.i.y));
				}
			}
			foreach (Pic drum in vf.drum)
			{
				if (drum.eticheta == 1)
					pnt.Add(new Point(drum.f.x, drum.f.y));
				else
				{
					if (MainForm.coord_egale(drum.i, vf)) pnt.Add(new Point(drum.f.x, drum.f.y));
					else pnt.Add(new Point(drum.i.x, drum.i.y));
				}
			}
			

			return pnt;
			/*return new Point[]
            {
                new Point(fromLocation.X-1, fromLocation.Y-1),
                new Point(fromLocation.X-1, fromLocation.Y  ),
                new Point(fromLocation.X-1, fromLocation.Y+1),
                new Point(fromLocation.X,   fromLocation.Y+1),
                new Point(fromLocation.X+1, fromLocation.Y+1),
                new Point(fromLocation.X+1, fromLocation.Y  ),
                new Point(fromLocation.X+1, fromLocation.Y-1),
                new Point(fromLocation.X,   fromLocation.Y-1)
            };*/
        }
    }
}

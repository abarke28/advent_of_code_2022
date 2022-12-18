using aoc.common;
using aoc.utils;
using aoc.utils.extensions;

namespace aoc.y2021.day_12
{
    // https://adventofcode.com/2021/day/12
    public class Day_12 : ISolver
    {
        private const string Start = "start";
        private const string End = "end";

        public void Solve()
        {
            var lines = FileUtils.ReadAllLines("2021/day_12/input.txt");

            var caveSystem = ParseInput(lines);
            var caves = caveSystem.GetAllNodes();

            var paths = FindPaths(caveSystem,
                                  Start,
                                  End,
                                  visited: new List<string>(),
                                  notVisited: new HashSet<string>(caves));

            Console.WriteLine(paths.Count);

        }

        private static IList<IList<string>> FindPaths(NodeGraph<string> caveSystem,
                                                      string start,
                                                      string destination,
                                                      IList<string> visited,
                                                      HashSet<string> notVisited)
        {
            var paths = new List<IList<string>>();

            var current = start;
            visited.Add(current);
            notVisited.Remove(current);

            if (current == destination)
            {
                return new List<IList<string>>()
                {
                    visited
                };
            }

            var adjacentNodes = caveSystem.GetAdjacentNodes(current);

            foreach (var adjacentNode in adjacentNodes)
            {
                if (IsBigCave(adjacentNode) || notVisited.Contains(adjacentNode))
                {
                    var newVisited = new List<string>(visited);
                    var newNotVisited = new HashSet<string>(notVisited);

                    paths.AddRange(FindPaths(caveSystem, adjacentNode, destination, newVisited, newNotVisited));
                }
            }

            return paths;
        }

        private static bool IsSmallCave(string s)
        {
            const int firstLowerCaseCodepoint = 97;

            if (s == Start || s == End)
            {
                return false;
            }

            return s.All(c => c >= firstLowerCaseCodepoint);
        }

        private static bool IsBigCave(string s)
        {
            const int firstLowerCaseCodepoint = 97;

            if (s == Start || s == End)
            {
                return false;
            }

            var isBigCave = s.All(c => c < firstLowerCaseCodepoint);

            return isBigCave;
        }

        private static NodeGraph<string> ParseInput(IEnumerable<string> lines)
        {
            var nodeConnections = lines.Select(l => l.Split('-')).Select(ws => ws.Select(w => w.Trim()).ToList());
            var nodes = nodeConnections.SelectMany(n => n).ToHashSet();

            var graph = new NodeGraph<string>(nodes.Count);

            foreach (var nodeConnection in nodeConnections)
            {
                graph.AddEdge(nodeConnection[0], nodeConnection[1], directed: false);
            }

            return graph;
        }
    }
}

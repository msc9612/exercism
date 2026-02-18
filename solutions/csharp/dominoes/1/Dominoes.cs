public static class Dominoes
{
    public static bool CanChain(IEnumerable<(int, int)> dominoes)
    {
        var list = dominoes.ToList();

        // No dominoes = trivially chainable
        if (list.Count == 0)
            return true;

        // Count degrees of each node
        var degree = new Dictionary<int, int>();
        var graph = new Dictionary<int, List<int>>();

        void AddToGraph(int a, int b)
        {
            if (!degree.ContainsKey(a)) degree[a] = 0;
            if (!degree.ContainsKey(b)) degree[b] = 0;

            degree[a]++;
            degree[b]++;

            if (!graph.ContainsKey(a)) graph[a] = new List<int>();
            if (!graph.ContainsKey(b)) graph[b] = new List<int>();

            graph[a].Add(b);
            graph[b].Add(a);
        }

        foreach (var (a, b) in list)
        {
            AddToGraph(a, b);
        }

        // --- Eulerian Cycle Conditions ---

        // 1. All vertices with edges must have even degree
        if (degree.Values.Any(d => d % 2 != 0))
            return false;

        // 2. Graph must be connected (excluding isolated vertices)
        int start = degree.Keys.First();

        var visited = new HashSet<int>();
        DFS(start);

        void DFS(int node)
        {
            visited.Add(node);
            foreach (var neighbor in graph[node])
            {
                if (!visited.Contains(neighbor))
                    DFS(neighbor);
            }
        }

        // Check that every node with degree > 0 was visited
        foreach (var node in degree.Keys)
        {
            if (degree[node] > 0 && !visited.Contains(node))
                return false;
        }

        return true;
    }
}

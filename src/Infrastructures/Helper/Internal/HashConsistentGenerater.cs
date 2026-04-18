﻿namespace Adnc.Infra.Helper.Internal;

/// <summary>
/// Consistent hashing algorithm
/// </summary>
public sealed class HashConsistentGenerater
{
    /// <summary>
    /// Real node information
    /// </summary>
    private readonly List<string> _nodes = [];

    /// <summary>
    /// Virtual node information (int type is used for binary search when retrieving virtual nodes)
    /// </summary>
    private readonly List<int> _virtualNode = [];

    /// <summary>
    /// Mapping from virtual nodes to real nodes; after finding a virtual node, the real node can be retrieved in O(1) time
    /// </summary>
    private readonly Dictionary<int, string> _virtualNodeAndNodeMap = [];

    /// <summary>
    /// Virtual node multiplier
    /// </summary>
    private int _virtualNodeMultiple = 100;

    internal HashConsistentGenerater()
    {
    }

    /// <summary>
    /// Adds nodes.
    /// </summary>
    /// <param name="hosts">Node collection</param>
    /// <returns>Operation result</returns>
    public bool AddNode(params string[] hosts)
    {
        if (hosts == null || hosts.Length == 0)
        {
            return false;
        }
        _nodes.AddRange(hosts); // Add nodes to the real node list first
        foreach (var item in hosts)
        {
            for (var i = 1; i <= _virtualNodeMultiple; i++) // Loop over real IP strings like "192.168.3.1" from 1 to 1000 to generate virtual nodes: "192.168.3.11" ... "192.168.3.11000"
            {
                var currentHash = GetHashCode(item + i) & int.MaxValue; // Compute a hash using a custom algorithm (the default string hash is not guaranteed to be stable); AND with int.MaxValue to ensure a positive value
                if (_virtualNodeAndNodeMap.TryAdd(currentHash, item)) // Hash collisions are possible; if this hash is already mapped, keep the first one and skip
                {
                    _virtualNode.Add(currentHash); // Add current virtual node to the virtual node list
                }
            }
        }
        _virtualNode.Sort(); // Sort after adding, enabling binary search when looking up by key hash
        return true;
    }

    /// <summary>
    /// Removes a node.
    /// </summary>
    /// <param name="host">The node to remove</param>
    /// <returns></returns>
    public bool RemoveNode(string host)
    {
        if (!_nodes.Remove(host)) // If removing the node from the real node list fails, skip remaining operations
        {
            return false;
        }
        for (var i = 1; i <= _virtualNodeMultiple; i++)
        {
            var currentHash = GetHashCode(host + i) & int.MaxValue; // Compute hash using custom algorithm; AND with int.MaxValue to keep it positive
            if (_virtualNodeAndNodeMap.TryGetValue(currentHash, out var value) && value == host) // Since hashes may collide, also verify the mapped node matches the target before removing
            {
                _virtualNode.Remove(currentHash); // Remove from virtual node list
                _virtualNodeAndNodeMap.Remove(currentHash); // Remove from virtual-to-real node mapping
            }
        }
        _virtualNode.Sort(); // Re-sort for binary search when looking up by key hash
        return true;
    }

    /// <summary>
    /// Gets all nodes.
    /// </summary>
    /// <returns></returns>
    public List<string> GetAllNodes()
    {
        var nodes = new List<string>(_nodes.Count);
        nodes.AddRange(_nodes);
        return nodes;
    }

    /// <summary>
    /// Gets the node count.
    /// </summary>
    /// <returns></returns>
    public int GetNodesCount()
    {
        return _nodes.Count;
    }

    /// <summary>
    /// Resets the virtual node multiplier.
    /// </summary>
    /// <param name="multiple"></param>
    public void ReSetVirtualNodeMultiple(int multiple)
    {
        if (multiple < 0 || multiple == _virtualNodeMultiple)
        {
            return;
        }
        var nodes = new List<string>(_nodes.Count);
        nodes.AddRange(_nodes); // Copy existing real nodes
        _virtualNodeMultiple = multiple; // Set new multiplier
        _nodes.Clear();
        _virtualNode.Clear();
        _virtualNodeAndNodeMap.Clear(); // Clear data
        AddNode(nodes.ToArray()); // Re-add all nodes
    }

    /// <summary>
    /// Gets a node.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public string GetNode(string key)
    {
        var hash = GetHashCode(key) & int.MaxValue;
        var start = 0;
        var end = _virtualNode.Count - 1;
        while (end - start > 1)
        {
            var index = (start + end) / 2;
            if (_virtualNode[index] > hash)
            {
                end = index;
            }
            else if (_virtualNode[index] < hash)
            {
                start = index;
            }
            else
            {
                start = end = index;
            }
        }
        return _virtualNodeAndNodeMap[_virtualNode[start]];
    }

    private static int GetHashCode(string key, int nTime = 0)
    {
        var keyBytes = Encoding.UTF8.GetBytes(key);
        var digest = MD5.HashData(keyBytes);
        var rv = (long)(digest[3 + nTime * 4] & 0xFF) << 24
                        | (long)(digest[2 + nTime * 4] & 0xFF) << 16
                        | (long)(digest[1 + nTime * 4] & 0xFF) << 8
                        | (long)digest[0 + nTime * 4] & 0xFF;
        return (int)(rv & 0xffffffffL);
    }
}

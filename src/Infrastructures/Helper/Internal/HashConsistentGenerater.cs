namespace Adnc.Infra.Helper.Internal;

/// <summary>
/// 一致性哈希算法
/// </summary>
public sealed class HashConsistentGenerater
{
    /// <summary>
    /// 真实节点信息
    /// </summary>
    private readonly List<string> _nodes = [];

    /// <summary>
    /// 虚拟节点信息（int类型主要是为了获取虚拟节点时的二分查找）
    /// </summary>
    private readonly List<int> _virtualNode = [];

    /// <summary>
    /// 虚拟节点和真实节点映射，在获取到虚拟节点之后，能以O(1)的时间复杂度返回真实节点
    /// </summary>
    private readonly Dictionary<int, string> _virtualNodeAndNodeMap = [];

    /// <summary>
    /// 虚拟节点倍数
    /// </summary>
    private int _virtualNodeMultiple = 100;

    internal HashConsistentGenerater()
    {
    }

    /// <summary>
    /// 增加节点
    /// </summary>
    /// <param name="hosts">节点集合</param>
    /// <returns>操作结果</returns>
    public bool AddNode(params string[] hosts)
    {
        if (hosts == null || hosts.Length == 0)
        {
            return false;
        }
        _nodes.AddRange(hosts); //先将节点增加到真实节点信息中。
        foreach (var item in hosts)
        {
            for (var i = 1; i <= _virtualNodeMultiple; i++) //此处循环为类似“192.168.3.1”这样的真实ip字符串从1加到1000，算作虚拟节点。192.168.3.11，192.168.3.11000
            {
                var currentHash = GetHashCode(item + i) & int.MaxValue; //计算一个hash，此处用自定义hash算法原因是字符串默认的哈希实现不保证对同一字符串获取hash时得到相同的值。和int.MaxValue进行位与操作是为了将获取到的hash值设置为正数
                if (_virtualNodeAndNodeMap.TryAdd(currentHash, item)) //因为hash可能会重复，如果当前hash已经包含在虚拟节点和真实节点映射中，则以第一次添加的为准，此处不再进行添加
                {
                    _virtualNode.Add(currentHash);//将当前虚拟节点添加到虚拟节点中
                }
            }
        }
        _virtualNode.Sort(); //操作完成之后进行一次映射，是为了后面根据key的hash值查找虚拟节点时使用二分查找。
        return true;
    }

    /// <summary>
    /// 移除节点
    /// </summary>
    /// <param name="host">指定节点</param>
    /// <returns></returns>
    public bool RemoveNode(string host)
    {
        if (!_nodes.Remove(host)) //如果将指定节点从真实节点集合中移出失败，后序操作不需要进行，直接返回
        {
            return false;
        }
        for (var i = 1; i <= _virtualNodeMultiple; i++)
        {
            var currentHash = GetHashCode(host + i) & int.MaxValue; //计算一个hash，此处用自定义hash算法原因是字符串默认的哈希实现不保证对同一字符串获取hash时得到相同的值。和int.MaxValue进行位与操作是为了将获取到的hash值设置为正数
            if (_virtualNodeAndNodeMap.TryGetValue(currentHash, out var value) && value == host) //因为hash可能会重复，所以此处判断在判断了哈希值是否存在于虚拟节点和节点映射中之后还需要判断通过当前hash值获取到的节点是否和指定节点一致，如果不一致，则证明这个这个虚拟节点不是当前hash值所拥有的
            {
                _virtualNode.Remove(currentHash); //从虚拟节点中移出
                _virtualNodeAndNodeMap.Remove(currentHash); //从虚拟节点和真实ip映射中移出
            }
        }
        _virtualNode.Sort(); //操作完成之后进行一次映射，是为了后面根据key的hash值查找虚拟节点时使用二分查找。
        return true;
    }

    /// <summary>
    /// 获取所有节点
    /// </summary>
    /// <returns></returns>
    public List<string> GetAllNodes()
    {
        var nodes = new List<string>(_nodes.Count);
        nodes.AddRange(_nodes);
        return nodes;
    }

    /// <summary>
    /// 获取节点数量
    /// </summary>
    /// <returns></returns>
    public int GetNodesCount()
    {
        return _nodes.Count;
    }

    /// <summary>
    /// 重新设置虚拟节点倍数
    /// </summary>
    /// <param name="multiple"></param>
    public void ReSetVirtualNodeMultiple(int multiple)
    {
        if (multiple < 0 || multiple == _virtualNodeMultiple)
        {
            return;
        }
        var nodes = new List<string>(_nodes.Count);
        nodes.AddRange(_nodes); //将现有的真实节点拷贝出来
        _virtualNodeMultiple = multiple; //设置倍数
        _nodes.Clear();
        _virtualNode.Clear();
        _virtualNodeAndNodeMap.Clear(); //清空数据
        AddNode(nodes.ToArray()); //重新添加
    }

    /// <summary>
    /// 获取节点
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

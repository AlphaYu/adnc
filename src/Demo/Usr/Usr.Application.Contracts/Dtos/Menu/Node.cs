namespace Adnc.Demo.Usr.Application.Contracts.Dtos;

/// <summary>
/// 业务节点
/// </summary>
/// <typeparam name="T"></typeparam>
[Serializable]
public class Node<T>
{
    /// <summary>
    /// 节点Id
    /// </summary>
    public T Id { get; set; }

    /// <summary>
    /// 父级Id
    /// </summary>
    public T PID { get; set; }

    /// <summary>
    /// 节点名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 是否选中
    /// </summary>
    public bool Checked { get; set; }

    /// <summary>
    /// 子节点
    /// </summary>
    public List<Node<T>> Children { get; private set; } = new List<Node<T>>();
}
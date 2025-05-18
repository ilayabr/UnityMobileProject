using UnityEngine;

/// <summary>
/// An interface to allow an object to be put into a ObjectPool
/// </summary>
public interface IPoolable
{
    /// <summary>
    /// The main object the pool will set active/inactive when managing itself
    /// </summary>
    GameObject mainObject { get; }
    
    /// <summary>
    /// Runs when this object first enter an object pool
    /// </summary>
    void OnEnterPool();
    
    /// <summary>
    /// Runs when this object exists an object pool
    /// </summary>
    void OnExitPool();

    /// <summary>
    /// Runs when the object returns back into the object pool
    /// </summary>
    void OnReturnedToPool();
}

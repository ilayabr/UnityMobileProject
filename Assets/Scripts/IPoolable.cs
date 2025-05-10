using UnityEngine;

public interface IPoolable
{
    GameObject mainObject { get; }
    
    void OnEnterPool();
    
    void OnExitPool();
}

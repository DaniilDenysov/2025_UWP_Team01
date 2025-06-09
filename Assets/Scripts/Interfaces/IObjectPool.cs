using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObjectPool<T>
{   
    public T GetObject(T prefab);
    public void ReleaseObject(T prefab);
}

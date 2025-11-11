using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_Base : MonoBehaviour
{
    protected Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();

    protected void Bind<T>(Type type) where T : UnityEngine.Object
    {
        string[] names = Enum.GetNames(type);
        UnityEngine.Object[] newObjects = new UnityEngine.Object[names.Length];

        for (int i = 0; i < names.Length; i++)
        {
            if (typeof(T) == typeof(GameObject))
                newObjects[i] = Util.FindChild(gameObject, names[i], true);
            else
                newObjects[i] = Util.FindChild<T>(gameObject, names[i], true);

            if (newObjects[i] == null)
                Debug.Log($"Failed to bind({names[i]})");
        }

        // 기존에 존재하는 Key의 인스턴스들을 바인드하는 경우 
        if (_objects.ContainsKey(typeof(T)))
        {
            UnityEngine.Object[] existingObjects = _objects[typeof(T)];
            UnityEngine.Object[] combined = new UnityEngine.Object[existingObjects.Length + newObjects.Length];
            existingObjects.CopyTo(combined, 0);
            newObjects.CopyTo(combined, existingObjects.Length);
            _objects[typeof(T)] = combined;
        }
        // 새로운 Key의 인스턴스들을 바인드하는 경우
        else
        {
            _objects.Add(typeof(T), newObjects);
        }
    }

    protected T Get<T>(int idx) where T : UnityEngine.Object
    {
        UnityEngine.Object[] objects = null;
        if (_objects.TryGetValue(typeof(T), out objects) == false)
            return null;

        return objects[idx] as T;
    }

    protected GameObject GetObject(int idx) { return Get<GameObject>(idx); }

    protected void GenerateEnumsSerialNumber<T>(Dictionary<string, int> dict) where T : UI_Base
    {
        var enums = typeof(T).GetNestedTypes().Where(t => t.IsEnum);

        int counter = 0;

        foreach (var e in enums)
        {
            foreach (var name in Enum.GetNames(e))
            {
                dict[$"{e.Name}.{name}"] = counter++;
            }
        }
    }

    protected string GetEnumFullName<T>(T value) where T : Enum
    {
        return $"{typeof(T).Name}.{value}";
    }
}
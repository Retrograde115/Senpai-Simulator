using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class ManagedUpdate
{
    object target;

    public ManagedUpdate(object targetClass)
    {
        target = targetClass;
    }

    public void UpdateThat()
    {
        var type = target.GetType();
        MethodInfo theMethod = type.GetMethod("UpdateThis");
        theMethod.Invoke(target, null);
    }
}

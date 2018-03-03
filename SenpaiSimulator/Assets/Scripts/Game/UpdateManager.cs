using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateManager : MonoBehaviour
{
    public static List<ManagedUpdate> updates = new List<ManagedUpdate>();
    public static int numUpdates = 0;

    private void Update()
    {
        for (int i = 0; i < numUpdates; i++)
        {
            updates[i].UpdateThat();
        }
    }

    public static ManagedUpdate AddManagedUpdate(object targetClass, ManagedUpdate updater)
    {
        updater = new ManagedUpdate(targetClass);
        updates.Add(updater);
        numUpdates++;
        return updater;
    }

}

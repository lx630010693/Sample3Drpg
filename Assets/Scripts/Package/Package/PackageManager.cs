using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageManager 
{
    private static PackageManager instance;
    public static PackageManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PackageManager();
            }
            return instance;
        }
    }

    public List<Package> packageList = new List<Package>();
    public Package FindUsingPackage()
    {
        for (int i = 0; i < packageList.Count; i++)
        {
            if (packageList[i].isUsing == true)
            {
                return packageList[i];
            }
        }
        return null;
    }
    public void Add(Package package)
    {
        packageList.Add(package);
    }
    public void Remove(Package package)
    {
        packageList.Remove(package);
    }
}

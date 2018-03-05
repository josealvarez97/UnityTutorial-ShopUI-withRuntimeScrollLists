using UnityEngine;
using System.Collections.Generic;

// A very simple object pooling class
public class SimpleObjectPool : MonoBehaviour
{
    // the prefab that this object pool returns instances of
    public GameObject prefab;
    //collection of currently inactive instances of the prefab
    private Stack<GameObject> inactiveInstances = new Stack<GameObject>();

    // Returns an instance of the prefab when needed
    public GameObject GetObject(Transform contentPanelParent)
    {
        GameObject spawnedGameObject;

        // if there is an inactive instance of the prefab ready to return, return it 
        if (inactiveInstances.Count > 0)
        {
            // remove the instance from the collection of inactive instances
            spawnedGameObject = inactiveInstances.Pop();
        }
        // otherwise, create a new instance; since it's necessary
        else
        {
            spawnedGameObject = (GameObject)GameObject.Instantiate(prefab);

            // add the PooledObject component to the prefab so we know it came from this pool
            PooledObject pooledObject = spawnedGameObject.AddComponent<PooledObject>();
            pooledObject.pool = this; // this pool...
            // why would there be more than one pool?
        }

        // put the instance in the root of the scene and enable it
        spawnedGameObject.transform.SetParent(contentPanelParent); // so Parent null follows using default at origin...
        spawnedGameObject.SetActive(true);

        // return a reference to the instance
        return spawnedGameObject;

    }

    // Return an instance of the prefab to the pool
    public void ReturnObject(GameObject toReturn)
    {
        PooledObject pooledObject = toReturn.GetComponent<PooledObject>(); //which pool it comes from...

        // if the instance came from this pool, return it to the pool
        if (pooledObject != null && pooledObject.pool == this)
        {
            // make the instance a child of this and disable it
            toReturn.transform.SetParent(transform); // didn't get this line...
            toReturn.SetActive(false);
        }
        // otherwise, just destroy it
        else
        {
            // WTF!!! Wouldn't a pool following a singleton pattern be better then???
            Debug.LogWarning(toReturn.name + " was returned to a pool it wasn't spawned from!!! Destroying...");
            Destroy(toReturn);
        }
    }
}

// a component that simply identifies the pool that a GameObject came from
public class PooledObject : MonoBehaviour
{
    public SimpleObjectPool pool; //weird patterns...
}


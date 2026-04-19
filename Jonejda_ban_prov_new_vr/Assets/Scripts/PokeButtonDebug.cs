using UnityEngine;

public class PokeButtonDebug : MonoBehaviour
{
    public GameObject hammerPrefab;
    public Transform hammerSocket; // Socket where the hammer spawns and can be grabbed from

    private GameObject currentHammer;

    public void SpawnHammer()
    {
        Debug.Log("SpawnHammer called on: " + gameObject.name);

        if (hammerPrefab == null) return;

        if (hammerSocket == null) return;

        if (currentHammer != null)
        {
            Debug.Log("Hammer already exists, destroying old one: " + currentHammer.name);
            Destroy(currentHammer);
            currentHammer = null;
        }

        currentHammer = Instantiate(hammerPrefab);
        currentHammer.name = hammerPrefab.name + "_SPAWNED";

        Debug.Log("Instantiated hammer object: " + currentHammer.name);
        
        currentHammer.transform.position = hammerSocket.position;
        currentHammer.transform.rotation = hammerSocket.rotation;
        currentHammer.transform.localScale = Vector3.one * 6f; 
        
        Debug.Log("Hammer spawned at " + hammerSocket.position + " with scale: " + currentHammer.transform.localScale);

        currentHammer.SetActive(true);
    }
}

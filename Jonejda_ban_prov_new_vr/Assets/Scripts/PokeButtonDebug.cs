using UnityEngine;

public class PokeButtonDebug : MonoBehaviour
{
    public GameObject hammerPrefab;
    public Transform rightHandAttach;

    private GameObject currentHammer;

    public void SpawnHammer()
    {
        Debug.Log("SpawnHammer called on: " + gameObject.name);

        if (hammerPrefab == null) return;

        if (rightHandAttach == null) return;

        if (currentHammer != null)
        {
            Debug.Log("Hammer already exists, destroying old one: " + currentHammer.name);
            Destroy(currentHammer);
            currentHammer = null;
        }

        currentHammer = Instantiate(hammerPrefab);
        currentHammer.name = hammerPrefab.name + "_SPAWNED";

        Debug.Log("Instantiated hammer object: " + currentHammer.name);

        currentHammer.transform.SetParent(rightHandAttach, worldPositionStays: false);
        currentHammer.transform.localPosition = Vector3.zero;
        currentHammer.transform.localRotation = Quaternion.identity;
        currentHammer.transform.localScale = Vector3.one;

        currentHammer.SetActive(true);
    }
}

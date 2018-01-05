using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Room : MonoBehaviour {

    public List<GeneratorDoor> doors;
    public List<Feature> features = new List<Feature>();

    public GameObject gameGeometryParent;

    //WARNING - when doing the dungeon gen we sometimes Instantiate a room, check if it will fit and if it doesn't
    //we IMMEDIATLY destroy it.  Awake() is called with instantiation - Start() waits until the function returns..
    //SO to be safe, don't use Awake if you don't have to.  Put all enemy and room specific instantiation in START!
    // void Awake() {
    // }

    // void Start () {
    // }

    // void Update () {
    // }

    private void OnDrawGizmos() {
        for (int i = 0; i < doors.Count; i++) {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(doors[i].transform.position, 0.1f);

        Gizmos.DrawRay(new Ray(doors[i].transform.position, doors[i].transform.right));

        Gizmos.color = Color.green;
        Gizmos.DrawRay(new Ray(doors[i].transform.position, doors[i].transform.up));

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(new Ray(doors[i].transform.position, doors[i].transform.forward));
        }
    }

    public GeneratorDoor GetRandomDoor(DRandom random) {
        doors.Shuffle(random.random);
        for (int i = 0; i < doors.Count; i++) {
            if (doors[i].open) return doors[i];
        }
        Debug.LogError("Room::GetRandomDoor() - No open doors...");
        return null;
    }

    public bool hasOpenDoors() {
        for (int i = 0; i < doors.Count; i++) {
            if (doors[i].open) return true;
        }
        return false;
    }

    public GeneratorDoor GetFirstOpenDoor() {
        for (int i = 0; i < doors.Count; i++) {
            if (doors[i].open) return doors[i];
        }
        Debug.LogError("Room::GetFirstOpenDoor() - No open doors...");
        return null;
    }


    // Logs all children of GeneratorGeometry that contain a Feature class, so we can replace them
    // with the appropriate tileset upon generation.  Used when designing a room.
    [ContextMenu("Save All Feature Positions")]
    public void SaveFeaturePositionAndDirection() 
    {
        GameObject geometryParent = transform.Find("GeneratorGeometry").gameObject;
        features.Clear();


        for (int i = 0; i < geometryParent.transform.childCount; i++)
        {
            var child = geometryParent.transform.GetChild(i);
            Feature feature = child.GetComponent<Feature>();
            if (!feature) 
            {
                continue;
            }
            Vector3 pos = child.position;
            Vector3 dir = child.rotation.eulerAngles;

            feature.Position = pos;
            feature.Direction = dir;

            features.Add(feature);

            Debug.Log("Added feature of type " + feature.type.ToString() + "!");
        }
    }
}


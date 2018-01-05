using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DungeonSet : ScriptableObject {

    public string Name = "";
    public DungeonType.Biome Biome;
    public BiomeFeatureSets BiomeFeatureSet;

    public List<Room> spawns = new List<Room>();
    public List<Room> bosses = new List<Room>();
    public List<Door> doors = new List<Door>();
    public List<Room> roomTemplates = new List<Room>();

}

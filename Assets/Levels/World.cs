using UnityEngine;
using System.Collections;

public class World : MonoBehaviour {

	public GameObject chunk;
	public Chunk[,,] chunks;
	public int chunkSize = 16;

	public byte[,,] data;
	public int worldSizeX = 16;
	public int worldSizeY = 16;
	public int worldSizeZ = 16;

	// Use this for initialization
	void Start () {
		print ("Start World");
		Texture.InitTextures ();
		data = new byte[worldSizeX, worldSizeY, worldSizeZ];
		chunks = new Chunk[Mathf.FloorToInt (worldSizeX / chunkSize), Mathf.FloorToInt (worldSizeY / chunkSize), Mathf.FloorToInt (worldSizeZ / chunkSize)];
		data = WorldGenerator.GenerateWorld (data);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void GenColumn(int x, int z) {
		for (int y = 0; y < chunks.GetLength (1); y++){				
			//Create a temporary Gameobject for the new chunk instead of using chunks[x,y,z]
			GameObject newChunk = (GameObject)Instantiate (chunk, new Vector3(x * chunkSize - 0.5f, y * chunkSize + 0.5f, z * chunkSize - 0.5f), new Quaternion(0, 0, 0, 0));

			//Now instead of using a temporary variable for the script assign it
			//to chunks[x,y,z] and use it instead of the old \"newChunkScript\" 
			chunks[x,y,z] = (Chunk)newChunk.GetComponent ("Chunk");
			chunks[x,y,z].worldGO = gameObject;
			chunks[x,y,z].chunkX = x * chunkSize;
			chunks[x,y,z].chunkY = y * chunkSize;
			chunks[x,y,z].chunkZ = z * chunkSize;				
		}
	}

	public void UnloadColumn(int x, int z) {
		for (int y = 0; y < chunks.GetLength (1); y++){				
			Object.Destroy(chunks[x, y, z].gameObject);				
		}
	}

	public byte Block(int x, int y, int z) {
		if(x > (worldSizeX-1) || x < 0 || y > (worldSizeY-1) || y < 0 || z > (worldSizeZ-1) || z < 0) {
			return Texture.VOID;
		} else {
			return data[x, y, z];
		}
	}
}

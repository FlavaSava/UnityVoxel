using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Chunk : MonoBehaviour {

	//Positions Informationen: Welcehr Chunk bin ich?
	public int chunkX;
	public int chunkY;
	public int chunkZ;

	//Bestimmt, ob der Chunk sich neu rendert
	public bool update;

	//Zugriff auf die Welt
	public GameObject worldGO;

	//Zeichenplatte
	private Mesh mesh;
	private MeshCollider col;

	//Informationen über alle Punkte und wie sie verbunden werden sollen
	private List<Vector3> newVertices = new List<Vector3>();
	private List<Vector2> newUV = new List<Vector2>();
	private List<int> newTriangles = new List<int>();

	//Zählt wie viel faces gespeichert sind
	private int faceCount;

	//Zugriff auf Funktionen im World-Skript
	private World world;

	// Use this for initialization
	void Start () {
		mesh = GetComponent<MeshFilter> ().mesh;
		col = GetComponent<MeshCollider> ();
		world = (World)worldGO.GetComponent ("World");
		GenerateMesh ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void LateUpdate() {
		if (update) {
			GenerateMesh();
			update = false;
		}
	}

	void CubeTop(int x, int y, int z, byte block) {
		newVertices.Add(new Vector3 (x,  y,  z + 1));
		newVertices.Add(new Vector3 (x + 1, y,  z + 1));
		newVertices.Add(new Vector3 (x + 1, y,  z ));
		newVertices.Add(new Vector3 (x,  y,  z ));

		Cube (Texture.GetTexture(block, Texture.TOP));
	}

	void CubeNorth(int x, int y, int z, byte block) {
		//CubeNorth
		newVertices.Add(new Vector3 (x + 1, y-1, z + 1));
		newVertices.Add(new Vector3 (x + 1, y, z + 1));
		newVertices.Add(new Vector3 (x, y, z + 1));
		newVertices.Add(new Vector3 (x, y-1, z + 1));

		Cube (Texture.GetTexture(block, Texture.SIDE));
	}

	void CubeSouth(int x, int y, int z, byte block) {
		//CubeSouth
		newVertices.Add(new Vector3 (x, y - 1, z));
		newVertices.Add(new Vector3 (x, y, z));
		newVertices.Add(new Vector3 (x + 1, y, z));
		newVertices.Add(new Vector3 (x + 1, y - 1, z));

		Cube (Texture.GetTexture(block, Texture.SIDE));
	}

	void CubeWest(int x, int y, int z, byte block) {
		//CubeWest
		newVertices.Add(new Vector3 (x, y- 1, z + 1));
		newVertices.Add(new Vector3 (x, y, z + 1));
		newVertices.Add(new Vector3 (x, y, z));
		newVertices.Add(new Vector3 (x, y - 1, z));

		Cube (Texture.GetTexture(block, Texture.SIDE));
	}

	void CubeEast(int x, int y, int z, byte block) {
		//CubeEast
		newVertices.Add(new Vector3 (x + 1, y - 1, z));
		newVertices.Add(new Vector3 (x + 1, y, z));
		newVertices.Add(new Vector3 (x + 1, y, z + 1));
		newVertices.Add(new Vector3 (x + 1, y - 1, z + 1));

		Cube (Texture.GetTexture(block, Texture.SIDE));
	}

	void CubeBottom(int x, int y, int z, byte block) {
		//CubeBot
		newVertices.Add(new Vector3 (x,  y-1,  z ));
		newVertices.Add(new Vector3 (x + 1, y-1,  z ));
		newVertices.Add(new Vector3 (x + 1, y-1,  z + 1));
		newVertices.Add(new Vector3 (x,  y-1,  z + 1));

		Cube (Texture.GetTexture(block, Texture.BOTTOM));
	}

	void Cube(Vector2 texturePos) {
		newTriangles.Add(faceCount * 4  ); //1
		newTriangles.Add(faceCount * 4 + 1 ); //2
		newTriangles.Add(faceCount * 4 + 2 ); //3
		newTriangles.Add(faceCount * 4  ); //1
		newTriangles.Add(faceCount * 4 + 2 ); //3
		newTriangles.Add(faceCount * 4 + 3 ); //4

		newUV.Add(new Vector2 (Texture.tUnit * texturePos.x + Texture.tUnit, Texture.tUnit * texturePos.y));
		newUV.Add(new Vector2 (Texture.tUnit * texturePos.x + Texture.tUnit, Texture.tUnit * texturePos.y + Texture.tUnit));
		newUV.Add(new Vector2 (Texture.tUnit * texturePos.x, Texture.tUnit * texturePos.y + Texture.tUnit));
		newUV.Add(new Vector2 (Texture.tUnit * texturePos.x, Texture.tUnit * texturePos.y));

		faceCount++;
	}

	public void GenerateMesh(){		
		for (int x = 0; x < world.chunkSize; x++){
			for (int y = 0; y < world.chunkSize; y++){
				for (int z = 0; z < world.chunkSize; z++){
					//This code will run for every block in the chunk
					
					if(Block (x, y, z) != 0){
						//If the block is solid
						
						if(Block (x, y + 1, z) == 0){
							//Block above is air
							CubeTop (x, y , z , Block (x, y, z));
						}
						
						if(Block (x, y - 1, z) == 0){
							//Block below is air
							CubeBottom (x , y , z , Block (x, y, z));
							
						}
						
						if(Block (x + 1, y, z) == 0){
							//Block east is air
							CubeEast (x , y , z , Block (x, y, z));
							
						}
						
						if(Block (x - 1, y, z) == 0){
							//Block west is air
							CubeWest (x , y , z , Block (x, y, z));
							
						}

						if(Block (x, y, z + 1) == 0){
							//Block north is air
							CubeNorth (x , y , z , Block (x, y, z));
							
						}
						
						if(Block (x, y, z - 1) == 0){
							//Block south is air
							CubeSouth (x , y , z , Block (x, y, z));
							
						}						
					}					
				}
			}
		}
		UpdateMesh ();
	}

	byte Block(int x, int y, int z) {
		return world.Block (x + chunkX, y + chunkY, z + chunkZ);
	}
	
	void UpdateMesh() {
		mesh.Clear ();
		mesh.vertices = newVertices.ToArray();
		mesh.uv = newUV.ToArray();
		mesh.triangles = newTriangles.ToArray();
		mesh.Optimize ();
		mesh.RecalculateNormals ();
		
		col.sharedMesh = null;
		col.sharedMesh = mesh;
		
		newVertices.Clear();
		newUV.Clear();
		newTriangles.Clear();
		
		faceCount=0;
	}
}

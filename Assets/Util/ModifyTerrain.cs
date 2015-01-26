using UnityEngine;
using System.Collections;

public class ModifyTerrain : MonoBehaviour {

    public GameObject worldObject;

    public int viewDistance = 5;

	private World world;
	private GameObject cameraGO;

	void Start() {
		world = (World)GetComponent ("World");
		cameraGO = GameObject.FindGameObjectWithTag ("MainCamera");     
	}

	void Update() {
		loadChunks(cameraGO.transform.position,32,48);
        MarkActiveBlock ();
		if (Input.GetMouseButtonDown (0)) {
			ReplaceBlockCursor(Texture.VOID);
		}
		if(Input.GetMouseButtonDown(1)) {
			AddBlockCursor(Texture.GRASS);
		}
	}

    void LateUpdate () {
        
    }

	public void loadChunks(Vector3 playerPos, float distToLoad, float distToUnload) {
		for(int x = 0; x < world.chunks.GetLength(0); x++){
			for(int z = 0; z < world.chunks.GetLength(2); z++){
				
				float dist = Vector2.Distance(new Vector2(x * world.chunkSize, z * world.chunkSize), new Vector2(playerPos.x, playerPos.z));
				
				if(dist < distToLoad){
					if(world.chunks[x, 0, z] == null){
						world.GenColumn(x, z);
					}
				} else if(dist > distToUnload){
					if(world.chunks[x, 0, z] != null){						
						world.UnloadColumn(x,z);
					}
				}				
			}
		}
	}

	public void ReplaceBlockCenter(float range, byte block){
		//Replaces the block directly in front of the player
		Ray ray = new Ray (cameraGO.transform.position, cameraGO.transform.forward);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit)) {
			if(hit.distance < range) {
				ReplaceBlockAt(hit, block);
			}
		}
	}
	
	public void AddBlockCenter(float range, byte block){
		//Adds the block specified directly in front of the player
		Ray ray = new Ray (cameraGO.transform.position, cameraGO.transform.forward);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit)) {
			if (hit.distance < range) {
				AddBlockAt (hit, block);
			}
			Debug.DrawLine (ray.origin, ray.origin + (ray.direction * hit.distance), Color.green, 2);
		}
	}
	
	public void ReplaceBlockCursor(byte block){
		//Replaces the block specified where the mouse cursor is pointing
        Vector3 focusedBlock = GetFocusedBlock ();
        if (Vector3.Distance (focusedBlock, cameraGO.transform.position) <= viewDistance) {
            SetBlockAt (focusedBlock, block);
        }	
	}
	
	public void AddBlockCursor(byte block){
		//Adds the block specified where the mouse cursor is pointing
		Vector3 focusedBlock = GetBlockNextToFocused();
        if (Vector3.Distance (focusedBlock, cameraGO.transform.position) <= viewDistance) {
            SetBlockAt(focusedBlock, block);
	    }
    }
	
	public void ReplaceBlockAt(RaycastHit hit, byte block) {
		//removes a block at these impact coordinates, you can raycast against the terrain and call this with the hit.point
		Vector3 position = hit.point;
		position += (hit.normal * - 0.5f);
		SetBlockAt (position, block);
	}
	
	public void AddBlockAt(RaycastHit hit, byte block) {
		//adds the specified block at these impact coordinates, you can raycast against the terrain and call this with the hit.point
		Vector3 position = hit.point;
		position += (hit.normal * 0.5f);
		SetBlockAt (position, block);
	}
	
	public void SetBlockAt(Vector3 position, byte block) {
		//sets the specified block at these coordinates
		SetBlockAt (Mathf.RoundToInt (position.x), Mathf.RoundToInt (position.y), Mathf.RoundToInt (position.z), block);
	}
	
	public void SetBlockAt(int x, int y, int z, byte block) {
		//adds the specified block at these coordinates
        if (x < world.worldSizeX && y < world.worldSizeY && z < world.worldSizeZ) {
            world.data [x, y, z] = block;
		    UpdateChunkAt (x, y, z);
	    }
    }
	
	public void UpdateChunkAt(int x, int y, int z) {
		//Updates the chunk containing this block
		int updateX = Mathf.FloorToInt(x / world.chunkSize);
		int updateY = Mathf.FloorToInt(y / world.chunkSize);
		int updateZ = Mathf.FloorToInt(z / world.chunkSize);

		if(x - (world.chunkSize * updateX) == 0 && updateX != 0){
			world.chunks[updateX - 1, updateY, updateZ].update = true;
		}
		
		if(x - (world.chunkSize * updateX) == 15 && updateX != world.chunks.GetLength(0) - 1){
			world.chunks[updateX + 1 ,updateY, updateZ].update = true;
		}
		
		if(y - (world.chunkSize * updateY) == 0 && updateY != 0){
			world.chunks[updateX, updateY - 1, updateZ].update = true;
		}
		
		if(y - (world.chunkSize * updateY) == 15 && updateY != world.chunks.GetLength(1) - 1){
			world.chunks[updateX, updateY + 1, updateZ].update = true;
		}
		
		if(z - (world.chunkSize * updateZ) == 0 && updateZ != 0){
			world.chunks[updateX, updateY, updateZ - 1].update = true;
		}
		
		if(z - (world.chunkSize * updateZ) == 15 && updateZ != world.chunks.GetLength(2) - 1){
			world.chunks[updateX, updateY, updateZ + 1].update = true;
		}

		world.chunks [updateX, updateY, updateZ].update = true;
	}

    public void MarkActiveBlock() {
        Vector3 focusedBlock = GetFocusedBlock();

        if (Vector3.Distance (focusedBlock, cameraGO.transform.position) <= viewDistance) {
            if (focusedBlock.x < world.worldSizeX && focusedBlock.y < world.worldSizeY && focusedBlock.z < world.worldSizeZ) {
                world.data[(int)focusedBlock.x, (int)focusedBlock.y, (int)focusedBlock.z] += 128;
                UpdateChunkAt ((int)focusedBlock.x, (int)focusedBlock.y, (int)focusedBlock.z);       
            }
        }
    }

    public Vector3 GetFocusedBlock () {
        Ray r = Camera.main.ScreenPointToRay (Input.mousePosition);
        RaycastHit hit;
        if (!Physics.Raycast (r, out hit)) {
            return new Vector3(world.worldSizeX, world.worldSizeY, world.worldSizeZ);
        }
        Vector3 position = hit.point;
        position += (hit.normal * -0.5f);

        position.x = Mathf.RoundToInt (position.x);
        position.y = Mathf.RoundToInt (position.y);
        position.z = Mathf.RoundToInt (position.z); 
         
        return position;  
    }

    public Vector3 GetBlockNextToFocused () {
        Ray r = Camera.main.ScreenPointToRay (Input.mousePosition);
        RaycastHit hit;
        if (!Physics.Raycast (r, out hit)) {
            return new Vector3 (world.worldSizeX, world.worldSizeY, world.worldSizeZ);
        }
        Vector3 position = hit.point;
        position += (hit.normal * 0.5f);

        position.x = Mathf.RoundToInt (position.x);
        position.y = Mathf.RoundToInt (position.y);
        position.z = Mathf.RoundToInt (position.z);

        return position;
    }

}

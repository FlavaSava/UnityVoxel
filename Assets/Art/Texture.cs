using UnityEngine;
using System.Collections.Generic;

//Texture Class for TileSheet128x128.png
public class Texture {

	public const int TOP = 1000;
	public const int BOTTOM = 2000;
	public const int SIDE = 3000;
    public const bool ACTIVE = true;
    public const bool INACTIVE = false;
	
	//Texture Coords
	static Dictionary<int, Vector2> TEXTURE_COORDS = new Dictionary<int, Vector2>(); //Normale Textur Koordinaten
    static Dictionary<int, Vector2> TEXTURE_COORDS_ACTIVE = new Dictionary<int, Vector2>(); //Textur Koordinaten mit einem Schwarzen Rand
	
	//Texture IDs
	public const byte VOID = 000;
	public const byte GRASS = 001;
	public const byte DIRT = 002;
	public const byte SAND = 003;
	public const byte STONE = 004;
	public const byte WOOD = 005;
	
	//Size per Texture
	public const float tUnit = 0.25f;
	
	//Add Texture Coords into Dictionary
	//This should be called once at Startup
	public static void InitTextures() {
		StoreTexture (GRASS, new Vector2 (0, 3), new Vector2(1,3), new Vector2(1,2), new Vector2(0,1), new Vector2(1,1), new Vector2(1,0));
		StoreTexture (SAND, new Vector2 (2, 3), new Vector2(2,1));
		StoreTexture (DIRT, new Vector2 (1, 3), new Vector2(1,1));
		StoreTexture (STONE, new Vector2 (3, 3), new Vector2(3, 1));
		StoreTexture (WOOD, new Vector2 (3,2), new Vector2(3,2), new Vector2(0, 2), new Vector2(2,0), new Vector2(2,0), new Vector2(0,0));
	}

	//Gibt die Texture mit der ID passen zu dem face zurück
	public static Vector2 GetTexture(byte id, int face, bool active) {
        if (!active) { 
		    return TEXTURE_COORDS [(face + id)];
        } else {
            return TEXTURE_COORDS_ACTIVE[(face + id)];
        }
	}

	//Fügt eine Texture in den Speicher ein, dessen Seiten alle
	//die gleiche Textur haben
	static void StoreTexture(byte id, Vector2 texCoordInactive, Vector2 texCoordActive) {
        StoreTexture(id, texCoordInactive, texCoordInactive, texCoordInactive, texCoordActive, texCoordActive, texCoordActive);
	}

	//Fügt eine Texture in den Speicher ein, die verschiede
	//Texture für jedes Face bereithält
    static void StoreTexture(byte id, Vector2 texCoordInactiveTop, Vector2 texCoordInactiveBot, Vector2 texCoordInactiveSide, Vector2 texCoordActiveTop, Vector2 texCoordActiveBot, Vector2 texCoordActiveSide) {
		TEXTURE_COORDS.Add ((TOP + id), texCoordInactiveTop);
		TEXTURE_COORDS.Add ((BOTTOM + id), texCoordInactiveBot);
		TEXTURE_COORDS.Add ((SIDE + id), texCoordInactiveSide);

        TEXTURE_COORDS_ACTIVE.Add((TOP + id), texCoordActiveTop);
        TEXTURE_COORDS_ACTIVE.Add((BOTTOM + id), texCoordActiveBot);
        TEXTURE_COORDS_ACTIVE.Add((SIDE + id), texCoordActiveSide);
	}

}

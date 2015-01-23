using UnityEngine;
using System.Collections.Generic;

//Texture Class for TileSheet128x128.png
public class Texture {

	public const int TOP = 1000;
	public const int BOTTOM = 2000;
	public const int SIDE = 3000;
	
	//Texture Coords
	static Dictionary<int, Vector2> TEXTURE_COORDS = new Dictionary<int, Vector2>();
	
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
		StoreTexture (GRASS, new Vector2 (0, 3));
		StoreTexture (SAND, new Vector2 (2, 3));
		StoreTexture (DIRT, new Vector2 (1, 3));
		StoreTexture (STONE, new Vector2 (3, 3));
		StoreTexture (WOOD, new Vector2 (0, 2));
	}

	//Gibt die Texture mit der ID passen zu dem face zurück
	public static Vector2 GetTexture(byte id, int face) {
		return TEXTURE_COORDS [(face + id)];
	}

	//Fügt eine Texture in den Speicher ein, dessen Seiten alle
	//die gleiche Textur haben
	static void StoreTexture(byte id, Vector2 texCoord) {
		StoreTexture (id, texCoord, texCoord, texCoord);
	}

	//Fügt eine Texture in den Speicher ein, die verschiede
	//Texture für jedes Face bereithält
	static void StoreTexture(byte id, Vector2 texCoordTop, Vector2 texCoordBot, Vector2 texCoordSide) {
		TEXTURE_COORDS.Add ((TOP + id), texCoordTop);
		TEXTURE_COORDS.Add ((BOTTOM + id), texCoordBot);
		TEXTURE_COORDS.Add ((SIDE + id), texCoordSide);
	}

}

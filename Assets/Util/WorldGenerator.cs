
public class WorldGenerator {

	public static byte[,,] GenerateWorld (byte[,,] data) {
		for (int x = 0; x < data.GetLength(0); x++){
			for (int z = 0; z < data.GetLength(2); z++){
				int stone = MathUtils.PerlinNoise (x, 0, z, 10, 3, 1.2f);
				stone += MathUtils.PerlinNoise (x, 300, z, 20, 4, 0) + 10;
				int dirt = MathUtils.PerlinNoise(x, 100, z, 50, 2, 0) + 1; //Added +1 to make sure minimum grass height is 1
				
				for (int y = 0; y < data.GetLength(1); y++){
					if(y <= stone){
						data[x, y, z] = Texture.SAND;
					} else if(y <= dirt + stone){
						data[x, y, z] = Texture.WOOD;
					}
				}
			}
		}
		return data;
	}

}
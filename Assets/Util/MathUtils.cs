using UnityEngine;
using System.Collections;

public class MathUtils {
	public static int PerlinNoise(int x,int y, int z, float scale, float height, float power){
		float rValue;
		rValue = Noise.GetNoise (((double)x) / scale, ((double)y) / scale, ((double)z) / scale);
		rValue *= height;

		if (power != 0) {
			rValue = Mathf.Pow (rValue, power);
		}
			return (int)rValue;
		}
}

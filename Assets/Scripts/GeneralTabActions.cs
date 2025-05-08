using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralTabActions : MonoBehaviour
{
	/// <summary>
	/// 创建Cube
	/// </summary>
	public void CreateCube()
	{
		GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
		cube.name = "Cube_" + System.Guid.NewGuid().ToString("N").Substring(0, 4);
		cube.transform.position = Vector3.zero;
	}

	/// <summary>
	/// 创建Sphere
	/// </summary>
	public void CreateSphere()
	{
		GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		sphere.name = "Sphere_" + System.Guid.NewGuid().ToString("N").Substring(0, 4);
		sphere.transform.position = Vector3.zero;
	}
}

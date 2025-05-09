using UnityEngine;

namespace EchoShaderLab.Command
{
	/// <summary>
	/// 创建Cube
	/// </summary>
	public class CreateCube : IMenuCommand
	{
		public void Execute()
		{
			GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
			cube.name = "Cube_" + System.Guid.NewGuid().ToString("N").Substring(0, 4);
			cube.transform.position = Vector3.zero;
		}
	}

	/// <summary>
	/// 创建Sphere
	/// </summary>
	public class CreateSphere : IMenuCommand
	{
		public void Execute()
		{
			GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			sphere.name = "Sphere_" + System.Guid.NewGuid().ToString("N").Substring(0, 4);
			sphere.transform.position = Vector3.zero;
		}
	}

}

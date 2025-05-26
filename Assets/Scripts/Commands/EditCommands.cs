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
			cube.transform.localScale = Vector3.one;

			//设置图层为previewLayer
			cube.layer = LayerMask.NameToLayer("PreviewLayer");
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

			//设置图层为previewLayer
			sphere.layer = LayerMask.NameToLayer("PreviewLayer");
		}
	}

	/// <summary>
	/// 创建Cylinder
	/// </summary>
	public class CreateCylinder : IMenuCommand
	{
		public void Execute()
		{
			GameObject cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
			cylinder.name = "Cylinder_" + System.Guid.NewGuid().ToString("N").Substring(0, 4);
			cylinder.transform.position = Vector3.zero;

			//设置图层为previewLayer
			cylinder.layer = LayerMask.NameToLayer("PreviewLayer");
		}
	}

	/// <summary>
	/// 创建Capsule
	/// </summary>
	public class CreateCapsule : IMenuCommand
	{
		public void Execute()
		{
			GameObject capsule = GameObject.CreatePrimitive(PrimitiveType.Capsule);
			capsule.name = "Capsule_" + System.Guid.NewGuid().ToString("N").Substring(0, 4);
			capsule.transform.position = Vector3.zero;

			//设置图层为previewLayer
			capsule.layer = LayerMask.NameToLayer("PreviewLayer");
		}
	}
}

using System.Collections.Generic;

namespace EchoShaderLab
{
	/// <summary>
	/// 菜单栏命令注册类
	/// </summary>
	public static class MenuCommandRegistry
	{
		private static Dictionary<string, Command.IMenuCommand> m_CommandMap = new Dictionary<string, Command.IMenuCommand>();

		/// <summary>
		/// 初始化
		/// </summary>
		public static void Init()
		{
			m_CommandMap.Clear();

			//命令注册
			Register("Cube", new Command.CreateCube());
			Register("Sphere", new Command.CreateSphere());
			Register("Cylinder", new Command.CreateCylinder());
			Register("Capsule", new Command.CreateCapsule());
		}

		/// <summary>
		/// 获取命令实例
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public static Command.IMenuCommand GetCommand(string name)
		{
			if (m_CommandMap.ContainsKey(name))
				return m_CommandMap[name];
			return null;
		}

		/// <summary>
		/// 命令注册
		/// </summary>
		/// <param name="name"></param>
		/// <param name="command"></param>
		private static void Register(string name, Command.IMenuCommand command)
		{
			m_CommandMap.Add(name, command);
		}
	}
}


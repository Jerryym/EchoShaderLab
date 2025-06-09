using System.Collections.Generic;
using UnityEngine;

namespace EchoShaderLab
{
	/// <summary>
	/// RT缓存池
	/// </summary>
	public static class RenderTexturePool
	{
		/// <summary>
		/// RT缓存池容器
		/// </summary>
		private static Dictionary<(int width, int height), Stack<RenderTexture>> m_RTPool = new Dictionary<(int width, int height), Stack<RenderTexture>> ();

		/// <summary>
		/// 获取RenderTexture, 若不存在则创建一个
		/// </summary>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="depth"></param>
		/// <returns></returns>
		public static RenderTexture Get(int width, int height, int depth = 24)
		{
			var key = (width, height);
			if (m_RTPool.TryGetValue(key, out var stackRT) && stackRT.Count > 0)
			{
				var renderTexture = stackRT.Pop();
				if (renderTexture != null && renderTexture.IsCreated())
				{
					return renderTexture;
				}
			}

			//没有匹配的RenderTexture, 则创建一个
			RenderTexture newRT = new RenderTexture(width, height, depth);
			newRT.name = $"RT_{width}*{height}";
			newRT.Create();
			return newRT;
		}

		/// <summary>
		/// 释放RenderTexture
		/// </summary>
		/// <param name="renderTexture"></param>
		public static void Release(RenderTexture renderTexture)
		{
			if (renderTexture == null)	return;
			
			var key = (renderTexture.width, renderTexture.height);
			if (!m_RTPool.ContainsKey(key))//判断容器中是否存在相同尺寸的RT，若不存在则创建一个新的
			{
				m_RTPool[key] = new Stack<RenderTexture>();
			}

			//释放已有数据
			renderTexture.DiscardContents();
			m_RTPool[key].Push(renderTexture);
		}

		/// <summary>
		/// 清空缓存池
		/// </summary>
		public static void Clear()
		{
			foreach (var stack in m_RTPool.Values)
			{
				while (stack.Count > 0)
				{
					var renderTexture = stack.Pop();
					renderTexture?.Release();
				}
			}
			m_RTPool.Clear();
		}
	}
}

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace EchoShaderLab.UI
{
	/// <summary>
	/// 预览区域
	/// </summary>
	public class PreviewPanel : MonoBehaviour
	{
		/// <summary>
		/// 预览区域Layer
		/// </summary>
		public LayerMask previewLayer;

		/// <summary>
		/// 预览相机
		/// </summary>
		private Camera m_preivewCamera;
		/// <summary>
		/// RawImage
		/// </summary>
		private RawImage m_previewImage;
		/// <summary>
		/// 渲染纹理
		/// </summary>
		private RenderTexture m_renderTexture;

		private void Start()
		{
			//初始化RawImage
			InitRawImage();
			//初始化预览相机
			InitPreviewCamera();

			StartCoroutine(ResizeRenderTexture());
		}

		/// <summary>
		/// 初始化RawImage
		/// </summary>
		private void InitRawImage()
		{
			GameObject RawImageGO = new GameObject("SceneView", typeof(RectTransform), typeof(RawImage));
			RawImageGO.transform.SetParent(transform, false);

			//RectTransform组件
			RectTransform rt = RawImageGO.GetComponent<RectTransform>();
			rt.anchorMin = Vector2.zero;
			rt.anchorMax = Vector2.one;
			rt.offsetMin = Vector2.zero;
			rt.offsetMax = Vector2.zero;
			rt.pivot = new Vector2(0.5f, 0.5f);

			//RawImage组件
			m_previewImage = RawImageGO.GetComponent<RawImage>();
		}

		/// <summary>
		/// 初始化预览相机
		/// </summary>
		private void InitPreviewCamera()
		{
			GameObject cameraGO = new GameObject("PreviewCamera");
			m_preivewCamera = cameraGO.AddComponent<Camera>();
			m_preivewCamera.enabled = true;

			//设置基本相机参数
			m_preivewCamera.transform.position = new Vector3(0, 1, -10);
			m_preivewCamera.orthographic = false;
			m_preivewCamera.fieldOfView = 60;

			//设置背景色
			m_preivewCamera.clearFlags = CameraClearFlags.SolidColor;
			m_preivewCamera.backgroundColor = Color.gray;

			//渲染指定图层
			m_preivewCamera.cullingMask = previewLayer;
		}

		/// <summary>
		/// 根据UI尺寸重置RT尺寸
		/// </summary>
		private void ResizeRenderTextureToMatchUI()
		{
			RectTransform rectTransform = m_previewImage.GetComponent<RectTransform>();
			Rect imageRect = rectTransform.rect;
			int width = Mathf.Max(1, Mathf.RoundToInt(imageRect.width));
			int height = Mathf.Max(1, Mathf.RoundToInt(imageRect.height));

			if (m_renderTexture != null)
			{
				if (m_renderTexture.width != width || m_renderTexture.height != width)
				{
					// 释放进缓存池
					RenderTexturePool.Release(m_renderTexture);
				}
			}

			m_renderTexture = RenderTexturePool.Get(width, height);
			m_previewImage.texture = m_renderTexture;
			m_preivewCamera.targetTexture = m_renderTexture;
			m_preivewCamera.aspect = (float)width / height;
		}

		/// <summary>
		/// 协程: 重置RenderTexture尺寸
		/// </summary>
		/// <returns></returns>
		private IEnumerator ResizeRenderTexture()
		{
			yield return null;
			ResizeRenderTextureToMatchUI();
		}
	}
}

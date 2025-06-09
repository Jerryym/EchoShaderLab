using TMPro;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.UI;

namespace EchoShaderLab.Controller
{
	/// <summary>
	/// 预览控制器
	/// </summary>
	public class PreviewController
	{
		/// <summary>
		/// 预览区域相机
		/// </summary>
		private Camera m_Camera;
		private RawImage m_Image;
		private LayerMask m_layerMask;
		/// <summary>
		/// 当前选中的对象
		/// </summary>
		private GameObject m_selectedObj;
		/// <summary>
		/// 模型原始材质
		/// </summary>
		private Material m_originalMaterial;
		/// <summary>
		/// 高亮材质
		/// </summary>
		private Material m_highlightMaterial;

		private Plane m_dragPlane;

		public PreviewController(Camera camera, RawImage image)
		{
			m_Camera = camera;
			m_Image = image;
			m_layerMask = LayerMask.GetMask("PreviewLayer");
			m_highlightMaterial = Resources.Load<Material>("Materials/HighlightMat");
		}

		/// <summary>
		/// 选择对象
		/// </summary>
		public void SelectObject()
		{
			//判断是否按下鼠标左键
			if (!Input.GetMouseButton(0))
				return;

			//判断鼠标点击的位置是否在RawImage的RectTransform中
			RectTransform rectTransform = m_Image.GetComponent<RectTransform>();
			if (!RectTransformUtility.RectangleContainsScreenPoint(rectTransform, Input.mousePosition)) 
				return;

			//将屏幕空间点转换为RawImage的RectTransform的本地空间中位于其矩形平面上的一个位置
			RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, Input.mousePosition, null, out Vector2 localPos);

			//射线探测
			Rect rect = rectTransform.rect;
			Vector2 viewport = new Vector2(localPos.x / rect.width + 0.5f, localPos.y / rect.height + 0.5f);
			Ray ray = m_Camera.ViewportPointToRay(viewport);
			if (Physics.Raycast(ray, out RaycastHit hit, 1000f, m_layerMask))
			{
				if (m_selectedObj != null && m_originalMaterial != null)
				{
					//恢复原始材质
					var renderer = m_selectedObj.GetComponent<Renderer>();
					if (renderer != null)
						renderer.material = m_originalMaterial;
				}
				m_selectedObj = hit.collider.gameObject;

				//高亮
				var selectedRenderer = m_selectedObj.GetComponent<Renderer>();
				if (selectedRenderer != null)
				{
					m_originalMaterial = selectedRenderer.material;
					selectedRenderer.material = m_highlightMaterial;
				}
			}
		}

		/// <summary>
		/// 鼠标缩放
		/// </summary>
		public void Zoom()
		{
			// 鼠标滚轮控制缩放
			if (Input.mouseScrollDelta.y != 0)
			{
				m_Camera.transform.position += m_Camera.transform.forward * Input.mouseScrollDelta.y * 0.5f;
			}
		}

		/// <summary>
		/// 旋转
		/// </summary>
		public void Rotation()
		{
			if (m_selectedObj == null) return;

			if (Input.GetMouseButton(1))  // 右键拖动
			{
				float rotX = Input.GetAxis("Mouse X") * 5f;
				float rotY = -Input.GetAxis("Mouse Y") * 5f;
				m_selectedObj.transform.Rotate(Vector3.up, rotX, Space.World);
				m_selectedObj.transform.Rotate(Vector3.right, rotY, Space.World);
			}
		}

		/// <summary>
		/// 移动
		/// </summary>
		public void Dragging()
		{
			if (m_selectedObj != null && Input.GetMouseButton(0))
			{
				RectTransform rectTransform = m_Image.GetComponent<RectTransform>();
				if (!RectTransformUtility.RectangleContainsScreenPoint(rectTransform, Input.mousePosition))
					return;

				RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, Input.mousePosition, null, out Vector2 localPos);
				Rect rect = rectTransform.rect;
				Vector2 viewport = new Vector2(localPos.x / rect.width + 0.5f, localPos.y / rect.height + 0.5f);
				Ray ray = m_Camera.ViewportPointToRay(viewport);

				if (m_dragPlane.Raycast(ray, out float enter))
				{
					Vector3 hitPoint = ray.GetPoint(enter);
					m_selectedObj.transform.position = hitPoint;
				}
			}

			if (Input.GetMouseButtonUp(0))
			{
				m_selectedObj = null;
			}
		}
	}
}

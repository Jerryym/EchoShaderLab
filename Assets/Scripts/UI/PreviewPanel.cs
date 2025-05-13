using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class PreviewPanel : MonoBehaviour
{

	/// <summary>
	/// 渲染纹理
	/// </summary>
	public RenderTexture renderTexture;
	/// <summary>
	/// 预览区域Layer
	/// </summary>
	public LayerMask previewLayer;

	/// <summary>
	/// 预览相机
	/// </summary>
	private Camera m_preivewCamera;
	private RawImage previewImage;


	private void Awake()
	{
		//初始化RawImage
		InitRawImage();
		//初始化预览相机
		InitPreviewCamera();
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
		previewImage = RawImageGO.GetComponent<RawImage>();
		previewImage.texture = renderTexture;
	}

	/// <summary>
	/// 初始化预览相机
	/// </summary>
	private void InitPreviewCamera()
	{
		GameObject cameraGO = new GameObject("PreviewCamera");
		cameraGO.transform.SetParent(transform, false);
		m_preivewCamera = cameraGO.AddComponent<Camera>();
		m_preivewCamera.enabled = true;

		//设置背景色
		m_preivewCamera.clearFlags = CameraClearFlags.SolidColor;
		m_preivewCamera.backgroundColor = Color.gray;

		//渲染指定图层
		m_preivewCamera.cullingMask = previewLayer;

		//将渲染结果输出到RenderTexture
		m_preivewCamera.targetTexture = renderTexture;
	}
}

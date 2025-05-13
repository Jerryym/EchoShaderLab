using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// UI生成器
/// </summary>
public class UIBuilder : MonoBehaviour
{
	/// <summary>
	/// 生成UI
	/// </summary>
	public void BuildUI()
	{
		//创建Canvas
		GameObject CanvasGO = CreateCanvas("MainCanvas");

		//创建菜单栏
		GameObject menuBar = CreatePanel("MenuBar", CanvasGO.transform);
		InitMenuBar(menuBar);

		//创建主面板
		GameObject mainContent = CreatePanel("MainContent", CanvasGO.transform);
		InitMainContent(mainContent);
	}

	/// <summary>
	/// 创建Canvas
	/// </summary>
	/// <returns></returns>
	private GameObject CreateCanvas(string name)
	{
		GameObject canvasGO = new GameObject(name, typeof(RectTransform));
		canvasGO.layer = LayerMask.NameToLayer("UI");

		//添加Canvas组件
		Canvas canvas = canvasGO.AddComponent<Canvas>();
		canvas.renderMode = RenderMode.ScreenSpaceOverlay;
		
		//添加CanvasScaler组件
		CanvasScaler canvasScaler = canvasGO.AddComponent<CanvasScaler>();
		canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
		canvasScaler.referenceResolution = new Vector2(1980, 1080);
		canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
		canvasScaler.matchWidthOrHeight = 0.5f;
		
		//添加GraphicRaycaster组件
		canvasGO.AddComponent<GraphicRaycaster>();

		//创建EventSystem
		if (FindAnyObjectByType<EventSystem>() == null)
		{
			GameObject eventSystem = new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
		}
		return canvasGO;
	}

	/// <summary>
	/// 创建Panel
	/// </summary>
	/// <param name="parent"></param>
	/// <returns></returns>
	private GameObject CreatePanel(string name, Transform parent)
	{
		GameObject panelGO = new GameObject(name);
		panelGO.transform.SetParent(parent, false);
		RectTransform rectTransform = panelGO.AddComponent<RectTransform>();
		rectTransform.localScale = Vector3.one;
		return panelGO;
	}

	/// <summary>
	/// 初始化菜单栏
	/// </summary>
	/// <param name="menuBar"></param>
	/// <exception cref="NotImplementedException"></exception>
	private void InitMenuBar(GameObject menuBar)
	{
		RectTransform rectTransform = menuBar.GetComponent<RectTransform>();
		rectTransform.anchorMin = new Vector2(0, 1);
		rectTransform.anchorMax = new Vector2(1, 1);
		rectTransform.pivot = new Vector2(0.5f, 1);
		rectTransform.sizeDelta = new Vector2(0, 60);
		rectTransform.anchoredPosition = new Vector2(0, 0);

		//水平布局
		HorizontalLayoutGroup hLayout = menuBar.AddComponent<HorizontalLayoutGroup>();
		hLayout.childAlignment = TextAnchor.MiddleLeft;
		hLayout.spacing = 20;
		hLayout.padding = new RectOffset(20, 0, 0, 0);
		hLayout.childForceExpandWidth = false;
		hLayout.childForceExpandHeight = false;

		//背景颜色
		menuBar.AddComponent<Image>().color = Color.grey;
	}

	/// <summary>
	/// 初始化主内容区域
	/// </summary>
	/// <param name="mainContent"></param>
	private void InitMainContent(GameObject mainContent)
	{
		RectTransform rt = mainContent.GetComponent<RectTransform>();
		rt.anchorMin = new Vector2(0, 0);
		rt.anchorMax = new Vector2(1, 1);
		rt.pivot = new Vector2(0.5f, 0.5f);
		rt.offsetMin = new Vector2(0, 0);
		rt.offsetMax = new Vector2(0, -60); // 顶部预留菜单栏高度

		HorizontalLayoutGroup hLayout = mainContent.AddComponent<HorizontalLayoutGroup>();
		hLayout.childAlignment = TextAnchor.MiddleLeft;
		hLayout.spacing = 4;
		hLayout.padding = new RectOffset(4, 4, 4, 4);
		hLayout.childForceExpandWidth = true;
		hLayout.childForceExpandHeight = true;
		hLayout.childControlHeight = true;

		mainContent.AddComponent<Image>().color = new Color(0.95f, 0.95f, 0.95f);

		//场景树
		GameObject treePanel = CreatePanel("TreePanel", mainContent.transform);
		InitTreePanel(treePanel);

		//预览区
		GameObject previewPanel = CreatePanel("PreviewPanel", mainContent.transform);
		InitPreviewPanel(previewPanel);

		//属性控制
		GameObject inspectorPanel = CreatePanel("InspectorPanel", mainContent.transform);
		InitInspectorPanel(inspectorPanel);
	}

	private void InitTreePanel(GameObject tree)
	{
		RectTransform rt = tree.GetComponent<RectTransform>();
		rt.sizeDelta = new Vector2(200, 0); // 固定宽度

		Image img = tree.AddComponent<Image>();
		img.color = new Color(0.8f, 0.85f, 0.9f);
	}

	private void InitPreviewPanel(GameObject preview)
	{
		RectTransform rt = preview.GetComponent<RectTransform>();
		rt.sizeDelta = new Vector2(0, 0); // 占据剩余空间

		Image img = preview.AddComponent<Image>();
		img.color = new Color(0.95f, 0.95f, 0.95f);
	}

	private void InitInspectorPanel(GameObject inspector)
	{
		RectTransform rt = inspector.GetComponent<RectTransform>();
		rt.sizeDelta = new Vector2(300, 0); // 固定宽度

		Image img = inspector.AddComponent<Image>();
		img.color = new Color(0.9f, 0.9f, 0.95f);
	}

}

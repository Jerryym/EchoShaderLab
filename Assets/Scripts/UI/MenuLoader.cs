using System;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EchoShaderLab.UI
{
	/// <summary>
	/// 菜单组的数据结构
	/// </summary>
	public class MenuItemData
	{
		/// <summary>
		/// 一级菜单名称
		/// </summary>
		public string groupName;
		public string groupLabel;
		/// <summary>
		/// 子菜单项列表
		/// </summary>
		public List<SubItemData> Items = new List<SubItemData>();

		public struct SubItemData
		{
			public string name;
			public string label;
			public string cmdName;

			public SubItemData(string name, string label, string cmdName)
			{
				this.name = name;
				this.label = label;
				this.cmdName = cmdName;
			}
		}

		public MenuItemData() { }

		public MenuItemData(string groupName)
		{
			this.groupName = groupName;
		}
	}


	/// <summary>
	/// 菜单栏加载器
	/// </summary>
	public class MenuLoader : MonoBehaviour
	{
		/// <summary>
		/// 一级菜单和子菜单按钮预制体
		/// </summary>
		public GameObject menuBtnPrefab;

		/// <summary>
		/// 菜单栏
		/// </summary>
		private List<MenuItemData> m_MenuItems = new List<MenuItemData>();
		/// <summary>
		/// 下拉菜单列表
		/// </summary>
		private List<GameObject> m_DropDownList = new List<GameObject>();

		// Start is called before the first frame update
		void Start()
		{
			//初始化命令注册
			MenuCommandRegistry.Init();
			//加载XML
			LoadMenuXML();
			//生成菜单栏
			CreateMenu();
		}

		/// <summary>
		/// 加载菜单栏配置XML文件
		/// </summary>
		private void LoadMenuXML()
		{
			//加载配置文件资源
			TextAsset xmlFile = Resources.Load<TextAsset>("MenuConfig");
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(xmlFile.text);

			//读取XML
			XmlNode root = xmlDocument.SelectSingleNode("Menu");
			foreach (XmlNode node in root.SelectNodes("MenuItem"))
			{
				MenuItemData menuItem = new MenuItemData(node.Attributes["name"].Value);
				menuItem.groupLabel = node.Attributes["label"].Value;
				foreach (XmlNode item in node.SelectNodes("Item"))
				{
					string name = item.Attributes["name"].Value;
					string label = item.Attributes["label"].Value;
					string command = item.Attributes["command"].Value;
					menuItem.Items.Add(new MenuItemData.SubItemData(name, label, command));
				}
				m_MenuItems.Add(menuItem);
			}
		}

		/// <summary>
		/// 创建菜单栏
		/// </summary>
		private void CreateMenu()
		{
			for (int i = 0; i < m_MenuItems.Count; i++)
			{
				var	menuItem = m_MenuItems[i];
				//创建一级按钮
				GameObject topBtn = CreateButton(menuItem.groupName, menuItem.groupLabel, transform, false, null);
				int index = i;
				topBtn.GetComponent<Button>().onClick.AddListener(() => OnTopMenuClick(index));

				//创建子菜单面板
				GameObject dropdownPanel = CreateDropdownPanel(topBtn.transform, menuItem.groupName);
				dropdownPanel.SetActive(false);
				m_DropDownList.Add(dropdownPanel);

				//创建子菜单项按钮
				foreach (var subItem in menuItem.Items)
				{
					CreateButton(subItem.name, subItem.label, dropdownPanel.transform, true, () =>
					{
						var cmd = MenuCommandRegistry.GetCommand(subItem.name);
						if (cmd != null)
						{
							cmd.Execute();
						}
						else
						{
							Debug.Log($"Execute Command: {subItem.cmdName}");
						}
						HideAllDropdowns();
					});
				}
			}

			//重置子菜单高度
			for (int i = 0; i < m_DropDownList.Count; i++)
			{
				GameObject dropdownPanel = m_DropDownList[i];
				RectTransform dropRT = dropdownPanel.GetComponent<RectTransform>();
				VerticalLayoutGroup layout = dropdownPanel.GetComponent<VerticalLayoutGroup>();

				float totalHeight = layout.padding.top + layout.padding.bottom;
				foreach (Transform child in dropdownPanel.transform)
				{
					RectTransform childRT = child.GetComponent<RectTransform>();
					totalHeight += childRT.sizeDelta.y + layout.spacing;
				}
				dropRT.sizeDelta = new Vector2(dropRT.sizeDelta.x, totalHeight);
			}
		}

		/// <summary>
		/// 创建按钮
		/// </summary>
		/// <param name="name"></param>
		/// <param name="label"></param>
		/// <param name="parent"></param>
		/// <param name="onClick"></param>
		/// <returns></returns>
		private GameObject CreateButton(string name, string label, Transform parent, bool bSubItem, Action onClick)
		{
			GameObject btnGO = Instantiate(menuBtnPrefab, parent);
			btnGO.name = name;

			RectTransform rt = btnGO.GetComponent<RectTransform>();
			rt.sizeDelta = (bSubItem == true) ? new Vector2(120, 35) : new Vector2(150, 40);

			LayoutElement layoutElement = btnGO.GetComponent<LayoutElement>();
			if (layoutElement == null)
			{
				layoutElement = btnGO.AddComponent<LayoutElement>();
			}
			layoutElement.preferredHeight = (bSubItem == true) ? 35 : 40;

			TextMeshProUGUI tmp = btnGO.GetComponentInChildren<TextMeshProUGUI>();
			tmp.text = label;
			tmp.fontSize = (bSubItem == true) ? 24 : 32;
			tmp.alignment = TextAlignmentOptions.Center;
			if (bSubItem)
			{
				tmp.color = Color.white;
				Image btnImage = btnGO.GetComponent<Image>();
				btnImage.color = new Color(0.2f, 0.2f, 0.2f, 0.95f);
			}

			if (onClick != null)
			{
				btnGO.GetComponent<Button>().onClick.AddListener(() => onClick());
			}
			return btnGO;
		}

		/// <summary>
		/// 创建子菜单面板
		/// </summary>
		/// <param name="topBtn"></param>
		/// <returns></returns>
		private GameObject CreateDropdownPanel(Transform topBtn, string name)
		{
			GameObject dropdownPanel = new GameObject("DropdownPanel_" + name, typeof(RectTransform), typeof(VerticalLayoutGroup), typeof(Image));
			dropdownPanel.transform.SetParent(transform.parent, false);

			RectTransform dropRT = dropdownPanel.GetComponent<RectTransform>();
			dropRT.pivot = new Vector2(0, 1);
			dropRT.anchorMin = new Vector2(0, 1);
			dropRT.anchorMax = new Vector2(0, 1);
			dropRT.sizeDelta = new Vector2(150, 0);

			Image panelImage = dropdownPanel.GetComponent<Image>();
			panelImage.color = new Color(0.2f, 0.2f, 0.2f, 0.95f);

			VerticalLayoutGroup layout = dropdownPanel.GetComponent<VerticalLayoutGroup>();
			layout.childAlignment = TextAnchor.UpperLeft;
			layout.childForceExpandWidth = true;
			layout.childForceExpandHeight = false;
			layout.spacing = 2;
			layout.padding = new RectOffset(10, 10, 10, 10);

			return dropdownPanel;
		}

		/// <summary>
		/// 单击一级菜单, 显示下拉菜单
		/// </summary>
		/// <param name="index"></param>
		private void OnTopMenuClick(int index)
		{
			//隐藏所有下拉菜单
			HideAllDropdowns();

			if (index < m_DropDownList.Count)
			{
				var dropdownPanel = m_DropDownList[index];
				dropdownPanel.SetActive(true);

				//更新下拉菜单位置
				Transform topBtn = transform.GetChild(index);
				RectTransform btnRT = topBtn.GetComponent<RectTransform>();
				RectTransform dropdownRT = dropdownPanel.GetComponent<RectTransform>();

				float dropdownWidth = dropdownRT.rect.width;
				Vector2 btnPos = btnRT.anchoredPosition;
				dropdownRT.anchoredPosition = new Vector2(btnPos.x - (dropdownWidth / 2), -60);
			}
		}

		/// <summary>
		/// 隐藏所有下拉菜单
		/// </summary>
		private void HideAllDropdowns()
		{
			foreach (var dropdownPanel in m_DropDownList)
			{
				dropdownPanel.SetActive(false);
			}
		}

	}

}

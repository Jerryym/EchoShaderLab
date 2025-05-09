using EchoShaderLab.Command;
using System;
using System.Collections.Generic;
using System.Windows.Input;
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
		public List<sSubItemData> Items = new List<sSubItemData>();

		public struct sSubItemData
		{
			public string name;
			public string label;
			public string cmdName;

			public sSubItemData(string name, string label, string cmdName)
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
		/// 菜单栏容器
		/// </summary>
		public Transform menuBar;
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
					menuItem.Items.Add(new MenuItemData.sSubItemData(name, label, command));
				}
				m_MenuItems.Add(menuItem);
			}
		}

		/// <summary>
		/// 创建菜单栏
		/// </summary>
		/// <exception cref="NotImplementedException"></exception>
		private void CreateMenu()
		{
			
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
			if (!bSubItem)
			{
				RectTransform panelRT = btnGO.GetComponent<RectTransform>();
				panelRT.sizeDelta = new Vector2(120, 35);
			}

			TextMeshProUGUI tmp = btnGO.GetComponentInChildren<TextMeshProUGUI>();
			tmp.text = label;
			tmp.fontSize = bSubItem ? 18 : 24;
			if (onClick != null)
			{
				btnGO.GetComponent<Button>().onClick.AddListener(() => onClick());
			}
			return btnGO;
		}


		private void HideAllDropdowns()
		{
			foreach (var dropdownPanel in m_DropDownList)
				dropdownPanel.SetActive(false);
			m_DropDownList.Clear();
		}

	}

}

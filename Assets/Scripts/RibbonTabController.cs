using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RibbonTabController : MonoBehaviour
{
	/// <summary>
	/// Tab Buttons
	/// </summary>
	public Button[] tabButtons;
	/// <summary>
	/// Tab Contents
	/// </summary>
	public GameObject[] tabContents;

	/// <summary>
	/// 当前选中的Tab索引
	/// </summary>
	private int m_iCurrentTabIndex = -1;

	// Start is called before the first frame update
	void Start()
	{
		// 确保按钮数和内容面板一致
		if (tabButtons.Length != tabContents.Length)
		{
			Debug.Log("TabButtons and TabContents must have the same length!");
			return;
		}

		for (int i = 0; i < tabButtons.Length; i++)
		{
			int iIndex = i;
			tabButtons[i].onClick.AddListener(() => SwitchTab(iIndex));
		}

		//默认选择第一个
		SwitchTab(0);
	}

	/// <summary>
	/// 切换Tab页
	/// </summary>
	/// <param name="index"></param>
	private void SwitchTab(int index)
	{
		if (index == m_iCurrentTabIndex)
			return;

		for (int i = 0;i < tabContents.Length; i++)
		{
			bool bActive = (i == index);

			//高亮TabButton
			var colors = tabButtons[i].colors;
			colors.normalColor = (bActive == true) ? Color.white : new Color(0.7f, 0.7f, 0.7f);
			tabButtons[i].colors = colors;

			//切换tabContent
			tabContents[i].SetActive(bActive);
		}
		m_iCurrentTabIndex = index;
	}
}

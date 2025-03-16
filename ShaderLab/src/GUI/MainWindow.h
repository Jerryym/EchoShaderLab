#pragma once
#include <NestUI.h>

namespace ShaderLab {

	class MainWindow : public NestUI::NestMainWindow
	{
	public:
		 MainWindow(const NestUI::sWindowProp& sProp = NestUI::sWindowProp(), QWidget* parent = nullptr);
		virtual ~MainWindow() {}

	public:
		/// @brief 获取窗口宽度
		virtual uint32_t GetWidth() const { return m_sProp.m_nWidth; }
		/// @brief 获取窗口高度
		virtual uint32_t GetHeight() const { return m_sProp.m_nHeight; }
		/// @brief 获取窗口标题
		virtual const QString& GetTitle() const override { return m_sProp.m_STitle; }

	private:
		/// @brief 初始化
		void Initialize();
		/// @brief 初始化DockWidgets
		void InitializeDockWidgets();
		/// @brief 初始化eRenderPreviewWgt
		void InitializeRenderPreviewWgt();

		SARibbonCategory* CreateCategory(const NestApp::RibbonPage& page);
		void CreateTestCategory(SARibbonBar* pRibbon);

	private:
		NestUI::sWindowProp m_sProp;
		/// @brief 停靠窗口管理器
		NestUI::DockWidgetManager* m_pDockManager;
	};

}

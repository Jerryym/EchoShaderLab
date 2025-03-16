#include "pch.h"
#include "MainWindow.h"
#include "ConsoleDockWgt.h"
#include "RenderPreviewWgt.h"

namespace ShaderLab {

	 ShaderLab::MainWindow::MainWindow(const NestUI::sWindowProp& sProp, QWidget* parent)
		: NestUI::NestMainWindow(parent), m_sProp(sProp)
	{
		 m_pDockManager = NestUI::DockWidgetManager::GetDockManager(this);
		//设置窗口标题
		setWindowTitle(sProp.m_STitle);
		//初始化窗口大小
		resize(QSize(sProp.m_nWidth, sProp.m_nHeight));
		showMaximized();

		Initialize();
	}

	void MainWindow::Initialize()
	{
		//创建DockWidget
		InitializeDockWidgets();
		//创建UnityWidget
		InitializeRenderPreviewWgt();

		//创建测试标签页
		SARibbonBar* pRibbon = ribbonBar();
		pRibbon->setContentsMargins(5, 0, 5, 0);
		CreateTestCategory(pRibbon);
	}

	void MainWindow::InitializeDockWidgets()
	{
		//ConSoleWidget
		ConsoleDockWgt* pConsole = new ConsoleDockWgt(this);
		NEST_CORE_ASSERT(pConsole == nullptr, "Create Console DockWidget Fail!");
		m_pDockManager->AddDockWidget("ConSole", pConsole, Qt::BottomDockWidgetArea);

		NEST_CLIENT_INFO("DockWidgetNum = {0}", m_pDockManager->GetDockWidgetNum());
	}

	void MainWindow::InitializeRenderPreviewWgt()
	{
		// 创建 CentralWidget
		QWidget* pCentralWidget = new QWidget(this);
		setCentralWidget(pCentralWidget);  // 设置 CentralWidget
		
		// 创建布局
		QVBoxLayout* layout = new QVBoxLayout(pCentralWidget);

		// 创建渲染预览组件
		RenderPrivewWgt* pRenderPreview = new RenderPrivewWgt(this);
		NEST_CORE_ASSERT(pRenderPreview == nullptr, "Create RenderPreview Fail!");
		layout->addWidget(pRenderPreview);
	}

	SARibbonCategory* MainWindow::CreateCategory(const NestApp::RibbonPage& page)
	{
		//创建标签页
		SARibbonCategory* pCategory = new SARibbonCategory;
		pCategory->setCategoryName(page.getPageName());
		pCategory->setObjectName(page.getName());

		//创建Pannel
		SARibbonPannel* pPannel = pCategory->addPannel(page.getPageName());

		//添加Action
		QHash<QString, NestApp::Action*> actions = page.getActions();
		for (const auto &key : actions.keys())
		{
			QAction *pAction = new QAction(this);
			pAction->setText(actions[key]->getLabel());
			pAction->setObjectName(actions[key]->getName());
			if (actions[key]->getIconUrl().isEmpty() != true)
			{
				pAction->setIcon(QIcon(actions[key]->getIconUrl()));
			}
			if (actions[key]->getShortCut().isEmpty() != true)
			{
				pAction->setShortcut(QKeySequence(actions[key]->getShortCut()));
			}
			switch (actions[key]->getSize())
			{
			case NestApp::ActionSize::Small:
				pPannel->addSmallAction(pAction);
				break;
			case NestApp::ActionSize::Normal:
				pPannel->addMediumAction(pAction);
				break;
			case NestApp::ActionSize::Large:
				pPannel->addLargeAction(pAction);
				break;
			}
		}
		return pCategory;
	}

	void MainWindow::CreateTestCategory(SARibbonBar* pRibbon)
	{
		NestApp::RibbonPage page("TestPage", "测试");
		page.addAction(new NestApp::Action(NestApp::ActionSize::Large, "TestAction", "测试", "TestPage", "测试"));
		SARibbonCategory* pCategory = CreateCategory(page);
		pRibbon->addCategoryPage(pCategory);
	}

}

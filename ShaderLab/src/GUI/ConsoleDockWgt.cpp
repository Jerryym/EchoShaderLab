#include "pch.h"
#include "ConsoleDockWgt.h"

#include <QGridLayout>

namespace ShaderLab {

	ConsoleDockWgt::ConsoleDockWgt(QWidget* parent)
		: NestUI::DockWidget(parent)
	{
		//设置标题
		setWindowTitle("Console");
		
		//初始化LogWidget
		m_pLogWidget = new QTextEdit(this);
		m_pLogWidget->setReadOnly(true);
		
		//设置栅格布局
		QWidget* dockWidgetContents = new QWidget(this);
		QGridLayout* layout = new QGridLayout(dockWidgetContents);
		layout->addWidget(m_pLogWidget, 0, 0, 1, 1);
		setWidget(dockWidgetContents);
		
		//初始化Log
		NestApp::Log::Init(m_pLogWidget);
		NEST_CORE_WARN("Initialiazed Log!");
		NEST_CLIENT_INFO("Hello, Echo ShaderLab!");
	}

	void ConsoleDockWgt::clearWidget()
	{
		m_pLogWidget->clear();
	}

	void ConsoleDockWgt::refreshWidget()
	{
	}

}

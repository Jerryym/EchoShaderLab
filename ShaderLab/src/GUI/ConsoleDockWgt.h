#pragma once
#include <NestUI.h>
#include <QTextEdit>

namespace ShaderLab {

	class ConsoleDockWgt : public NestUI::DockWidget
	{
		Q_OBJECT
	public:
		ConsoleDockWgt(QWidget* parent = nullptr);
		virtual ~ConsoleDockWgt() {}

	public:
		virtual void clearWidget() override;
		virtual void refreshWidget() override;

	private:
		QTextEdit* m_pLogWidget;
	};

}

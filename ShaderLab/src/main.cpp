#include <NestApp.h>
#include "GUI/MainWindow.h"

int main(int argc, char* argv[])
{
	SARibbonBar::initHighDpi();
	NestApp::Application app(argc, argv);

	//创建主窗口
	ShaderLab::MainWindow* pMainWindow = new ShaderLab::MainWindow(NestUI::sWindowProp(1600, 900, "Echo ShaderLab"));
	app.SetMainWindow(pMainWindow);
	app.Run();

	return app.exec();
}

cmake_minimum_required(VERSION 3.16)

project(ShaderLab VERSION ${SHADERLAB_VERSION} LANGUAGES CXX)
set(CMAKE_INCLUDE_CURRENT_DIR ON)
set(CMAKE_AUTOMOC ON)
set(CMAKE_AUTORCC ON)

# 加载Qt库，设置最低版本为6.5.3
find_package(QT NAMES Qt6 REQUIRED COMPONENTS Core Gui Widgets)
find_package(Qt${QT_VERSION_MAJOR} 6.5.3 COMPONENTS Core Gui Widgets)
message(STATUS "Current Qt Version: Qt${QT_VERSION_MAJOR}.${QT_VERSION_MINOR}.${QT_VERSION_PATCH}")
qt_standard_project_setup()

# 定义头文件列表
set(SHADERLAB_HEADERS
    src/GUI/MainWindow.h
	src/GUI/ConsoleDockWgt.h
	src/GUI/RenderPreviewWgt.h
)

# 定义源文件列表
set(SHADERLAB_SOURCES
	src/GUI/MainWindow.cpp
	src/GUI/ConsoleDockWgt.cpp
	src/GUI/RenderPreviewWgt.cpp
	src/main.cpp
)

add_executable(ShaderLab WIN32
	${SHADERLAB_HEADERS}
	${SHADERLAB_SOURCES}
)

# 预编译头文件
target_precompile_headers(ShaderLab PRIVATE src/pch.h)

# 添加头文件
target_include_directories(ShaderLab PUBLIC
    ${PROJECT_SOURCE_DIR}/src
)

target_link_libraries(ShaderLab PUBLIC NestApp)
target_link_libraries(ShaderLab PUBLIC NestUI)
target_link_libraries(ShaderLab PUBLIC
	Qt${QT_VERSION_MAJOR}::Core 
	Qt${QT_VERSION_MAJOR}::Gui
	Qt${QT_VERSION_MAJOR}::Widgets
)

set_target_properties(ShaderLab PROPERTIES
	AUTOMOC ON
	AUTORCC ON
	AUTOUIC ON
	CXX_EXTENSIONS OFF
	DEBUG_POSTFIX ${CMAKE_DEBUG_POSTFIX}
	VERSION ${SHADERLAB_VERSION}
	EXPORT_NAME ShaderLab
	
	# Debug
	RUNTIME_OUTPUT_DIRECTORY_DEBUG ${CMAKE_BINARY_DIR}/${CMAKE_INSTALL_BINDIR}/../bin/Debug-windows-x86_64/ShaderLab
	ARCHIVE_OUTPUT_DIRECTORY_DEBUG ${CMAKE_BINARY_DIR}/${CMAKE_INSTALL_BINDIR}/../bin/Debug-windows-x86_64/ShaderLab
	LIBRARY_OUTPUT_DIRECTORY_DEBUG ${CMAKE_BINARY_DIR}/${CMAKE_INSTALL_BINDIR}/../bin/Debug-windows-x86_64/ShaderLab

	# Release
	RUNTIME_OUTPUT_DIRECTORY_RELEASE ${CMAKE_BINARY_DIR}/${CMAKE_INSTALL_BINDIR}/../bin/Release-windows-x86_64/ShaderLab
	ARCHIVE_OUTPUT_DIRECTORY_RELEASE ${CMAKE_BINARY_DIR}/${CMAKE_INSTALL_BINDIR}/../bin/Release-windows-x86_64/ShaderLab
	LIBRARY_OUTPUT_DIRECTORY_RELEASE ${CMAKE_BINARY_DIR}/${CMAKE_INSTALL_BINDIR}/../bin/Release-windows-x86_64/ShaderLab
)

# 设置Visual Studio的文件树结构
file(GLOB Base_SRC "src/*.h" "src/*.cpp")
file(GLOB GUI_SRC "src/GUI/*.h" "src/GUI/*.cpp")
source_group("src" FILES ${Base_SRC})
source_group("src\\GUI" FILES ${GUI_SRC})

# 设置生成后事件
add_custom_command(
	TARGET ShaderLab POST_BUILD

	# 复制NestApp的DLL文件到相应的文件夹
	COMMAND ${CMAKE_COMMAND} -E copy
	$<TARGET_FILE:NestApp> # 获取生成的 DLL 文件
	$<IF:$<CONFIG:Debug>,${CMAKE_BINARY_DIR}/bin/Debug-windows-x86_64/ShaderLab/NestAppd.dll,${CMAKE_BINARY_DIR}/bin/Release-windows-x86_64/ShaderLab/NestApp.dll>

	# 复制SARibbon的DLL文件到相应的文件夹
	COMMAND ${CMAKE_COMMAND} -E copy
	$<TARGET_FILE:SARibbonBar> # 获取生成的 DLL 文件
	$<IF:$<CONFIG:Debug>,${CMAKE_BINARY_DIR}/bin/Debug-windows-x86_64/ShaderLab/SARibbonBard.dll,${CMAKE_BINARY_DIR}/bin/Release-windows-x86_64/ShaderLab/SARibbonBar.dll>

	# 复制NestUI的DLL文件到相应的文件夹
	COMMAND ${CMAKE_COMMAND} -E copy
	$<TARGET_FILE:NestUI> # 获取生成的 DLL 文件
	$<IF:$<CONFIG:Debug>,${CMAKE_BINARY_DIR}/bin/Debug-windows-x86_64/ShaderLab/NestUId.dll,${CMAKE_BINARY_DIR}/bin/Release-windows-x86_64/ShaderLab/NestUI.dll>

	VERBATIM
)

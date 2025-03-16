# CMakeList.txt: 顶层 CMake 项目文件，在此处执行全局配置
# 并包含子项目。
#
cmake_minimum_required(VERSION 3.16)

# 设置项目版本号
set(SHADERLAB_VERSION_MAJOR 0)
set(SHADERLAB_VERSION_MINOR 1)
set(SHADERLAB_VERSION_PATCH 3)
set(SHADERLAB_VERSION "${SHADERLAB_VERSION_MAJOR}.${SHADERLAB_VERSION_MINOR}.${SHADERLAB_VERSION_PATCH}")
message(STATUS "Echo ShaderLab Version: ${SHADERLAB_VERSION}")
project(EchoShaderLab VERSION ${SHADERLAB_VERSION} LANGUAGES CXX)

# 加载Qt库，设置最低版本为6.5.3
set(MIN_QT_VERSION 6.5.3)
find_package(QT NAMES Qt6 REQUIRED COMPONENTS Core Widgets)
find_package(Qt${QT_VERSION_MAJOR} REQUIRED COMPONENTS Core Widgets)
message(STATUS "Current Qt Version: Qt${QT_VERSION_MAJOR}.${QT_VERSION_MINOR}.${QT_VERSION_PATCH}")
qt_standard_project_setup()

# 设置C++版本
set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED ON)

# 设置编译后文件名后缀
set(CMAKE_DEBUG_POSTFIX "d" CACHE STRING "add a postfix, usually d on windows")
set(CMAKE_RELEASE_POSTFIX "" CACHE STRING "add a postfix, usually empty on windows")
set(CMAKE_RELWITHDEBINFO_POSTFIX "rd" CACHE STRING "add a postfix, usually empty on windows")
set(CMAKE_MINSIZEREL_POSTFIX "s" CACHE STRING "add a postfix, usually empty on windows")

# 设置编译类型
if(CMAKE_BUILD_TYPE MATCHES "Release")
    set(CMAKE_BUILD_POSTFIX "${CMAKE_RELEASE_POSTFIX}")
elseif (CMAKE_BUILD_TYPE MATCHES "MinSizeRel")
    set(CMAKE_BUILD_POSTFIX "${CMAKE_MINSIZEREL_POSTFIX}")
elseif(CMAKE_BUILD_TYPE MATCHES "RelWithDebInfo")
    set(CMAKE_BUILD_POSTFIX "${CMAKE_RELWITHDEBINFO_POSTFIX}")
elseif(CMAKE_BUILD_TYPE MATCHES "Debug")
    set(CMAKE_BUILD_POSTFIX "${CMAKE_DEBUG_POSTFIX}")
else()
    set(CMAKE_BUILD_POSTFIX "")
endif()

# 禁用警告
if(MSVC)
    set(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} /wd4819")
endif()

# 设置平台
set(NEST_PLATFORM "x64")

##################################
# 添加子项目
##################################
add_subdirectory(Nest)
add_subdirectory(ShaderLab)

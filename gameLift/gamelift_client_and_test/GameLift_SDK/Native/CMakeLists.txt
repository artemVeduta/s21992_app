CMAKE_MINIMUM_REQUIRED(VERSION 3.11)

project(GameLiftRealtimeNative)

include(FetchContent)

set(CMAKE_CXX_STANDARD 14)
set(CMAKE_CXX_STANDARD_REQUIRED ON)

option(BUILD_FRAMEFORK "Build this Native library as a framework for iOS development" OFF)

FetchContent_Declare(
  mbedtls
  GIT_REPOSITORY https://github.com/ARMmbed/mbedtls.git
  GIT_TAG mbedtls-2.16.2
)

FetchContent_GetProperties(mbedtls)
if(NOT mbedtls_POPULATED)
  FetchContent_Populate(mbedtls)
  add_subdirectory(${mbedtls_SOURCE_DIR} ${mbedtls_BINARY_DIR} EXCLUDE_FROM_ALL)
endif()

SET(MBEDTLS_INCLUDE_DIR ${mbedtls_SOURCE_DIR}/include)
SET(MBEDTLS_SOURCE_DIR ${mbedtls_SOURCE_DIR}/library)

include_directories(include ${MBEDTLS_INCLUDE_DIR} ${CMAKE_SOURCE_DIR}/build)

# include all MBedTLS source files so we can build a single shared library
file(GLOB SOURCES
    src/*.cpp
    build/*.cxx
    "${MBEDTLS_SOURCE_DIR}/*.c"
)

add_library(GameLiftRealtimeNative SHARED ${SOURCES})

if(BUILD_FRAMEWORK)
  set_target_properties(GameLiftRealtimeNative PROPERTIES
      FRAMEWORK TRUE
      FRAMEWORK_VERSION A
      MACOSX_FRAMEWORK_IDENTIFIER com.amazon.gamelift.clientFramework
  )
endif(BUILD_FRAMEWORK)
unset(BUILD_FRAMEWORK CACHE)
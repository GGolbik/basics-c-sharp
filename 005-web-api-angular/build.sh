#!/bin/bash

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" >/dev/null 2>&1 && pwd)"

# "Debug" or empty for release build
BUILD_TYPE="$1"
if [ -z "$BUILD_TYPE" ]; then
  BUILD_TYPE="Release"
fi
BUILD_DIR="${SCRIPT_DIR}/build"
SRC_DIR="${SCRIPT_DIR}/src"
TOOLCHAIN_DOTNET="$(which dotnet)"

if [[ ! -d ${BUILD_DIR} ]]; then
  mkdir -p ${BUILD_DIR}
fi

/bin/bash -c "set -o pipefail \
  && cd ${SRC_DIR} \
  && ${TOOLCHAIN_DOTNET} restore \
  && ${TOOLCHAIN_DOTNET} publish -c ${BUILD_TYPE} -o ${BUILD_DIR}/${BUILD_TYPE}"
# --self-contained: This will include the .NET runtime.
# --runtime linux-x64 --self-contained
# --runtime win10-x64 --self-contained
EXIT_CODE=$?
if [[ ${EXIT_CODE} -eq 0 ]]; then
  echo "Build was successful"
else
  echo "Build failed with ERROR: ${EXIT_CODE}"
  exit ${EXIT_CODE}
fi

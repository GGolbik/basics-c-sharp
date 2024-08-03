#!/bin/bash

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" >/dev/null 2>&1 && pwd)"

# "Debug" or empty for release build
BUILD_TYPE="$1"
if [ -z "$BUILD_TYPE" ]; then
  BUILD_TYPE="Release"
fi
BUILD_DIR="${SCRIPT_DIR}/build"
TOOLCHAIN_DOTNET="$(which dotnet)"
TEST_DIR="${SCRIPT_DIR}/test"
TEST_OUT_DIR="${BUILD_DIR}/test"
TOOLCHAIN_DOTNET="$(which dotnet)"
REPORT_GENERATOR=${HOME}/.nuget/packages/reportgenerator/5.0.4/tools/net6.0/ReportGenerator.dll

if [[ ! -d ${BUILD_DIR} ]]; then
  mkdir -p ${BUILD_DIR}
fi

/bin/bash -c "set -o pipefail \
  && cd ${TEST_DIR} \
  && ${TOOLCHAIN_DOTNET} restore \
  && ${TOOLCHAIN_DOTNET} test --logger \"html;logfilename=result.html\" --results-directory ${TEST_OUT_DIR} --collect:\"XPlat Code Coverage\""

EXIT_CODE=$?
if [[ ${EXIT_CODE} -eq 0 ]]; then
  echo "Test was successful"
else
  echo "Test failed with ERROR: ${EXIT_CODE}"
fi

COVERAGE_FILE=$(find "${SCRIPT_DIR}/build/test/" -name "coverage.cobertura.xml" -print -quit)
EXIT_CODE=$?
if [[ ${EXIT_CODE} -eq 0 ]]; then
  mv "${COVERAGE_FILE}" ${TEST_OUT_DIR}/coverage.cobertura.xml
  /bin/bash -c "set -o pipefail \
    && cd ${TEST_DIR} \
    && ${TOOLCHAIN_DOTNET} ${REPORT_GENERATOR} \"-reports:${TEST_OUT_DIR}/coverage.cobertura.xml\" \"-targetdir:${TEST_OUT_DIR}/coverage\" -reporttypes:HTML;"
else
  echo "Coverage report has not been found. ERROR: ${EXIT_CODE}"
fi

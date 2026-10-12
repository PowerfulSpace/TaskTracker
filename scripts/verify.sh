#!/usr/bin/env bash
set -u

cd "$(dirname "$0")/.." || exit 1

dotnet build src/Backend/PS.TaskTracker/PS.TaskTracker.slnx --nologo
if [ $? -ne 0 ]; then
  echo "BUILD FAILED"
  exit 1
fi

dotnet test src/Backend/PS.TaskTracker/PS.TaskTracker.slnx --nologo
if [ $? -ne 0 ]; then
  echo "TESTS FAILED"
  exit 1
fi

echo "VERIFY OK"
exit 0

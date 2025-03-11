#!/bin/bash

if [ $# -ne 3 ]; then
    echo "need to pass a path to a git repo as first argument. Second is YYYY-MM-DD format, Third is output folder"
    exit 1
fi

mkdir -p $3
echo "building git log"
git -C $1 log --all --numstat --date=short --pretty=format:'--%h--%ad--%aN--%s' --no-renames --after=$2 > $3/logfile.log
echo "finished git log"


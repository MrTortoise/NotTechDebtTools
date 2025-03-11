#!/bin/bash

if [ $# -ne 2 ]; then
    echo "need to pass a path to a git repo as first argument. Second is output folder"
    exit 1
fi

mkdir -p $2
echo "building file log"
git -C $1 ls-tree -r HEAD --name-only > $2/filePaths.log
echo "finished file log"




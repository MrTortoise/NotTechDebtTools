#!/bin/bash

if [ $# -ne 3 ]; then
    echo "need to pass a path to a git repo as first argument. Second is YYYY-MM-DD format, third is output folder"
    exit 1
fi

mkdir -p $3
echo "building log"
git -C $1 log --all --numstat --date=short --pretty=format:'--%h--%ad--%aN--%s' --no-renames --after=$2 > $3/logfile.log
echo "building hot spots"
docker run -v $3:/data -it code-maat-app -l /data/logfile.log -c git2 > $3/hotspot.csv
echo "summary"
docker run -v $3:/data -it code-maat-app -l /data/logfile.log -c git2 -a summary > $3/summary.csv
echo "coupling"
docker run -v $3:/data -it code-maat-app -l /data/logfile.log -c git2 -a coupling > $3/coupling.csv

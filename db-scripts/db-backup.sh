#!/bin/bash
host=$1
username=$2
password=$3
output=$4

if [[ -z "$host" || -z "$username" || -z "$password" || -z "$output" ]];
then
    echo "usage: ./db-backup.sh <host> <username> <password> <output-dir>"
    exit 1
fi

if [[ ! "$output" == */ ]]; then 
    output="$output/"
fi

echo "info: makedir if '$output' not exist"
if [[ ! -e $output ]]; then
    mkdir -p $output
fi

echo "info: dump all user databases from host '$host' to individual dump file"
datestr=$(date +"%Y%m%d")
prefix="mysql-$datestr"
db_query="SELECT schema_name from INFORMATION_SCHEMA.SCHEMATA  WHERE schema_name NOT IN('information_schema', 'mysql', 'performance_schema', 'sys');"
sed_pattern="s:\(.*\):mysqldump -h $host -u $username --password=$password \1 > $output$prefix-\1.sql:"
mysql -h $host -u $username --password=$password --execute="$db_query" | awk '{print $1}' | grep -iv ^schema_name$ | sed "$sed_pattern" | sh

echo "info: gzip the dump files to individual zip file"
ls -1 $output*.sql | sed 's:\(.*\)\.sql:tar zcvf \1.tar.gz \1.sql:' | sh

echo "info: delete the dump files"
rm -f $output*.sql

echo "info: delete the backup files older than 10 days"
find $output -type f -name "*.tar.gz" -mtime +10 -exec rm -rf {} \;

echo "info: done!"

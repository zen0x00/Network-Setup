#!/bin/bash

nmcli -t -f IN-USE,SSID,SIGNAL,SECURITY device wifi list | \
awk -F: '
BEGIN {
    print "{ \"networks\": ["
}
$2 != "" {
    printf "  { \"ssid\": \"%s\", \"signal\": %d, \"security\": \"%s\", \"connected\": %s },\n",
           $2, $3, $4, ($1=="*"?"true":"false")
}
END {
    print "  {} ] }"
}
'

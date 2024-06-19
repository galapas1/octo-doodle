#!/bin/bash

if [[ $# -ne 1 ]]; then
    echo "Usage: $0 <filename>"
    exit 1
fi

pdfFileName=$1

URL=https://localhost:44355
RSP=$(curl -si -H "Content-Type: multipart/form-data" --form "file=@${pdfFileName}" ${URL}/api/parsePDF)

echo $RSP

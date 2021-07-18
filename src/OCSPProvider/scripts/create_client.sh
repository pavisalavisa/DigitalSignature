#!/bin/sh
#First generate the key for the client

openssl genrsa \
      -out /root/ca/intermediate/private/$1.key.pem 2048 &>/dev/null
chmod 400 /root/ca/intermediate/private/$1.key.pem

#Then create the certificate signing request
openssl req -config /root/ca/intermediate/openssl.cnf \
      -key /root/ca/intermediate/private/$1.key.pem \
      -new -sha256 -out /root/ca/intermediate/csr/$1.csr.pem \
      -subj "/C=HR/ST=Splitsko-Dalmatinska/L=Split/O=UNIST/OU=UNIST/CN=$1.unist/EMAIL=$1@unist.hr" &>/dev/null
#Now sign it with the intermediate CA
echo -e "y\ny\n" | openssl ca -config /root/ca/intermediate/openssl.cnf \
      -extensions $2 -days 365 -notext -md sha256 \
      -in /root/ca/intermediate/csr/$1.csr.pem \
      -out /root/ca/intermediate/certs/$1.cert.pem &>/dev/null


chmod 444 /root/ca/intermediate/certs/$1.cert.pem
echo "$(cat /root/ca/intermediate/certs/$1.cert.pem)"
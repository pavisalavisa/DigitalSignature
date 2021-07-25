#!/bin/sh
#First generate the key for the client

openssl genrsa -out root/ca/intermediate/private/$1.key.pem 2048
chmod 400 root/ca/intermediate/private/$1.key.pem

#Then create the certificate signing request
openssl req -config root/ca/intermediate/openssl.conf \
      -key root/ca/intermediate/private/$1.key.pem \
      -new -sha256 -out root/ca/intermediate/csr/$1.csr.pem \
      -subj "/C=HR/ST=Splitsko-Dalmatinska/L=Split/O=UNIST/OU=UNIST/CN=$1.unist/emailAddress=$1@unist.hr"
#Now sign it with the intermediate CA
openssl ca -batch -config root/ca/intermediate/openssl.conf \
      -extensions usr_cert -days 365 -notext -md sha256 \
      -in root/ca/intermediate/csr/$1.csr.pem \
      -out root/ca/intermediate/certs/$1.cert.pem

chmod 444 root/ca/intermediate/certs/$1.cert.pem
echo "$(cat root/ca/intermediate/certs/$1.cert.pem)"
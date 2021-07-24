#!/bin/sh

pushd root/ca/intermediate/certs >&/dev/null

cat $1.cert.pem \
ca-chain.cert.pem  > tempCertChain.pem

openssl pkcs12 -export \
-out $1.cert.p12 \
-in tempCertChain.pem \
-inkey ../private/$1.key.pem \
-password pass:Admin.123

rm tempCertChain.pem
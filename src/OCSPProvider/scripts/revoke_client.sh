#!/bin/sh
#Add client to the CRL (certificate revocation list)
openssl ca -config /root/ca/intermediate/openssl.cnf \
      -revoke /root/ca/intermediate/certs/$1.cert.pem
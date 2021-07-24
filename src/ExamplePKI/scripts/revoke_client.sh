#!/bin/sh
#Add client to the CRL (certificate revocation list)
openssl ca -config root/ca/intermediate/openssl.conf \
      -revoke root/ca/intermediate/certs/$1.cert.pem

openssl ca -config root/ca/intermediate/openssl.conf \
      -gencrl -out root/ca/intermediate/crl/intermediate.crl.pem
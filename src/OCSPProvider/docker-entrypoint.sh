#!/bin/sh
#This entrypoint is responsible for leaving the OSCP running to accept requests
openssl ocsp -port 0.0.0.0:2560 -text -sha256 \
      -index /root/ca/intermediate/index.txt \
      -CA /root/ca/intermediate/certs/ca-chain.cert.pem \
      -rkey /root/ca/intermediate/private/ocsp.key.pem \
      -rsigner /root/ca/intermediate/certs/ocsp.cert.pem
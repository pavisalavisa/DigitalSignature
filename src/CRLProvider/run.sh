sh ./build.sh

echo "*** Running CRL provider on localhost:80"
docker run --rm -d \
--name crl \
-p 80:80 \
crl-provider
sh ./build.sh

pushd ../

echo "*** Running CRL provider on localhost:80"
docker run --rm -d \
--name crl \
-p 80:80 \
-v "$(pwd)"/ExamplePKI/root/ca/crl:/srv/crl/ca \
-v "$(pwd)"/ExamplePKI/root/ca/intermediate/crl:/srv/crl/intermediate \
crl-provider
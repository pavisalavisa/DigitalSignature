sh ./build.sh

pushd ../

echo "*** Running OCSP provider on localhost:2560"
docker run --rm -d \
--name ocsp \
-p 2560:2560 \
-v "$(pwd)"/ExamplePKI/root/:/root \
ocsp-provider
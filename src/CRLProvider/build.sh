mkdir .build

echo "*** Taking example PKI CRLs"
cp -a ../ExamplePKI/root/ca/crl/. .build/
cp -a ../ExamplePKI/root/ca/intermediate/crl/. .build/

echo "*** Building docker image"
docker image build -t crl-provider .
echo "*** Docker image successfully built"

echo "*** Cleaning up"
rm -rf .build
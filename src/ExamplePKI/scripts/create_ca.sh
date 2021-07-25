#!/bin/sh
#based on https://jamielinux.com/docs/openssl-certificate-authority/create-the-root-pair.html with modifications regarding Docker | automation

echo "*** Setting up the CA directory structure"
mkdir -p root/ca/certs root/ca/crl root/ca/newcerts root/ca/private
cp config/root-openssl.conf root/ca/openssl.conf
#Read and write to root in private folder
chmod 700 root/ca/private
touch root/ca/index.txt
#Echo the user id
echo 1000 > root/ca/serial
echo 1000 > root/ca/crlnumber
echo "*** Succesfully set up the CA directory structure"

#Generating the root key for the CA | For simplicity without passphrase for usage within docker
openssl genrsa -out root/ca/private/ca.key.pem 4096
#Read-only rights to the running user, root in this cases, as there is no need for any changes to the Dockerfile to declare another user and simplicity
chmod 400 root/ca/private/ca.key.pem
echo "*** Created root CA private key"

#Now let's create the certificate for the authority and pass along the subject as will be ran in non-interactive mode
openssl req -config root/ca/openssl.conf \
      -key root/ca/private/ca.key.pem \
      -new -x509 -days 3650 -sha256 -extensions v3_ca \
      -out root/ca/certs/ca.cert.pem \
      -subj "/C=HR/ST=Splitsko-Dalmatinska/L=Split/O=UNIST/OU=UNIST Certificate Authority /CN=www.unist.hr CA/emailAddress=info@unist.hr"

echo "*** Created Root Certificate"
#Grant everyone reading rights
chmod 444 root/ca/certs/ca.cert.pem

#Create a Certificate revocation list of the root CA
openssl ca -config root/ca/openssl.conf \
      -gencrl -out root/ca/crl/ca.crl.pem
echo "*** Created CRL of the root CA"

#Now that we created the root pair, we should use and intermediate one.
#This part is the same as above except for the folder

echo "*** Setting up the intermediate CA directory structure"
mkdir -p root/ca/intermediate/certs root/ca/intermediate/crl root/ca/intermediate/csr root/ca/intermediate/newcerts root/ca/intermediate/private
cp config/intermediate-openssl.conf root/ca/intermediate/openssl.conf
chmod 700 root/ca/intermediate/private
touch root/ca/intermediate/index.txt
#We must create a serial file to add serial numbers to our certificates - This will be useful when revoking as well
echo 1000 > root/ca/intermediate/serial
echo 1000 > root/ca/intermediate/crlnumber
touch root/ca/intermediate/certs.db
echo "*** Successfully set up the intermediate CA directory structure"

openssl genrsa -out root/ca/intermediate/private/intermediate.key.pem 4096
chmod 400 root/ca/intermediate/private/intermediate.key.pem

echo "*** Created Intermediate Private Key"

#Creating the intermediate certificate signing request using the intermediate ca config
openssl req -config root/ca/intermediate/openssl.conf \
      -key root/ca/intermediate/private/intermediate.key.pem \
      -new -sha256 \
      -out root/ca/intermediate/csr/intermediate.csr.pem \
      -subj "/C=HR/ST=Splitsko-Dalmatinska/L=Split/O=UNIST/OU=UNIST Certificate Authority /CN=www.unist.hr Intermediate CA/emailAddress=info@unist.hr"

echo "*** Created Intermediate CSR"

#Creating an intermediate certificate, by signing the previous csr with the CA key based on root ca config with the directive v3_intermediate_ca extension to sign the intermediate CSR
openssl ca -batch -config root/ca/openssl.conf -extensions v3_intermediate_ca \
      -days 3650 -md sha256 \
      -in root/ca/intermediate/csr/intermediate.csr.pem \
      -out root/ca/intermediate/certs/intermediate.cert.pem

echo "*** Created Intermediate Certificate Signed by root CA"

#Grant everyone reading rights
chmod 444 root/ca/intermediate/certs/intermediate.cert.pem


#Creating certificate chain with intermediate and root
# Root CA can be installed on client so it doesn't have to be in this file
# in that case, only intermediate cert should be in this chain
cat root/ca/intermediate/certs/intermediate.cert.pem \
      root/ca/certs/ca.cert.pem > root/ca/intermediate/certs/ca-chain.cert.pem
chmod 444 root/ca/intermediate/certs/ca-chain.cert.pem
echo "*** Created certificate chain with intermediate and root"

#Create a Certificate revocation list of the intermediate CA
openssl ca -config root/ca/intermediate/openssl.conf \
      -gencrl -out root/ca/intermediate/crl/intermediate.crl.pem
echo "*** Created CRL of the intermediate CA"

#Create OCSP key pair
openssl genrsa \
      -out root/ca/intermediate/private/ocsp.key.pem 4096
echo "*** Created OCSP private key"

#Create the OCSP CSR
openssl req -config root/ca/intermediate/openssl.conf -new -sha256 \
      -key root/ca/intermediate/private/ocsp.key.pem \
      -out root/ca/intermediate/csr/ocsp.csr.pem \
      -nodes \
      -subj "/C=HR/ST=Splitsko-Dalmatinska/L=Split/O=UNIST/OU=UNIST ocsp/CN=ocsp.unist.hr/emailAddress=info@ocsp.unist.hr"
echo "*** Created OCSP CSR"

#Sign it
openssl ca -batch -config root/ca/intermediate/openssl.conf \
      -extensions ocsp -days 375 -notext -md sha256 \
      -in root/ca/intermediate/csr/ocsp.csr.pem \
      -out root/ca/intermediate/certs/ocsp.cert.pem

echo "*** Created OCSP certificate"
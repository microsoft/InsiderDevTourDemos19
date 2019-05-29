#!/bin/bash

CN=""
CN+="$(echo -e $(hostname -I) | tr -d '[:space:]')"

echo "GENERATING CERT FOR $CN"
openssl req \
    -newkey rsa:2048 \
    -x509 \
    -nodes \
    -keyout ../cert.key \
    -new \
    -out ../cert.cert \
    -subj /CN=MicrosoftSelfSignedDemo \
    -reqexts SAN \
    -extensions SAN \
    -config <(cat /etc/ssl/openssl.cnf \
        <(printf "[SAN]\nsubjectAltName = IP:$CN")) \
    -sha256 \
    -days 3650
echo "GENERATING CERT DONE"
cd ..
sudo cp cert.cert /mnt/c/Users/Kevin\ Gallo/Downloads/cert.cer
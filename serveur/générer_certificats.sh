openssl req -newkey rsa:2048 -keyout key.pem -out csr.pem
openssl x509 -signkey key.pem -in csr.pem -req -days 365 -out crt.pem
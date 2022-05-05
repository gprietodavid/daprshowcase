FROM daprio/daprd:1.7.0-linux-amd64 AS base
ADD nscacert.pem /etc/ssl/certs/nscacert.pem
ADD nstenantcert.pem /etc/ssl/certs/nstenantcert.pem

EXPOSE 6050
EXPOSE 50006

FROM base AS final
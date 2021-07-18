FROM alpine:3.7
LABEL maintainer="Antonio Kristicevic"

RUN apk --update add openssl

COPY scripts/* .

COPY config/root-openssl.conf /root/ca/openssl.cnf
COPY config/intermediate-openssl.conf /root/ca/intermediate/openssl.cnf

RUN chmod +x create_ca.sh && \
    chmod +x create_client.sh && \
    chmod +x revoke_client.sh && \
    chmod +x get_chain.sh && \
    chmod +x get_cert.sh
RUN ./create_ca.sh

COPY docker-entrypoint.sh docker-entrypoint.sh
EXPOSE 2560
ENTRYPOINT [ "/bin/sh", "docker-entrypoint.sh" ]

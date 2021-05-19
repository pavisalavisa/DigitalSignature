package com.example.digitalsignatureapi.models.requests;

public class SignatureRequestModel extends BaseFileRequestModel {
    private CertificateModel certificate;

    public CertificateModel getCertificate() {
        return certificate;
    }

    public void setCertificate(CertificateModel certificate) {
        this.certificate = certificate;
    }
}

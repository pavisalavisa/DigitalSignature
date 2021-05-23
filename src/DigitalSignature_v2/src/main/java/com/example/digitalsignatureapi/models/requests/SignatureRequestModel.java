package com.example.digitalsignatureapi.models.requests;

public class SignatureRequestModel extends BaseFileRequestModel {
    private CertificateModel certificate;
    private boolean includeTimestamp;

    public CertificateModel getCertificate() {
        return certificate;
    }

    public void setCertificate(CertificateModel certificate) {
        this.certificate = certificate;
    }

    public boolean isIncludeTimestamp() {
        return includeTimestamp;
    }

    public void setIncludeTimestamp(boolean includeTimestamp) {
        this.includeTimestamp = includeTimestamp;
    }
}

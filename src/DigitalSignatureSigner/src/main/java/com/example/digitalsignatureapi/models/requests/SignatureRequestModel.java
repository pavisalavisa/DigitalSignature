package com.example.digitalsignatureapi.models.requests;

import com.example.digitalsignatureapi.common.SignatureProfile;

public class SignatureRequestModel extends BaseFileRequestModel {
    private CertificateModel certificate;
    private SignatureProfile profile;

    public CertificateModel getCertificate() {
        return certificate;
    }

    public void setCertificate(CertificateModel certificate) {
        this.certificate = certificate;
    }

    public SignatureProfile getProfile() {
        return profile;
    }

    public void setProfile(SignatureProfile profile) {
        this.profile = profile;
    }
}

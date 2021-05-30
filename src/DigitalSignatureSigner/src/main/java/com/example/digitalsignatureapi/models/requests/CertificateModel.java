package com.example.digitalsignatureapi.models.requests;

public class CertificateModel {

    private String b64Certificate;
    private String certificatePassword;

    public String getB64Certificate() {
        return b64Certificate;
    }

    public void setB64Certificate(String b64Certificate) {
        this.b64Certificate = b64Certificate;
    }

    public String getCertificatePassword() {
        return certificatePassword;
    }

    public void setCertificatePassword(String certificatePassword) {
        this.certificatePassword = certificatePassword;
    }
}

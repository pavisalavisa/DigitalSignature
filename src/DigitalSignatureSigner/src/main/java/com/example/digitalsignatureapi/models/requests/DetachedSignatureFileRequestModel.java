package com.example.digitalsignatureapi.models.requests;

public class DetachedSignatureFileRequestModel extends BaseFileRequestModel {
    private String b64XadesSignature;

    public String getB64XadesSignature() {
        return b64XadesSignature;
    }

    public void setB64XadesSignature(String b64XadesSignature) {
        this.b64XadesSignature = b64XadesSignature;
    }
}

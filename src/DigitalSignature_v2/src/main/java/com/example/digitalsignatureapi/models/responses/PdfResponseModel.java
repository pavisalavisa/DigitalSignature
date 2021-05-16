package com.example.digitalsignatureapi.models.responses;

public class PdfResponseModel {
    private String fileName;
    private String signedB64Bytes;

    public String getFileName() {
        return fileName;
    }

    public void setFileName(String fileName) {
        this.fileName = fileName;
    }

    public String getSignedB64Bytes() {
        return signedB64Bytes;
    }

    public void setSignedB64Bytes(String signedB64Bytes) {
        this.signedB64Bytes = signedB64Bytes;
    }
}

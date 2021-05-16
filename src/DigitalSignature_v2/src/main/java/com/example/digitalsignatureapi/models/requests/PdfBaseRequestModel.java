package com.example.digitalsignatureapi.models.requests;

public class PdfBaseRequestModel {
    private String fileName;
    private String b64Bytes;

    public String getFileName() {
        return fileName;
    }

    public void setFileName(String fileName) {
        this.fileName = fileName;
    }

    public String getB64Bytes() {
        return b64Bytes;
    }

    public void setB64Bytes(String b64Bytes) {
        this.b64Bytes = b64Bytes;
    }
}

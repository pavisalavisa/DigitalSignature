package com.example.digitalsignatureapi.common;

import org.springframework.http.HttpHeaders;
import org.springframework.http.HttpStatus;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;

public class PdfResponse extends ResponseEntity<byte[]> {
    private PdfResponse(byte[] pdfBytes, HttpHeaders headers) {
        super(pdfBytes, headers, HttpStatus.OK);
    }

    public static PdfResponse Create(byte[] pdfBytes, String name) {
        return new PdfResponse(pdfBytes, BuildHeaders(name));
    }

    private static HttpHeaders BuildHeaders(String name) {
        HttpHeaders headers = new HttpHeaders();
        headers.setContentType(MediaType.APPLICATION_PDF);

        headers.setContentDispositionFormData(name, name);
        headers.setCacheControl("must-revalidate, post-check=0, pre-check=0");

        return headers;
    }
}

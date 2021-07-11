package com.example.digitalsignatureapi.common;

import org.springframework.http.HttpHeaders;
import org.springframework.http.HttpStatus;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;

public class FileResponse extends ResponseEntity<byte[]> {
    private FileResponse(byte[] bytes, HttpHeaders headers) {
        super(bytes, headers, HttpStatus.OK);
    }

    public static FileResponse Create(byte[] bytes, String name, MediaType mediaType) {
        return new FileResponse(bytes, BuildHeaders(name, mediaType));
    }

    private static HttpHeaders BuildHeaders(String name, MediaType mediaType) {
        HttpHeaders headers = new HttpHeaders();
        headers.setContentType(mediaType);

        headers.setContentDispositionFormData(name, name);
        headers.setCacheControl("must-revalidate, post-check=0, pre-check=0");

        return headers;
    }
}

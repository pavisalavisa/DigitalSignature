package com.example.digitalsignatureapi.common;

import org.springframework.beans.factory.annotation.Value;
import org.springframework.core.io.Resource;
import org.springframework.stereotype.Component;

import java.io.IOException;
import java.nio.charset.StandardCharsets;
import java.nio.file.Files;
import java.nio.file.Paths;

@Component
public class DummyData {

    @Value("classpath:AK_cert.txt")
    private Resource B64Certificate;

    @Value("classpath:ExamplePdf.txt")
    private Resource ExamplePdfB64;

    public final String CertificatePassword = "admin123!";

    public String GetB64Cert() throws IOException {
        return Files.readString(Paths.get(B64Certificate.getURI()),StandardCharsets.UTF_8);
    }

    public String GetExamplePdf() throws IOException {
        return Files.readString(Paths.get(ExamplePdfB64.getURI()),StandardCharsets.UTF_8);
    }
}

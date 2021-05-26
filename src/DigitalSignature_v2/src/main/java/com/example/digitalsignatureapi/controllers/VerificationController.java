package com.example.digitalsignatureapi.controllers;

import com.example.digitalsignatureapi.models.requests.BaseFileRequestModel;
import com.example.digitalsignatureapi.models.requests.DetachedSignatureFileRequestModel;
import com.example.digitalsignatureapi.services.contracts.VerificationService;
import eu.europa.esig.dss.model.DSSDocument;
import eu.europa.esig.dss.model.InMemoryDocument;
import eu.europa.esig.dss.model.MimeType;
import eu.europa.esig.dss.simplereport.SimpleReport;
import org.springframework.web.bind.annotation.*;

import java.util.Base64;

@CrossOrigin(origins = "http://localhost:3000")
@RestController
@RequestMapping(path = "${v1API}/verification")
public class VerificationController {

    private final VerificationService verificationService;

    public VerificationController(VerificationService verificationService) {
        this.verificationService = verificationService;
    }

    @PostMapping("/pdf")
    public SimpleReport VerifySignedPdf(@RequestBody BaseFileRequestModel model) {
        DSSDocument signedDocument = new InMemoryDocument(Base64.getDecoder().decode(model.getB64Bytes()), model.getFileName(), MimeType.PDF);

        return this.verificationService.VerifySignedPdf(signedDocument);
    }

    @PostMapping("/binary")
    public SimpleReport VerifySignedBinary(@RequestBody DetachedSignatureFileRequestModel model) {
        DSSDocument originalDocument = new InMemoryDocument(Base64.getDecoder().decode(model.getB64Bytes()), model.getFileName(), MimeType.BINARY);
        DSSDocument xadesSignature = new InMemoryDocument(Base64.getDecoder().decode(model.getB64XadesSignature()), "signed" + model.getFileName(), MimeType.XML);

        return this.verificationService.VerifySignedBinary(originalDocument, xadesSignature);
    }
}

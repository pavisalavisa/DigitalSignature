package com.example.digitalsignatureapi.controllers;

import com.example.digitalsignatureapi.models.requests.PdfRequestModel;
import com.example.digitalsignatureapi.services.contracts.VerificationService;
import eu.europa.esig.dss.model.DSSDocument;
import eu.europa.esig.dss.model.InMemoryDocument;
import eu.europa.esig.dss.model.MimeType;
import eu.europa.esig.dss.simplereport.SimpleReport;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import java.io.IOException;
import java.util.Base64;

@RestController
@RequestMapping(path = "${v1API}/verification")
public class VerificationController {

        private final VerificationService verificationService;

        public VerificationController(VerificationService verificationService) {
                this.verificationService = verificationService;
        }

        @PostMapping("/pdf")
        public SimpleReport VerifySignedPdf(@RequestBody PdfRequestModel model) throws IOException {
                DSSDocument documentToSign = new InMemoryDocument(Base64.getDecoder().decode(model.getB64Bytes()), model.getFileName(), MimeType.PDF);

                return this.verificationService.VerifySignedPdf(documentToSign);
        }
}

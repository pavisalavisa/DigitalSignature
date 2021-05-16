package com.example.digitalsignatureapi.controllers;

import com.example.digitalsignatureapi.common.PdfResponse;
import com.example.digitalsignatureapi.models.requests.PdfSignatureRequestModel;
import com.example.digitalsignatureapi.models.responses.PdfResponseModel;
import com.example.digitalsignatureapi.services.contracts.SignatureService;
import eu.europa.esig.dss.model.DSSDocument;
import eu.europa.esig.dss.model.InMemoryDocument;
import eu.europa.esig.dss.model.MimeType;
import eu.europa.esig.dss.spi.DSSUtils;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import java.util.Base64;

@RestController
@RequestMapping(path = "${v1API}/signature")
public class SignatureController {

    private final SignatureService signatureService;

    public SignatureController(SignatureService signatureService) {
        this.signatureService = signatureService;
    }

    @PostMapping("/pdf")
    public PdfResponseModel SignPdf(@RequestBody PdfSignatureRequestModel model) {
        DSSDocument documentToSign = new InMemoryDocument(Base64.getDecoder().decode(model.getB64Bytes()), model.getFileName(), MimeType.PDF);

        DSSDocument signedDocument = this.signatureService.SignPdf(model.getCertificate().getB64Certificate(),model.getCertificate().getCertificatePassword(), documentToSign);

        return new PdfResponseModel(){{
            setFileName("signed" + model.getFileName());
            setSignedB64Bytes(Base64.getEncoder().encodeToString(DSSUtils.toByteArray(signedDocument)));
        }};
    }

    @PostMapping("/pdf/download")
    public PdfResponse DownloadSignedPdf(@RequestBody PdfSignatureRequestModel model) {
        DSSDocument documentToSign = new InMemoryDocument(Base64.getDecoder().decode(model.getB64Bytes()), model.getFileName(), MimeType.PDF);

        DSSDocument signedDocument = this.signatureService.SignPdf(model.getCertificate().getB64Certificate(),model.getCertificate().getCertificatePassword(), documentToSign);

        return PdfResponse.Create(DSSUtils.toByteArray(signedDocument), "signed" + model.getFileName());
    }
}

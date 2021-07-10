package com.example.digitalsignatureapi.controllers;

import com.example.digitalsignatureapi.common.FileResponse;
import com.example.digitalsignatureapi.models.requests.SignatureRequestModel;
import com.example.digitalsignatureapi.models.responses.SignedFileResponseModel;
import com.example.digitalsignatureapi.services.contracts.SignatureService;
import eu.europa.esig.dss.model.DSSDocument;
import eu.europa.esig.dss.model.InMemoryDocument;
import eu.europa.esig.dss.model.MimeType;
import eu.europa.esig.dss.spi.DSSUtils;
import org.springframework.http.MediaType;
import org.springframework.web.bind.annotation.*;

import java.util.Base64;

@CrossOrigin(origins = "http://localhost:3000")
@RestController
@RequestMapping(path = "${v1API}/signature")
public class SignatureController {

    private final SignatureService signatureService;

    public SignatureController(SignatureService signatureService) {
        this.signatureService = signatureService;
    }

    @PostMapping("/pdf")
    public SignedFileResponseModel SignPdf(@RequestBody SignatureRequestModel model) {
        DSSDocument documentToSign = new InMemoryDocument(Base64.getDecoder().decode(model.getB64Bytes()), model.getFileName(), MimeType.PDF);

        DSSDocument signedDocument = this.signatureService.SignPdf(model.getCertificate(), documentToSign, model.getProfile().ToPadesSignatureLevel());

        return new SignedFileResponseModel() {{
            setFileName(signedDocument.getName());
            setSignedB64Bytes(Base64.getEncoder().encodeToString(DSSUtils.toByteArray(signedDocument)));
        }};
    }

    @PostMapping("/pdf/download")
    public FileResponse DownloadSignedPdf(@RequestBody SignatureRequestModel model) {
        DSSDocument documentToSign = new InMemoryDocument(Base64.getDecoder().decode(model.getB64Bytes()), model.getFileName(), MimeType.PDF);

        DSSDocument signedDocument = this.signatureService.SignPdf(model.getCertificate(), documentToSign, model.getProfile().ToPadesSignatureLevel());

        return FileResponse.Create(DSSUtils.toByteArray(signedDocument), "signed" + model.getFileName(), MediaType.APPLICATION_PDF);
    }

    @PostMapping("/binary")
    public SignedFileResponseModel SignBinaryData(@RequestBody SignatureRequestModel model) {
        DSSDocument documentToSign = new InMemoryDocument(Base64.getDecoder().decode(model.getB64Bytes()), model.getFileName(), MimeType.BINARY);

        DSSDocument signedDocument = this.signatureService.SignBinary(model.getCertificate(), documentToSign, model.getProfile().ToXadesSignatureLevel());

        return new SignedFileResponseModel() {{
            setFileName(signedDocument.getName());
            setSignedB64Bytes(Base64.getEncoder().encodeToString(DSSUtils.toByteArray(signedDocument)));
        }};
    }

    @PostMapping("/binary/download")
    public FileResponse DownloadSignedSignBinaryData(@RequestBody SignatureRequestModel model) {
        DSSDocument documentToSign = new InMemoryDocument(Base64.getDecoder().decode(model.getB64Bytes()), model.getFileName(), MimeType.BINARY);

        DSSDocument signedDocument = this.signatureService.SignBinary(model.getCertificate(), documentToSign, model.getProfile().ToXadesSignatureLevel());

        return FileResponse.Create(DSSUtils.toByteArray(signedDocument), model.getFileName() + "_signature.xml", MediaType.APPLICATION_XML);
    }
}

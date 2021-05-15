package com.example.digitalsignatureapi.controllers;

import com.example.digitalsignatureapi.common.DummyData;
import com.example.digitalsignatureapi.common.PdfResponse;
import com.example.digitalsignatureapi.models.requests.SignPdfRequestModel;
import com.example.digitalsignatureapi.models.responses.SignPdfResponseModel;
import com.example.digitalsignatureapi.services.SignatureServiceImpl;
import eu.europa.esig.dss.model.DSSDocument;
import eu.europa.esig.dss.model.InMemoryDocument;
import eu.europa.esig.dss.model.MimeType;
import eu.europa.esig.dss.spi.DSSUtils;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import java.io.IOException;
import java.util.Base64;

@RestController
@RequestMapping(path = "${v1API}/signature")
public class SignatureController {

    private final DummyData dummyData;
    private final SignatureServiceImpl signatureService;

    public SignatureController(DummyData dummyData, SignatureServiceImpl signatureService) {
        this.dummyData = dummyData;
        this.signatureService = signatureService;
    }

    @PostMapping("/pdf")
    public SignPdfResponseModel SignPdf(@RequestBody SignPdfRequestModel model) throws IOException {
        DSSDocument documentToSign = new InMemoryDocument(Base64.getDecoder().decode(model.getB64Bytes()), model.getFileName(), MimeType.PDF);

        DSSDocument signedDocument = this.signatureService.SignPdf(dummyData.GetB64Cert(), dummyData.CertificatePassword, documentToSign);

        return new SignPdfResponseModel(){{
            setFileName("signed" + model.getFileName());
            setSignedB64Bytes(Base64.getEncoder().encodeToString(DSSUtils.toByteArray(signedDocument)));
        }};
    }

    @PostMapping("/pdf/download")
    public PdfResponse DownloadSignedPdf(@RequestBody SignPdfRequestModel model) throws IOException {
        DSSDocument documentToSign = new InMemoryDocument(Base64.getDecoder().decode(model.getB64Bytes()), model.getFileName(), MimeType.PDF);

        DSSDocument signedDocument = this.signatureService.SignPdf(dummyData.GetB64Cert(), dummyData.CertificatePassword, documentToSign);

        return PdfResponse.Create(DSSUtils.toByteArray(signedDocument), "signed" + model.getFileName());
    }
}

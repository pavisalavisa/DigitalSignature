package com.example.digitalsignatureapi.services.contracts;

import com.example.digitalsignatureapi.models.requests.CertificateModel;
import eu.europa.esig.dss.enumerations.SignatureLevel;
import eu.europa.esig.dss.model.DSSDocument;

public interface SignatureService {
    DSSDocument SignPdf(CertificateModel certificate, DSSDocument document, SignatureLevel signatureLevel);

    DSSDocument SignBinary(CertificateModel certificate, DSSDocument document, SignatureLevel signatureLevel);
}

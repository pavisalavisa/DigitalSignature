package com.example.digitalsignatureapi.services.contracts;

import eu.europa.esig.dss.model.DSSDocument;

public interface SignatureService {
    DSSDocument SignPdf(String b64Certificate, String password, DSSDocument document);

    DSSDocument SignBinary(String b64Certificate, String password, DSSDocument document);
}

package com.example.digitalsignatureapi.services.contracts;

import eu.europa.esig.dss.model.DSSDocument;
import eu.europa.esig.dss.simplereport.SimpleReport;

public interface VerificationService {

    SimpleReport VerifySignedPdf(DSSDocument document);

    SimpleReport VerifySignedBinary(DSSDocument originalDocument, DSSDocument xadesSignature);
}

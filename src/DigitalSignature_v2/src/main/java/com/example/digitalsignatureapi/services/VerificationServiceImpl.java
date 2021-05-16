package com.example.digitalsignatureapi.services;

import com.example.digitalsignatureapi.services.contracts.VerificationService;
import eu.europa.esig.dss.enumerations.TokenExtractionStrategy;
import eu.europa.esig.dss.model.DSSDocument;
import eu.europa.esig.dss.policy.ValidationPolicy;
import eu.europa.esig.dss.simplereport.SimpleReport;
import eu.europa.esig.dss.validation.CertificateVerifier;
import eu.europa.esig.dss.validation.SignedDocumentValidator;
import eu.europa.esig.dss.validation.executor.ValidationLevel;
import eu.europa.esig.dss.validation.reports.Reports;
import org.springframework.stereotype.Service;

@Service
public class VerificationServiceImpl implements VerificationService {
    private final CertificateVerifier certificateVerifier;
    private final ValidationPolicy validationPolicy;

    public VerificationServiceImpl(CertificateVerifier certificateVerifier, ValidationPolicy validationPolicy) {
        this.certificateVerifier = certificateVerifier;
        this.validationPolicy = validationPolicy;
    }

    @Override
    public SimpleReport VerifySignedPdf(DSSDocument document) {
        SignedDocumentValidator validator = SignedDocumentValidator.fromDocument(document);

        validator.setValidationLevel(ValidationLevel.BASIC_SIGNATURES);
        validator.setCertificateVerifier(this.certificateVerifier);
        validator.setTokenExtractionStrategy(TokenExtractionStrategy.EXTRACT_CERTIFICATES_ONLY);

        Reports reports = validator.validateDocument(this.validationPolicy);

        return reports.getSimpleReport();
    }
}

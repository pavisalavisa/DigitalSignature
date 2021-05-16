package com.example.digitalsignatureapi.services;

import com.example.digitalsignatureapi.services.contracts.VerificationService;
import eu.europa.esig.dss.enumerations.TokenExtractionStrategy;
import eu.europa.esig.dss.model.DSSDocument;
import eu.europa.esig.dss.policy.EtsiValidationPolicy;
import eu.europa.esig.dss.policy.ValidationPolicy;
import eu.europa.esig.dss.policy.jaxb.*;
import eu.europa.esig.dss.simplereport.SimpleReport;
import eu.europa.esig.dss.validation.CertificateVerifier;
import eu.europa.esig.dss.validation.SignaturePolicyProvider;
import eu.europa.esig.dss.validation.SignedDocumentValidator;
import eu.europa.esig.dss.validation.executor.ValidationLevel;
import eu.europa.esig.dss.validation.reports.Reports;
import org.checkerframework.checker.units.qual.C;
import org.springframework.stereotype.Service;

@Service
public class VerificationServiceImpl implements VerificationService {
    private final CertificateVerifier certificateVerifier;

    public VerificationServiceImpl(CertificateVerifier certificateVerifier) {
        this.certificateVerifier = certificateVerifier;
    }

    @Override
    public SimpleReport VerifySignedPdf(DSSDocument document) {
        SignedDocumentValidator validator = SignedDocumentValidator.fromDocument(document);

        validator.setValidationLevel(ValidationLevel.BASIC_SIGNATURES);
        validator.setCertificateVerifier(this.certificateVerifier);
        validator.setTokenExtractionStrategy(TokenExtractionStrategy.EXTRACT_CERTIFICATES_ONLY);

        RevocationConstraints revocationConstraints= new RevocationConstraints();
        BasicSignatureConstraints bsc = new BasicSignatureConstraints();

        revocationConstraints.setBasicSignatureConstraints(bsc);
        revocationConstraints.setLevel(Level.INFORM);

        ConstraintsParameters params = new ConstraintsParameters();
        params.setRevocation(revocationConstraints);

        ValidationPolicy policy = new EtsiValidationPolicy(params);

        Reports reports = validator.validateDocument(policy);

        return reports.getSimpleReport();
    }
}

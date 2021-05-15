package com.example.digitalsignatureapi.services;

import com.example.digitalsignatureapi.services.contracts.VerificationService;
import eu.europa.esig.dss.validation.CertificateVerifier;
import org.springframework.stereotype.Service;

@Service
public class VerificationServiceImpl implements VerificationService {
    private final CertificateVerifier certificateVerifier;

    public VerificationServiceImpl(CertificateVerifier certificateVerifier) {
        this.certificateVerifier = certificateVerifier;
    }
}

package com.example.digitalsignatureapi.services;

import com.example.digitalsignatureapi.services.contracts.SignatureService;
import eu.europa.esig.dss.enumerations.DigestAlgorithm;
import eu.europa.esig.dss.enumerations.SignatureLevel;
import eu.europa.esig.dss.enumerations.SignaturePackaging;
import eu.europa.esig.dss.model.DSSDocument;
import eu.europa.esig.dss.model.SignatureValue;
import eu.europa.esig.dss.model.ToBeSigned;
import eu.europa.esig.dss.model.x509.CertificateToken;
import eu.europa.esig.dss.pades.PAdESSignatureParameters;
import eu.europa.esig.dss.pades.signature.PAdESService;
import eu.europa.esig.dss.token.DSSPrivateKeyEntry;
import eu.europa.esig.dss.token.Pkcs12SignatureToken;
import eu.europa.esig.dss.xades.XAdESSignatureParameters;
import eu.europa.esig.dss.xades.signature.XAdESService;
import org.springframework.stereotype.Service;

import java.security.KeyStore;
import java.security.cert.X509Certificate;
import java.util.Arrays;
import java.util.Base64;
import java.util.List;

@Service
public class SignatureServiceImpl implements SignatureService {

    private final SignatureLevel DefaultPdfSignatureLevel = SignatureLevel.PAdES_BASELINE_B;
    private final SignatureLevel DefaultXadesSignatureLevel = SignatureLevel.XAdES_BASELINE_B;
    private final DigestAlgorithm DefaultDigestAlgorithm = DigestAlgorithm.SHA256;

    private final PAdESService padesService;
    private final XAdESService xadesService;

    public SignatureServiceImpl(PAdESService padesService, XAdESService xadesService) {
        this.padesService = padesService;
        this.xadesService = xadesService;
    }

    public DSSDocument SignPdf(String b64Certificate, String password, DSSDocument document) {
        Pkcs12SignatureToken token = BuildSignatureToken(b64Certificate, password);
        DSSPrivateKeyEntry privateKey = GetPrivateKey(token);

        PAdESSignatureParameters parameters = GetPadesParameters(privateKey.getCertificate().getCertificate(), privateKey.getCertificateChain());

        // Get the SignedInfo segment that need to be signed.
        ToBeSigned dataToSign = padesService.getDataToSign(document, parameters);

        // This function obtains the signature value for signed information using the
        // private key and specified algorithm
        DigestAlgorithm digestAlgorithm = parameters.getDigestAlgorithm();
        SignatureValue signatureValue = token.sign(dataToSign, digestAlgorithm, privateKey);

        // We invoke the padesService to sign the document with the signature value obtained in the previous step.
        return padesService.signDocument(document, parameters, signatureValue);
    }

    public DSSDocument SignBinary(String b64Certificate, String password, DSSDocument document) {
        Pkcs12SignatureToken token = BuildSignatureToken(b64Certificate, password);
        DSSPrivateKeyEntry privateKey = GetPrivateKey(token);

        XAdESSignatureParameters parameters = GetXadesParameters(privateKey.getCertificate().getCertificate(), privateKey.getCertificateChain());

        // Get the SignedInfo XML segment that need to be signed.
        ToBeSigned dataToSign = xadesService.getDataToSign(document, parameters);
        // This function obtains the signature value for signed information using the
        // private key and specified algorithm
        SignatureValue signatureValue = token.sign(dataToSign, parameters.getDigestAlgorithm(), privateKey);
        // We invoke the service to sign the document with the signature value obtained in
        // the previous step.
        DSSDocument signedDocument = xadesService.signDocument(document, parameters, signatureValue);

        return signedDocument;
    }

    private DSSPrivateKeyEntry GetPrivateKey(Pkcs12SignatureToken token) {
        List<DSSPrivateKeyEntry> privateKeys = token.getKeys();

        return privateKeys.get(0);
    }

    private Pkcs12SignatureToken BuildSignatureToken(String b64Certificate, String password) {
        byte[] certificateBytes = Base64.getDecoder().decode(b64Certificate);

        return new Pkcs12SignatureToken(certificateBytes, new KeyStore.PasswordProtection(password.toCharArray()));
    }

    private PAdESSignatureParameters GetPadesParameters(X509Certificate certificate, CertificateToken[] certificateChain) {
        PAdESSignatureParameters parameters = new PAdESSignatureParameters();

        // We choose the level of the signature (-B, -T, -LT, -LTA).
        parameters.setSignatureLevel(DefaultPdfSignatureLevel);
        // We set the digest algorithm to use with the signature algorithm. You must use the
        // same parameter when you invoke the method sign on the token. The default value is
        // SHA256
        parameters.setDigestAlgorithm(DefaultDigestAlgorithm);

        // Preparing parameters for the PAdES signature

        // We set the signing certificate
        CertificateToken token = new CertificateToken(certificate);
        parameters.setSigningCertificate(token);

        // We set the certificate chain
        parameters.setCertificateChain(Arrays.asList(certificateChain));

        return parameters;
    }

    private XAdESSignatureParameters GetXadesParameters(X509Certificate certificate, CertificateToken[] certificateChain) {
        XAdESSignatureParameters parameters = new XAdESSignatureParameters();
        parameters.setSignatureLevel(DefaultXadesSignatureLevel);
        // We choose the type of the signature packaging (ENVELOPED, ENVELOPING, DETACHED).
        parameters.setSignaturePackaging(SignaturePackaging.DETACHED);
        // We set the digest algorithm to use with the signature algorithm. You must use the
        // same parameter when you invoke the method sign on the token. The default value is
        //SHA256
        parameters.setDigestAlgorithm(DefaultDigestAlgorithm);

        CertificateToken token = new CertificateToken(certificate);
        parameters.setSigningCertificate(token);
        parameters.setCertificateChain(Arrays.asList(certificateChain));

        return parameters;
    }
}

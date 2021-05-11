package com.example.digitalsignatureapi.services;

import eu.europa.esig.dss.enumerations.DigestAlgorithm;
import eu.europa.esig.dss.enumerations.SignatureLevel;
import eu.europa.esig.dss.model.DSSDocument;
import eu.europa.esig.dss.model.SignatureValue;
import eu.europa.esig.dss.model.ToBeSigned;
import eu.europa.esig.dss.model.x509.CertificateToken;
import eu.europa.esig.dss.pades.PAdESSignatureParameters;
import eu.europa.esig.dss.pades.signature.PAdESService;
import eu.europa.esig.dss.token.DSSPrivateKeyEntry;
import eu.europa.esig.dss.token.Pkcs12SignatureToken;
import eu.europa.esig.dss.validation.CommonCertificateVerifier;
import org.springframework.stereotype.Service;

import java.security.KeyStore;
import java.security.cert.X509Certificate;
import java.util.Arrays;
import java.util.Base64;
import java.util.List;

@Service
public class SignatureServiceImpl {

    private final SignatureLevel DefaultSignatureLevel = SignatureLevel.PAdES_BASELINE_B;
    private final DigestAlgorithm DefaultDigestAlgorithm = DigestAlgorithm.SHA256;
    private final String PrivateKeyAlias = "TempKey";

    public DSSDocument SignPdf(String b64Certificate, String password, DSSDocument document) {
        Pkcs12SignatureToken token = BuildSignatureToken(b64Certificate, password);
        List<DSSPrivateKeyEntry> privateKeys = token.getKeys();

        DSSPrivateKeyEntry privateKey = privateKeys.get(0);

        PAdESSignatureParameters parameters = GetParameters(privateKey.getCertificate().getCertificate(), privateKey.getCertificateChain());

        CommonCertificateVerifier commonCertificateVerifier = new CommonCertificateVerifier();

        // Create PAdESService for signature
        PAdESService service = new PAdESService(commonCertificateVerifier);

        // Get the SignedInfo segment that need to be signed.
        ToBeSigned dataToSign = service.getDataToSign(document, parameters);

        // This function obtains the signature value for signed information using the
        // private key and specified algorithm
        DigestAlgorithm digestAlgorithm = parameters.getDigestAlgorithm();
        SignatureValue signatureValue = token.sign(dataToSign, digestAlgorithm, privateKey);

        // We invoke the padesService to sign the document with the signature value obtained in the previous step.
        return service.signDocument(document, parameters, signatureValue);
    }

    private Pkcs12SignatureToken BuildSignatureToken(String b64Certificate, String password) {
        byte[] certificateBytes = Base64.getDecoder().decode(b64Certificate);

        return new Pkcs12SignatureToken(certificateBytes, new KeyStore.PasswordProtection(password.toCharArray()));
    }

    private PAdESSignatureParameters GetParameters(X509Certificate certificate, CertificateToken[] certificateChain){
        PAdESSignatureParameters parameters = new PAdESSignatureParameters();

        // We choose the level of the signature (-B, -T, -LT, -LTA).
        parameters.setSignatureLevel(DefaultSignatureLevel);
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
}

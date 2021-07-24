package com.example.digitalsignatureapi.config;

import eu.europa.esig.dss.alert.LogOnStatusAlert;
import eu.europa.esig.dss.enumerations.DigestAlgorithm;
import eu.europa.esig.dss.model.DSSException;
import eu.europa.esig.dss.model.x509.CertificateToken;
import eu.europa.esig.dss.model.x509.revocation.crl.CRL;
import eu.europa.esig.dss.model.x509.revocation.ocsp.OCSP;
import eu.europa.esig.dss.pades.signature.PAdESService;
import eu.europa.esig.dss.policy.ValidationPolicy;
import eu.europa.esig.dss.policy.ValidationPolicyFacade;
import eu.europa.esig.dss.service.SecureRandomNonceSource;
import eu.europa.esig.dss.service.crl.OnlineCRLSource;
import eu.europa.esig.dss.service.http.commons.CommonsDataLoader;
import eu.europa.esig.dss.service.http.commons.OCSPDataLoader;
import eu.europa.esig.dss.service.http.commons.TimestampDataLoader;
import eu.europa.esig.dss.service.ocsp.OnlineOCSPSource;
import eu.europa.esig.dss.service.tsp.OnlineTSPSource;
import eu.europa.esig.dss.spi.client.http.Protocol;
import eu.europa.esig.dss.spi.x509.CommonTrustedCertificateSource;
import eu.europa.esig.dss.spi.x509.KeyStoreCertificateSource;
import eu.europa.esig.dss.spi.x509.revocation.RevocationSource;
import eu.europa.esig.dss.spi.x509.tsp.TSPSource;
import eu.europa.esig.dss.validation.CertificateVerifier;
import eu.europa.esig.dss.validation.CommonCertificateVerifier;
import eu.europa.esig.dss.xades.signature.XAdESService;
import org.slf4j.event.Level;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.core.io.Resource;
import org.xml.sax.SAXException;

import javax.xml.bind.JAXBException;
import javax.xml.stream.XMLStreamException;
import java.io.IOException;
import java.security.cert.CertificateException;
import java.security.cert.CertificateFactory;
import java.security.cert.X509Certificate;

@Configuration
public class BeanConfig {

    @Value("${customRootCA.keystore.type}")
    private String customRootCAKsType;

    @Value("${customRootCA.keystore.password}")
    private String customRootCAKsPassword;

    @Value("classpath:rootCA.p12")
    private Resource rootCA;

    @Value("classpath:rootCA2.pem")
    private Resource rootCA2;

    @Value("classpath:intermediateCA.pem")
    private Resource intermediateCA;

    @Value("classpath:intermediateOcsp.pem")
    private Resource intermediateOcsp;

    @Value("classpath:timestampRootCa.pem")
    private Resource timestampRootCa;

    @Value("classpath:customValidationPolicy.xml")
    private Resource CustomValidationPolicy;

    @Value("${tspServerUri}")
    private String tspServer;

    @Bean
    public CertificateVerifier certificateVerifier() {
        CommonCertificateVerifier certificateVerifier = new CommonCertificateVerifier();

        certificateVerifier.setDataLoader(new CommonsDataLoader());
        certificateVerifier.setTrustedCertSources(rootCaCertificateSource());
        certificateVerifier.setAdjunctCertSources(adjunctCertificatesSource());

        certificateVerifier.setOcspSource(OCSPRevocationSource());
        certificateVerifier.setCrlSource(CRLRevocationSource());

        certificateVerifier.setCheckRevocationForUntrustedChains(true);
        certificateVerifier.setAlertOnMissingRevocationData(new LogOnStatusAlert(Level.WARN));
        certificateVerifier.setAlertOnInvalidTimestamp(new LogOnStatusAlert(Level.WARN));
        certificateVerifier.setAlertOnRevokedCertificate(new LogOnStatusAlert(Level.WARN));
        certificateVerifier.setAlertOnNoRevocationAfterBestSignatureTime(new LogOnStatusAlert(Level.WARN));
        certificateVerifier.setAlertOnUncoveredPOE(new LogOnStatusAlert(Level.WARN));

        return certificateVerifier;
    }

    @Bean
    public TSPSource tspSource() {
        OnlineTSPSource tspSource = new OnlineTSPSource(tspServer);
        tspSource.setDataLoader(new TimestampDataLoader());

        return tspSource;
    }

    @Bean
    public PAdESService padesService() {
        PAdESService service = new PAdESService(certificateVerifier());
        service.setTspSource(tspSource());

        return service;
    }

    @Bean
    public XAdESService xadesService() {
        XAdESService service = new XAdESService(certificateVerifier());
        service.setTspSource(tspSource());

        return service;
    }

    @Bean
    public CommonTrustedCertificateSource rootCaCertificateSource() {
        try {
            KeyStoreCertificateSource keystore = new KeyStoreCertificateSource(rootCA.getFile(), customRootCAKsType, customRootCAKsPassword);

            CommonTrustedCertificateSource trustedCertificateSource = new CommonTrustedCertificateSource();
            trustedCertificateSource.importAsTrusted(keystore);

            CertificateFactory certFactory = CertificateFactory.getInstance("X.509");
            X509Certificate myPkiRootCert = (X509Certificate) certFactory.generateCertificate(rootCA2.getInputStream());
            X509Certificate timestampPkiRootCert = (X509Certificate) certFactory.generateCertificate(timestampRootCa.getInputStream());

            trustedCertificateSource.addCertificate(new CertificateToken(myPkiRootCert));
            trustedCertificateSource.addCertificate(new CertificateToken(timestampPkiRootCert));

            return trustedCertificateSource;
        } catch (IOException | CertificateException e) {
            throw new DSSException("Unable to load the root CA file ", e);
        }
    }

    @Bean
    public CommonTrustedCertificateSource adjunctCertificatesSource() {
        try {
            CommonTrustedCertificateSource trustedCertificateSource = new CommonTrustedCertificateSource();

            CertificateFactory certFactory = CertificateFactory.getInstance("X.509");
            X509Certificate intermediateCaCert = (X509Certificate) certFactory.generateCertificate(intermediateCA.getInputStream());
            X509Certificate intermediateOcspCert = (X509Certificate) certFactory.generateCertificate(intermediateOcsp.getInputStream());

            trustedCertificateSource.addCertificate(new CertificateToken(intermediateCaCert));
            trustedCertificateSource.addCertificate(new CertificateToken(intermediateOcspCert));

            return trustedCertificateSource;
        } catch (IOException | CertificateException e) {
            throw new DSSException("Unable to load the intermediate CA files ", e);
        }
    }

    @Bean
    public ValidationPolicy validationPolicy() throws IOException, XMLStreamException, JAXBException, SAXException {
        return ValidationPolicyFacade.newFacade().getValidationPolicy(CustomValidationPolicy.getInputStream());
    }

    @Bean
    public RevocationSource<CRL> CRLRevocationSource(){
        OnlineCRLSource onlineCRLSource = new OnlineCRLSource();

        onlineCRLSource.setDataLoader(new CommonsDataLoader());

        onlineCRLSource.setPreferredProtocol(Protocol.HTTP);

        return onlineCRLSource;
    }

    @Bean
    public RevocationSource<OCSP> OCSPRevocationSource(){
        // Instantiates a new OnlineOCSPSource object
        OnlineOCSPSource onlineOCSPSource = new OnlineOCSPSource();
        // Allows setting an implementation of `DataLoader` interface,
        // processing a querying of a remote revocation server.
        // `CommonsDataLoader` instance is used by default.
        onlineOCSPSource.setDataLoader(new OCSPDataLoader());
        // Defines an arbitrary integer used in OCSP source querying in order to prevent a
        // replay attack.
        // Default : null (not used by default).
        onlineOCSPSource.setNonceSource(new SecureRandomNonceSource());
        // Defines a DigestAlgorithm being used to generate a CertificateID in order to
        //complete an OCSP request.
        // OCSP servers supporting multiple hash functions may produce a revocation response
        // with a digest algorithm depending on the provided CertificateID's algorithm.
        // Default : SHA1 (as a mandatory requirement to be implemented by OCSP servers. See
        //RFC 5019).
        onlineOCSPSource.setCertIDDigestAlgorithm(DigestAlgorithm.SHA256);

        return onlineOCSPSource;
    }
}

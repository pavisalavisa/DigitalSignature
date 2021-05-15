package com.example.digitalsignatureapi.config;

import eu.europa.esig.dss.alert.LogOnStatusAlert;
import eu.europa.esig.dss.model.DSSException;
import eu.europa.esig.dss.pades.signature.PAdESService;
import eu.europa.esig.dss.spi.x509.CommonTrustedCertificateSource;
import eu.europa.esig.dss.spi.x509.KeyStoreCertificateSource;
import eu.europa.esig.dss.validation.CertificateVerifier;
import eu.europa.esig.dss.validation.CommonCertificateVerifier;
import org.slf4j.event.Level;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.core.io.Resource;

import java.io.IOException;

@Configuration
public class BeanConfig {

    @Value("${customRootCA.keystore.type}")
    private String customRootCAKsType;

    @Value("${customRootCA.keystore.password}")
    private String customRootCAKsPassword;

    @Value("classpath:rootCA.p12")
    private Resource rootCA;

    @Bean
    public CertificateVerifier certificateVerifier() {
        CommonCertificateVerifier certificateVerifier = new CommonCertificateVerifier();

        certificateVerifier.setTrustedCertSources(customRootCAKeyStore());

        certificateVerifier.setCheckRevocationForUntrustedChains(false);
        certificateVerifier.setAlertOnMissingRevocationData(new LogOnStatusAlert(Level.WARN));
        certificateVerifier.setAlertOnInvalidTimestamp(new LogOnStatusAlert(Level.WARN));
        certificateVerifier.setAlertOnRevokedCertificate(new LogOnStatusAlert(Level.WARN));
        certificateVerifier.setAlertOnNoRevocationAfterBestSignatureTime(new LogOnStatusAlert(Level.WARN));
        certificateVerifier.setAlertOnUncoveredPOE(new LogOnStatusAlert(Level.WARN));

        return certificateVerifier;
    }

    @Bean
    public PAdESService padesService() {
        return new PAdESService(certificateVerifier());
    }

    @Bean
    public CommonTrustedCertificateSource customRootCAKeyStore(){
        try {

            KeyStoreCertificateSource keystore = new KeyStoreCertificateSource(rootCA.getFile(), customRootCAKsType, customRootCAKsPassword);

            CommonTrustedCertificateSource trustedCertificateSource = new CommonTrustedCertificateSource();
            trustedCertificateSource.importAsTrusted(keystore);

            return trustedCertificateSource;
        } catch (IOException e) {
            throw new DSSException("Unable to load the root CA file ", e);
        }
    }
}

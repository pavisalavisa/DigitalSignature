package eu.europa.esig.dss.web.config;

import java.io.File;
import java.io.IOException;
import java.security.KeyStore.PasswordProtection;
import java.sql.SQLException;

import javax.annotation.PostConstruct;
import javax.annotation.PreDestroy;
import javax.sql.DataSource;

import eu.europa.esig.dss.alert.LogOnStatusAlert;
import eu.europa.esig.dss.spi.x509.CommonTrustedCertificateSource;
import org.apache.http.conn.ssl.TrustAllStrategy;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.slf4j.event.Level;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.ComponentScan;
import org.springframework.context.annotation.Configuration;
import org.springframework.context.annotation.Import;
import org.springframework.context.annotation.ImportResource;
import org.springframework.core.io.ClassPathResource;

import eu.europa.esig.dss.alert.ExceptionOnStatusAlert;
import eu.europa.esig.dss.asic.cades.signature.ASiCWithCAdESService;
import eu.europa.esig.dss.asic.xades.signature.ASiCWithXAdESService;
import eu.europa.esig.dss.cades.signature.CAdESService;
import eu.europa.esig.dss.jades.signature.JAdESService;
import eu.europa.esig.dss.model.DSSException;
import eu.europa.esig.dss.pades.signature.PAdESService;
import eu.europa.esig.dss.service.crl.JdbcCacheCRLSource;
import eu.europa.esig.dss.service.crl.OnlineCRLSource;
import eu.europa.esig.dss.service.http.commons.CommonsDataLoader;
import eu.europa.esig.dss.service.http.commons.FileCacheDataLoader;
import eu.europa.esig.dss.service.http.commons.OCSPDataLoader;
import eu.europa.esig.dss.service.http.commons.SSLCertificateLoader;
import eu.europa.esig.dss.service.http.proxy.ProxyConfig;
import eu.europa.esig.dss.service.ocsp.OnlineOCSPSource;
import eu.europa.esig.dss.spi.client.http.DSSFileLoader;
import eu.europa.esig.dss.spi.client.http.IgnoreDataLoader;
import eu.europa.esig.dss.spi.tsl.TrustedListsCertificateSource;
import eu.europa.esig.dss.spi.x509.KeyStoreCertificateSource;
import eu.europa.esig.dss.spi.x509.tsp.TSPSource;
import eu.europa.esig.dss.token.KeyStoreSignatureTokenConnection;
import eu.europa.esig.dss.tsl.function.OfficialJournalSchemeInformationURI;
import eu.europa.esig.dss.tsl.job.TLValidationJob;
import eu.europa.esig.dss.tsl.source.LOTLSource;
import eu.europa.esig.dss.validation.CertificateVerifier;
import eu.europa.esig.dss.validation.CommonCertificateVerifier;
import eu.europa.esig.dss.ws.cert.validation.common.RemoteCertificateValidationService;
import eu.europa.esig.dss.ws.server.signing.common.RemoteSignatureTokenConnection;
import eu.europa.esig.dss.ws.server.signing.common.RemoteSignatureTokenConnectionImpl;
import eu.europa.esig.dss.ws.signature.common.RemoteDocumentSignatureServiceImpl;
import eu.europa.esig.dss.ws.signature.common.RemoteMultipleDocumentsSignatureServiceImpl;
import eu.europa.esig.dss.ws.timestamp.remote.RemoteTimestampService;
import eu.europa.esig.dss.ws.validation.common.RemoteDocumentValidationService;
import eu.europa.esig.dss.xades.signature.XAdESService;

@Configuration
@ComponentScan(basePackages = { "eu.europa.esig.dss.web.job", "eu.europa.esig.dss.web.service" })
@Import({ PropertiesConfig.class, CXFConfig.class, PersistenceConfig.class, ProxyConfiguration.class, WebSecurityConfig.class,
		SchedulingConfig.class })
@ImportResource({ "${tsp-source}" })
public class DSSBeanConfig {

	private static final Logger LOG = LoggerFactory.getLogger(DSSBeanConfig.class);

	@Value("${default.validation.policy}")
	private String defaultValidationPolicy;

	@Value("${current.lotl.url}")
	private String lotlUrl;

	@Value("${lotl.country.code}")
	private String lotlCountryCode;

	@Value("${current.oj.url}")
	private String currentOjUrl;

	@Value("${custom.keystore.type}")
	private String customKsType;

	@Value("${custom.keystore.filename}")
	private String customKsFilename;

	@Value("${custom.keystore.password}")
	private String customKsPassword;

	@Value("${oj.content.keystore.type}")
	private String ksType;

	@Value("${oj.content.keystore.filename}")
	private String ksFilename;

	@Value("${oj.content.keystore.password}")
	private String ksPassword;

	@Value("${dss.server.signing.keystore.type}")
	private String serverSigningKeystoreType;

	@Value("${dss.server.signing.keystore.filename}")
	private String serverSigningKeystoreFilename;

	@Value("${dss.server.signing.keystore.password}")
	private String serverSigningKeystorePassword;

	@Autowired
	private TSPSource tspSource;

	@Autowired
	private DataSource dataSource;

	// can be null
	@Autowired(required = false)
	private ProxyConfig proxyConfig;
	
	@PostConstruct
	public void cachedCRLSourceInitialization() throws SQLException {
		JdbcCacheCRLSource jdbcCacheCRLSource = cachedCRLSource();
		jdbcCacheCRLSource.initTable();
	}

	@PreDestroy
	public void cachedCRLSourceClean() throws SQLException {
		JdbcCacheCRLSource jdbcCacheCRLSource = cachedCRLSource();
		jdbcCacheCRLSource.destroyTable();
	}

	@Bean
	public CommonsDataLoader dataLoader() {
		CommonsDataLoader dataLoader = new CommonsDataLoader();
		dataLoader.setProxyConfig(proxyConfig);
		return dataLoader;
	}
	
	@Bean
    public CommonsDataLoader trustAllDataLoader() {
        CommonsDataLoader dataLoader = new CommonsDataLoader();
		dataLoader.setProxyConfig(proxyConfig);
		dataLoader.setTrustStrategy(TrustAllStrategy.INSTANCE);
        return dataLoader;
    }

	@Bean
	public OCSPDataLoader ocspDataLoader() {
		OCSPDataLoader ocspDataLoader = new OCSPDataLoader();
		ocspDataLoader.setProxyConfig(proxyConfig);
		return ocspDataLoader;
	}

	@Bean
	public FileCacheDataLoader fileCacheDataLoader() {
		FileCacheDataLoader fileCacheDataLoader = new FileCacheDataLoader();
		fileCacheDataLoader.setDataLoader(dataLoader());
		// Per default uses "java.io.tmpdir" property
		// fileCacheDataLoader.setFileCacheDirectory(new File("/tmp"));
		return fileCacheDataLoader;
	}

	@Bean
	public OnlineCRLSource onlineCRLSource() {
		OnlineCRLSource onlineCRLSource = new OnlineCRLSource();
		onlineCRLSource.setDataLoader(dataLoader());
		return onlineCRLSource;
	}

	@Bean
	public JdbcCacheCRLSource cachedCRLSource() {
		JdbcCacheCRLSource jdbcCacheCRLSource = new JdbcCacheCRLSource();
		jdbcCacheCRLSource.setDataSource(dataSource);
		jdbcCacheCRLSource.setProxySource(onlineCRLSource());
		jdbcCacheCRLSource.setDefaultNextUpdateDelay((long) (60 * 10)); // 10 minutes
		return jdbcCacheCRLSource;
	}

	@Bean
	public OnlineOCSPSource onlineOcspSource() {
		OnlineOCSPSource onlineOCSPSource = new OnlineOCSPSource();
		onlineOCSPSource.setDataLoader(ocspDataLoader());
		return onlineOCSPSource;
	}

	@Bean(name = "european-trusted-list-certificate-source")
	public TrustedListsCertificateSource trustedListSource() {
		return new TrustedListsCertificateSource();
	}

	@Bean
	public CertificateVerifier certificateVerifier() {
		CommonCertificateVerifier certificateVerifier = new CommonCertificateVerifier();
		certificateVerifier.setCrlSource(cachedCRLSource());
		certificateVerifier.setOcspSource(onlineOcspSource());
		certificateVerifier.setDataLoader(dataLoader());
		// TODO: Add trusted cert sources here
		certificateVerifier.setTrustedCertSources(/*trustedListSource(),*/ customContentKeyStore());
		// Default configs
		certificateVerifier.setAlertOnMissingRevocationData(new LogOnStatusAlert(Level.WARN));
		certificateVerifier.setAlertOnInvalidTimestamp(new LogOnStatusAlert(Level.WARN));
		certificateVerifier.setAlertOnRevokedCertificate(new LogOnStatusAlert(Level.WARN));
		certificateVerifier.setAlertOnNoRevocationAfterBestSignatureTime(new LogOnStatusAlert(Level.WARN));
		certificateVerifier.setAlertOnUncoveredPOE(new LogOnStatusAlert(Level.WARN));

		certificateVerifier.setCheckRevocationForUntrustedChains(false);

		return certificateVerifier;
	}

	@Bean
	public ClassPathResource defaultPolicy() {
		return new ClassPathResource(defaultValidationPolicy);
	}

	@Bean
	public CAdESService cadesService() {
		CAdESService service = new CAdESService(certificateVerifier());
		service.setTspSource(tspSource);
		return service;
	}

	@Bean
	public XAdESService xadesService() {
		XAdESService service = new XAdESService(certificateVerifier());
		service.setTspSource(tspSource);
		return service;
	}

	@Bean
	public PAdESService padesService() {
		PAdESService service = new PAdESService(certificateVerifier());
		service.setTspSource(tspSource);
		return service;
	}

	@Bean
	public JAdESService jadesService() {
		JAdESService service = new JAdESService(certificateVerifier());
		service.setTspSource(tspSource);
		return service;
	}

	@Bean
	public ASiCWithCAdESService asicWithCadesService() {
		ASiCWithCAdESService service = new ASiCWithCAdESService(certificateVerifier());
		service.setTspSource(tspSource);
		return service;
	}

	@Bean
	public ASiCWithXAdESService asicWithXadesService() {
		ASiCWithXAdESService service = new ASiCWithXAdESService(certificateVerifier());
		service.setTspSource(tspSource);
		return service;
	}

	@Bean
	public RemoteDocumentSignatureServiceImpl remoteSignatureService() {
		RemoteDocumentSignatureServiceImpl service = new RemoteDocumentSignatureServiceImpl();
		service.setAsicWithCAdESService(asicWithCadesService());
		service.setAsicWithXAdESService(asicWithXadesService());
		service.setCadesService(cadesService());
		service.setXadesService(xadesService());
		service.setPadesService(padesService());
		service.setJadesService(jadesService());
		return service;
	}

	@Bean
	public RemoteMultipleDocumentsSignatureServiceImpl remoteMultipleDocumentsSignatureService() {
		RemoteMultipleDocumentsSignatureServiceImpl service = new RemoteMultipleDocumentsSignatureServiceImpl();
		service.setAsicWithCAdESService(asicWithCadesService());
		service.setAsicWithXAdESService(asicWithXadesService());
		service.setXadesService(xadesService());
		service.setJadesService(jadesService());
		return service;
	}

	@Bean
	public RemoteDocumentValidationService remoteValidationService() {
		RemoteDocumentValidationService service = new RemoteDocumentValidationService();
		service.setVerifier(certificateVerifier());
		return service;
	}
	
	@Bean
	public RemoteCertificateValidationService RemoteCertificateValidationService() {
		RemoteCertificateValidationService service = new RemoteCertificateValidationService();
		service.setVerifier(certificateVerifier());
		return service;
	}

	@Bean
	public KeyStoreSignatureTokenConnection remoteToken() throws IOException {
		return new KeyStoreSignatureTokenConnection(new ClassPathResource(serverSigningKeystoreFilename).getFile(), serverSigningKeystoreType,
				new PasswordProtection(serverSigningKeystorePassword.toCharArray()));
	}

	@Bean
	public RemoteSignatureTokenConnection serverToken() throws IOException {
		RemoteSignatureTokenConnectionImpl remoteSignatureTokenConnectionImpl = new RemoteSignatureTokenConnectionImpl();
		remoteSignatureTokenConnectionImpl.setToken(remoteToken());
		return remoteSignatureTokenConnectionImpl;
	}
	
	@Bean
	public RemoteTimestampService timestampService() throws IOException {
		RemoteTimestampService timestampService = new RemoteTimestampService();
		timestampService.setTSPSource(tspSource);
		return timestampService;
	}

	@Bean
	public KeyStoreCertificateSource ojContentKeyStore() {
		try {
			return new KeyStoreCertificateSource(new ClassPathResource(ksFilename).getFile(), ksType, ksPassword);
		} catch (IOException e) {
			throw new DSSException("Unable to load the file " + ksFilename, e);
		}
	}

	@Bean
	public CommonTrustedCertificateSource customContentKeyStore(){
		try {
            KeyStoreCertificateSource keystore = new KeyStoreCertificateSource(new ClassPathResource(customKsFilename).getFile(), customKsType, customKsPassword);

            CommonTrustedCertificateSource trustedCertificateSource = new CommonTrustedCertificateSource();
            trustedCertificateSource.importAsTrusted(keystore);

            return trustedCertificateSource;
        } catch (IOException e) {
			throw new DSSException("Unable to load the file " + customKsFilename, e);
		}
	}
	
	@Bean 
	public TLValidationJob job() {
		TLValidationJob job = new TLValidationJob();
		job.setTrustedListCertificateSource(trustedListSource());
		job.setListOfTrustedListSources(europeanLOTL());
		job.setOfflineDataLoader(offlineLoader());
		job.setOnlineDataLoader(onlineLoader());
		return job;
	}

	@Bean
	public DSSFileLoader onlineLoader() {
		FileCacheDataLoader onlineFileLoader = new FileCacheDataLoader();
		onlineFileLoader.setCacheExpirationTime(0);
		onlineFileLoader.setDataLoader(dataLoader());
		onlineFileLoader.setFileCacheDirectory(tlCacheDirectory());
		return onlineFileLoader;
	}

	@Bean(name = "european-lotl-source")
	public LOTLSource europeanLOTL() {
		LOTLSource lotlSource = new LOTLSource();
		lotlSource.setUrl(lotlUrl);
		lotlSource.setCertificateSource(ojContentKeyStore());
		lotlSource.setSigningCertificatesAnnouncementPredicate(new OfficialJournalSchemeInformationURI(currentOjUrl));
		lotlSource.setPivotSupport(true);
		return lotlSource;
	}

	@Bean
	public DSSFileLoader offlineLoader() {
		FileCacheDataLoader offlineFileLoader = new FileCacheDataLoader();
		offlineFileLoader.setCacheExpirationTime(Long.MAX_VALUE);
		offlineFileLoader.setDataLoader(new IgnoreDataLoader());
		offlineFileLoader.setFileCacheDirectory(tlCacheDirectory());
		return offlineFileLoader;
	}

	@Bean
	public File tlCacheDirectory() {
		File rootFolder = new File(System.getProperty("java.io.tmpdir"));
		File tslCache = new File(rootFolder, "dss-tsl-loader");
		if (tslCache.mkdirs()) {
			LOG.info("TL Cache folder : {}", tslCache.getAbsolutePath());
		}
		return tslCache;
	}
	
    /* QWAC Validation */

    @Bean
    public SSLCertificateLoader sslCertificateLoader() {
        SSLCertificateLoader sslCertificateLoader = new SSLCertificateLoader();
        sslCertificateLoader.setCommonsDataLoader(trustAllDataLoader());
        return sslCertificateLoader;
    }

}
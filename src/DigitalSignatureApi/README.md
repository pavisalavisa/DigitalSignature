## Demonstrations for DSS : Digital Signature Service

This is the demonstration repository for project DSS : https://ec.europa.eu/cefdigital/wiki/display/CEFDIGITAL/eSignature. 

The demonstration is stripped leaving only REST services packed in a Docker image.

## How to start
Open terminal at `src/DigitalSignatureApi`

Execute the following script to build the Docker image. This might take a while.
```
$ docker image build -t digital-signature-api
```

Execute the following script to start the REST API
```
$ docker run -it --rm -p 8080:8080 digital-signature-api
```

The API is now running on `localhost:8080`.

Check out [the official documentation](https://github.com/esig/dss/blob/master/dss-cookbook/src/main/asciidoc/dss-documentation.adoc) to learn more or check out `http://localhost:8080/services/` to list available services and corresponding swagger UI. 

*Note that SOAP is disabled and WADL does not exist.*
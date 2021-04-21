#
# Build stage
#
FROM maven AS build
EXPOSE 8080
COPY . /home/app/

COPY pom.xml /home/app/pom.xml
RUN mvn -f /home/app/pom.xml clean install -DskipTests 

# #
# # Package stage
# #
FROM openjdk:11-jre-slim

COPY --from=build /home/app/dss-demo-bundle/target/dss-demo-bundle-5.8.1.tar.gz /usr/local/lib/dss-demo-bundle-5.8.1.tar.gz
WORKDIR /usr/local/lib/
RUN tar -xvf dss-demo-bundle-5.8.1.tar.gz
RUN chmod +x dss-demo-bundle-5.8.1/apache-tomcat-8.5.61/bin/startup.sh dss-demo-bundle-5.8.1/apache-tomcat-8.5.61/bin/catalina.sh

ENTRYPOINT ["sh","dss-demo-bundle-5.8.1/apache-tomcat-8.5.61/bin/catalina.sh", "run"]

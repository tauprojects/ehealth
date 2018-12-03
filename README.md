# ehealth
eHealth workshop repository

### Server
Java Gradle Spring framwork based HTTP server

#### API 



##1. Strain Info 
### GET /ehealth/strain/{strain-name}
Example: http://ehealth:8080/ehealth/strain/Alaska

  `{
      "request_id": "8df4b9c9-1aca-41fe-9cac-6123e96279fa",
      "status": "Exist",
      "body": "Strain(id=8, name=Alaska, race=sativa, desc=Alaska, developed by Tikun Olam, is an Israeli strain comprised of 70% sativa genetics. With uplifting effects intended for daytime consumption, Alaska has been found to successfully treat an array of medical symptoms including inflammation, pain, nausea, insomnia, and gastrointestinal disorders. )"
  }`


#### Build and debug
1. In windows envrionment it is recommended to download spring sts https://spring.io/tools eclipse based or JetBrain InteliJ IDE.

2. For Linux envrionment 

        $ git clone https://github.com/tauprojects/ehealth.git
        $ cd servers
        $ ./gradlew build

Once the build finished succesfully the server `jar` executable file can be foune under `build/libs`

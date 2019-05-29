README for obligatorisk oppgave 2 i faget WebApplikasjoner

INNHOLD:
-- 1. URL for løsning på nett.
	- 1.2 DateTime behandling.
-- 2. Påloggingsinformasjon for admin-bruker.
   - 2.2 Autorisasjon for administratorsider

-- 3. Gjenbruk av kode fra forrige oppgave.

-- 4. Endringslogg.

-- 5. Sletting i database.

-- 6. Feilmeldinger.

-- 7. Readme fra oppgave 1.


===============================================================================

1.1 URL FOR LØSNING PÅ NETT:

URL:https://holbergsfilm-2.azurewebsites.net/

MERK: Azure-url’en vil forsvinne i løpet av 25.11.2018 da vi kun får tilgang 
til trial accounts hos Microsoft/Azure.

1.2 DateTime behandling:
Local viser to timer bak på tid. Azure-Server viser riktig. 
Samme løsning som Oblig 1. Se pkt 7. 
===============================================================================

2.1 PÅLOGGINGSINFORMASJON FOR ADMIN-BRUKER:
URL:https://holbergsfilm-2.azurewebsites.net/

Epostadresse: admin@holbergs.no
Passord: admadm

Ved innlogging vil en bruker som er en administrator omdirigeres til 
administrasjonsgrensesnittet.
En administrator har etter innlogging mulighet til å gå til nettbutikkens 
hovedside, og tilbake via lenker i brukerens dropdownmeny (øverst til høyre).

2.2 Autorisasjon for administratorsider:
For å hindre tilgang til administrasjon uten adminrettigheter har vi lagt til 
en sjekk på at man både er logget inn og har en administratornbruker i alle 
relevante “ActionResults”.

===============================================================================

3. GJENBRUK AV KODE FRA FORRIGE OPPGAVE:

Vi har valgt å lagdele også hele løsningen fra første oppgave for å forenkle 
gjenbruk av metoder. Store deler av koden er refaktorert for å fungere i 
forhold til ”dependency injection”.
Noe av denne gamle koden er enhetstestet som følge av at koden er gjenbrukt, 
men gammel kode som ikke brukes i løsningen av nåværende oppgave er ikke 
enhetstestet.

All controller-kode for denne oppgaven (oppgave 2) er i controlleren 
“AdminController”, og all denne koden er testet i henhold til oppgaveteksten. 
Vi har valgt å gjøre det slik for å synliggjøre hvilken kode som hører til 
denne oppgaven. Vi har endret navn på prosjektet og solution men nameSpacet 
heter forsåvidt enda oblig1. 

===============================================================================

4. ENDRINGSLOGG: 

Vi har overridet context.savechanges() til å registrere endringer av tabeller 
og da lagre disse i endringstabellen. Vi registrerer også når nye elementer 
blir lagt til i databasen. 

===============================================================================

5. SLETTING I DATABASE:

Vi har valgt å aldri slette fra databasen, men setter heller en bool active til 
å være false for å deaktivere filmer, brukere osv.

===============================================================================

6. FEILMELDINGER:

Vi har implementert error håndtering av databasekall-feil. Der det kastes 
et DatabaseErrorException og gis en melding i web interfacet. Samtidig lagrer 
vi feilemeldinger i error.txt og i en errortabell i databasen.

===============================================================================

7. README FRA OBLIG1
URL TIL LØSNINGEN PÅ NETT (i Azure sin sky):
    https://holbergsfilm.azurewebsites.net/

MERKNADER TIL VÅR LØSNING:

Validering:
    Vi validerer de fleste input-felter på serversiden. 
    Ett unntak er at epostadressen som fylles ut i registreringsformen testes dynamisk opp mot 
    databasen når brukeren har fylt den ut. Finnes den i databasen fra før vises det en melding til 
    brukeren. 

Postnummer og poststed:
    Vi går ut ifra at kunden skriver inn riktig informasjon i postnummer og poststedsfeltene. 
    Informasjon lagres til databasens poststedstabell basert på denne informasjonen.
    Vi har valgt å ikke legge en komplett poststedstabell med alle landets poststeder i databasen.

DateTime behandling:
    Når vi lagrer datetime i tabeller, i dette tilfellet Ordretabellen, så lagrer vi dem "raw". 
    Det innebærer at det er serveren applikasjonen kjøres fra som bestemmer tiden. Dermed blir det 
    UTC tidsformat på Azure sin server som brukes. Dette omgjøres til vår tidsone (+2 timer) når vi 
    henter tiden ut for å vise ordretabbellen. Dette medføerer fremvisningen av tid er feil ved 
    lokal kjøring (i IIS Express), men altså riktig for vår løsning på nett.
    Se DBHandler.OrdreHandler.AllekundensOrdre.
    
Språk i Kode:
    Da oppgaven er gitt på norsk, og det er spesifisert at språket på nettsiden skal være norsk, 
    har vi valgt å skrive koden på norsk. Vi har prøvd å holde "engelsk-norsk" til et minimum men 
    ser likevel nå at det vil være hensiktsmessig å gå over til engelsk kode før neste del av 
    oppgaven. (Og da refaktorere all eksisterende kode.)

Ubrukt kode:
    Det er noe kode som ikke er i bruk, men som er der for eventuell senere bruk ved neste 
    obligatoriske oppgave. Det er kommentarer som beskriver dette der det 


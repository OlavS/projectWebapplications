var valgtFilmIdListe = [];
var eidFilmIdListe = [];

$(document).ready(function () {
    //Henter kundens eide filmer når siden lastes.
    $.ajax({
        url: AppUrl.HentEideFilmer,
        datatype: "application/json",
        success: function (eidefilmer) {
            var filmListe;
            try {
                filmListe = JSON.parse(eidefilmer);
            } catch (feil) {
                filmListe = null;
            }
            if (filmListe != null) {
                eidFilmIdListe = filmListe;
            }
            // Gjør et kall på filmThumbEid når siden lastes.
            filmThumbEid();
        }
    });

    //Henter handlekurven når siden lastes.
    $.ajax({
        url: AppUrl.HentHandlekurv,
        datatype: "application/json",
        success: function (handlekurven) {
            var filmListe;
            try {
                filmListe = JSON.parse(handlekurven);
            } catch (feil) {
                filmListe = null;
            }
            if (filmListe != null) {
                valgtFilmIdListe = filmListe;
            }
            // Gjør et kall på filmThumbHandlekurv når siden lastes.
            filmThumbHandlekurv();
        }
    });
});

/**
 * filmThumbEid() gjør en gjennomgang av listen over filmer kunden eier, og når
 * den finner et filmtommel-element med id: filmthumb + filmId'en fra listen,
 * gis det elementet class="filmThumbEid". Denne class'en har css styling for å
 * visualisere i view'et at filmen allerede er i kundens eie.
 */
function filmThumbEid() {
    for (var j = 0; j < eidFilmIdListe.length; j++) {
        var filmId = eidFilmIdListe[j];
        $(".filmThumb" + filmId).addClass("filmThumbEid");
    }
};

/**
 * filmThumbHandlekurv() gjør en gjennomgang av listen over filmer i handlekurven, og når
 * den finner et filmtommel-element med id: filmthumb + filmId'en fra listen,
 * gis det elementet class="filmThumbHandlekurv". Denne class'en har css styling for å
 * visualisere i view'et at filmen allerede ligger i handlekurven.
 */
function filmThumbHandlekurv() {
    for (var i = 0; i < valgtFilmIdListe.length; i++) {
        var filmId = valgtFilmIdListe[i];
        $(".filmThumb" + filmId).addClass("filmThumbHandlekurv");
    }
}

/**
 * Legger til klassen filmThumbHandlekurv på thumbnailen til filmen som er lagt i handlekurven.
 */
function leggTilFilmThumbHandlekurv(filmId) {
    $(".filmThumb" + filmId).addClass("filmThumbHandlekurv");
}

/**
 * Fjerner klassen filmThumbHandlekurv på thumbnailen til filmen som fjernes fra handlekurven.
 */
function fjernFilmThumbHandlekurv(filmId) {
    $(".filmThumb" + filmId).removeClass("filmThumbHandlekurv")
}
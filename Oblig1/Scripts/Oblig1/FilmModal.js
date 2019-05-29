/**
 * Denne filen fyller ut film modalen og styrer innholdet i handlekurven.
 */
var valgtFilm;
var ikkeBestilling;

/**
 * Funksjonen fyller inn i modalen når film thumben blir trykket på.
*/
function LagModal(film) {
    valgtFilm = film;

    var sjangerStreng = "Sjanger: ";

    film.Genres.forEach(function (element) {
        sjangerStreng += element.Name + ", ";
    });
    sjangerStreng = sjangerStreng.slice(0, -2);

    $("#tittelFelt").text(film.Title);
    $("#beskrivelse").text(film.Description);
    $("#bilde").attr("src", film.ImgURL);
    $("#modalSjangere").text(sjangerStreng);
    $("#modalPris").text("Pris: " + film.Price + " kr");
    if (ikkeBestilling) {
        deaktiverBestillKnapp();
    } else {
        filmEid();
    }
}

/**
 * Funksjon som trigges når bestillknappen i film modalen trykkes på. Knappen får
 * enten en ekstra klasse active, eller så fjernes klassen fra knappen.
 * Videre gis et kall til metoden doChange med knappen som parameter.
 */
$("#bestillKnapp").on("click", function () {
    if ($(this).hasClass("active")) {
        $(this).removeClass("active");
        $(this).prop("value", "Bestill");
    } else {
        $(this).addClass("active");
        $(this).prop("value", "I handlekurv");
    }
    endreHandlekurv($(this));
});

/**
 * Legger inn og fjerner elementer fra handlekurven. Kaller på fjernFilmThumbHandlekurv(id)
 * som fjerner classen filmThumbHandlekurv fra film thumben.
 * Kaller også filmThumbHandlekurv som legger til klassen filmThumbHandlekurv på film thumben.
 */
function endreHandlekurv(knapp) {
    if (knapp.hasClass("active")) {
        valgtFilmIdListe.push(valgtFilm.Id);
        leggTilFilmThumbHandlekurv(valgtFilm.Id);
    } else {
        for (var i = 0; i < valgtFilmIdListe.length; i++) {
            if (valgtFilmIdListe[i] == valgtFilm.Id) {
                valgtFilmIdListe.splice(i, 1);
                fjernFilmThumbHandlekurv(valgtFilm.Id);
                break;
            }
        }
    }
    sendValgtFilmIdListe();
}

/**
 * Finner ut om kunden eier filmen når filmmodalen åpnes.
 * Dersom kunden eier filmen som han/hun trykker på så disables
 * bestillknappen, og I bibliotek skrives i den.
 */
function filmEid() {
    $("#bestillKnapp").prop("value", "Bestill");
    $("#bestillKnapp").prop("disabled", false);

    if (!valgtFilm.Active) {
        $("#bestillKnapp").prop("value", "Ikke tilgjengelig");
        $("#bestillKnapp").removeClass("active");
        $("#bestillKnapp").prop("disabled", true);
        return;
    } else {
        for (var j = 0; j < eidFilmIdListe.length; j++) {
            if (eidFilmIdListe[j] === valgtFilm.Id) {
                $("#bestillKnapp").prop("value", "I bibliotek");
                $("#bestillKnapp").removeClass("active");
                $("#bestillKnapp").prop("disabled", true);
                return;
            }
        }

        gjørAktiv(valgtFilm.Id);
    }
}

/**
 * Gjør bestillknappen aktiv dersom en film er lagt i handlekurven,
 * og deaktiv dersom den ikke er det.
 */
function gjørAktiv(id) {
    if (erValgt(id)) {
        $("#bestillKnapp").addClass("active");
        $("#bestillKnapp").prop("value", "I handlekurv");
    } else {
        $("#bestillKnapp").removeClass("active");
        $("#bestillKnapp").prop("value", "Bestill");
    }
}

/**
 * Sjekker om en film ligger i handlekurven.
 */
function erValgt(value) {
    if (valgtFilmIdListe.find(f => f == value)) {
        return true;
    } else {
        return false;
    }
}

/**
 * Kalles fra _Layout.cshtml, og brukes til å deaktivere bestillknappen dersom
 * brukeren ikke er logget inn.
 */
function forhindreBestilling() {
    ikkeBestilling = true;
}

/**
 * Deaktiverer bestillknappen dersom brukeren ikke er logget inn.
 */
function deaktiverBestillKnapp() {
    $("#MeldingBestill").empty();
    $("#MeldingBestill").append("Logg inn før bestilling");
    $("#bestillKnapp").prop("disabled", true);
}

/**
 * Sender valgtFilmIdListe altså det som ligger i handlekurven til
 * controlleren, der den lagres i Session["Handlekurv"].
 */
function sendValgtFilmIdListe() {
    $.ajax({
        url: AppUrl.LagreHandlekurv,
        cache: false,
        dataType: "application/json",
        data: { filmIdList: JSON.stringify(valgtFilmIdListe) }
    });
}